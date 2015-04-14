using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace StatoScope.CLI.Model
{
    public class TwitterAnalysis
    {
        public ITweet Tweet { get; private set; }
        public IList<TwitterMediaAnalysis> MediaAnalysis { get; private set; }

        public DateTime CreatedAt { get { return Tweet.CreatedAt; } }
        public int Faces { get { return MediaAnalysis.Sum(x => x.Faces); } }
        public int FemaleFaces { get { return MediaAnalysis.Sum(x => x.FemaleFaces); } }
        public int MaleFaces { get { return MediaAnalysis.Sum(x => x.MaleFaces); } }

        #region .ctor
        private TwitterAnalysis()
        { }
        #endregion

        #region Create
        public static TwitterAnalysis Create(ITweet tweet, IEnumerable<TwitterMediaAnalysis> mediaAnalysis)
        { return Create(tweet, mediaAnalysis.ToList()); }

        public static TwitterAnalysis Create(ITweet tweet, IList<TwitterMediaAnalysis> mediaAnalysis)
        {
            if (tweet == null)
                throw new NullReferenceException("Argument cannot be null: tweet");
            if (mediaAnalysis == null)
                throw new NullReferenceException("Argument cannot be null: mediaAnalysis");
            if (mediaAnalysis.Any(x => !tweet.Media.Contains(x.Media)))
                throw new ArgumentException("One or more mediaAnalysis is not part of tweet");
            return new TwitterAnalysis {
                Tweet = tweet,
                MediaAnalysis = mediaAnalysis,
            };
        }
        #endregion
    }
}
