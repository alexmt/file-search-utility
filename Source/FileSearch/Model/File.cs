using System.IO;

namespace FileSearch.Model
{
    public class File : FileSystemItem
    {
        private readonly IFileSystem fileSystem;

        public File(string fullPath, IFileSystem fileSystem) : base(fullPath)
        {
            this.fileSystem = fileSystem;
        }

        public string Name
        {
            get { return Path.GetFileName(FullPath); }
        }

        public long Size
        {
            get
            {
                return fileSystem.GetFileInfo(FullPath).Length;
            }
        }
    }
}