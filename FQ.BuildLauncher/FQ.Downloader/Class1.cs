using System;
using System.IO;
using System.Net;

namespace FQ.Downloader
{
    public class Class1
    {
        public void f()
        {
            string path = "http://www.educative.io/cdn-cgi/image/f=auto,fit=contain,w=2400/api/edpresso/shot/5224207262154752/image/5022165490991104.png";
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(path), "image.png");
            }

            if(File.Exists("image.png"))
            {
                Console.WriteLine("File Downloaded Successfully");
            } 
            else
            {
                Console.WriteLine("Not able to download the file.");
            }        
        }
    }
}