using System;

namespace FileSearch.Common
{
    [Serializable]
    public class PluginInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string FileExtension { get; set; }
    }
}
