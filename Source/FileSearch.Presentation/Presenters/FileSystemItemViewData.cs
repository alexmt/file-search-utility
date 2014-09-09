namespace FileSearch.Presentation.Presenters
{
    public enum FileSystemItemType
    {
        File = 1,
        Directory = 2,
        ParentDirectory = 3
    }

    public class FileSystemItemViewData
    {
        public string FullPath { get; set; }

        public string Title { get; set; }

        public FileSystemItemType ItemType { get; set; }

        public string ImageKey { get; set; }
    }
}
