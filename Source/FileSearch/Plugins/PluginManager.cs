using System;
using FileSearch.Common;
using FileSearch.IO;

namespace FileSearch.Plugins
{
    public class PluginManager
    {
        #region Private Fields

        private readonly IFileSource fileSource;
        private PluginsHost pluginHost;

        #endregion

        #region Private Methods

        private TResult ExecuteSafe<TResult>(Func<TResult> func, TResult defaultResult)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                OnLoadError(e);
                return defaultResult;
            }
        }
        
            
        private void UpdateFileSource(FileSourceUpdatedArgs args)
        {
            lock (fileSource.SyncRoot)
            {
                ISearchPlugin[] newPlugins;
                bool needReloadPlugins;
                pluginHost.NotifyAssembliesUpdated(args, out newPlugins, out needReloadPlugins);
                if (newPlugins.Length > 0)
                {
                    OnNewPluginsLoaded(newPlugins);
                }
                if (needReloadPlugins)
                {
                    var newPluginHost = new PluginsHost(fileSource.GetSourceFiles());
                    OnPluginsReloaded(newPluginHost.GetLoadedPlugins());
                    pluginHost.Dispose();
                    pluginHost = newPluginHost;
                }
            }

        }

        private void OnFileSourceUpdated(FileSourceUpdatedArgs args)
        {
            ExecuteSafe<object>(() =>
            {
                UpdateFileSource(args);
                return null;
            }, null);
        }

        private void OnLoadError(Exception e)
        {
            if(LoadError != null)
            {
                LoadError(e);
            }
        }

        private void OnNewPluginsLoaded(ISearchPlugin[] newPlugins)
        {
            if(NewPluginsLoaded != null)
            {
                NewPluginsLoaded(newPlugins);
            }
        }

        private void OnPluginsReloaded(ISearchPlugin[] loadedPlugins)
        {
            if(PluginsReloaded != null)
            {
                PluginsReloaded(loadedPlugins);
            }
        }

        

        #endregion

        #region Constructor

        public PluginManager(IFileSource fileSource)
        {
            this.fileSource = fileSource;
            fileSource.FileSourceUpdated += OnFileSourceUpdated;
            lock (fileSource.SyncRoot)
            {
                pluginHost = new PluginsHost(fileSource.GetSourceFiles());
            }
        }

        #endregion

        #region Public Properties

        public event Action<ISearchPlugin[]> NewPluginsLoaded;
        public event Action<ISearchPlugin[]> PluginsReloaded;
        public event Action<Exception> LoadError;

        #endregion

        #region Publc Methods

        public ISearchPlugin[] GetSearchPlugins()
        {
            lock (fileSource.SyncRoot)
            {
                return ExecuteSafe(() => pluginHost.GetLoadedPlugins(), new ISearchPlugin[0]);
            }
        }

        #endregion
    }
}