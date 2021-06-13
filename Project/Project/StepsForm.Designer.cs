namespace Project
{
    partial class StepsForm
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
            this.stepsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // stepsRichTextBox
            // 
            this.stepsRichTextBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsRichTextBox.Location = new System.Drawing.Point(25, 23);
            this.stepsRichTextBox.Name = "stepsRichTextBox";
            this.stepsRichTextBox.ReadOnly = true;
            this.stepsRichTextBox.Size = new System.Drawing.Size(353, 400);
            this.stepsRichTextBox.TabIndex = 0;
            this.stepsRichTextBox.Text = "";
            // 
            // StepsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 461);
            this.Controls.Add(this.stepsRichTextBox);
            this.Name = "StepsForm";
            this.Text = "Steps";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox stepsRichTextBox;
    }
}