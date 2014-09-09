using System;
using System.Windows.Forms;
using FileSearch.IO;
using FileSearch.Plugins;
using FileSearch.Presentation.Presenters;
using FileSearch.Presentation.Views;

namespace FileSearch.Presentation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var fileSearchForm = new FileSearchForm();
            var pluginSource = new DirectoryFileSource("Plugins", "*.dll");
            var pluginManager = new PluginManager(pluginSource);
            var fileSearchPresenter = new FileSearchPresenter(new FileSystem(), pluginManager, fileSearchForm);
            fileSearchForm.Presenter = fileSearchPresenter;

            Application.Run(fileSearchForm);
        }
    }
}
