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
                ResizePictureBoxToFitPanel();
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

        private void ResizePictureBoxToFitPanel()
        {
            if (image == null) return;

            float panelAspect = imagePanel.Width / (float)imagePanel.Height;
            float imageAspect = image.Width / (float)image.Height;
            int newWidth, newHeight;

            if (imageAspect > panelAspect)
            {
                newWidth = imagePanel.Width;
                newHeight = (int)(imagePanel.Width / imageAspect);
            }
            else
            {
                newHeight = imagePanel.Height;
                newWidth = (int)(imagePanel.Height * imageAspect);
            }

            pictureBox1.Size = new Size(newWidth, newHeight);
            pictureBox1.Left = (imagePanel.Width - newWidth) / 2;
            pictureBox1.Top = (imagePanel.Height - newHeight) / 2;
        }

        private void imagePanel_Resize(object sender, EventArgs e)
        {
            ResizePictureBoxToFitPanel();
            selectionStart = new Point(0, 0);
            selectionEnd = new Point(0, 0);
            isSelecting = false;
        }

        private void btnInpaint_Click(object sender, EventArgs e)
        {
            Rectangle scaledRect = new Rectangle(
                (int)(selectionStart.X * image.Width / (float)pictureBox1.Width),
                (int)(selectionStart.Y * image.Height / (float)pictureBox1.Height),
                (int)((selectionEnd.X - selectionStart.X) * image.Width / (float)pictureBox1.Width),
                (int)((selectionEnd.Y - selectionStart.Y) * image.Height / (float)pictureBox1.Height)
            );
            Bitmap img = Inpaint(image, scaledRect);
            pictureBox1.Image = img;
            ResizePictureBoxToFitPanel();
        }

        private Bitmap Inpaint(Bitmap img, Rectangle rect)
        {
            Random rnd = new Random();
            int randomStrength = 150;
            Bitmap imgCopy = new Bitmap(img);
            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                if (rect.Left <= 0) throw new Exception("Out of range");
                Color currentColor = img.GetPixel(rect.Left - 1, y);
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    Color newColor = Color.FromArgb(Math.Clamp((currentColor.R + rnd.Next(0, randomStrength) - randomStrength/2), 0, 255),
                                                Math.Clamp((currentColor.G + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((currentColor.B + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255));
                    imgCopy.SetPixel(x, y, newColor);
                }
            }
            return imgCopy;
        }
    }
}
