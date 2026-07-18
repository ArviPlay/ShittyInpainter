namespace ShittyInpainter
{
    public enum SelectionMode
    {
        Rectangle,
        Lasso
    }

    public partial class Form1 : Form
    {
        SelectionMode currentMode = SelectionMode.Rectangle;

        Point mousePos = new Point(0, 0);
        Point selectionStart = new Point(0, 0);
        Point selectionEnd = new Point(0, 0);
        bool isSelecting = false;
        bool isEditingSelection = false;
        bool isMouseInsidePB = false;

        Bitmap image;
        Bitmap previousImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadImage();
        }
        private void LoadImage()
        {
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (image != null) previousImage = new Bitmap(image);
                    image = new Bitmap(ofd.FileName);
                    pictureBox1.Image = image;
                    selectionStart = new Point(0, 0);
                    selectionEnd = new Point(0, 0);
                    isSelecting = false;
                    ResizePictureBoxToFitPanel();
                    pictureBox1.Invalidate();
                    pictureBox1.Cursor = Cursors.Cross;
                    tbRandomStrength.Enabled = true;
                    this.Text = $"ShittyInpainter - loaded: {ofd.FileName}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = $"ShittyInpainter - loading error";
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
            pictureBox1.Invalidate();
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    if (e.Button == MouseButtons.Right)
                    {
                        if (selectionStart == selectionEnd) return;
                        if (isEditingSelection)
                        {
                            Point upperLeft = selectionStart;
                            Point upperRight = new Point(selectionEnd.X, selectionStart.Y);
                            Point lowerLeft = new Point(selectionStart.X, selectionEnd.Y);
                            Point lowerRight = selectionEnd;
                            double ulDist = Math.Sqrt(Math.Pow(mousePos.X - upperLeft.X, 2) + Math.Pow(mousePos.Y - upperLeft.Y, 2));
                            double urDist = Math.Sqrt(Math.Pow(mousePos.X - upperRight.X, 2) + Math.Pow(mousePos.Y - upperRight.Y, 2));
                            double llDist = Math.Sqrt(Math.Pow(mousePos.X - lowerLeft.X, 2) + Math.Pow(mousePos.Y - lowerLeft.Y, 2));
                            double lrDist = Math.Sqrt(Math.Pow(mousePos.X - lowerRight.X, 2) + Math.Pow(mousePos.Y - lowerRight.Y, 2));
                            string minDistName = "ul";
                            double minDist = ulDist;
                            if (urDist < ulDist) { minDistName = "ur"; minDist = urDist; }
                            if (llDist < minDist) { minDistName = "ll"; minDist = llDist; }
                            if (lrDist < minDist) { minDistName = "lr"; minDist = lrDist; }
                            if (minDist <= 20)
                            {
                                switch (minDistName)
                                {
                                    case "ul":
                                        selectionStart = mousePos;
                                        break;
                                    case "ur":
                                        selectionStart.Y = mousePos.Y;
                                        selectionEnd.X = mousePos.X;
                                        break;
                                    case "ll":
                                        selectionStart.X = mousePos.X;
                                        selectionEnd.Y = mousePos.Y;
                                        break;
                                    case "lr":
                                        selectionEnd = mousePos;
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    break;
            }
            
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
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    using (Pen pen = new Pen(Color.DarkRed))
                    {
                        if (isSelecting)
                        {
                            e.Graphics.DrawRectangle(pen, Math.Min(selectionStart.X, mousePos.X), Math.Min(selectionStart.Y, mousePos.Y), Math.Abs(mousePos.X - selectionStart.X), Math.Abs(mousePos.Y - selectionStart.Y));
                        }
                        else
                        {
                            e.Graphics.DrawRectangle(pen, selectionStart.X, selectionStart.Y, selectionEnd.X - selectionStart.X, selectionEnd.Y - selectionStart.Y);
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    break;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (image == null) return;

            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    if (e.Button == MouseButtons.Left)
                    {
                        isSelecting = true;
                        selectionStart = mousePos;
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (selectionStart == selectionEnd) return;
                        else
                        {
                            isEditingSelection = true;
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (image == null) return;

            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    if (e.Button == MouseButtons.Left)
                    {
                        isSelecting = false;

                        int left = Math.Min(selectionStart.X, mousePos.X);
                        int top = Math.Min(selectionStart.Y, mousePos.Y);
                        int right = Math.Max(selectionStart.X, mousePos.X);
                        int bottom = Math.Max(selectionStart.Y, mousePos.Y);

                        selectionStart = new Point(left, top);
                        selectionEnd = new Point(right, bottom);
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        isEditingSelection = false;
                    }
                    selectionStart.X = Math.Clamp(selectionStart.X, 0, pictureBox1.Width - 1);
                    selectionStart.Y = Math.Clamp(selectionStart.Y, 0, pictureBox1.Height - 1);
                    selectionEnd.X = Math.Clamp(selectionEnd.X, 0, pictureBox1.Width - 1);
                    selectionEnd.Y = Math.Clamp(selectionEnd.Y, 0, pictureBox1.Height - 1);
                    pictureBox1.Invalidate();
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveImage();
        }
        private void SaveImage()
        {
            try
            {
                if (pictureBox1.Image == null) MessageBox.Show("Select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox1.Image.Save(sfd.FileName);
                        this.Text = $"ShittyInpainter - saved: {sfd.FileName}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = $"ShittyInpainter - saving error";
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

            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    selectionStart = new Point(0, 0);
                    selectionEnd = new Point(0, 0);
                    isSelecting = false;
                    break;
                case SelectionMode.Lasso:
                    break;
            }
        }

        private void btnInpaint_Click(object sender, EventArgs e)
        {
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    try
                    {
                        if (image == null) MessageBox.Show("Select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            Rectangle scaledRect = new Rectangle(
                            (int)(selectionStart.X * image.Width / (float)pictureBox1.Width),
                            (int)(selectionStart.Y * image.Height / (float)pictureBox1.Height),
                            (int)((selectionEnd.X - selectionStart.X) * image.Width / (float)pictureBox1.Width),
                            (int)((selectionEnd.Y - selectionStart.Y) * image.Height / (float)pictureBox1.Height)
                            );
                            if (scaledRect.Width <= 0 || scaledRect.Height <= 0) return;
                            btnLoad.Enabled = false;
                            btnInpaint.Enabled = false;
                            btnSave.Enabled = false;
                            tbRandomStrength.Enabled = false;
                            int randomStrength = tbRandomStrength.Value;
                            Bitmap imageCopy = new Bitmap(image);
                            previousImage?.Dispose();
                            previousImage = new Bitmap(image);
                            this.Text = "ShittyInpainter - working...";
                            Task.Run(() =>
                            {
                                try
                                {
                                    Bitmap img = Inpaint(imageCopy, scaledRect, randomStrength);
                                    Bitmap oldImage = image;
                                    image = img;

                                    imageCopy?.Dispose();
                                    this.Invoke((Action)(() =>
                                    {
                                        pictureBox1.Image = image;
                                        oldImage?.Dispose();
                                        ResizePictureBoxToFitPanel();
                                        btnLoad.Enabled = true;
                                        btnInpaint.Enabled = true;
                                        btnSave.Enabled = true;
                                        tbRandomStrength.Enabled = true;
                                        this.Text = "ShittyInpainter - completed";
                                    }));
                                }
                                catch (Exception ex)
                                {
                                    this.Invoke((Action)((() =>
                                    {
                                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        btnLoad.Enabled = true;
                                        btnInpaint.Enabled = true;
                                        btnSave.Enabled = true;
                                        tbRandomStrength.Enabled = true;
                                        this.Text = "ShittyInpainter - inpainting error";
                                    })));
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case SelectionMode.Lasso:
                    break;
            }
            
        }

        private Bitmap Inpaint(Bitmap img, Rectangle rect, int randomStrength)
        {
            Random rnd = new Random();

            this.BeginInvoke((Action)(() =>
            {
                this.Text = $"ShittyInpainter - converting to array";
            }));
            Color[,] imgArr = ImageHelper.BitmapToArray(img);

            int totalPixels = rect.Width * rect.Height * 4;
            int processedPixels = 0;

            if (rect.Width <= 0 || rect.Height <= 0) return img;
            for (int y = rect.Top; y < rect.Bottom; y++) // left to right
            {
                Color leftColor = rect.Left - 1 >= 0 ? imgArr[rect.Left - 1, y] : imgArr[rect.Left, y];
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    Color newColor = Color.FromArgb(Math.Clamp((leftColor.R + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((leftColor.G + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((leftColor.B + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255));
                    imgArr[x, y] = newColor;
                    processedPixels++;
                    if (processedPixels % 50000 == 0)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            this.Text = $"ShittyInpainter - inpainting: {Math.Round((float)processedPixels / totalPixels * 100, 2)}%/100%";
                        }));
                    }
                }
            }
            for (int y = rect.Top; y < rect.Bottom; y++) // right to left
            {
                Color rightColor = rect.Right + 1 < imgArr.GetLength(0) ? imgArr[rect.Right + 1, y] : imgArr[rect.Right, y];
                for (int x = rect.Right; x > rect.Left; x--)
                {
                    Color newColor = Color.FromArgb(Math.Clamp((rightColor.R + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((rightColor.G + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((rightColor.B + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255));
                    Color mixedColor = Color.FromArgb((imgArr[x, y].R + newColor.R) / 2,
                        (imgArr[x, y].G + newColor.G) / 2,
                        (imgArr[x, y].B + newColor.B) / 2);
                    imgArr[x, y] = mixedColor;
                    processedPixels++;
                    if (processedPixels % 50000 == 0)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            this.Text = $"ShittyInpainter - inpainting: {Math.Round((float)processedPixels / totalPixels * 100, 2)}%/100%";
                        }));
                    }
                }
            }
            for (int x = rect.Left; x < rect.Right; x++) // top to bottom
            {
                Color topColor = rect.Top - 1 >= 0 ? imgArr[x, rect.Top - 1] : imgArr[x, rect.Top];
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    Color newColor = Color.FromArgb(Math.Clamp((topColor.R + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((topColor.G + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((topColor.B + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255));
                    Color mixedColor = Color.FromArgb((imgArr[x, y].R + newColor.R) / 2,
                        (imgArr[x, y].G + newColor.G) / 2,
                        (imgArr[x, y].B + newColor.B) / 2);
                    imgArr[x, y] = mixedColor;
                    processedPixels++;
                    if (processedPixels % 50000 == 0)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            this.Text = $"ShittyInpainter - inpainting: {Math.Round((float)processedPixels / totalPixels * 100, 2)}%/100%";
                        }));
                    }
                }
            }
            for (int x = rect.Left; x < rect.Right; x++) // bottom to top
            {
                Color bottomColor = rect.Bottom + 1 < imgArr.GetLength(1) ? imgArr[x, rect.Bottom + 1] : imgArr[x, rect.Bottom];
                for (int y = rect.Bottom; y > rect.Top; y--)
                {
                    Color newColor = Color.FromArgb(Math.Clamp((bottomColor.R + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((bottomColor.G + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255),
                                                Math.Clamp((bottomColor.B + rnd.Next(0, randomStrength) - randomStrength / 2), 0, 255));
                    Color mixedColor = Color.FromArgb((imgArr[x, y].R + newColor.R) / 2,
                        (imgArr[x, y].G + newColor.G) / 2,
                        (imgArr[x, y].B + newColor.B) / 2);
                    imgArr[x, y] = mixedColor;
                    processedPixels++;
                    if (processedPixels % 50000 == 0)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            this.Text = $"ShittyInpainter - inpainting: {Math.Round((float)processedPixels / totalPixels * 100, 2)}%/100%";
                        }));
                    }
                }
            }
            this.BeginInvoke((Action)(() =>
            {
                this.Text = $"ShittyInpainter - converting to image";
            }));
            Bitmap newImg = ImageHelper.ArrayToBitmap(imgArr);
            img = null;
            imgArr = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return newImg;
        }

        private void tbRandomStrength_Scroll(object sender, EventArgs e)
        {
            lblRandomStrength.Text = $"Random strength: {tbRandomStrength.Value}";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                e.SuppressKeyPress = true;
                if (previousImage != null)
                {
                    image = new Bitmap(previousImage);
                    previousImage = null;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                    this.Text = $"ShittyInpainter - undo";
                }
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                e.SuppressKeyPress = true;
                LoadImage();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                SaveImage();
            }
        }

        private void rbRectMode_Click(object sender, EventArgs e)
        {
            currentMode = SelectionMode.Rectangle;
        }

        private void rbLassoMode_Click(object sender, EventArgs e)
        {
            currentMode = SelectionMode.Lasso;
        }
    }
}
