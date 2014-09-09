using System.IO;
using FileSearch.Common;

namespace FileSearch.TextSearch
{
    public class TextFileFilter : IFileFilter
    {
        #region Private Fields

        private readonly string text;

        #endregion

        #region Constructor

        public TextFileFilter(string text)
        {
            this.text = text;
        }

        #endregion

        #region IFileFilter Members

        public bool IsMatch(FileInfo input, Stream stream)
        {
            return new StreamReader(stream).ReadToEnd().Contains(text);
        }

        #endregion
    }
}