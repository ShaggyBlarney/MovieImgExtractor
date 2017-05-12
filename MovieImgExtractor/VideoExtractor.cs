/*
 * uses https://www.nrecosite.com/video_converter_net.aspx 
 * to get video thumbnails from MOV and mp4 files
 */

using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieImgExtractor
{
    public partial class VideoExtractor : Form
    {
        public VideoExtractor()
        {
            InitializeComponent();
        }

        /*
         * Listener for the Browse button
         * Used to get the file path of a video file
         */
        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Video Files (*.mov, *.mp4)|*.mov;*.mp4"; //filter out non movie files
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName; // get name of file
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding()))
                {
                    FilePath.Text = path; 
                }
            }
        }

        /*
         * Listener for the Open button
         * Used to get a frame from the video in the given in FilePath
         * and set it as the thumbnail on the form
         */ 
        private void Open_Click(object sender, EventArgs e)
        {
            //get thumbnail from video and save it as a jpg
            string path = FilePath.Text.ToString();
            MediaFile inputFile = new MediaFile { Filename = path };
            MediaFile outputFile = new MediaFile { Filename = @"thumbnails/thumbnail.jpg" };

            using (Engine engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                // Saves the frame located on the 15th second of the video.
                ConversionOptions options = new ConversionOptions { Seek = TimeSpan.FromSeconds(1) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }

            //set the picturebox to the thumbnail image
            Thumbnail_Box.SizeMode = PictureBoxSizeMode.StretchImage;
            Thumbnail_Box.Image = new Bitmap(@"thumbnails/thumbnail.jpg");
        }
    }
}
