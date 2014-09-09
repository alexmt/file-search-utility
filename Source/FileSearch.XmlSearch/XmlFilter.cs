using System;
using System.Xml;
using FileSearch.Common;

namespace FileSearch.XmlSearch
{
    public class XmlFilter : IFileFilter
    {
        private readonly string attributeName;
        private readonly string attributeValue;

        public XmlFilter(string attributeValue, string attributeName)
        {
            this.attributeValue = attributeValue;
            this.attributeName = attributeName;
        }

        #region IFileFilter Members

        public bool IsMatch(System.IO.FileInfo input, System.IO.Stream stream)
        {
            try
            {
                XmlReader reader = XmlReader.Create(stream);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.GetAttribute(attributeName) == attributeValue)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        #endregion
    }
}
