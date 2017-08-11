namespace DoomLauncher
{
    using PresentationControls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class SearchControl : UserControl
    {
        private CheckBoxComboBox cmbFilter;
        private IContainer components;
        private string[] m_items = new string[0];
        private TextBox txtSearch;

        [field: CompilerGenerated]
        public event EventHandler SearchTextChanged;

        public SearchControl()
        {
            this.InitializeComponent();
            this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool GetSearchFilter(string item) => 
            this.cmbFilter.CheckBoxItems[item].Checked;

        public string[] GetSearchFilters() => 
            this.m_items.ToArray<string>();

        public string[] GetSelectedSearchFilters() => 
            (from item in this.m_items
                where this.cmbFilter.CheckBoxItems[item].Checked
                select item).ToArray<string>();

        private void InitializeComponent()
        {
            CheckBoxProperties properties = new CheckBoxProperties();
            this.cmbFilter = new CheckBoxComboBox();
            this.txtSearch = new TextBox();
            base.SuspendLayout();
            this.cmbFilter.BackColor = SystemColors.InfoText;
            properties.ForeColor = SystemColors.ControlText;
            this.cmbFilter.CheckBoxProperties = properties;
            this.cmbFilter.DisplayMemberSingleItem = "";
            this.cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFilter.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new Point(0, 0);
            this.cmbFilter.Margin = new Padding(0);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new Size(0x89, 20);
            this.cmbFilter.TabIndex = 0;
            this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearch.Location = new Point(0, 0);
            this.txtSearch.Margin = new Padding(0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(120, 20);
            this.txtSearch.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.txtSearch);
            base.Controls.Add(this.cmbFilter);
            base.Name = "SearchControl";
            base.Size = new Size(140, 0x15);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SetSearchFilter(string item, bool check)
        {
            this.cmbFilter.CheckBoxItems[item].Checked = check;
        }

        public void SetSearchFilters(IEnumerable<string> items)
        {
            this.cmbFilter.Items.Clear();
            this.m_items = items.ToArray<string>();
            foreach (string str in items)
            {
                this.cmbFilter.Items.Add(str);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (this.SearchTextChanged != null)
            {
                this.SearchTextChanged(this, e);
            }
        }

        public string SearchText
        {
            get => 
                this.txtSearch.Text;
            set
            {
                this.txtSearch.Text = value;
            }
        }
    }
}

