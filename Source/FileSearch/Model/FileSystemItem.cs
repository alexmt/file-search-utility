namespace FileSearch.Model
{
    public class FileSystemItem
    {
        private readonly string fullPath;

        public FileSystemItem(string fullPath)
        {
            this.fullPath = fullPath;
        }

        public string FullPath
        {
            get { return fullPath; }
        }
    }
}