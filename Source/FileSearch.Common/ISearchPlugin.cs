using System;
using System.Collections.Generic;

namespace FileSearch.Common
{
    public interface ISearchPlugin : IDisposable
    {
        PluginInfo Info { get; }

        void BuildSearchOptionsGui(IGuiBuilder guiBuilder);

        IFileFilter CreateFileFilter(Dictionary<string, object> searchOptions);
    }
}