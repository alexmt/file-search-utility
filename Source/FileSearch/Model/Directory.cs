using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileSearch.Model
{
    public class Directory : FileSystemItem
    {
        #region Private Fields

        private readonly string fullPath;
        private readonly IFileSystem fileSystem;

        #endregion

        #region Private Methods

        private File CreateFile(string path)
        {
            return new File(path, fileSystem);
        }


        private void SearchWithOptionsSafe(SearchOptions options, ISearchResultReceiver searchResultReceiver)
        {
            try
            {
                SearchWithOptions(options, searchResultReceiver);
            }
            catch (Exception ex)
            {
                searchResultReceiver.SearchCompleted(ex);
            }
            finally
            {
                searchResultReceiver.SearchCompleted(null);
            }
        }

        private void SearchWithOptions(SearchOptions options, ISearchResultReceiver searchResultReceiver)
        {
            foreach (File file in ListFiles(options.SearchPattern, options.IsIncludeSubDirectories))
            {
                if(searchResultReceiver.CancelSearch)
                {
                    return;
                }
                if(options.MaxFileSize == null || file.Size <= options.MaxFileSize)
                {
                    using (var stream = System.IO.File.OpenRead(file.FullPath))
                    {
                        if(options.FileFilter.IsMatch(new FileInfo(file.FullPath), stream))
                        {
                            searchResultReceiver.ReceiveResult(new[] { file });
                        }
                    }
                }
            }
        }

        #endregion

        #region Constructor

        public Directory(string fullPath, IFileSystem fileSystem) : base(fullPath)
        {
            this.fullPath = fullPath;
            this.fileSystem = fileSystem;
        }

        #endregion

        #region Public Properties

        public string Name 
        {
            get { return Path.GetFileName(FullPath); } 
        }

        public string ParentDirectoryPath
        {
            get { return Path.GetDirectoryName(FullPath); }
        }

        public bool HasParentDirectory
        {
            get { return !string.IsNullOrEmpty(ParentDirectoryPath); }
        }

        #endregion

        #region Public Methods

        public IEnumerable<File> ListFiles(string searchPattern, bool includeSubDirectories)
        {
            IEnumerable<File> files = fileSystem.
                LoadDirectoryFiles(fullPath, searchPattern).
                Select(path => CreateFile(path));
            if(includeSubDirectories)
            {
                files = files.
                    Concat(ListChildDirectories().SelectMany(directory => directory.ListFiles(searchPattern, true)));
            }
            return files;
        }

        public IEnumerable<Directory> ListChildDirectories()
        {
            return fileSystem.
                LoadChildDirectories(fullPath).
                Select(path => new Directory(path, fileSystem));
        }

        public void StartSearchWithOptions(SearchOptions options, ISearchResultReceiver searchResultReceiver)
        {
            ThreadPool.QueueUserWorkItem(item => SearchWithOptionsSafe(options, searchResultReceiver));
        }

        #endregion
    }
}
