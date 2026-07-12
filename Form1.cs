namespace ShittyInpainter
{
    public partial class Form1 : Form
    {
        Point mousePos = new Point(0, 0);
        Point selectionStart = new Point(0, 0);
        Point selectionEnd = new Point(0, 0);
        bool isSelecting = false;
        bool isMouseInsidePB = false;

        Bitmap image;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(ofd.FileName);
                pictureBox1.Image = image;
                selectionStart = new Point(0, 0);
                selectionEnd = new Point(0, 0);
                isSelecting = false;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (image == null) return;

            mousePos = e.Location;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (isMouseInsidePB)
            {
                using (Brush brush = new SolidBrush(Color.Red))
                {
                    e.Graphics.FillEllipse(brush, mousePos.X - 3, mousePos.Y - 3, 6, 6);
                }
            }
            using (Pen pen = new Pen(Color.DarkRed))
            {
                if (isSelecting)
                {
                    e.Graphics.DrawRectangle(pen, selectionStart.X, selectionStart.Y, mousePos.X - selectionStart.X, mousePos.Y - selectionStart.Y);
                }
                else
                {
                    e.Graphics.DrawRectangle(pen, selectionStart.X, selectionStart.Y, selectionEnd.X - selectionStart.X, selectionEnd.Y - selectionStart.Y);
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (image == null) return;

            isSelecting = true;
            selectionStart = mousePos;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (image == null) return;

            isSelecting = false;
            selectionEnd = mousePos;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) MessageBox.Show("Select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(sfd.FileName);
                }
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            isMouseInsidePB = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            isMouseInsidePB = false;
            pictureBox1.Invalidate();
        }
    }
}
