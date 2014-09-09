using System;
using FileSearch.Common;

namespace FileSearch.XmlSearch
{
    public class XmlFilterPlugin : ISearchPlugin
    {
        #region ISearchPlugin Members

        public PluginInfo Info
        {
            get
            {
                return new PluginInfo
                {
                    Description = "Searches xml files with specified node value",
                    Name = "Xml search",
                    FileExtension = "xml"
                };
            }
        }

        public void BuildSearchOptionsGui(IGuiBuilder guiBuilder)
        {
            guiBuilder.AddTextField("name", "Attribute name", string.Empty);
            guiBuilder.AddTextField("value", "Attribute value", string.Empty);
        }

        public IFileFilter CreateFileFilter(System.Collections.Generic.Dictionary<string, object> searchOptions)
        {
            string name = (string) searchOptions["name"];
            string value = (string) searchOptions["value"];
            return new XmlFilter(value, name);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}