using System;
using System.IO;

namespace FileSearch.IO
{
    public class TempFile : IDisposable
    {
        #region Private Fields

        private readonly string sourceFilePath;
        private string temporalFilePath;

        #endregion

        #region Constructor

        public TempFile(string sourceFilePath)
        {
            this.sourceFilePath = sourceFilePath;
        }

        #endregion

        #region Public Properties

        public string FilePath
        {
            get
            {
                if(string.IsNullOrEmpty(temporalFilePath))
                {
                    temporalFilePath = Path.GetTempFileName();
                    File.Copy(sourceFilePath, temporalFilePath, true);
                }
                return temporalFilePath;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if(!string.IsNullOrEmpty(temporalFilePath))
            {
                File.Delete(temporalFilePath);
            }
        }

        #endregion
    }
}