using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Emgu.CV.Structure;

namespace StatoScope.CLI.Model
{
    public class TwitterMediaAnalysis
    {
        public IMediaEntity Media { get; set; }
        public string UriString { get; set; }
        public byte[] Data { get; set; }
        public string Filepath { get; set; }
        public MCvAvgComp[] HaarCascade { get; set; }

        public int Faces { get; set; }
        public int FemaleFaces { get; set; }
        public int MaleFaces { get; set; }

        #region .ctor
        private TwitterMediaAnalysis()
        { }
        #endregion

        #region Create
        public static TwitterMediaAnalysis Create(IMediaEntity media)
        {
            if (media == null)
                throw new NullReferenceException("Argument cannot be null: media");
            return new TwitterMediaAnalysis
            {
                Media = media,
            };
        }
        #endregion
    }
}
