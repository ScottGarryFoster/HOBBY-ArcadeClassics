using System;
using System.IO;
using System.Net;
using System.Windows.Input;
using FQ.Libraries.Tools.HighLevelElements;

namespace FQ.BuildLauncher
{
    public class DownloaderViewmodel : NotifyPropertyChanged
    {
        public ICommand DownloadButton { get; set; }
        
        public string DownloadOutput
        {
            get => downloadOutput;
            set
            {
                if (value != this.downloadOutput)
                {
                    downloadOutput = value;
                    OnPropertyChanged(nameof(DownloadOutput));
                }
            }
        }
        private string downloadOutput;

        private WebClient webClient;
        
        public DownloaderViewmodel()
        {
            this.DownloadButton = new Command(_ => { OnDownloadButton();}, _ => true);
            this.webClient = new WebClient();

            this.webClient.DownloadProgressChanged += OnDownloadProgressChanged;
            this.webClient.DownloadDataCompleted += OnDownloadCompleted;
        }

        private void OnDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            
            if(File.Exists("presentation.pptx"))
            {
                DownloadOutput = "File Downloaded Successfully";
            } 
            else
            {
                DownloadOutput = "Not able to download the file.";
            } 
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                DownloadOutput = "File Downloaded Successfully";
                return;
            }
            
            DownloadOutput = $"Downloading: {e.ProgressPercentage} / 100";
        }

        private void OnDownloadButton()
        {
            string path = "https://drive.google.com/uc?export=download&id=1Wl9m3ZPmFica144v9tpNWrnDI3qU9PIU";
            this.webClient.DownloadFileAsync(new Uri(path), "presentation.pptx");
        }
    }
}