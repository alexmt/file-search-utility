using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSearch.Common;
using FileSearch.IO;

namespace FileSearch.Plugins
{
    public class PluginsHost : IDisposable
    {
        #region Private Fields

        private AppDomain domain;
        private readonly List<string> assemblies;
        private Dictionary<string, PluginsAssemblyInfo> assemblyByFilePathCache;

        #endregion

        #region Constructor

        public PluginsHost(string[] assemblies)
        {
            this.assemblies = new List<string>(assemblies);
        }

        #endregion

        #region Private Methods

        private ISearchPlugin[] GetAssemblyPlugins(string assemblyPath)
        {
            PluginsAssemblyInfo info;
            if (assemblyByFilePathCache.TryGetValue(assemblyPath, out info))
            {
                return info.Plugins;
            }
            return new ISearchPlugin[0];
        }


        private bool TryGetAssemblyInfo(string filePath, out PluginsAssemblyInfo info)
        {
            info = null;
            return assemblyByFilePathCache != null && assemblyByFilePathCache.TryGetValue(filePath, out info);
        }

        private void EnsurePluginsLoaded()
        {
            if (assemblyByFilePathCache == null || domain == null)
            {
                domain = AppDomain.CreateDomain("Plugins application domain");
                assemblyByFilePathCache = new Dictionary<string, PluginsAssemblyInfo>();
                LoadNewAssemlies(assemblies);
            }
        }

        private void LoadNewAssemlies(IEnumerable<string> newAssemblies)
        {
            Type type = typeof(PluginLoader);
            // ReSharper disable AssignNullToNotNullAttribute
            PluginLoader loader = (PluginLoader)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
            // ReSharper restore AssignNullToNotNullAttribute
            foreach (string assembly in newAssemblies)
            {
                var tempFile = new TempFile(assembly);
                assemblyByFilePathCache[assembly] = new PluginsAssemblyInfo(tempFile, loader.LoadPlugins(tempFile.FilePath));
            }
        }

        #endregion

        #region Public Methods

        public void NotifyAssembliesUpdated(FileSourceUpdatedArgs updatedArgs, out ISearchPlugin[] newPlugins, out bool needReloadPlugins)
        {
            needReloadPlugins = false;
            newPlugins = new ISearchPlugin[0];
            PluginsAssemblyInfo info;
            switch (updatedArgs.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    EnsurePluginsLoaded();
                    LoadNewAssemlies(new[] {updatedArgs.FilePath});
                    newPlugins = GetAssemblyPlugins(updatedArgs.FilePath);
                    break;
                case WatcherChangeTypes.Deleted:
                    if(TryGetAssemblyInfo(updatedArgs.FilePath, out info))
                    {
                        if(info.Plugins.Length > 0)
                            needReloadPlugins = true;
                        else
                            assemblyByFilePathCache.Remove(updatedArgs.FilePath);
                    }
                    assemblies.Remove(updatedArgs.FilePath);
                    break;
                case WatcherChangeTypes.Changed:
                    needReloadPlugins = true;
                    break;
                case WatcherChangeTypes.Renamed:
                    if(TryGetAssemblyInfo(updatedArgs.FilePath, out info))
                    {
                        assemblyByFilePathCache.Remove(updatedArgs.FilePath);
                        assemblyByFilePathCache[updatedArgs.NewFilePath] = info;
                    }
                    else
                    {
                        LoadNewAssemlies(new[] { updatedArgs.NewFilePath });
                        newPlugins = GetAssemblyPlugins(updatedArgs.NewFilePath);
                    }
                    break;
            }
        }

        public ISearchPlugin[] GetLoadedPlugins()
        {
            EnsurePluginsLoaded();
            return assemblyByFilePathCache.Values.SelectMany(info => info.Plugins).ToArray();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var pluginAssembly in assemblyByFilePathCache.Values)
            {
                pluginAssembly.DisposePlugins();
            }
            if (domain != null)
            {
                AppDomain.Unload(domain);
                domain = null;
            }
            foreach (var pluginAssembly in assemblyByFilePathCache.Values)
            {
                pluginAssembly.TempFile.Dispose();
            }
            assemblyByFilePathCache = null;
        }

        #endregion
    }
}