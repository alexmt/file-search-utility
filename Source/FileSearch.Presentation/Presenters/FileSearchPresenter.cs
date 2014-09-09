using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using FileSearch.Common;
using FileSearch.Model;
using FileSearch.Plugins;
using FileSearch.Presentation.Views;

namespace FileSearch.Presentation.Presenters
{
    public class FileSearchPresenter : ISearchResultReceiver
    {

        #region Private Fields

        private const string SEARCH_PLUGIN_NOTIFICATION = "Search plugin notification";
        private readonly IFileSystem fileSystem;
        private readonly IFileSearchView view;
        private readonly PluginManager pluginsManager;
        private string searchingDirectory;
        private bool cancelSearch;
        private readonly ManualResetEvent searchCompletedEvent = new ManualResetEvent(true);

        #endregion

        #region Private Methods

        private static FileSystemItemViewData ConvertFileToViewItem(File item)
        {
            return new FileSystemItemViewData
            {
                Title = item.Name,
                FullPath = item.FullPath,
                ItemType = FileSystemItemType.File,
            };
        }

        private static FileSystemItemViewData ConvertDirectoryToViewItem(Directory item)
        {
            return new FileSystemItemViewData
            {
                Title = item.Name,
                FullPath = item.FullPath,
                ItemType = FileSystemItemType.Directory
            };
        }

        private static ListItem ConvertPluginToListItem(ISearchPlugin searchPlugin)
        {
            return new ListItem
            {
                Name = searchPlugin.Info.Name,
                Value = searchPlugin
            };
        }

        private bool CanDoNavigation(string directoryPath, out MessageInfo errorMessage)
        {
            const string navigationError = "Navigation error";

            if(!string.IsNullOrEmpty(searchingDirectory))
            {
                errorMessage = new MessageInfo
                {
                    Caption = navigationError,
                    Text = "Searching in progress. Please cancel searching on wait for completion."
                };
                return false;
            }
            if (!fileSystem.IsDirectoryExists(directoryPath))
            {
                errorMessage = new MessageInfo
                {
                    Caption = navigationError,
                    Text = "Cannot file path specified"
                };
                return false;
            }
            errorMessage = null;
            return true;
        }

        private string GetSearchingStatusText()
        {
            return string.Format("Searching in '{0}' directory...", searchingDirectory);
        }

        private static ListItem[] GetFileSizeItems()
        {
            return new[]
            {
                new ListItem {Name = "Unlimited", Value = null},
                new ListItem {Name = "1 kb.", Value = (long) 1024},
                new ListItem {Name = "100 kb.", Value = (long) (1024 * 100)},
                new ListItem {Name = "1 mb.", Value = (long) (1024 * 1024)},
                new ListItem {Name = "1 mb.", Value = (long) ((1024 * 1024) * 10)}
            };
        }

        private void CompleteSearch(Exception exception)
        {
            if (exception != null)
            {
                view.Invoke(() =>
                {
                    var statusText = string.Format("{0} Completed with error.", GetSearchingStatusText());
                    view.SetStatusText(statusText);
                    view.ShowErrorMessage(new MessageInfo
                    {
                        Caption = "Search error",
                        Text = exception.Message
                    });
                });
            }
            else
            {
                view.Invoke(() => view.SetStatusText(string.Format("{0} Done.", GetSearchingStatusText())));
            }
            searchingDirectory = null;
            cancelSearch = false;
            searchCompletedEvent.Set();
            view.Invoke(() => view.SetButtonsVisibility(false, false, true));
        }

        #endregion

        #region Constructor

        public FileSearchPresenter(IFileSystem fileSystem, PluginManager pluginsManager, IFileSearchView view)
        {
            this.fileSystem = fileSystem;
            this.pluginsManager = pluginsManager;
            this.view = view;
            pluginsManager.NewPluginsLoaded += PluginsManager_NewPluginsLoaded;
            pluginsManager.PluginsReloaded += PluginsManager_PluginsReloaded;
            pluginsManager.LoadError += PluginsManager_LoadError;
        }


        #endregion

        #region Event Handlers

        private void PluginsManager_LoadError(Exception obj)
        {
            view.Invoke(() => view.ShowNotification(SEARCH_PLUGIN_NOTIFICATION, string.Format("Plugins were not loaded: {0}", obj.Message)));
        }

        private void PluginsManager_NewPluginsLoaded(ISearchPlugin[] newPlugins)
        {
            view.Invoke(()=>
            {
                view.FillPluginsList(newPlugins.Select(plugin => ConvertPluginToListItem(plugin)).ToArray(), false);
                view.ShowNotification(SEARCH_PLUGIN_NOTIFICATION, string.Format("{0} plugin(s) have been loaded.", newPlugins.Length));
            });
        }

