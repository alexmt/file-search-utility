using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FileSearch.Model;
using FileSearch.Presentation.Presenters;

namespace FileSearch.Presentation.Views
{
    public partial class FileSearchForm : Form, IFileSearchView
    {
        #region Private Fields

        private readonly SearchOptionsForm searchOptionsForm;

        #endregion

        #region Private Methods

        private void NavigateToSelectedItem()
        {
            if (filesListView.SelectedItems.Count == 1)
            {
                var selectedViewItem = filesListView.SelectedItems[0].Tag as FileSystemItemViewData;
                if (selectedViewItem != null)
                {
                    switch (selectedViewItem.ItemType)
                    {
                        case FileSystemItemType.Directory:
                            Presenter.NavigateTo(selectedViewItem.FullPath);
                            break;
                        case FileSystemItemType.ParentDirectory:
                            Presenter.NavigateToParent(directoryPathTextBox.Text);
                            break;
                    }
                }
            }
        }

        private void InitializeEvents()
        {
            Load += (sender, e) => Presenter.NavigateTo(Environment.CurrentDirectory);
            directoryPathTextBox.KeyDown += OnDirectoryPathTextBoxOnKeyDown;
            filesListView.KeyDown += OnFilesListViewOnKeyDown;
            filesListView.DoubleClick += (sender, e) => NavigateToSelectedItem();
            searchButton.Click += OnSearchButtonClick;
            cancelSearchButton.Click += (sender, e) => Presenter.CancelSearch();
            browseButton.Click += (sender, e) => Presenter.NavigateTo(directoryPathTextBox.Text);
            searchOptionsForm.SearchOptionsAccepted += SearchOptionsForm_SearchOptionsAccepted;
            notifyIcon.BalloonTipClosed += (sender, e) => notifyIcon.Visible = false;
        }

        #endregion

        #region Constructor

        public FileSearchForm()
        {
            searchOptionsForm = new SearchOptionsForm();
            InitializeComponent();
            InitializeEvents();
        }

        #endregion

        #region Public Properties

        public FileSearchPresenter Presenter { get; set; }

        #endregion

        #region Event Handlers

        private void SearchOptionsForm_SearchOptionsAccepted(SearchOptions options)
        {
            Presenter.StartSearch(directoryPathTextBox.Text, options);
        }

        private void OnFilesListViewOnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    NavigateToSelectedItem();
                    break;
                case Keys.Back:
                    Presenter.NavigateToParent(directoryPathTextBox.Text);
                    break;
            }
        }

        private void OnDirectoryPathTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Presenter.NavigateTo(directoryPathTextBox.Text);
            }
        }

        private void OnSearchButtonClick(object sender, EventArgs e)
        {
            Presenter.ShowSearchOptions();
        }


        #endregion

        #region IFileSearchView Members

        public void AddFileSystemItems(FileSystemItemViewData[] items, Dictionary<string, Icon> images)
        {
            foreach (var image in images)
            {
                imageList.Images.Add(image.Key, image.Value);
            }
            foreach (var viewItem in items)
            {
                var item = filesListView.Items.Add(viewItem.Title, viewItem.ImageKey);
                item.SubItems.Add(viewItem.FullPath);
                item.Tag = viewItem;
            }
        }

        public void ShowViewData(SearchFormViewData viewData)
        {
            imageList.Images.Clear();
            filesListView.Items.Clear();
            AddFileSystemItems(viewData.DirectoryItems.ToArray(), viewData.Images);
            directoryPathTextBox.Text = viewData.FullPath;
        }

        public void ShowErrorMessage(MessageInfo message)
        {
            MessageBox.Show(message.Text, message.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void OpenSearchOptions()
        {
            searchOptionsForm.ShowDialog();
        }

        public void FillFileSizeList(ListItem[] items)
        {
            searchOptionsForm.FillSizeList(items);
        }

        public void FillPluginsList(ListItem[] items, bool clearList)
        {
            searchOptionsForm.FillPluginsList(items, clearList);
        }

        public void SetStatusText(string statusText)
        {
            statusLabel.Text = statusText;
        }

        public void SetButtonsVisibility(bool searchButtonVisible, bool cancelSearchButtonVisible, bool browseButtonVisible)
        {
            searchButton.Visible = searchButtonVisible;
            cancelSearchButton.Visible = cancelSearchButtonVisible;
            browseButton.Visible = browseButtonVisible;
        }

        public void Invoke(Action action)
        {
            Invoke((Delegate) action);
        }

        public void ShowNotification(string title, string notification)
        {
            if (!notifyIcon.Visible)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip((int) TimeSpan.FromSeconds(5).TotalMilliseconds, title, notification, ToolTipIcon.Info);
            }
        }

        public void SetFileViewMode(bool isDetails)
        {
            filesListView.View = isDetails ? View.Details : View.LargeIcon;
        }

        #endregion
    }
}
