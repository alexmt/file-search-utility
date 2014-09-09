using System.IO;

namespace FileSearch.IO
{
    public class FileSourceUpdatedArgs
    {
        #region Private Fields

        private readonly string filePath;
        private readonly WatcherChangeTypes changeType;
        private readonly string newFilePath;

        #endregion

        #region Constructor

        public FileSourceUpdatedArgs(string filePath, string newFilePath) : this(filePath, WatcherChangeTypes.Renamed)
        {
            this.newFilePath = newFilePath;
        }

        public FileSourceUpdatedArgs(string filePath, WatcherChangeTypes changeType)
        {
            this.filePath = filePath;
            this.changeType = changeType;
        }

        #endregion

        #region Public Properties

        public string NewFilePath
        {
            get { return newFilePath; }
        }

        public string FilePath
        {
            get { return filePath; }
        }

        public WatcherChangeTypes ChangeType
        {
            get { return changeType; }
        }

        #endregion

    }
}