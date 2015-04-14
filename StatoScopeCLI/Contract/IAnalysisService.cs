using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Contract
{
    public interface IAnalysisService
    {
        MCvAvgComp[] DetectFaces(string filename);
        int CountFaces(MCvAvgComp[] haarCascade);
        int CountFemaleFaces(MCvAvgComp[] haarCascade);
        int CountMaleFaces(MCvAvgComp[] haarCascade);
    }
}
