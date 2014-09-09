using System.IO;

namespace FileSearch.Common
{
    public interface IGuiBuilder
    {
        void AddTextField(string name, string caption, string value);
    }

    public interface IFileFilter
    {
        bool IsMatch(FileInfo input, Stream stream);
    }
}