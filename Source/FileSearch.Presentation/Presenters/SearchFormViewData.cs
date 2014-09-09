using System.Collections.Generic;
using System.Drawing;

namespace FileSearch.Presentation.Presenters
{
    public class SearchFormViewData
    {
        public SearchFormViewData()
        {
            DirectoryItems = new List<FileSystemItemViewData>();
            Images = new Dictionary<string, Icon>();
        }

        public string FullPath { get; set; }

        public IEnumerable<FileSystemItemViewData> DirectoryItems { get; set; }

        public Dictionary<string, Icon> Images { get; set; }
    }
}