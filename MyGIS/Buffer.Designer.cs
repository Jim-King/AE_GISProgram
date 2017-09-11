namespace MyGIS
{
    partial class Buffer
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
            this.BufferLayerCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBufferDistance = new System.Windows.Forms.TextBox();
            this.BufferUnitCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateBuffer = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BufferLayerCombo
            // 
            this.BufferLayerCombo.FormattingEnabled = true;
            this.BufferLayerCombo.Location = new System.Drawing.Point(87, 52);
            this.BufferLayerCombo.Name = "BufferLayerCombo";
            this.BufferLayerCombo.Size = new System.Drawing.Size(118, 20);
            this.BufferLayerCombo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Layer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Distance";
            // 
            // txtBufferDistance
            // 
            this.txtBufferDistance.Location = new System.Drawing.Point(87, 109);
            this.txtBufferDistance.Name = "txtBufferDistance";
            this.txtBufferDistance.Size = new System.Drawing.Size(118, 21);
            this.txtBufferDistance.TabIndex = 3;
            // 
            // BufferUnitCombo
            // 
            this.BufferUnitCombo.FormattingEnabled = true;
            this.BufferUnitCombo.Items.AddRange(new object[] {
            "Meters",
            "Kilometers",
            "Miles"});
            this.BufferUnitCombo.Location = new System.Drawing.Point(87, 166);
            this.BufferUnitCombo.Name = "BufferUnitCombo";
            this.BufferUnitCombo.Size = new System.Drawing.Size(118, 20);
            this.BufferUnitCombo.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Unit";
            // 
            // btnCreateBuffer
            // 
            this.btnCreateBuffer.Location = new System.Drawing.Point(87, 266);
            this.btnCreateBuffer.Name = "btnCreateBuffer";
            this.btnCreateBuffer.Size = new System.Drawing.Size(105, 38);
            this.btnCreateBuffer.TabIndex = 6;
            this.btnCreateBuffer.Text = "OK";
            this.btnCreateBuffer.UseVisualStyleBackColor = true;
            this.btnCreateBuffer.Click += new System.EventHandler(this.btnCreateBuffer_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(87, 214);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(166, 21);
            this.txtOutputPath.TabIndex = 7;
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(12, 214);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(69, 21);
            this.btnFilePath.TabIndex = 8;
            this.btnFilePath.Text = "FilePath";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // Buffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 316);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.btnCreateBuffer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BufferUnitCombo);
            this.Controls.Add(this.txtBufferDistance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BufferLayerCombo);
            this.Name = "Buffer";
            this.Text = "Buffer";
            this.Load += new System.EventHandler(this.Buffer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox BufferLayerCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBufferDistance;
        private System.Windows.Forms.ComboBox BufferUnitCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateBuffer;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnFilePath;
    }
}