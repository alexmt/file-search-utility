using System;
using System.Collections.Generic;

namespace FileSearch.Model
{
    public class CachedSearchResultReceiver : ISearchResultReceiver
    {
        #region Private Fields

        private readonly ISearchResultReceiver receiver;
        private readonly TimeSpan flashCacheTimeout;
        private DateTime lastFlashTime = DateTime.MinValue;
        private readonly List<File> cache = new List<File>();

        #endregion

        #region Private Methods

        private void FlashCache()
        {
            if (cache.Count > 0)
            {
                lastFlashTime = DateTime.Now;
                var allFiles = cache.ToArray();
                cache.Clear();
                receiver.ReceiveResult(allFiles);
            }
        }

        #endregion

        #region Constructor

        public CachedSearchResultReceiver(ISearchResultReceiver receiver, TimeSpan flashCacheTimeout)
        {
            this.receiver = receiver;
            this.flashCacheTimeout = flashCacheTimeout;
        }

        #endregion

        #region ISearchResultReceiver Members

        public void ReceiveResult(File[] files)
        {
            cache.AddRange(files);
            if((DateTime.Now - lastFlashTime) > flashCacheTimeout)
            {
                FlashCache();
            }
        }


        public void SearchCompleted(Exception exception)
        {
            FlashCache();
            receiver.SearchCompleted(exception);
        }

        public bool CancelSearch
        {
            get { return receiver.CancelSearch; }
        }

        #endregion
    }
}