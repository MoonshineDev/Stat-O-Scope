using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using StatoScope.CLI.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace StatoScope.CLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var downloadService = DownloadService.Create();
            var storageService = StorageService.Create();
            var analysisService = AnalysisService.Create();
            var streamer = TwitterService.Create(downloadService, storageService, analysisService);
            streamer.SetTrackParameter("#twitter");
        }
    }
}
