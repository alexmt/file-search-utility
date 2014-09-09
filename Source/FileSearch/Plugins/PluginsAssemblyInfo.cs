using FileSearch.Common;
using FileSearch.IO;

namespace FileSearch.Plugins
{
    public class PluginsAssemblyInfo
    {
        #region Private Fields

        private readonly TempFile tempFile;
        private readonly ISearchPlugin[] plugins;

        #endregion

        #region Constructor

        public PluginsAssemblyInfo(TempFile tempFile, ISearchPlugin[] plugins)
        {
            this.tempFile = tempFile;
            this.plugins = plugins;
        }

        #endregion

        #region Public Properties

        public string FilePath
        {
            get { return tempFile.FilePath; }
        }

        public TempFile TempFile
        {
            get { return tempFile; }
        }

        public ISearchPlugin[] Plugins
        {
            get { return plugins; }
        }

        #endregion

        #region Public Methods

        public void DisposePlugins()
        {
            foreach (var plugin in plugins)
            {
                plugin.Dispose();
            }
        }

        #endregion
    }
}