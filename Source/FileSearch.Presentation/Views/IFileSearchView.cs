using System;
using System.Collections.Generic;
using System.Drawing;
using FileSearch.Presentation.Presenters;

namespace FileSearch.Presentation.Views
{
    public interface IFileSearchView
    {
        void AddFileSystemItems(FileSystemItemViewData[] items, Dictionary<string, Icon> images);

        void ShowViewData(SearchFormViewData viewData);

        void ShowErrorMessage(MessageInfo errorMessage);

        void OpenSearchOptions();

        void FillPluginsList(ListItem[] items, bool clearList);

        void FillFileSizeList(ListItem[] items);
        
        void SetStatusText(string statusText);

        void SetButtonsVisibility(bool searchButtonVisible, bool cancelSearchButtonVisible, bool b);

        void Invoke(Action action);

        void ShowNotification(string title, string notification);

        void SetFileViewMode(bool isDetails);
    }
}