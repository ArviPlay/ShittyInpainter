namespace ShittyInpainter
{
    internal class InpainterEngine
    {
        Random rnd = new Random();
        public delegate void ProgressChangedHandler(double progress);
        public event ProgressChangedHandler ProgressChanged;

        Bitmap image;
        public int ImageWidth => image?.Width ?? 0; 
        public int ImageHeight => image?.Height ?? 0;

        public InpainterEngine(Bitmap image, ProgressChangedHandler progressChanged)
        {
            this.image = image;
            ProgressChanged += progressChanged;
        }

        public Bitmap GetImage() =>
            new Bitmap(image);

        public void SetImage(Bitmap img) =>
            image = new Bitmap(img);

        public bool HasImage() =>
            image != null;

        public void InpaintRect(Rectangle rect, int randomStrength)
        {
            if (image == null) return;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            ProgressChanged?.Invoke(-2);

            Color[,] imgArr = ImageHelper.BitmapToArray(image);

            int totalPixels = rect.Width * rect.Height * 4;
            int processedPixels = 0;

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
                        ProgressChanged?.Invoke(Math.Round((float)processedPixels / totalPixels * 100, 2));
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
                        ProgressChanged?.Invoke(Math.Round((float)processedPixels / totalPixels * 100, 2));
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
                        ProgressChanged?.Invoke(Math.Round((float)processedPixels / totalPixels * 100, 2));
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
                        ProgressChanged?.Invoke(Math.Round((float)processedPixels / totalPixels * 100, 2));
                    }
                }
            }
            ProgressChanged?.Invoke(-1);
            image = ImageHelper.ArrayToBitmap(imgArr);
            imgArr = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void InpaintLasso(Point[] selection, int randomStrength)
        {
            ProgressChanged?.Invoke(-2);

            Color[,] imgArr = ImageHelper.BitmapToArray(image);
            Color[,] resultArr = (Color[,])imgArr.Clone();

            if (selection == null || selection.Length < 3) return;

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
                    if (IsPointInPolygon(new Point(x, y), selection))
                    {
                        Color leftColor = Color.Black;
                        for (int lx = x; lx >= 0; lx--)
                        {
                            if (!IsPointInPolygon(new Point(lx, y), selection))
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
                    if (processedPixels % 100 == 0)
                    {
                        ProgressChanged?.Invoke(Math.Round((float)processedPixels / totalPixels * 100, 2));
                    }
                }
            }

            ProgressChanged?.Invoke(-1);
            image = ImageHelper.ArrayToBitmap(resultArr);

            imgArr = null;
            resultArr = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
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