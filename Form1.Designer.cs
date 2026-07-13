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
            tableLayoutPanel3 = new TableLayoutPanel();
            tbRandomStrength = new TrackBar();
            lblRandomStrength = new Label();
            ofd = new OpenFileDialog();
            sfd = new SaveFileDialog();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            imagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbRandomStrength).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(imagePanel, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 13.3072405F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.197652F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 77.49511F));
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
            tableLayoutPanel2.Size = new Size(728, 62);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Dock = DockStyle.Fill;
            btnLoad.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoad.Location = new Point(148, 3);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(139, 56);
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
            btnInpaint.Size = new Size(139, 56);
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
            btnSave.Size = new Size(139, 56);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // imagePanel
            // 
            imagePanel.Controls.Add(pictureBox1);
            imagePanel.Dock = DockStyle.Fill;
            imagePanel.Location = new Point(3, 118);
            imagePanel.Name = "imagePanel";
            imagePanel.Size = new Size(728, 390);
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
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.56593F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.4340668F));
            tableLayoutPanel3.Controls.Add(tbRandomStrength, 0, 0);
            tableLayoutPanel3.Controls.Add(lblRandomStrength, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 71);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(728, 41);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // tbRandomStrength
            // 
            tbRandomStrength.Dock = DockStyle.Fill;
            tbRandomStrength.Enabled = false;
            tbRandomStrength.Location = new Point(3, 3);
            tbRandomStrength.Maximum = 255;
            tbRandomStrength.Name = "tbRandomStrength";
            tbRandomStrength.Size = new Size(515, 35);
            tbRandomStrength.TabIndex = 0;
            tbRandomStrength.Value = 150;
            tbRandomStrength.Scroll += tbRandomStrength_Scroll;
            // 
            // lblRandomStrength
            // 
            lblRandomStrength.AutoSize = true;
            lblRandomStrength.Dock = DockStyle.Fill;
            lblRandomStrength.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRandomStrength.Location = new Point(524, 0);
            lblRandomStrength.Name = "lblRandomStrength";
            lblRandomStrength.Size = new Size(201, 41);
            lblRandomStrength.TabIndex = 1;
            lblRandomStrength.Text = "Random strength: 150";
            lblRandomStrength.TextAlign = ContentAlignment.MiddleCenter;
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
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbRandomStrength).EndInit();
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
        private TableLayoutPanel tableLayoutPanel3;
        private TrackBar tbRandomStrength;
        private Label lblRandomStrength;
    }
}
