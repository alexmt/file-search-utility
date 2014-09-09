using FileSearch.Common;

namespace FileSearch.Model
{
    public class SearchOptions
    {
        public bool IsIncludeSubDirectories { get; set; }

        public long? MaxFileSize { get; set; }

        public IFileFilter FileFilter { get; set; }

        public string SearchPattern { get; set; }
    }
}