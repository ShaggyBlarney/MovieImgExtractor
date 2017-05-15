using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;

namespace VideoPlayer
{
    public partial class Form1 : Form
    {
        public Video MyVideo;
        public int PanelWidth, PanelHeight;
        public string PlayingPosition, Duration;

        public Form1()
        {
            InitializeComponent();
            timer1.Stop();
        }

        //returns time in a string formated as hh:mm:ss
        public string CalculateTime(double Time)
        {
            String mm, ss;
            int h, m, s, T;

            Time = Math.Round(Time);
            T = Convert.ToInt32(Time);

            //split time T down into hours, minutes, seconds.
            h = (T / 3600);
            T = T % 3600;
            m = (T / 60);
            s = T % 60;

            //adds a '0' to single digits for minutes and seconds
            if (m < 10)
                mm = string.Format("0{0}", m);
            else
                mm = m.ToString();

            if (s < 10)
                ss = string.Format("0{0}", s);
            else
                ss = s.ToString();

            return string.Format("{0}:{1}:{2}", h, mm, ss);
        }

        [STAThread]
        public void StartUp()
        {
            // clear video of any gabage data
            if (MyVideo != null)
                MyVideo.Dispose();

            //setup file dialog and panel width and height
            Text = openFileDialog1.SafeFileName; //+ " - MPlayer 2.0";
            PanelWidth = VideoPanel.Width;
            PanelHeight = VideoPanel.Height;

            //associate file dialog with video
            MyVideo = new Video(openFileDialog1.FileName);
            MyVideo.Owner = VideoPanel;
            VideoPanel.Width = PanelWidth;
            VideoPanel.Height = PanelHeight;

            //setup seek track bar
            SeekTrackBar.Minimum = Convert.ToInt32(MyVideo.CurrentPosition);
            SeekTrackBar.Maximum = Convert.ToInt32(MyVideo.Duration);
            SeekTrackBar.Value = 0;
            PlayPauseBtn.Text = "\u23F8";

            Duration = CalculateTime(MyVideo.Duration);
            PlayingPosition = "00:00:00";
            TimeTextBox.Text = PlayingPosition + " / " + Duration;

            //show first frame of video
            MyVideo.Play();
            MyVideo.Pause();
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select Video File...";
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.DefaultExt = ".mov";
            openFileDialog1.Filter = "MOV Files|*.mov|MPG Files|*.mp4|All Files|*.*";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Console.WriteLine(openFileDialog1.SafeFileName);
            StartUp();
        }

        private void PlayPauseBtn_Click(object sender, EventArgs e)
        {
            if (MyVideo != null)
            {
                if (MyVideo.Playing)
                {
                    MyVideo.Pause();
                    timer1.Stop();
                    PlayPauseBtn.Text = "\u23F5";
                }
                else
                {
                    MyVideo.Play();
                    timer1.Start();
                    PlayPauseBtn.Text = "\u23F8";
                }

            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            if (MyVideo != null)
            {
                MyVideo.StopWhenReady();
                timer1.Stop();
                StartUp();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SeekTrackBar.Value = Convert.ToInt32(MyVideo.CurrentPosition);
            PlayingPosition = CalculateTime(MyVideo.CurrentPosition);
            TimeTextBox.Text = PlayingPosition + " / " + Duration;

            if (PlayingPosition == Duration)
            {
                timer1.Stop();
                StartUp();
            }
        }

        private void SeekTrackBar_Scroll(object sender, EventArgs e)
        {
            if (MyVideo != null)
            {
                MyVideo.CurrentPosition = SeekTrackBar.Value;
            }
            else
            {
                SeekTrackBar.Value = 0;
            }

        }

    }
}
