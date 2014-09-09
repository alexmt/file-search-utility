namespace FileSearch.Presentation.Views
{
    partial class SearchOptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label labelPlugin;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchOptionsForm));
            this.pluginsComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.labelNoPlugins = new System.Windows.Forms.Label();
            this.checkBoxInludeSubDirectory = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMaxSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            labelPlugin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlugin
            // 
            labelPlugin.AutoSize = true;
            labelPlugin.Location = new System.Drawing.Point(12, 15);
            labelPlugin.Name = "labelPlugin";
            labelPlugin.Size = new System.Drawing.Size(75, 13);
            labelPlugin.TabIndex = 1;
            labelPlugin.Text = "Search plugin:";
            // 
            // pluginsComboBox
            // 
            this.pluginsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pluginsComboBox.FormattingEnabled = true;
            this.pluginsComboBox.Location = new System.Drawing.Point(127, 12);
            this.pluginsComboBox.Name = "pluginsComboBox";
            this.pluginsComboBox.Size = new System.Drawing.Size(167, 21);
            this.pluginsComboBox.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(135, 159);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(216, 159);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // labelNoPlugins
            // 
            this.labelNoPlugins.AutoSize = true;
            this.labelNoPlugins.Location = new System.Drawing.Point(124, 15);
            this.labelNoPlugins.Name = "labelNoPlugins";
            this.labelNoPlugins.Size = new System.Drawing.Size(102, 13);
            this.labelNoPlugins.TabIndex = 5;
            this.labelNoPlugins.Text = "No plugins available";
            // 
            // checkBoxInludeSubDirectory
            // 
            this.checkBoxInludeSubDirectory.AutoSize = true;
            this.checkBoxInludeSubDirectory.Location = new System.Drawing.Point(127, 66);
            this.checkBoxInludeSubDirectory.Name = "checkBoxInludeSubDirectory";
            this.checkBoxInludeSubDirectory.Size = new System.Drawing.Size(15, 14);
            this.checkBoxInludeSubDirectory.TabIndex = 6;
            this.checkBoxInludeSubDirectory.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Maximum file size:";
            // 
            // comboBoxMaxSize
            // 
            this.comboBoxMaxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaxSize.FormattingEnabled = true;
            this.comboBoxMaxSize.Location = new System.Drawing.Point(127, 39);
            this.comboBoxMaxSize.Name = "comboBoxMaxSize";
            this.comboBoxMaxSize.Size = new System.Drawing.Size(167, 21);
            this.comboBoxMaxSize.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Include subdirectories:";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.86021F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.13979F));
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 86);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(285, 16);
            this.tableLayoutPanel.TabIndex = 10;
            // 
            // SearchOptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(303, 194);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxMaxSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxInludeSubDirectory);
            this.Controls.Add(this.pluginsComboBox);
            this.Controls.Add(this.labelNoPlugins);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(labelPlugin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox pluginsComboBox;
        private System.Windows.Forms.Label labelNoPlugins;
        private System.Windows.Forms.CheckBox checkBoxInludeSubDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMaxSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}