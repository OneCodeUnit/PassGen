namespace PassGen
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ButtonGen = new Button();
            FieldPassword = new TextBox();
            CheckBoxCapital = new CheckBox();
            CheckBoxLower = new CheckBox();
            CheckBoxNumber = new CheckBox();
            CheckBoxSpace = new CheckBox();
            CheckBoxSpecial = new CheckBox();
            CheckBoxLine = new CheckBox();
            ButtonCopy = new Button();
            SizeBar = new TrackBar();
            LabelScrollValue = new Label();
            ((System.ComponentModel.ISupportInitialize)SizeBar).BeginInit();
            SuspendLayout();
            // 
            // ButtonGen
            // 
            ButtonGen.Location = new Point(13, 55);
            ButtonGen.Margin = new Padding(4);
            ButtonGen.Name = "ButtonGen";
            ButtonGen.Size = new Size(355, 42);
            ButtonGen.TabIndex = 0;
            ButtonGen.Text = "Новый пароль";
            ButtonGen.UseVisualStyleBackColor = true;
            ButtonGen.Click += ButtonGen_Click;
            // 
            // FieldPassword
            // 
            FieldPassword.Location = new Point(13, 13);
            FieldPassword.Margin = new Padding(4);
            FieldPassword.Name = "FieldPassword";
            FieldPassword.ReadOnly = true;
            FieldPassword.Size = new Size(313, 34);
            FieldPassword.TabIndex = 2;
            // 
            // CheckBoxCapital
            // 
            CheckBoxCapital.AutoSize = true;
            CheckBoxCapital.Checked = true;
            CheckBoxCapital.CheckState = CheckState.Checked;
            CheckBoxCapital.Location = new Point(13, 197);
            CheckBoxCapital.Margin = new Padding(4);
            CheckBoxCapital.Name = "CheckBoxCapital";
            CheckBoxCapital.Size = new Size(131, 32);
            CheckBoxCapital.TabIndex = 3;
            CheckBoxCapital.Text = "Заглавные";
            CheckBoxCapital.UseVisualStyleBackColor = true;
            CheckBoxCapital.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // CheckBoxLower
            // 
            CheckBoxLower.AutoSize = true;
            CheckBoxLower.Checked = true;
            CheckBoxLower.CheckState = CheckState.Checked;
            CheckBoxLower.Location = new Point(13, 237);
            CheckBoxLower.Margin = new Padding(4);
            CheckBoxLower.Name = "CheckBoxLower";
            CheckBoxLower.Size = new Size(125, 32);
            CheckBoxLower.TabIndex = 4;
            CheckBoxLower.Text = "Строчные";
            CheckBoxLower.UseVisualStyleBackColor = true;
            CheckBoxLower.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // CheckBoxNumber
            // 
            CheckBoxNumber.AutoSize = true;
            CheckBoxNumber.Checked = true;
            CheckBoxNumber.CheckState = CheckState.Checked;
            CheckBoxNumber.Location = new Point(13, 277);
            CheckBoxNumber.Margin = new Padding(4);
            CheckBoxNumber.Name = "CheckBoxNumber";
            CheckBoxNumber.Size = new Size(101, 32);
            CheckBoxNumber.TabIndex = 5;
            CheckBoxNumber.Text = "Цифры";
            CheckBoxNumber.UseVisualStyleBackColor = true;
            CheckBoxNumber.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // CheckBoxSpace
            // 
            CheckBoxSpace.AutoSize = true;
            CheckBoxSpace.Location = new Point(187, 277);
            CheckBoxSpace.Margin = new Padding(4);
            CheckBoxSpace.Name = "CheckBoxSpace";
            CheckBoxSpace.Size = new Size(105, 32);
            CheckBoxSpace.TabIndex = 8;
            CheckBoxSpace.Text = "Пробел";
            CheckBoxSpace.UseVisualStyleBackColor = true;
            CheckBoxSpace.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // CheckBoxSpecial
            // 
            CheckBoxSpecial.AutoSize = true;
            CheckBoxSpecial.Location = new Point(187, 237);
            CheckBoxSpecial.Margin = new Padding(4);
            CheckBoxSpecial.Name = "CheckBoxSpecial";
            CheckBoxSpecial.Size = new Size(120, 32);
            CheckBoxSpecial.TabIndex = 7;
            CheckBoxSpecial.Text = "Символы";
            CheckBoxSpecial.UseVisualStyleBackColor = true;
            CheckBoxSpecial.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // CheckBoxLine
            // 
            CheckBoxLine.AutoSize = true;
            CheckBoxLine.Location = new Point(187, 197);
            CheckBoxLine.Margin = new Padding(4);
            CheckBoxLine.Name = "CheckBoxLine";
            CheckBoxLine.Size = new Size(181, 32);
            CheckBoxLine.TabIndex = 6;
            CheckBoxLine.Text = "Подчёркивание";
            CheckBoxLine.UseVisualStyleBackColor = true;
            CheckBoxLine.CheckedChanged += CheckBox_CheckedChanged;
            // 
            // ButtonCopy
            // 
            ButtonCopy.BackgroundImage = Properties.Resources.Copy;
            ButtonCopy.BackgroundImageLayout = ImageLayout.Stretch;
            ButtonCopy.Location = new Point(334, 13);
            ButtonCopy.Margin = new Padding(4);
            ButtonCopy.Name = "ButtonCopy";
            ButtonCopy.Size = new Size(34, 34);
            ButtonCopy.TabIndex = 9;
            ButtonCopy.UseVisualStyleBackColor = true;
            ButtonCopy.Click += ButtonCopy_Click;
            // 
            // SizeBar
            // 
            SizeBar.Location = new Point(13, 133);
            SizeBar.Margin = new Padding(4);
            SizeBar.Maximum = 30;
            SizeBar.Minimum = 3;
            SizeBar.Name = "SizeBar";
            SizeBar.Size = new Size(355, 56);
            SizeBar.TabIndex = 10;
            SizeBar.Value = 12;
            SizeBar.Scroll += SizeBar_Scroll;
            // 
            // LabelScrollValue
            // 
            LabelScrollValue.AutoSize = true;
            LabelScrollValue.Location = new Point(13, 101);
            LabelScrollValue.Margin = new Padding(4, 0, 4, 0);
            LabelScrollValue.Name = "LabelScrollValue";
            LabelScrollValue.Size = new Size(174, 28);
            LabelScrollValue.TabIndex = 11;
            LabelScrollValue.Text = "Длина пароля: 12";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(381, 323);
            Controls.Add(LabelScrollValue);
            Controls.Add(SizeBar);
            Controls.Add(ButtonCopy);
            Controls.Add(CheckBoxSpace);
            Controls.Add(CheckBoxSpecial);
            Controls.Add(CheckBoxLine);
            Controls.Add(CheckBoxNumber);
            Controls.Add(CheckBoxLower);
            Controls.Add(CheckBoxCapital);
            Controls.Add(FieldPassword);
            Controls.Add(ButtonGen);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Создай пароль!";
            ((System.ComponentModel.ISupportInitialize)SizeBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ButtonGen;
        private TextBox FieldPassword;
        private CheckBox CheckBoxCapital;
        private CheckBox CheckBoxLower;
        private CheckBox CheckBoxNumber;
        private CheckBox CheckBoxSpace;
        private CheckBox CheckBoxSpecial;
        private CheckBox CheckBoxLine;
        private Button ButtonCopy;
        private TrackBar SizeBar;
        private Label LabelScrollValue;
    }
}