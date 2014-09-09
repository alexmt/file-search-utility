using System;

namespace FileSearch.Model
{
    public interface ISearchResultReceiver
    {
        void ReceiveResult(File[] files);

        void SearchCompleted(Exception exception);

        bool CancelSearch { get; }
    }
}