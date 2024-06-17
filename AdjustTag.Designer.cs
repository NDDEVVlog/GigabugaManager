
namespace GiGaBuGaManager
{
    partial class AdjustTag
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.buttonTextColor = new System.Windows.Forms.Button();
            this.buttonBackgroundColor = new System.Windows.Forms.Button();
            this.buttonButtonColor = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTextColor
            // 
            this.buttonTextColor.Location = new System.Drawing.Point(84, 12);
            this.buttonTextColor.Name = "buttonTextColor";
            this.buttonTextColor.Size = new System.Drawing.Size(170, 65);
            this.buttonTextColor.TabIndex = 0;
            this.buttonTextColor.Text = "Text Color";
            this.buttonTextColor.UseVisualStyleBackColor = true;
            this.buttonTextColor.Click += new System.EventHandler(this.buttonTextColor_Click);
            // 
            // buttonBackgroundColor
            // 
            this.buttonBackgroundColor.Location = new System.Drawing.Point(84, 103);
            this.buttonBackgroundColor.Name = "buttonBackgroundColor";
            this.buttonBackgroundColor.Size = new System.Drawing.Size(170, 65);
            this.buttonBackgroundColor.TabIndex = 1;
            this.buttonBackgroundColor.Text = "Background Color";
            this.buttonBackgroundColor.UseVisualStyleBackColor = true;
            this.buttonBackgroundColor.Click += new System.EventHandler(this.buttonBackgroundColor_Click);
            // 
            // buttonButtonColor
            // 
            this.buttonButtonColor.Location = new System.Drawing.Point(84, 192);
            this.buttonButtonColor.Name = "buttonButtonColor";
            this.buttonButtonColor.Size = new System.Drawing.Size(170, 65);
            this.buttonButtonColor.TabIndex = 2;
            this.buttonButtonColor.Text = "OutLineColor";
            this.buttonButtonColor.UseVisualStyleBackColor = true;
            this.buttonButtonColor.Click += new System.EventHandler(this.buttonButtonColor_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(84, 296);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(170, 65);
            this.Save.TabIndex = 3;
            this.Save.Text = "OutLineColor";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // AdjustTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 450);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.buttonButtonColor);
            this.Controls.Add(this.buttonBackgroundColor);
            this.Controls.Add(this.buttonTextColor);
            this.Name = "AdjustTag";
            this.Text = "AdjustTag";
            this.Load += new System.EventHandler(this.AdjustTag_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button buttonTextColor;
        private System.Windows.Forms.Button buttonBackgroundColor;
        private System.Windows.Forms.Button buttonButtonColor;
        private System.Windows.Forms.Button Save;
    }
}