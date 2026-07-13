namespace ShittyInpainter
{
    partial class Form1
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnLoad = new Button();
            btnInpaint = new Button();
            btnSave = new Button();
            imagePanel = new Panel();
            pictureBox1 = new PictureBox();
            ofd = new OpenFileDialog();
            sfd = new SaveFileDialog();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(imagePanel, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 11.7777777F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 88.22222F));
            tableLayoutPanel1.Size = new Size(734, 511);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Controls.Add(btnLoad, 1, 0);
            tableLayoutPanel2.Controls.Add(btnInpaint, 2, 0);
            tableLayoutPanel2.Controls.Add(btnSave, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(728, 54);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Dock = DockStyle.Fill;
            btnLoad.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoad.Location = new Point(148, 3);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(139, 48);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnInpaint
            // 
            btnInpaint.Dock = DockStyle.Fill;
            btnInpaint.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnInpaint.Location = new Point(293, 3);
            btnInpaint.Name = "btnInpaint";
            btnInpaint.Size = new Size(139, 48);
            btnInpaint.TabIndex = 1;
            btnInpaint.Text = "Inpaint";
            btnInpaint.UseVisualStyleBackColor = true;
            btnInpaint.Click += btnInpaint_Click;
            // 
            // btnSave
            // 
            btnSave.Dock = DockStyle.Fill;
            btnSave.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(438, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(139, 48);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // imagePanel
            // 
            imagePanel.Controls.Add(pictureBox1);
            imagePanel.Dock = DockStyle.Fill;
            imagePanel.Location = new Point(3, 63);
            imagePanel.Name = "imagePanel";
            imagePanel.Size = new Size(728, 445);
            imagePanel.TabIndex = 1;
            imagePanel.Resize += imagePanel_Resize;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(369, 180);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // ofd
            // 
            ofd.FileName = "openFileDialog1";
            ofd.Filter = "Images|*.png;*.jpg;*.jpeg";
            // 
            // sfd
            // 
            sfd.FileName = "image.png";
            sfd.Filter = "Images|*.png;*.jpg;*.jpeg";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 511);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MinimumSize = new Size(750, 550);
            Name = "Form1";
            Text = "ShittyInpainter";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            imagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnLoad;
        private Button btnInpaint;
        private Button btnSave;
        private OpenFileDialog ofd;
        private SaveFileDialog sfd;
        private Panel imagePanel;
        private PictureBox pictureBox1;
    }
}
