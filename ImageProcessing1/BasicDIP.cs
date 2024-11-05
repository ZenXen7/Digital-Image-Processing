using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing1
{
    static class BasicDIP
    {


        // Image Filter Methods


        public static void BasicCopy(ref Bitmap loaded, ref Bitmap processed)
        {
           
            processed = new Bitmap(loaded.Width, loaded.Height);

           
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color pixel = loaded.GetPixel(i, j);

                    processed.SetPixel(i, j, pixel);
                }
            }
        }


        public static void Brightness(ref Bitmap a, ref Bitmap b, int value)
        {

            b = new Bitmap(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
            {
                for (int j = 0; j < a.Height; j++)
                {
                    Color temp = a.GetPixel(i, j);
                    Color changed;
                    if (value > 0)
                    {
                        changed = Color.FromArgb(Math.Min(temp.R + value, 255), Math.Min(temp.G + value, 255), Math.Min(temp.G + value, 255));
                    } else
                    {
                        changed = Color.FromArgb(Math.Max(temp.R + value, 0), Math.Max(temp.G + value, 0), Math.Max(temp.G + value, 0));

                    }

                    b.SetPixel(i, j, changed);  




                }
            }


        }

        public static void GreyScale(ref Bitmap loaded, ref Bitmap processed)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int average;

            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);
                    average = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color grey = Color.FromArgb(average, average, average);
                    processed.SetPixel(i, j, grey);
                }
            }
        }

        public static void Invert(ref Bitmap loaded, ref Bitmap processed)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);
                    Color inverted = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(i, j, inverted);
                }
            }
        }

        public static void Flip(ref Bitmap loaded, ref Bitmap processed)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int width = loaded.Width;

            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);
                    processed.SetPixel(width - 1 - i, j, pixel);
                }
            }
        }


        public static void Mirror(ref Bitmap loaded, ref Bitmap processed)
        {
         
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

          
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);

                    processed.SetPixel(loaded.Width - 1 - i, j, pixel);
                }
            }
        }


        public static void Sepia(ref Bitmap loaded, ref Bitmap processed)
        {
            
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    pixel = loaded.GetPixel(i, j);

                   
                    int tr = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int tg = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int tb = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                  
                    tr = Math.Min(255, tr);
                    tg = Math.Min(255, tg);
                    tb = Math.Min(255, tb);

                   
                    Color sepia = Color.FromArgb(tr, tg, tb);
                    processed.SetPixel(i, j, sepia);
                }
            }
        }





        public static void Histogram(ref Bitmap a, ref Bitmap b)
        {
            Color sample;
            Color gray;
            Byte graydata;

            for (int i = 0; i < a.Width; i++)
            {
                for (int j = 0; j < a.Height; j++)
                {
                    sample = a.GetPixel(i, j);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(i, j, gray);
                }
            }

            int[] histdata = new int[256];

            for (int i = 0; i < a.Width; i++)
            {
                for (int j = 0; j < a.Height; j++)
                {
                    sample = a.GetPixel(i, j);
                    histdata[sample.R]++;
                }
            }

            b = new Bitmap(256, 800);

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 800; j++)
                {
                    b.SetPixel(i, j ,Color.White);
                }   
            }


            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < Math.Min(histdata[i] / 5, b.Height - 1); j++)
                {
                    b.SetPixel(i, (b.Height - 1) - j, Color.Black);
                }
            }


        }

        public static void Subtract(ref Bitmap a, ref Bitmap b, ref Bitmap c) {
            Color mygreen = Color.FromArgb(0, 0,255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;


            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    Color pixel =  b.GetPixel(i, j);
                    Color backpixel = a.GetPixel(i, j);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);

                    if (subtractvalue > threshold)
                    {
                       c.SetPixel(i, j, pixel);
                    }
                    else
                    {
                       c.SetPixel(i, j, backpixel);
                    }

                    




                }
            }

            


        }
    }
}
