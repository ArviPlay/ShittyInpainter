namespace ShittyInpainter
{
    public static class ImageHelper
    {
        public static Color[,] BitmapToArray(Bitmap bmp)
        {
            Color[,] arr = new Color[bmp.Width, bmp.Height];

            for(int x = 0; x < bmp.Width; x++)
            {
                for(int y = 0; y < bmp.Height; y++)
                {
                    arr[x, y] = bmp.GetPixel(x, y);
                }
            }

            return arr;
        }
    }
}
