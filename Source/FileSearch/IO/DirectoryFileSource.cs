using System;
using System.IO;

namespace FileSearch.IO
{
    public class DirectoryFileSource : IFileSource
    {
        #region Private Fields

        private readonly string directoryPath;
        private readonly FileSystemWatcher fileSystemWatcher;

        #endregion

        #region Private Methods

        private void OnFileSourceUpdated(FileSourceUpdatedArgs fileSourceUpdatedArgs)
        {
            if(FileSourceUpdated != null)
            {
                lock (SyncRoot)
                {
                    FileSourceUpdated(fileSourceUpdatedArgs);
                }
            }
        }

        #endregion


        #region Constructor

        public DirectoryFileSource(string directoryPath, string filter)
        {
            this.directoryPath = directoryPath;
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            fileSystemWatcher = new FileSystemWatcher(directoryPath, filter);
            fileSystemWatcher.Created += OnFileCreated;
            fileSystemWatcher.Deleted += OnFileDeleted;
            fileSystemWatcher.Changed += OnFileChanged;
            fileSystemWatcher.Renamed += OnFileRenamed;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        #endregion


        #region Event Handlers

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            OnFileSourceUpdated(new FileSourceUpdatedArgs(e.OldFullPath, e.FullPath));
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            OnFileSourceUpdated(new FileSourceUpdatedArgs(e.FullPath, e.ChangeType));
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            OnFileSourceUpdated(new FileSourceUpdatedArgs(e.FullPath, e.ChangeType));
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            OnFileSourceUpdated(new FileSourceUpdatedArgs(e.FullPath, e.ChangeType));
        }

        #endregion

        #region IFileSource Members

        public object SyncRoot
        {
            get { return fileSystemWatcher; }
        }

        public event Action<FileSourceUpdatedArgs> FileSourceUpdated;

        public string[] GetSourceFiles()
        {
            return Directory.GetFiles(directoryPath, fileSystemWatcher.Filter);
        }

        #endregion
    }
}