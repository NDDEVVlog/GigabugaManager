namespace GiGaBuGaManager
{
    partial class Gigabuga
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
            this.components = new System.ComponentModel.Container();
            this.TagFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.FileFormatSelection = new System.Windows.Forms.ComboBox();
            this.TagName = new System.Windows.Forms.TextBox();
            this.AddTagButton = new System.Windows.Forms.Button();
            this.ListItemView = new System.Windows.Forms.ListView();
            this.LargeIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.adjustProperties = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // TagFlow
            // 
            this.TagFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TagFlow.AutoScroll = true;
            this.TagFlow.Location = new System.Drawing.Point(13, 69);
            this.TagFlow.Name = "TagFlow";
            this.TagFlow.Size = new System.Drawing.Size(213, 406);
            this.TagFlow.TabIndex = 0;
            this.TagFlow.TabStop = true;
            // 
            // FileFormatSelection
            // 
            this.FileFormatSelection.FormattingEnabled = true;
            this.FileFormatSelection.Items.AddRange(new object[] {
            "None"});
            this.FileFormatSelection.Location = new System.Drawing.Point(246, 13);
            this.FileFormatSelection.Name = "FileFormatSelection";
            this.FileFormatSelection.Size = new System.Drawing.Size(121, 24);
            this.FileFormatSelection.TabIndex = 2;
            // 
            // TagName
            // 
            this.TagName.Location = new System.Drawing.Point(94, 13);
            this.TagName.Name = "TagName";
            this.TagName.Size = new System.Drawing.Size(131, 22);
            this.TagName.TabIndex = 3;
            this.TagName.Text = "TagName...";
            // 
            // AddTagButton
            // 
            this.AddTagButton.Location = new System.Drawing.Point(12, 12);
            this.AddTagButton.Name = "AddTagButton";
            this.AddTagButton.Size = new System.Drawing.Size(75, 23);
            this.AddTagButton.TabIndex = 4;
            this.AddTagButton.Text = "ADD";
            this.AddTagButton.UseVisualStyleBackColor = true;
            this.AddTagButton.Click += new System.EventHandler(this.AddTagButton_Click);
            // 
            // ListItemView
            // 
            this.ListItemView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListItemView.HideSelection = false;
            this.ListItemView.LargeImageList = this.LargeIconImageList;
            this.ListItemView.Location = new System.Drawing.Point(246, 51);
            this.ListItemView.Name = "ListItemView";
            this.ListItemView.Size = new System.Drawing.Size(582, 424);
            this.ListItemView.TabIndex = 5;
            this.ListItemView.UseCompatibleStateImageBehavior = false;
            this.ListItemView.View = System.Windows.Forms.View.Details;
            // 
            // LargeIconImageList
            // 
            this.LargeIconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.LargeIconImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.LargeIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Icon";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "File Name";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "File Path";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tag";
            // 
            // adjustProperties
            // 
            this.adjustProperties.AutoSize = true;
            this.adjustProperties.Location = new System.Drawing.Point(13, 42);
            this.adjustProperties.Name = "adjustProperties";
            this.adjustProperties.Size = new System.Drawing.Size(137, 21);
            this.adjustProperties.TabIndex = 6;
            this.adjustProperties.TabStop = true;
            this.adjustProperties.Text = "Adjust Properties";
            this.adjustProperties.UseVisualStyleBackColor = true;
            this.adjustProperties.CheckedChanged += new System.EventHandler(this.adjustProperties_CheckedChanged_1);
            // 
            // Gigabuga
            // 
            this.ClientSize = new System.Drawing.Size(840, 487);
            this.Controls.Add(this.adjustProperties);
            this.Controls.Add(this.ListItemView);
            this.Controls.Add(this.AddTagButton);
            this.Controls.Add(this.TagName);
            this.Controls.Add(this.FileFormatSelection);
            this.Controls.Add(this.TagFlow);
            this.Name = "Gigabuga";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel TagFlow;
        private System.Windows.Forms.ComboBox FileFormatSelection;
        private System.Windows.Forms.TextBox TagName;
        private System.Windows.Forms.Button AddTagButton;
        private System.Windows.Forms.ListView ListItemView;
        private System.Windows.Forms.ImageList LargeIconImageList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.RadioButton adjustProperties;
    }
}
