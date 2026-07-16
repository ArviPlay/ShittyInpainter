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

        public static Bitmap ArrayToBitmap(Color[,] arr)
        {
            Bitmap bmp = new Bitmap(arr.GetLength(0), arr.GetLength(1));

            for (int x = 0; x < arr.GetLength(0); x++)
            {
                for (int y = 0; y < arr.GetLength(1); y++)
                {
                    bmp.SetPixel(x, y, arr[x, y]);
                }
            }
        }
    }
}
