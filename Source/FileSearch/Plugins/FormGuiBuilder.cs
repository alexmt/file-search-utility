using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FileSearch.Common;

namespace FileSearch.Plugins
{
    public class FormGuiBuilder : MarshalByRefObject, IGuiBuilder
    {
        #region Private Fields

        private readonly TableLayoutPanel tablePanel;
        private readonly Dictionary<string, Func<object>> valuesSourcesByName = new Dictionary<string, Func<object>>();

        #endregion

        #region Constructor

        public FormGuiBuilder(TableLayoutPanel tablePanel)
        {
            this.tablePanel = tablePanel;
        }

        #endregion

        #region Private Methods

        private void AddToCurrentRow(Control control, int column)
        {
            tablePanel.Controls.Add(control);
            tablePanel.SetRow(control, tablePanel.RowCount);
            tablePanel.SetColumn(control, column);
        }

        private void AddNewRow()
        {
            tablePanel.RowCount++;
        }

        #endregion

        #region IGuiBuilder Members

        public void AddTextField(string name, string caption, string value)
        {
            AddNewRow();
            AddToCurrentRow(new Label
            {
                Text = string.Format("{0}:", caption)
            }, 0);

            var textBox = new TextBox
            {
                Dock =  DockStyle.Fill
            };
            valuesSourcesByName[name] = () => textBox.Text;
            AddToCurrentRow(textBox, 1);
        }

        #endregion

        #region Public Methods

        public Dictionary<string, object > GetEnteredValues()
        {
            Dictionary<string , object> result = new Dictionary<string, object>();
            foreach (var item in valuesSourcesByName)
            {
                result[item.Key] = item.Value();
            }
            return result;
        }

        #endregion

    }
}