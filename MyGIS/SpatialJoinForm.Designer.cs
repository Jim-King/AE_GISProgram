namespace MyGIS
{
    partial class SpatialJoinForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbOverlay = new System.Windows.Forms.ComboBox();
            this.txtOutFilePath = new System.Windows.Forms.TextBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnOverlay = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOverlay = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "input layer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "overlay layer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "overlay method";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Location = new System.Drawing.Point(12, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 116);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "message";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "output path";
            // 
            // cmbOverlay
            // 
            this.cmbOverlay.FormattingEnabled = true;
            this.cmbOverlay.Location = new System.Drawing.Point(108, 110);
            this.cmbOverlay.Name = "cmbOverlay";
            this.cmbOverlay.Size = new System.Drawing.Size(157, 20);
            this.cmbOverlay.TabIndex = 5;
            // 
            // txtOutFilePath
            // 
            this.txtOutFilePath.Location = new System.Drawing.Point(108, 150);
            this.txtOutFilePath.Name = "txtOutFilePath";
            this.txtOutFilePath.ReadOnly = true;
            this.txtOutFilePath.Size = new System.Drawing.Size(157, 21);
            this.txtOutFilePath.TabIndex = 6;
            // 
            // btnOutput
            // 
            this.btnOutput.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOutput.Location = new System.Drawing.Point(276, 150);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(53, 23);
            this.btnOutput.TabIndex = 7;
            this.btnOutput.Text = "...";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnOverlay
            // 
            this.btnOverlay.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOverlay.Location = new System.Drawing.Point(276, 68);
            this.btnOverlay.Name = "btnOverlay";
            this.btnOverlay.Size = new System.Drawing.Size(53, 23);
            this.btnOverlay.TabIndex = 8;
            this.btnOverlay.Text = "...";
            this.btnOverlay.UseVisualStyleBackColor = true;
            this.btnOverlay.Click += new System.EventHandler(this.btnOverlay_Click);
            // 
            // btnInput
            // 
            this.btnInput.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInput.Location = new System.Drawing.Point(276, 28);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(53, 23);
            this.btnInput.TabIndex = 9;
            this.btnInput.Text = "...";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(108, 30);
            this.txtInput.Name = "txtInput";
            this.txtInput.ReadOnly = true;
            this.txtInput.Size = new System.Drawing.Size(157, 21);
            this.txtInput.TabIndex = 10;
            // 
            // txtOverlay
            // 
            this.txtOverlay.Location = new System.Drawing.Point(108, 70);
            this.txtOverlay.Name = "txtOverlay";
            this.txtOverlay.ReadOnly = true;
            this.txtOverlay.Size = new System.Drawing.Size(157, 21);
            this.txtOverlay.TabIndex = 11;
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(3, 17);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(322, 21);
            this.txtMessage.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(64, 191);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 33);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 33);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SpatialJoinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 358);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtOverlay);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.btnOverlay);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.txtOutFilePath);
            this.Controls.Add(this.cmbOverlay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SpatialJoinForm";
            this.Text = "SpatialJoinForm";
            this.Load += new System.EventHandler(this.SpatialJoinForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbOverlay;
        private System.Windows.Forms.TextBox txtOutFilePath;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnOverlay;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOverlay;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}