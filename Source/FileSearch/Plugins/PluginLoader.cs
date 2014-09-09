using System;
using System.Linq;
using System.Reflection;
using FileSearch.Common;

namespace FileSearch.Plugins
{
    public class PluginLoader : MarshalByRefObject
    {
        public ISearchPlugin[] LoadPlugins(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            var plugins =
                from type in assembly.GetTypes()
                where type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof (ISearchPlugin)) 
                let plugin = new SearchPluginProxy((ISearchPlugin) Activator.CreateInstance(type))
                select plugin;
            return plugins.ToArray();
        }
    }
}