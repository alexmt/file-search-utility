using System;
using FileSearch.Common;

namespace FileSearch.Plugins
{
    public class FileFilterProxy : MarshalByRefObject, IFileFilter
    {
        private readonly IFileFilter filter;

        public FileFilterProxy(IFileFilter filter)
        {
            this.filter = filter;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region IFileFilter Members

        public bool IsMatch(System.IO.FileInfo input, System.IO.Stream stream)
        {
            return filter.IsMatch(input, stream);
        }

        #endregion

    }
}