        private void PluginsManager_PluginsReloaded(ISearchPlugin[] newPlugins)
        {
            lock (searchCompletedEvent)
            {
                if(!string.IsNullOrEmpty(searchingDirectory))
                {
                    searchCompletedEvent.Reset();
                    view.Invoke(() =>
                    {
                        CancelSearch();
                        view.ShowNotification(SEARCH_PLUGIN_NOTIFICATION,"Search must be canceled since search plugins are changed.");
                    });
                }
            }
            searchCompletedEvent.WaitOne();
            view.Invoke(()=> view.FillPluginsList(newPlugins.Select(plugin => ConvertPluginToListItem(plugin)).ToArray(), true));
        }

        #endregion

        #region Public Methods

        public void NavigateTo(string directoryPath)
        {
            MessageInfo errorMessage;
            if (!CanDoNavigation(directoryPath, out errorMessage))
            {
                view.ShowErrorMessage(errorMessage);
            }
            else
            {
                Directory directory = new Directory(directoryPath, fileSystem);
                Dictionary<string, Icon> images = new Dictionary<string, Icon>();
                List<FileSystemItemViewData> childDirectories = new List<FileSystemItemViewData>();
                foreach (var item in directory.ListChildDirectories())
                {
                    var viewItem = ConvertDirectoryToViewItem(item);
                    var icon = fileSystem.GetAssociatedIcon(item.FullPath, true);
                    if (icon != null)
                    {
                        viewItem.ImageKey = item.FullPath;
                        images.Add(item.FullPath, icon);
                    }
                    childDirectories.Add(viewItem);
                }

                List<FileSystemItemViewData> childFiles = new List<FileSystemItemViewData>();
                foreach (var item in directory.ListFiles("*", false))
                {
                    var viewItem = ConvertFileToViewItem(item);
                    var icon = fileSystem.GetAssociatedIcon(item.FullPath, false);
                    if (icon != null)
                    {
                        viewItem.ImageKey = item.FullPath;
                        images.Add(item.FullPath, icon);
                    }
                    childFiles.Add(viewItem);
                }
                var items = childDirectories.Concat(childFiles);
                if (directory.HasParentDirectory)
                {
                    items = items.Concat(new[]
                    {
                        new FileSystemItemViewData
                        {
                            Title = "..",
                            FullPath = directoryPath,
                            ItemType = FileSystemItemType.ParentDirectory
                        }
                    });
                }
                view.SetFileViewMode(false);

                view.ShowViewData(new SearchFormViewData
                {
                    DirectoryItems = from item in items orderby item.ItemType descending, item.FullPath select item,
                    FullPath = directoryPath,
                    Images = images
                });
                view.SetButtonsVisibility(true, false, false);
            }
        }

        public void NavigateToParent(string directoryPath)
        {
            MessageInfo errorMessage;
            if (!CanDoNavigation(directoryPath, out errorMessage))
            {
                view.ShowErrorMessage(errorMessage);
            }
            Directory directory = new Directory(directoryPath, fileSystem);
            if(!directory.HasParentDirectory)
            {
                view.ShowErrorMessage(new MessageInfo
                {
                    Caption = "Navigation Error",
                    Text = string.Format("Cannot find parent for directory {0}", directoryPath)
                });
            }
            else
            {
                NavigateTo(directory.ParentDirectoryPath);
            }
        }

        public void ShowSearchOptions()
        {
            view.FillFileSizeList(GetFileSizeItems());
            view.FillPluginsList(pluginsManager.GetSearchPlugins().Select(plugin => ConvertPluginToListItem(plugin)).ToArray(), true);
            view.OpenSearchOptions();
        }        

        public void StartSearch(string directoryPath, SearchOptions options)
        {
            MessageInfo errorMessage;
            if(CanDoNavigation(directoryPath, out errorMessage))
            {
                searchingDirectory = directoryPath;
                view.SetStatusText(GetSearchingStatusText());
                var directory = new Directory(directoryPath, fileSystem);
                directory.StartSearchWithOptions(options, new CachedSearchResultReceiver(this, TimeSpan.FromSeconds(1)));
            }
            else
            {
                view.ShowErrorMessage(errorMessage);
            }
            view.ShowViewData(new SearchFormViewData
            {
                DirectoryItems = new FileSystemItemViewData[0],
                FullPath = directoryPath
            });
            view.SetButtonsVisibility(false, true, false);
            view.SetFileViewMode(true);
        }

        public void CancelSearch()
        {
            cancelSearch = true;
            view.SetStatusText(GetSearchingStatusText() + " Canceling...");
        }

        #endregion

        #region ISearchResultReceiver Members

        void ISearchResultReceiver.ReceiveResult(File[] fileses)
        {
            var viewItems = fileses.Select(file => ConvertFileToViewItem(file));
            view.Invoke(() => view.AddFileSystemItems(viewItems.ToArray(), new Dictionary<string, Icon>()));
        }

        void ISearchResultReceiver.SearchCompleted(Exception exception)
        {
            lock (searchCompletedEvent)
            {
                CompleteSearch(exception);
            }
        }

        bool ISearchResultReceiver.CancelSearch
        {
            get { return cancelSearch; }
        }

        #endregion
    }
}