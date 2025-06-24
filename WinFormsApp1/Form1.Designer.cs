namespace Autoclicker
{
    partial class UI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            input = new TextBox();
            input2 = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(170, 20);
            label1.Name = "label1";
            label1.Size = new Size(28, 15);
            label1.TabIndex = 1;
            label1.Text = "CPS";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(11, 20);
            label2.Name = "label2";
            label2.Size = new Size(71, 15);
            label2.TabIndex = 2;
            label2.Text = "Click Speed:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(11, 98);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 3;
            label3.Text = "Toggle Key:";
            // 
            // input
            // 
            input.AcceptsReturn = true;
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Location = new Point(110, 17);
            input.Name = "input";
            input.Size = new Size(54, 23);
            input.TabIndex = 4;
            input.Text = "0";
            input.TextAlign = HorizontalAlignment.Center;
            input.WordWrap = false;
            input.KeyPress += input_KeyPress;
            input.LostFocus += input_LostFocus;
            // 
            // input2
            // 
            input2.AcceptsReturn = true;
            input2.BorderStyle = BorderStyle.FixedSingle;
            input2.Location = new Point(110, 58);
            input2.Name = "input2";
            input2.Size = new Size(54, 23);
            input2.TabIndex = 8;
            input2.Text = "0";
            input2.TextAlign = HorizontalAlignment.Center;
            input2.WordWrap = false;
            input2.KeyPress += input2_KeyPress;
            input2.LostFocus += input2_LostFocus;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(11, 61);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 7;
            label4.Text = "Toggle Delay:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(170, 61);
            label5.Name = "label5";
            label5.Size = new Size(73, 15);
            label5.TabIndex = 6;
            label5.Text = "Milliseconds";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.BorderStyle = BorderStyle.FixedSingle;
            label6.FlatStyle = FlatStyle.Popup;
            label6.ForeColor = Color.White;
            label6.Location = new Point(110, 98);
            label6.Name = "label6";
            label6.Size = new Size(43, 17);
            label6.TabIndex = 9;
            label6.Text = "Period";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            label6.Click += label6_Click;
            // 
            // UI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(50, 50, 50);
            ClientSize = new Size(259, 136);
            Controls.Add(label6);
            Controls.Add(input2);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(input);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(275, 175);
            MinimumSize = new Size(275, 175);
            Name = "UI";
            StartPosition = FormStartPosition.WindowsDefaultBounds;
            Text = "NUT CLICKER 2";
            TopMost = true;
            KeyDown += UI_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox input;
        private TextBox input2;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
