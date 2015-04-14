using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using StatoScope.CLI.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Service
{
    public class AnalysisService : IAnalysisService
    {
        private HaarCascade Face { get; set; }

        #region .ctor
        private AnalysisService(HaarCascade face)
        {
            Face = face;
        }
        #endregion

        #region Create
        public static AnalysisService Create()
        {
            // Load haarcascades for face detection (file grabbed from OpenCV at https://github.com/Itseez/opencv/blob/master/data/haarcascades/haarcascade_frontalface_default.xml)
            var face = new HaarCascade("haarcascade_frontalface_default.xml");
            return new AnalysisService(face);
        }
        #endregion

        public MCvAvgComp[] DetectFaces(string filename)
        {
            // Load image from file
            var image = new Image<Bgr, Byte>(filename);
            // Convert it to Grayscaled
            var gray = image.Convert<Gray, Byte>();
            // Face Detector
            var haar = gray.DetectHaarCascade(
                Face,
                1.2,
                10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));
            // Return matches
            return haar[0];
        }

        public int CountFaces(MCvAvgComp[] haarCascade)
        {
            if (haarCascade == null)
                return 0;
            return haarCascade.Count();
        }

        public int CountFemaleFaces(MCvAvgComp[] haarCascade)
        {
            if (haarCascade == null)
                return 0;
            // TODO: Add gender detection
            Func<MCvAvgComp, bool> isFemale = haar => false;
            return haarCascade.Where(isFemale).Count();
        }

        public int CountMaleFaces(MCvAvgComp[] haarCascade)
        {
            if (haarCascade == null)
                return 0;
            // TODO: Add gender detection
            Func<MCvAvgComp, bool> isMale = haar => false;
            return haarCascade.Where(isMale).Count();
        }
    }
}
