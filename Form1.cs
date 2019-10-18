using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Model;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace MediaDownloader
{
    public partial class Form1 : Form
    {
        string fileName = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadVideo("c:\\", textBox1.Text);
        }

        /// <summary>
        /// libvideo kullanýr
        /// </summary>
        /// <param name="saveToFolder"></param>
        /// <param name="VideoURL"></param>        
        private void DownloadVideo(string saveToFolder, string VideoURL)
        {
            YouTube youtube = YouTube.Default;
            YouTubeVideo vid = youtube.GetVideo(VideoURL);

            fileName = saveToFolder + vid.FullName;
            File.WriteAllBytes(fileName, vid.GetBytes());

            MessageBox.Show("Download tamam");
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            SaveMp3(fileName, fileName + ".mp3");
        }

        /// <summary>
        /// MediaToolkit kullanýr. ffmpeg'de kullanýyor mu?
        /// </summary>
        /// <param name="videoFileName"></param>
        /// <param name="mp3Name"></param>
        public void SaveMp3(string videoFileName, string mp3Name)
        {
            MediaFile inputFile = new MediaFile { Filename = videoFileName };
            MediaFile outputFile = new MediaFile { Filename = mp3Name };

            try
            {
                using (Engine engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    engine.Convert(inputFile, outputFile);
                }
            }
            catch (Exception ex)
            {

            }

            MessageBox.Show("mp3 tamam");
        }

        #region kullanilmayan metodlar

        /// <summary>
        /// Xabe.ffmpeg ve ffmpeg.exe kullanýyor
        /// </summary>
        public void GetMp3FromMp4()
        {
            Task<IConversionResult> task = ConvertMp3("c:\\ali_berekat_nahna_siit_haydar_ali.webm", "c:\\haydar_ali.mp3");

            task.Wait();

            MessageBox.Show(task.Result.Success.ToString());
        }

        public async Task<IConversionResult> ConvertMp3(string videoFileName, string mp3Name)
        {
            FFmpeg.ExecutablesPath = @"C:\Program Files\ffmpeg-20191013-4f4334b-win64-static";
            IConversionResult result = await Conversion.ExtractAudio(videoFileName, mp3Name)
                                                       .Start();

            return result;
        }

        #endregion kullanilmayan metodlar        
    }
}
