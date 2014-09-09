using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FileSearch.Model
{
    public interface IFileSystem
    {
        bool IsDirectoryExists(string directoryPath);

        IEnumerable<string> LoadDirectoryFiles(string directoryPath, string filter);

        IEnumerable<string> LoadChildDirectories(string directoryPath);

        Icon GetAssociatedIcon(string filePath, bool isDirectory);
        
        FileInfo GetFileInfo(string fullPath);
    }
}