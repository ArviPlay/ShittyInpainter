namespace ShittyInpainter
{
    public enum SelectionMode
    {
        Rectangle,
        Lasso
    }

    public partial class Form1 : Form
    {
        InpainterEngine engine;

        SelectionMode currentMode = SelectionMode.Rectangle;

        Point mousePos = new Point(0, 0);
        Point rectSelectionStart = new Point(0, 0);
        Point rectSelectionEnd = new Point(0, 0);
        bool rectIsSelecting = false;
        bool rectIsEditingSelection = false;

        List<Point> lassoSelectionPoints = new List<Point>();
        bool lassoIsSelecting = false;

        Bitmap previousImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e) =>
            LoadImage();
        private void LoadImage()
        {
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    engine = new InpainterEngine(new Bitmap(ofd.FileName), ProgressChanged);
                    if (!engine.HasImage()) previousImage = new Bitmap(engine.GetImage());
                    pictureBox1.Image = engine.GetImage();
                    rectSelectionStart = new Point(0, 0);
                    rectSelectionEnd = new Point(0, 0);
                    rectIsSelecting = false;
                    ResizePictureBoxToFitPanel();
                    pictureBox1.Invalidate();
                    pictureBox1.Cursor = Cursors.Cross;
                    tbRandomStrength.Enabled = true;
                    this.Text = $"ShittyInpainter - loaded: {ofd.FileName}";
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = $"ShittyInpainter - loading error";
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (engine == null || !engine.HasImage()) return;

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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
            pictureBox1.Invalidate();
            if (engine == null || !engine.HasImage()) return;
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
                            lassoSelectionPoints.Add(mousePos);
                    }
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (engine == null || !engine.HasImage()) return;

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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    using (Pen pen = new Pen(Color.DarkRed))
                    {
                        if (rectIsSelecting)
                            e.Graphics.DrawRectangle(pen, Math.Min(rectSelectionStart.X, mousePos.X), Math.Min(rectSelectionStart.Y, mousePos.Y), Math.Abs(mousePos.X - rectSelectionStart.X), Math.Abs(mousePos.Y - rectSelectionStart.Y));
                        else
                            e.Graphics.DrawRectangle(pen, rectSelectionStart.X, rectSelectionStart.Y, rectSelectionEnd.X - rectSelectionStart.X, rectSelectionEnd.Y - rectSelectionStart.Y);
                    }
                    break;
                case SelectionMode.Lasso:
                    using (Pen pen = new Pen(Color.Red))
                    {
                        if(lassoSelectionPoints.Count >= 2)
                            e.Graphics.DrawLines(pen, lassoSelectionPoints.ToArray());
                    }
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) =>
            SaveImage();
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

        private void ResizePictureBoxToFitPanel()
        {
            if (engine == null || !engine.HasImage()) return;

            float panelAspect = imagePanel.Width / (float)imagePanel.Height;
            float imageAspect = engine.ImageWidth / (float)engine.ImageHeight;
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
                    newPoints.Add(new Point((int)(points[i].X * engine.GetImage().Width / (float)pictureBox1.Width), (int)(points[i].Y * engine.GetImage().Height / (float)pictureBox1.Height)));
                return newPoints.ToArray();
            }
            if (engine == null || !engine.HasImage()) {MessageBox.Show("Select an image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            int randomStrength = tbRandomStrength.Value;
            switch (currentMode)
            {
                case SelectionMode.Rectangle:
                    Rectangle scaledRect = new Rectangle(
                    (int)(rectSelectionStart.X * engine.GetImage().Width / (float)pictureBox1.Width),
                    (int)(rectSelectionStart.Y * engine.GetImage().Height / (float)pictureBox1.Height),
                    (int)((rectSelectionEnd.X - rectSelectionStart.X) * engine.GetImage().Width / (float)pictureBox1.Width),
                    (int)((rectSelectionEnd.Y - rectSelectionStart.Y) * engine.GetImage().Height / (float)pictureBox1.Height)
                    );
                    if (scaledRect.Width <= 0 || scaledRect.Height <= 0) return;
                    btnLoad.Enabled = false;
                    btnInpaint.Enabled = false;
                    btnSave.Enabled = false;
                    tbRandomStrength.Enabled = false;
                    previousImage?.Dispose();
                    previousImage = new Bitmap(engine.GetImage());
                    this.Text = "ShittyInpainter - working...";
                    Task.Run(() =>
                    {
                        try
                        {
                            engine.InpaintRect(scaledRect, randomStrength);
                            this.Invoke((Action)(() =>
                            {
                                pictureBox1.Image = engine.GetImage();
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
                case SelectionMode.Lasso:
                    this.Text = "ShittyInpainter - preparing...";
                    Point[] scaledLasso = ScalePointsToImage(lassoSelectionPoints.ToArray());
                    btnLoad.Enabled = false;
                    btnInpaint.Enabled = false;
                    btnSave.Enabled = false;
                    tbRandomStrength.Enabled = false;
                    previousImage?.Dispose();
                    previousImage = engine.GetImage();
                    this.Text = "ShittyInpainter - working...";
                    Task.Run(() =>
                    {
                        try
                        {
                            engine.InpaintLasso(scaledLasso, randomStrength);
                            this.Invoke((Action)(() =>
                            {
                                pictureBox1.Image = engine.GetImage();
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

        private void tbRandomStrength_Scroll(object sender, EventArgs e) =>
            lblRandomStrength.Text = $"Random strength: {tbRandomStrength.Value}";

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (previousImage != null && engine != null)
                {
                    engine.SetImage(previousImage);
                    previousImage = null;
                    pictureBox1.Image = engine.GetImage();
                    pictureBox1.Invalidate();
                    this.Text = $"ShittyInpainter - undo";
                }
            }
            else if (e.Control && e.KeyCode == Keys.O)
                LoadImage();
            else if (e.Control && e.KeyCode == Keys.S)
                SaveImage();
        }

        private void rbRectMode_Click(object sender, EventArgs e) =>
            currentMode = SelectionMode.Rectangle;

        private void rbLassoMode_Click(object sender, EventArgs e) =>
            currentMode = SelectionMode.Lasso;

        private void ProgressChanged(double progress)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => ProgressChanged(progress)));
            switch (progress)
            {
                case -2:
                    this.Text = $"ShittyInpainter - converting to array";
                    break;
                case -1:
                    this.Text = $"ShittyInpainter - converting to image";
                    break;
                default:
                    this.Text = $"ShittyInpainter - inpainting: {progress}%/100%";
                    break;
            }
        }
    }
}
