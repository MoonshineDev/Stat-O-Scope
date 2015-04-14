using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using StatoScope.CLI.Contract;
using StatoScope.CLI.Model;
using System.Diagnostics;

namespace StatoScope.CLI.Service
{
    public class TwitterService : ITwitterService
    {
        private IDownloadService DownloadService { get; set; }
        private IStorageService StorageService { get; set; }
        private IAnalysisService AnalysisService { get; set; }

        private IFilteredStream FilterStream { get; set; }
        private IList<TwitterAnalysis> Analysis { get; set; }
        private Stopwatch sw;

        #region .ctor
        private TwitterService(IDownloadService downloadService, IStorageService storageService, IAnalysisService analysisService)
        {
            DownloadService = downloadService;
            StorageService = storageService;
            AnalysisService = analysisService;
            Analysis = new List<TwitterAnalysis>();
            sw = new Stopwatch();

            // TODO: Remove OAuth API keys from source
            var consumerKey = "";
            var consumerSecret = "";
            var userAccessToken = "";
            var userAccessSecret = "";
            TwitterCredentials.SetCredentials(
                userAccessToken,
                userAccessSecret,
                consumerKey,
                consumerSecret);
            FilterStream = Stream.CreateFilteredStream();
            FilterStream.MatchingTweetReceived += OnMatchingTweetReceived;
            FilterStream.StreamPaused += OnStreamPaused;
            FilterStream.StreamResumed += OnStreamResumed;
            FilterStream.StreamStarted += OnStreamStarted;
            FilterStream.StreamStopped += OnStreamStopped;
        }
        #endregion

        #region Create
        public static TwitterService Create(IDownloadService downloadService, IStorageService storageService, IAnalysisService analysisService)
        {
            if (downloadService == null)
                throw new NullReferenceException("Argument cannot be null: downloadService");
            if (storageService == null)
                throw new NullReferenceException("Argument cannot be null: storageService");
            if (analysisService == null)
                throw new NullReferenceException("Argument cannot be null: analysisService");
            return new TwitterService(downloadService, storageService, analysisService);
        }
        #endregion

        private async Task<TwitterMediaAnalysis> AnalyzeMediaAsync(ITweet tweet, IMediaEntity media)
        {
            var mediaAnalysis = TwitterMediaAnalysis.Create(media);
            switch (media.MediaType)
            {
                case "photo":
                    Console.WriteLine(media.MediaURL);
                    mediaAnalysis.UriString = media.MediaURL;
                    mediaAnalysis.Data = await DownloadService.DownloadAsync(mediaAnalysis.UriString);
                    var fileExt = DownloadService.ToFileExt(mediaAnalysis.UriString);
                    var filename = String.Format("{0:yyyyMMddHHmmss}_{1}_{2}{3}", tweet.CreatedAt, tweet.Id, media.Id, fileExt);
                    mediaAnalysis.Filepath = StorageService.Store(mediaAnalysis.Data, filename);
                    mediaAnalysis.HaarCascade = AnalysisService.DetectFaces(mediaAnalysis.Filepath);
                    mediaAnalysis.Faces = AnalysisService.CountFaces(mediaAnalysis.HaarCascade);
                    mediaAnalysis.FemaleFaces = AnalysisService.CountFemaleFaces(mediaAnalysis.HaarCascade);
                    mediaAnalysis.MaleFaces = AnalysisService.CountMaleFaces(mediaAnalysis.HaarCascade);
                    break;
                default:
                    break;
            }
            return mediaAnalysis;
        }

        public void SetTrackParameter(string track)
        {
            FilterStream.StopStream();
            // TODO: Make Analysis threadsafe
            Analysis.Clear();
            FilterStream.ClearTracks();
            FilterStream.AddTrack(track);
            FilterStream.StartStreamMatchingAllConditions();
        }

        public TwitterStatistics GetStatistics()
        {
            // TODO: Make Analysis threadsafe
            var orderedList = Analysis.OrderBy(x => x.CreatedAt).ToList();
            return new TwitterStatistics {
                Duration = sw.Elapsed,
                Faces = orderedList.Sum(x => x.Faces),
                FemaleFaces = orderedList.Sum(x => x.FemaleFaces),
                MaleFaces = orderedList.Sum(x => x.MaleFaces),
                Pics = orderedList
                    .SelectMany(x => x.MediaAnalysis)
                    .Select(x => x.Filepath)
                    .Take(10)
                    .ToList(),
            };
        }

        private void OnMatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs e)
        {
            var tweet = e.Tweet;
            var mediaList = tweet.Media;
            if (mediaList == null)
                return;
            var mediaAnalysis = mediaList
                .Select(x => AnalyzeMediaAsync(tweet, x))
                .ToArray()
                .Select(x => {
                    x.Wait();
                    return x.Result;
                })
                .Where(x => x != null)
                .ToList();
            var analysis = TwitterAnalysis.Create(tweet, mediaAnalysis);
            // TODO: Make Analysis threadsafe
            Analysis.Add(analysis);
        }

        private void OnStreamPaused(object sender, EventArgs e)
        {
            sw.Stop();
        }

        private void OnStreamResumed(object sender, EventArgs e)
        {
            sw.Start();
        }

        private void OnStreamStarted(object sender, EventArgs e)
        {
            sw.Restart();
        }

        private void OnStreamStopped(object sender, StreamExceptionEventArgs e)
        {
            sw.Stop();
        }
    }
}
