namespace BingChillingApp
{
    partial class BingChillingMaze
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
            btnOpenFile = new Button();
            checkBoxBFS = new CheckBox();
            checkBoxDFS = new CheckBox();
            fileInputBox = new TextBox();
            FilenameLabel = new Label();
            pictureBox1 = new PictureBox();
            AlgorithmLabel = new Label();
            openMazeFile = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnOpenFile
            // 
            btnOpenFile.BackColor = SystemColors.ControlLight;
            btnOpenFile.Font = new Font("JetBrains Mono", 11.999999F, FontStyle.Regular, GraphicsUnit.Point);
            btnOpenFile.Location = new Point(161, 800);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(524, 101);
            btnOpenFile.TabIndex = 0;
            btnOpenFile.Text = "Visualize";
            btnOpenFile.UseVisualStyleBackColor = false;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // checkBoxBFS
            // 
            checkBoxBFS.AutoSize = true;
            checkBoxBFS.BackColor = Color.Transparent;
            checkBoxBFS.Font = new Font("JetBrains Mono Light", 11.999999F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxBFS.ForeColor = SystemColors.ButtonFace;
            checkBoxBFS.Location = new Point(161, 562);
            checkBoxBFS.Name = "checkBoxBFS";
            checkBoxBFS.Size = new Size(160, 67);
            checkBoxBFS.TabIndex = 1;
            checkBoxBFS.Text = "BFS";
            checkBoxBFS.UseVisualStyleBackColor = false;
            checkBoxBFS.CheckedChanged += checkBoxBFS_CheckedChanged;
            // 
            // checkBoxDFS
            // 
            checkBoxDFS.AutoSize = true;
            checkBoxDFS.BackColor = Color.Transparent;
            checkBoxDFS.Font = new Font("JetBrains Mono Light", 11.999999F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxDFS.ForeColor = SystemColors.ButtonFace;
            checkBoxDFS.Location = new Point(161, 674);
            checkBoxDFS.Name = "checkBoxDFS";
            checkBoxDFS.Size = new Size(160, 67);
            checkBoxDFS.TabIndex = 2;
            checkBoxDFS.Text = "DFS";
            checkBoxDFS.UseVisualStyleBackColor = false;
            checkBoxDFS.CheckedChanged += checkBoxDFS_CheckedChanged;
            // 
            // fileInputBox
            // 
            fileInputBox.BackColor = SystemColors.ControlLight;
            fileInputBox.Font = new Font("JetBrains Mono", 11.999999F, FontStyle.Regular, GraphicsUnit.Point);
            fileInputBox.Location = new Point(161, 300);
            fileInputBox.Name = "fileInputBox";
            fileInputBox.Size = new Size(524, 71);
            fileInputBox.TabIndex = 3;
            fileInputBox.Text = "\r\n\r\n\r\n";
            fileInputBox.TextChanged += textBox1_TextChanged;
            // 
            // FilenameLabel
            // 
            FilenameLabel.AutoSize = true;
            FilenameLabel.BackColor = Color.Transparent;
            FilenameLabel.Font = new Font("JetBrains Mono", 15.9999981F, FontStyle.Regular, GraphicsUnit.Point);
            FilenameLabel.ForeColor = SystemColors.ButtonFace;
            FilenameLabel.Location = new Point(161, 170);
            FilenameLabel.Margin = new Padding(4, 0, 4, 0);
            FilenameLabel.Name = "FilenameLabel";
            FilenameLabel.Size = new Size(339, 84);
            FilenameLabel.TabIndex = 4;
            FilenameLabel.Text = "Filename";
            FilenameLabel.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlLight;
            pictureBox1.Location = new Point(829, 170);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(720, 731);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // AlgorithmLabel
            // 
            AlgorithmLabel.AutoSize = true;
            AlgorithmLabel.BackColor = Color.Transparent;
            AlgorithmLabel.Font = new Font("JetBrains Mono", 15.9999981F, FontStyle.Regular, GraphicsUnit.Point);
            AlgorithmLabel.ForeColor = SystemColors.ButtonFace;
            AlgorithmLabel.Location = new Point(161, 424);
            AlgorithmLabel.Name = "AlgorithmLabel";
            AlgorithmLabel.Size = new Size(377, 84);
            AlgorithmLabel.TabIndex = 6;
            AlgorithmLabel.Text = "Algorithm";
            // 
            // openMazeFile
            // 
            openMazeFile.FileName = "openMazeFile";
            openMazeFile.Title = "Open Maze File";
            openMazeFile.FileOk += openMazeFile_FileOk;
            // 
            // BingChillingMaze
            // 
            AutoScaleDimensions = new SizeF(20F, 48F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SlateGray;
            ClientSize = new Size(1706, 1084);
            Controls.Add(AlgorithmLabel);
            Controls.Add(pictureBox1);
            Controls.Add(FilenameLabel);
            Controls.Add(fileInputBox);
            Controls.Add(checkBoxDFS);
            Controls.Add(checkBoxBFS);
            Controls.Add(btnOpenFile);
            Name = "BingChillingMaze";
            Text = "BingChilling";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOpenFile;
        private CheckBox checkBoxBFS;
        private CheckBox checkBoxDFS;
        private TextBox fileInputBox;
        private Label FilenameLabel;
        private PictureBox pictureBox1;
        private Label AlgorithmLabel;
        private OpenFileDialog openMazeFile;
    }
}