using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace ImageProcessing1
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, imageB, imageA, resultImage;
      


        FilterInfoCollection _filterInfoCollection;
        VideoCaptureDevice _videoCaptureDevice;

        private enum FilterType { None, Grayscale, Sepia, Invert, Flip, Histogram, Subtract }
        private FilterType _selectedFilter = FilterType.None;   

        public Form1()
        {
            InitializeComponent();
        }

        private void dIPToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

      


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }



       

        private void mirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            BasicDIP.BasicCopy(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        // Image Filters 
        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.GreyScale(ref loaded, ref processed); 
            pictureBox2.Image = processed;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Invert(ref loaded, ref processed); 
            pictureBox2.Image = processed;
        }

        private void flipImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            BasicDIP.Flip(ref loaded, ref processed); 
            pictureBox2.Image = processed;

        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Histogram(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Sepia(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            BasicDIP.Brightness(ref loaded, ref processed, trackBar1.Value);
            pictureBox2.Image = processed;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            resultImage = new Bitmap(imageA.Width, imageA.Height);

            BasicDIP.Subtract(ref imageA, ref imageB, ref resultImage);
            pictureBox3.Image = resultImage;


        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();

        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_filterInfoCollection == null)
            {

                _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (FilterInfo device in _filterInfoCollection)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
            }

           

        }



        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            using (Bitmap originalFrame = (Bitmap)eventArgs.Frame.Clone())
            {
                if (originalFrame != null)
                {
                    Bitmap filteredFrame = ApplySelectedFilter(originalFrame);
                    if (filteredFrame != null)
                    {
                        pictureBox3.Image = (Bitmap)filteredFrame.Clone(); 
                    }
                }
            }
        }



        private Bitmap ApplySelectedFilter(Bitmap originalFrame)
        {
            switch (_selectedFilter)
            {
                case FilterType.Grayscale:
                    return VideoFilter.ApplyGrayscaleFilter(originalFrame);
                case FilterType.Sepia:
                    return VideoFilter.ApplySepiaFilter(originalFrame);
                case FilterType.Invert:
                    return VideoFilter.ApplyInvertFilter(originalFrame);
                case FilterType.Flip:
                    return VideoFilter.ApplyFlipFilter(originalFrame);
                case FilterType.Histogram:
                    return VideoFilter.ApplyHistogramFilter(originalFrame);
                case FilterType.Subtract:
                    return VideoFilter.ApplySubtractFilter(loaded,originalFrame);
                default:
                    return originalFrame;
            }
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {



            

            if (_videoCaptureDevice == null)
            {
                _videoCaptureDevice = new VideoCaptureDevice(_filterInfoCollection[5].MonikerString);
                _videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
                _videoCaptureDevice.Start();
            }


        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (_videoCaptureDevice != null)
            {
                
                _videoCaptureDevice.Stop();
                pictureBox3.Image=null;
              
                _videoCaptureDevice=null;
                _selectedFilter = FilterType.None;
            }
        }


        //Video Filters  
       

        private void sepiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Sepia;

        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Grayscale;

        }

        private void flipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Flip;

        }

        private void invertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Invert;

        }

        private void histogramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Histogram;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedFilter = FilterType.Subtract;
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

       

       

       

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }
    }
}
