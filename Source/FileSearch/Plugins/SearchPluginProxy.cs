using System;
using System.Collections.Generic;
using FileSearch.Common;

namespace FileSearch.Plugins
{
    public class SearchPluginProxy : MarshalByRefObject, ISearchPlugin
    {
        #region Private Fields

        private readonly ISearchPlugin plugin;

        #endregion

        #region Constructor

        public SearchPluginProxy(ISearchPlugin plugin)
        {
            this.plugin = plugin;
        }

        #endregion

        #region ISearchPlugin Members

        public void BuildSearchOptionsGui(IGuiBuilder guiBuilder)
        {
            plugin.BuildSearchOptionsGui(guiBuilder);
        }

        public IFileFilter CreateFileFilter(Dictionary<string, object> searchOptions)
        {
            return new FileFilterProxy(plugin.CreateFileFilter(searchOptions));
        }

        public PluginInfo Info
        {
            get { return plugin.Info; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            plugin.Dispose();
        }

        #endregion
    }
}
