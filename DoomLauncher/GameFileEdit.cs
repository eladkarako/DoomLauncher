namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public class GameFileEdit : UserControl
    {
        private CheckBox chkAuthor;
        private CheckBox chkComments;
        private CheckBox chkDescription;
        private CheckBox chkRating;
        private CheckBox chkReleaseDate;
        private CheckBox chkTitle;
        private IContainer components;
        private RatingControl ctrlStarRating;
        private DateTimePicker dtRelease;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label lblTags;
        private bool m_showCheckBoxes;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox txtAuthor;
        private TextBox txtComments;
        private TextBox txtDescription;
        private TextBox txtTitle;

        public GameFileEdit()
        {
            this.InitializeComponent();
        }

        private bool AssertSet(CheckBox chk, List<GameFileFieldType> fields, GameFileFieldType field)
        {
            if ((!this.m_showCheckBoxes || !chk.Checked) && this.m_showCheckBoxes)
            {
                return false;
            }
            fields.Add(field);
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.lblTags = new Label();
            this.txtAuthor = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.txtTitle = new TextBox();
            this.txtDescription = new TextBox();
            this.txtComments = new TextBox();
            this.dtRelease = new DateTimePicker();
            this.ctrlStarRating = new RatingControl();
            this.label7 = new Label();
            this.chkTitle = new CheckBox();
            this.chkAuthor = new CheckBox();
            this.chkReleaseDate = new CheckBox();
            this.chkRating = new CheckBox();
            this.chkDescription = new CheckBox();
            this.chkComments = new CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.lblTags, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtAuthor, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtTitle, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtComments, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.dtRelease, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctrlStarRating, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkAuthor, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkReleaseDate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkRating, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkDescription, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkComments, 1, 6);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 160f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 160f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
            this.tableLayoutPanel1.Size = new Size(0x15d, 0x1f6);
            this.tableLayoutPanel1.TabIndex = 0;
            this.lblTags.Anchor = AnchorStyles.Left;
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new Point(0x8f, 0x71);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new Size(0x1f, 13);
            this.lblTags.TabIndex = 13;
            this.lblTags.Text = "Tags";
            this.txtAuthor.Dock = DockStyle.Fill;
            this.txtAuthor.Location = new Point(0x8f, 0x1b);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new Size(0xcb, 20);
            this.txtAuthor.TabIndex = 7;
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1b, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 0x1d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x26, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Author";
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 0x35);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Release Date";
            this.label4.Anchor = AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(3, 0x53);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x26, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Rating";
            this.label5.Anchor = AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 0xcd);
            this.label5.Name = "label5";
            this.label5.Size = new Size(60, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description";
            this.label6.Anchor = AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(3, 0x16d);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Comments";
            this.txtTitle.Dock = DockStyle.Fill;
            this.txtTitle.Location = new Point(0x8f, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new Size(0xcb, 20);
            this.txtTitle.TabIndex = 6;
            this.txtDescription.Dock = DockStyle.Fill;
            this.txtDescription.Location = new Point(0x8f, 0x87);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new Size(0xcb, 0x9a);
            this.txtDescription.TabIndex = 8;
            this.txtComments.Dock = DockStyle.Fill;
            this.txtComments.Location = new Point(0x8f, 0x127);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new Size(0xcb, 0x9a);
            this.txtComments.TabIndex = 9;
            this.dtRelease.Format = DateTimePickerFormat.Custom;
            this.dtRelease.Location = new Point(0x8f, 0x33);
            this.dtRelease.Name = "dtRelease";
            this.dtRelease.ShowCheckBox = true;
            this.dtRelease.Size = new Size(200, 20);
            this.dtRelease.TabIndex = 10;
            this.ctrlStarRating.Anchor = AnchorStyles.Left;
            this.ctrlStarRating.Location = new Point(0x8f, 80);
            this.ctrlStarRating.Name = "ctrlStarRating";
            this.ctrlStarRating.SelectedRating = 0;
            this.ctrlStarRating.Size = new Size(0x69, 20);
            this.ctrlStarRating.TabIndex = 11;
            this.label7.Anchor = AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(3, 0x71);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x1f, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tags";
            this.chkTitle.Anchor = AnchorStyles.Top;
            this.chkTitle.AutoSize = true;
            this.chkTitle.Checked = true;
            this.chkTitle.CheckState = CheckState.Checked;
            this.chkTitle.Location = new Point(0x7b, 6);
            this.chkTitle.Margin = new Padding(3, 6, 3, 3);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new Size(14, 15);
            this.chkTitle.TabIndex = 14;
            this.chkTitle.Text = "checkBox1";
            this.chkTitle.UseVisualStyleBackColor = true;
            this.chkAuthor.Anchor = AnchorStyles.Top;
            this.chkAuthor.AutoSize = true;
            this.chkAuthor.Checked = true;
            this.chkAuthor.CheckState = CheckState.Checked;
            this.chkAuthor.Location = new Point(0x7b, 30);
            this.chkAuthor.Margin = new Padding(3, 6, 3, 3);
            this.chkAuthor.Name = "chkAuthor";
            this.chkAuthor.Size = new Size(14, 15);
            this.chkAuthor.TabIndex = 15;
            this.chkAuthor.Text = "checkBox2";
            this.chkAuthor.UseVisualStyleBackColor = true;
            this.chkReleaseDate.Anchor = AnchorStyles.Top;
            this.chkReleaseDate.AutoSize = true;
            this.chkReleaseDate.Checked = true;
            this.chkReleaseDate.CheckState = CheckState.Checked;
            this.chkReleaseDate.Location = new Point(0x7b, 0x36);
            this.chkReleaseDate.Margin = new Padding(3, 6, 3, 3);
            this.chkReleaseDate.Name = "chkReleaseDate";
            this.chkReleaseDate.Size = new Size(14, 15);
            this.chkReleaseDate.TabIndex = 0x10;
            this.chkReleaseDate.Text = "checkBox3";
            this.chkReleaseDate.UseVisualStyleBackColor = true;
            this.chkRating.Anchor = AnchorStyles.Top;
            this.chkRating.AutoSize = true;
            this.chkRating.Checked = true;
            this.chkRating.CheckState = CheckState.Checked;
            this.chkRating.Location = new Point(0x7b, 0x4e);
            this.chkRating.Margin = new Padding(3, 6, 3, 3);
            this.chkRating.Name = "chkRating";
            this.chkRating.Size = new Size(14, 0x11);
            this.chkRating.TabIndex = 0x11;
            this.chkRating.Text = "checkBox4";
            this.chkRating.UseVisualStyleBackColor = true;
            this.chkDescription.Anchor = AnchorStyles.Top;
            this.chkDescription.AutoSize = true;
            this.chkDescription.Checked = true;
            this.chkDescription.CheckState = CheckState.Checked;
            this.chkDescription.Location = new Point(0x7b, 0x8a);
            this.chkDescription.Margin = new Padding(3, 6, 3, 3);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new Size(14, 0x11);
            this.chkDescription.TabIndex = 0x12;
            this.chkDescription.Text = "checkBox5";
            this.chkDescription.UseVisualStyleBackColor = true;
            this.chkComments.Anchor = AnchorStyles.Top;
            this.chkComments.AutoSize = true;
            this.chkComments.Checked = true;
            this.chkComments.CheckState = CheckState.Checked;
            this.chkComments.Location = new Point(0x7b, 0x12a);
            this.chkComments.Margin = new Padding(3, 6, 3, 3);
            this.chkComments.Name = "chkComments";
            this.chkComments.Size = new Size(14, 0x11);
            this.chkComments.TabIndex = 0x13;
            this.chkComments.Text = "chkComments";
            this.chkComments.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Name = "GameFileEdit";
            base.Size = new Size(0x15d, 0x1f6);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SetCheckBoxesChecked(bool set)
        {
            this.chkAuthor.Checked = this.chkDescription.Checked = this.chkRating.Checked = this.chkReleaseDate.Checked = this.chkTitle.Checked = this.chkComments.Checked = set;
        }

        public void SetDataSource(IGameFileDataSource ds, IEnumerable<ITagDataSource> tags)
        {
            if (!string.IsNullOrEmpty(ds.Title))
            {
                this.txtTitle.Text = ds.Title;
            }
            else
            {
                this.txtTitle.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(ds.Author))
            {
                this.txtAuthor.Text = ds.Author;
            }
            else
            {
                this.txtAuthor.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(ds.Description))
            {
                this.txtDescription.Text = ds.Description.Replace("\n", "\r\n");
            }
            else
            {
                this.txtDescription.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(ds.Comments))
            {
                this.txtComments.Text = ds.Comments;
            }
            else
            {
                this.txtComments.Text = string.Empty;
            }
            if (ds.ReleaseDate.HasValue)
            {
                this.dtRelease.Value = ds.ReleaseDate.Value;
            }
            else
            {
                this.dtRelease.Value = DateTime.Now;
            }
            if (ds.Rating.HasValue)
            {
                this.ctrlStarRating.SelectedRating = Convert.ToInt32(ds.Rating.Value);
            }
            else
            {
                this.ctrlStarRating.SelectedRating = 0;
            }
            this.dtRelease.Checked = ds.ReleaseDate.HasValue;
            StringBuilder sb = new StringBuilder();
            if (tags.Count<ITagDataSource>() > 0)
            {
                Array.ForEach<ITagDataSource>(tags.ToArray<ITagDataSource>(), x => sb.Append(x.Name + ", "));
                sb.Remove(sb.Length - 2, 2);
            }
            this.lblTags.Text = sb.ToString();
        }

        public void SetShowCheckBoxes(bool set)
        {
            this.chkAuthor.Visible = this.chkDescription.Visible = this.chkRating.Visible = this.chkReleaseDate.Visible = this.chkTitle.Visible = this.chkComments.Visible = set;
            this.m_showCheckBoxes = true;
        }

        public List<GameFileFieldType> UpdateDataSource(IGameFileDataSource ds)
        {
            List<GameFileFieldType> fields = new List<GameFileFieldType>();
            if (this.AssertSet(this.chkTitle, fields, GameFileFieldType.Title))
            {
                ds.Title = this.txtTitle.Text;
            }
            if (this.AssertSet(this.chkAuthor, fields, GameFileFieldType.Author))
            {
                ds.Author = this.txtAuthor.Text;
            }
            if (this.AssertSet(this.chkDescription, fields, GameFileFieldType.Description))
            {
                ds.Description = this.txtDescription.Text;
            }
            if (this.AssertSet(this.chkComments, fields, GameFileFieldType.Comments))
            {
                ds.Comments = this.txtComments.Text;
            }
            if (this.AssertSet(this.chkReleaseDate, fields, GameFileFieldType.ReleaseDate))
            {
                if (this.dtRelease.Checked)
                {
                    ds.ReleaseDate = new DateTime(this.dtRelease.Value.Year, this.dtRelease.Value.Month, this.dtRelease.Value.Day);
                }
                else
                {
                    ds.ReleaseDate = null;
                }
            }
            if (this.AssertSet(this.chkRating, fields, GameFileFieldType.Rating))
            {
                ds.Rating = new double?((double) this.ctrlStarRating.SelectedRating);
            }
            return fields;
        }

        public bool AuthorChecked
        {
            get => 
                this.chkAuthor.Checked;
            set
            {
                this.chkAuthor.Checked = value;
            }
        }

        public bool CommentsChecked
        {
            get => 
                this.chkComments.Checked;
            set
            {
                this.chkComments.Checked = value;
            }
        }

        public bool DescriptionChecked
        {
            get => 
                this.chkDescription.Checked;
            set
            {
                this.chkDescription.Checked = value;
            }
        }

        public bool RatingChecked
        {
            get => 
                this.chkRating.Checked;
            set
            {
                this.chkRating.Checked = value;
            }
        }

        public bool ReleaseDateChecked
        {
            get => 
                this.chkReleaseDate.Checked;
            set
            {
                this.chkReleaseDate.Checked = value;
            }
        }

        public bool TitleChecked
        {
            get => 
                this.chkTitle.Checked;
            set
            {
                this.chkTitle.Checked = value;
            }
        }
    }
}

