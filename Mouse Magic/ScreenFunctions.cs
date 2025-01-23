using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mouse_Magic
{
    public class ScreenFunctions
    {
        public Bitmap GetBitmapAt(int x, int y, int x2, int y2)
        {
            Bitmap bmp = new Bitmap(Math.Abs(x2 - x), Math.Abs(y2 - y));
            Rectangle bounds = new Rectangle(x, y, x2 - x, y2 - y);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp;
        }

        [DllImport("msvcrt.dll")]
        public static extern int memcmp(IntPtr b1, IntPtr b2, long count);
        public bool CompareMemCmp(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }

        public Color GetColorAt(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }
        //Functions to draw directly on the sceen

        public void DrawRect(object sender, EventArgs e)
        {
            drawonscreen(Cursor.Position.X, Cursor.Position.Y);
        }

        public int prevX = 0;
        public int prevY = 0;
        public Bitmap prevImg;

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public void drawonscreen(int x, int y)
        {
            if (prevImg != null)
            {
                IntPtr desktopPtr1 = GetDC(IntPtr.Zero);
                Graphics g1 = Graphics.FromHdc(desktopPtr1);

                g1.DrawImage(prevImg, prevX - 1 - 25, prevY - 1 - 12);
                g1.Dispose();
                ReleaseDC(IntPtr.Zero, desktopPtr1);
            }

            prevX = x;
            prevY = y;
            prevImg = GetBitmapAt(x - 1 - 25, y - 1 - 12, x + 50 + 1 - 24, y + 24 + 1 - 12);

            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);

            var pen = new Pen(Brushes.Black)
            {
                Width = 2.0F
            };

            g.DrawRectangle(pen, x - 25, y - 12, 50, 24);
            g.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public Bitmap createRandomBitmap()
        {
            int width = Screen.PrimaryScreen.Bounds.Width, height = Screen.PrimaryScreen.Bounds.Height;

            //bitmap
            Bitmap bmp = new Bitmap(width, height);

            //random number
            Random rand = new Random();

            //create random pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //generate random ARGB value
                    int a = rand.Next(256);
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);

                    //set ARGB value
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            //save (write) random pixel image
            //bmp.Save(@"C:\\Users\Christopher Battista\Desktop\rand2.bmp");
            return bmp;
        }

        public int counter = 508;
        public void TakeSceenshot(int x0, int y0, int x1, int y1)
        {
            //Create the Bitmap
            Bitmap printscreen = GetBitmapAt(x0, y0, x1, y1);// GetBitmapAt(x0, y0, x1, y1);
            var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            var quality = (long)10;
            var ratio = new EncoderParameter(qualityEncoder, quality);
            var codecParams = new EncoderParameters(1);
            codecParams.Param[0] = ratio;
            var jpegCodecInfo = ImageCodecInfo.GetImageEncoders()[0];
            printscreen.Save(Environment.CurrentDirectory + @"\imgs\" + counter.ToString() + ".jpg", jpegCodecInfo, codecParams); // Save to JPG
            counter++;
        }
    }
}
