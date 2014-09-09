using System.Collections.Generic;
using FileSearch.Common;

namespace FileSearch.TextSearch
{
    public class TextSearchPlugin: ISearchPlugin
    {
        #region ISearchPlugin Members

        public PluginInfo Info
        {
            get
            {
                return new PluginInfo
                {
                    Description = "Searches files with specified text",
                    Name = "Text search",
                    FileExtension = "txt"
                };
            }
        }

        public void BuildSearchOptionsGui(IGuiBuilder guiBuilder)
        {
            guiBuilder.AddTextField("inputText", "Find what", string.Empty);
        }

        public IFileFilter CreateFileFilter(Dictionary<string, object> searchOptions)
        {
            var inputText = (string) searchOptions["inputText"];
            return new TextFileFilter(inputText);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
