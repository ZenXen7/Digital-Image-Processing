using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing1
{
    public class VideoFilter
    {


        // Video Filter Methods
        public static Bitmap ApplyInvertFilter(Bitmap frame)
        {
            Bitmap processed = new Bitmap(frame.Width, frame.Height);

           
            Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
            BitmapData frameData = frame.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData processedData = processed.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                
                byte* framePtr = (byte*)frameData.Scan0;
                byte* processedPtr = (byte*)processedData.Scan0;

                int bytesPerPixel = 4; 
                int height = frame.Height;
                int width = frame.Width;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                       
                        int pixelIndex = (y * frameData.Stride) + (x * bytesPerPixel);

                       
                        byte b = framePtr[pixelIndex]; 
                        byte g = framePtr[pixelIndex + 1]; 
                        byte r = framePtr[pixelIndex + 2]; 
                        byte a = framePtr[pixelIndex + 3]; 

                      
                        processedPtr[pixelIndex] = (byte)(255 - b); 
                        processedPtr[pixelIndex + 1] = (byte)(255 - g); 
                        processedPtr[pixelIndex + 2] = (byte)(255 - r); 
                        processedPtr[pixelIndex + 3] = a; 
                    }
                }
            }

            
            frame.UnlockBits(frameData);
            processed.UnlockBits(processedData);

            return processed;
        }

        public static Bitmap ApplyFlipFilter(Bitmap frame)
        {
            int width = frame.Width;
            Bitmap processed = new Bitmap(width, frame.Height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < frame.Height; j++)
                {
                    Color pixel = frame.GetPixel(i, j);
                    processed.SetPixel(width - 1 - i, j, pixel);
                }
            }
            return processed;
        }

        public static Bitmap ApplyGrayscaleFilter(Bitmap frame)
        {
            Bitmap processed = new Bitmap(frame.Width, frame.Height);

            Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
            BitmapData frameData = frame.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData processedData = processed.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* framePtr = (byte*)frameData.Scan0;
                byte* processedPtr = (byte*)processedData.Scan0;

                int bytesPerPixel = 4;
                int height = frame.Height;
                int width = frame.Width;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int pixelIndex = (y * frameData.Stride) + (x * bytesPerPixel);

                       
                        byte b = framePtr[pixelIndex];
                        byte g = framePtr[pixelIndex + 1];
                        byte r = framePtr[pixelIndex + 2];

                       
                        byte gray = (byte)((r + g + b) / 3);

                     
                        processedPtr[pixelIndex] = gray; 
                        processedPtr[pixelIndex + 1] = gray; 
                        processedPtr[pixelIndex + 2] = gray; 
                        processedPtr[pixelIndex + 3] = framePtr[pixelIndex + 3]; 
                    }
                }
            }

            frame.UnlockBits(frameData);
            processed.UnlockBits(processedData);

            return processed;
        }

        public static Bitmap ApplySepiaFilter(Bitmap frame)
        {
            Bitmap processed = new Bitmap(frame.Width, frame.Height);

            Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
            BitmapData frameData = frame.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData processedData = processed.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* framePtr = (byte*)frameData.Scan0;
                byte* processedPtr = (byte*)processedData.Scan0;

                int bytesPerPixel = 4;
                int height = frame.Height;
                int width = frame.Width;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int pixelIndex = (y * frameData.Stride) + (x * bytesPerPixel);

                        byte b = framePtr[pixelIndex];
                        byte g = framePtr[pixelIndex + 1];
                        byte r = framePtr[pixelIndex + 2];

                        
                        int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                        int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                        int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                       
                        processedPtr[pixelIndex] = (byte)Math.Min(255, tb); 
                        processedPtr[pixelIndex + 1] = (byte)Math.Min(255, tg); 
                        processedPtr[pixelIndex + 2] = (byte)Math.Min(255, tr); 
                        processedPtr[pixelIndex + 3] = framePtr[pixelIndex + 3]; 
                    }
                }
            }

            frame.UnlockBits(frameData);
            processed.UnlockBits(processedData);

            return processed;
        }




        public static Bitmap ApplyHistogramFilter(Bitmap frame)
        {
            Bitmap processed = new Bitmap(frame.Width, frame.Height);
            int[] histogram = new int[256];

            for (int i = 0; i < frame.Width; i++)
            {
                for (int j = 0; j < frame.Height; j++)
                {
                    Color pixel = frame.GetPixel(i, j);
                    int grayValue = (pixel.R + pixel.G + pixel.B) / 3;
                    histogram[grayValue]++;
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    processed.SetPixel(i, j, grayColor);
                }
            }

            Bitmap histogramImage = new Bitmap(256, 800);
            for (int i = 0; i < 256; i++)
            {
                int value = Math.Min(histogram[i] / 5, histogramImage.Height - 1);
                for (int j = 0; j < value; j++)
                {
                    histogramImage.SetPixel(i, histogramImage.Height - 1 - j, Color.Black);
                }
            }

            return histogramImage;
        }

        public static Bitmap ApplySubtractFilter(Bitmap background, Bitmap foreground)
        {
            Bitmap result = new Bitmap(foreground.Width, foreground.Height);
            Color green = Color.FromArgb(0, 0, 255);
            int greenGray = (green.R + green.G + green.B) / 3;
            int threshold = 5;

            for (int i = 0; i < foreground.Width; i++)
            {
                for (int j = 0; j < foreground.Height; j++)
                {
                    Color fgPixel = foreground.GetPixel(i, j);
                    Color bgPixel = background.GetPixel(i, j);
                    int gray = (fgPixel.R + fgPixel.G + fgPixel.B) / 3;
                    int difference = Math.Abs(gray - greenGray);

                    result.SetPixel(i, j, difference > threshold ? fgPixel : bgPixel);
                }
            }
            return result;
        }






    }
}
