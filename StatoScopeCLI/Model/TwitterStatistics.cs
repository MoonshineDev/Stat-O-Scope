using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Model
{
    public class TwitterStatistics
    {
        public TimeSpan Duration { get; set; }
        public int Faces { get; set; }
        public int FemaleFaces { get; set; }
        public int MaleFaces { get; set; }
        public IList<string> Pics { get; set; }
    }
}
