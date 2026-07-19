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
        Point rectSelectionStart = new Point(0, 0);
        Point rectSelectionEnd = new Point(0, 0);
        bool rectIsSelecting = false;
        bool rectIsEditingSelection = false;
        bool isMouseInsidePB = false;

        List<Point> lassoSelectionPoints = new List<Point>();
        bool lassoIsSelecting = false;

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
                    rectSelectionStart = new Point(0, 0);
                    rectSelectionEnd = new Point(0, 0);
                    rectIsSelecting = false;
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
            if (image == null) return;
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    if (e.Button == MouseButtons.Right)
                    {
                        if (rectSelectionStart == rectSelectionEnd) return;
                        if (rectIsEditingSelection)
                        {
                            Point upperLeft = rectSelectionStart;
                            Point upperRight = new Point(rectSelectionEnd.X, rectSelectionStart.Y);
                            Point lowerLeft = new Point(rectSelectionStart.X, rectSelectionEnd.Y);
                            Point lowerRight = rectSelectionEnd;
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
                                        rectSelectionStart = mousePos;
                                        break;
                                    case "ur":
                                        rectSelectionStart.Y = mousePos.Y;
                                        rectSelectionEnd.X = mousePos.X;
                                        break;
                                    case "ll":
                                        rectSelectionStart.X = mousePos.X;
                                        rectSelectionEnd.Y = mousePos.Y;
                                        break;
                                    case "lr":
                                        rectSelectionEnd = mousePos;
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    if (lassoIsSelecting)
                    {
                        if (!lassoSelectionPoints.Contains(mousePos))
                        {
                            lassoSelectionPoints.Add(mousePos);
                        }
                    }
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
                        if (rectIsSelecting)
                        {
                            e.Graphics.DrawRectangle(pen, Math.Min(rectSelectionStart.X, mousePos.X), Math.Min(rectSelectionStart.Y, mousePos.Y), Math.Abs(mousePos.X - rectSelectionStart.X), Math.Abs(mousePos.Y - rectSelectionStart.Y));
                        }
                        else
                        {
                            e.Graphics.DrawRectangle(pen, rectSelectionStart.X, rectSelectionStart.Y, rectSelectionEnd.X - rectSelectionStart.X, rectSelectionEnd.Y - rectSelectionStart.Y);
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    using (Pen pen = new Pen(Color.Red))
                    {
                        if(lassoSelectionPoints.Count >= 2)
                        {
                            e.Graphics.DrawLines(pen, lassoSelectionPoints.ToArray());
                        }
                    }
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
                        rectIsSelecting = true;
                        rectSelectionStart = mousePos;
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (rectSelectionStart == rectSelectionEnd) return;
                        else
                        {
                            rectIsEditingSelection = true;
                        }
                    }
                    break;
                case SelectionMode.Lasso:
                    if (e.Button == MouseButtons.Left)
                    {
                        lassoSelectionPoints.Clear();
                        lassoIsSelecting = true;
                        lassoSelectionPoints.Add(mousePos);
                    }
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
                        rectIsSelecting = false;

                        int left = Math.Min(rectSelectionStart.X, mousePos.X);
                        int top = Math.Min(rectSelectionStart.Y, mousePos.Y);
                        int right = Math.Max(rectSelectionStart.X, mousePos.X);
                        int bottom = Math.Max(rectSelectionStart.Y, mousePos.Y);

                        rectSelectionStart = new Point(left, top);
                        rectSelectionEnd = new Point(right, bottom);
                    }
                    if (e.Button == MouseButtons.Right)
                    {
                        rectIsEditingSelection = false;
                    }
                    rectSelectionStart.X = Math.Clamp(rectSelectionStart.X, 0, pictureBox1.Width - 1);
                    rectSelectionStart.Y = Math.Clamp(rectSelectionStart.Y, 0, pictureBox1.Height - 1);
                    rectSelectionEnd.X = Math.Clamp(rectSelectionEnd.X, 0, pictureBox1.Width - 1);
                    rectSelectionEnd.Y = Math.Clamp(rectSelectionEnd.Y, 0, pictureBox1.Height - 1);
                    pictureBox1.Invalidate();
                    break;
                case SelectionMode.Lasso:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (lassoSelectionPoints.Count == 0) return;
                        lassoIsSelecting = false;
                        lassoSelectionPoints.Add(mousePos);

                        int dx = lassoSelectionPoints[0].X - lassoSelectionPoints.Last().X;
                        int dy = lassoSelectionPoints[0].Y - lassoSelectionPoints.Last().Y;
                        int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
                        Point startPt = lassoSelectionPoints.Last();
                        for (int i = 0; i < steps; i++)
                        {
                            Point current = new Point(startPt.X + (dx * i) / steps, startPt.Y + (dy * i) / steps);
                            lassoSelectionPoints.Add(current);
                        }

                        for (int i = 0; i < lassoSelectionPoints.Count; i++)
                        {
                            lassoSelectionPoints[i] = new Point(Math.Clamp(lassoSelectionPoints[i].X, 0, pictureBox1.Width - 1), Math.Clamp(lassoSelectionPoints[i].Y, 0, pictureBox1.Height - 1));
                        }

                        pictureBox1.Invalidate();
                    }
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
                    rectSelectionStart = new Point(0, 0);
                    rectSelectionEnd = new Point(0, 0);
                    rectIsSelecting = false;
                    break;
                case SelectionMode.Lasso:
                    lassoSelectionPoints = new List<Point>();
                    lassoIsSelecting = false;
                    break;
            }
        }

        private void btnInpaint_Click(object sender, EventArgs e)
        {
            Point[] ScalePointsToImage(Point[] points)
            {
                List<Point> newPoints = new List<Point>();
                for (int i = 0; i < points.Length; i++)
                {
                    newPoints.Add(new Point((int)(points[i].X * image.Width / (float)pictureBox1.Width), (int)(points[i].Y * image.Height / (float)pictureBox1.Height)));
                }
                return newPoints.ToArray();
            }

            int randomStrength = tbRandomStrength.Value;
            Bitmap imageCopy = new Bitmap(image);

            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    try
                    {
                        if (image == null) MessageBox.Show("Select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            Rectangle scaledRect = new Rectangle(
                            (int)(rectSelectionStart.X * image.Width / (float)pictureBox1.Width),
                            (int)(rectSelectionStart.Y * image.Height / (float)pictureBox1.Height),
                            (int)((rectSelectionEnd.X - rectSelectionStart.X) * image.Width / (float)pictureBox1.Width),
                            (int)((rectSelectionEnd.Y - rectSelectionStart.Y) * image.Height / (float)pictureBox1.Height)
                            );
                            if (scaledRect.Width <= 0 || scaledRect.Height <= 0) return;
                            btnLoad.Enabled = false;
                            btnInpaint.Enabled = false;
                            btnSave.Enabled = false;
                            tbRandomStrength.Enabled = false;
                            previousImage?.Dispose();
                            previousImage = new Bitmap(image);
                            this.Text = "ShittyInpainter - working...";
                            Task.Run(() =>
                            {
                                try
                                {
                                    Bitmap img = InpaintRect(imageCopy, scaledRect, randomStrength);
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
                    Point[] scaledLasso = ScalePointsToImage(lassoSelectionPoints.ToArray());
                    btnLoad.Enabled = false;
                    btnInpaint.Enabled = false;
                    btnSave.Enabled = false;
                    tbRandomStrength.Enabled = false;
                    previousImage?.Dispose();
                    previousImage = new Bitmap(image);
                    this.Text = "ShittyInpainter - working...";
                    Task.Run(() =>
                    {
                        try
                        {
                            Bitmap img = InpaintLasso(imageCopy, scaledLasso, randomStrength);
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

                    break;
            }
        }

        private Bitmap InpaintRect(Bitmap img, Rectangle rect, int randomStrength)
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

        private Bitmap InpaintLasso(Bitmap img, Point[] selection, int randomStrength)
        {
            Random rnd = new Random();

            this.BeginInvoke((Action)(() =>
            {
                this.Text = $"ShittyInpainter - converting to array";
            }));
            Color[,] imgArr = ImageHelper.BitmapToArray(img);
            Color[,] resultArr = (Color[,])imgArr.Clone();

            if (selection == null || selection.Length < 3) return img;

            int minX = selection[0].X;
            int maxX = selection[0].X;
            int minY = selection[0].Y;
            int maxY = selection[0].Y;
            for (int i = 1; i < selection.Length; i++)
            {
                if (selection[i].X < minX) minX = selection[i].X;
                if (selection[i].X > maxX) maxX = selection[i].X;
                if (selection[i].Y < minY) minY = selection[i].Y;
                if (selection[i].Y > maxY) maxY = selection[i].Y;
            }
            minX = Math.Max(0, minX);
            maxX = Math.Min(imgArr.GetLength(0) - 1, maxX);
            minY = Math.Max(0, minY);
            maxY = Math.Min(imgArr.GetLength(1) - 1, maxY);

            int totalPixels = (maxX - minX + 1) * (maxY - minY + 1);
            int processedPixels = 0;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (IsPointInPolygon(new Point(x,y), selection))
                    {
                        Color leftColor = Color.Black;
                        for (int lx = x; lx >= 0; lx--)
                        {
                            if (!IsPointInPolygon(new Point(lx,y), selection))
                            {
                                leftColor = imgArr[lx, y];
                                break;
                            }
                        }

                        Color rightColor = Color.Black;
                        for (int rx = x; rx < imgArr.GetLength(0); rx++)
                        {
                            if (!IsPointInPolygon(new Point(rx, y), selection))
                            {
                                rightColor = imgArr[rx, y];
                                break;
                            }
                        }

                        Color upColor = Color.Black;
                        for (int uy = y; uy >= 0; uy--)
                        {
                            if (!IsPointInPolygon(new Point(x, uy), selection))
                            {
                                upColor = imgArr[x, uy];
                                break;
                            }
                        }

                        Color downColor = Color.Black;
                        for (int dy = y; dy < imgArr.GetLength(1); dy++)
                        {
                            if (!IsPointInPolygon(new Point(x, dy), selection))
                            {
                                downColor = imgArr[x, dy];
                                break;
                            }
                        }

                        int noise = rnd.Next(0, randomStrength) - randomStrength / 2;
                        Color mixedColor = Color.FromArgb(Math.Clamp((leftColor.R + rightColor.R + upColor.R + downColor.R) / 4 + noise, 0, 255),
                                                          Math.Clamp((leftColor.G + rightColor.G + upColor.G + downColor.G) / 4 + noise, 0, 255),
                                                          Math.Clamp((leftColor.B + rightColor.B + upColor.B + downColor.B) / 4 + noise, 0, 255));
                        resultArr[x, y] = mixedColor;
                    }

                    processedPixels++;
                    if(processedPixels % 100 == 0)
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
            Bitmap newImg = ImageHelper.ArrayToBitmap(resultArr);

            img = null;
            imgArr = null;
            resultArr = null;
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

        private bool IsPointInPolygon(Point pnt, Point[] polygon)
        {
            if (polygon == null || polygon.Length < 3) return false;
            bool inside = false;
            int count = polygon.Length;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if (((polygon[i].Y > pnt.Y) != (polygon[j].Y > pnt.Y)) &&
                    (pnt.X < (polygon[j].X - polygon[i].X) * (pnt.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }
    }
}
