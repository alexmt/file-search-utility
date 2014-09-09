using System;
using System.Windows.Forms;
using FileSearch.Common;
using FileSearch.Model;
using FileSearch.Plugins;
using FileSearch.Presentation.Presenters;

namespace FileSearch.Presentation.Views
{
    public partial class SearchOptionsForm : Form
    {
        #region Private Fields

        private FormGuiBuilder guiBuilder;

        #endregion


        #region Private Methods

        private void InitializeEvents()
        {
            pluginsComboBox.SelectedIndexChanged += PluginsComboBox_SelectedIndexChanged;
            okButton.Click += OkButton_Click;
        }

        private ISearchPlugin GetSearchPlugin()
        {
            var item = (pluginsComboBox.SelectedItem as ListItem);
            return item.Value as ISearchPlugin;
        }

        private void ClearPluginGui()
        {
            tableLayoutPanel.Controls.Clear();
        }

        #endregion

        #region Constructor

        public SearchOptionsForm()
        {
            InitializeComponent();
            pluginsComboBox.DisplayMember = "Name";
            comboBoxMaxSize.DisplayMember = "Name";
            okButton.Enabled = false;
            InitializeEvents();
        }

        #endregion

        #region Event Handlers

        private void OkButton_Click(object sender, EventArgs e)
        {
            if(guiBuilder != null && SearchOptionsAccepted != null)
            {
                long? maxFileSize = (long?) (comboBoxMaxSize.SelectedItem as ListItem).Value;
                var searchPlugin = GetSearchPlugin();
                SearchOptionsAccepted(new SearchOptions
                {
                    FileFilter = searchPlugin.CreateFileFilter(guiBuilder.GetEnteredValues()),
                    IsIncludeSubDirectories = checkBoxInludeSubDirectory.Checked,
                    MaxFileSize = maxFileSize,
                    SearchPattern = string.Format("*.{0}", searchPlugin.Info.FileExtension)
                });
            }
        }

        private void PluginsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pluginsComboBox.SelectedIndex >= 0)
            {
                ClearPluginGui();
                guiBuilder = new FormGuiBuilder(tableLayoutPanel);
                GetSearchPlugin().BuildSearchOptionsGui(guiBuilder);
                okButton.Enabled = true;
            }
            else
            {
                okButton.Enabled = false;
            }
        }

        #endregion

        #region Public Members

        public event Action<SearchOptions> SearchOptionsAccepted;

        public void FillSizeList(ListItem[] items)
        {
            comboBoxMaxSize.Items.Clear();
            comboBoxMaxSize.Items.AddRange(items);
            if(comboBoxMaxSize.Items.Count > 0)
            {
                comboBoxMaxSize.SelectedIndex = 0;
            }
        }

        public void FillPluginsList(ListItem[] items, bool clearList)
        {
            if (clearList)
            {
                pluginsComboBox.Items.Clear();
                ClearPluginGui();
            }
            pluginsComboBox.Items.AddRange(items);
            pluginsComboBox.Visible = pluginsComboBox.Items.Count > 0;
            labelNoPlugins.Visible = !pluginsComboBox.Visible;
            okButton.Enabled = pluginsComboBox.Items.Count > 0;
            if(pluginsComboBox.Items.Count > 0)
            {
                pluginsComboBox.SelectedIndex = 0;
            }
        }

        #endregion
    }
}
