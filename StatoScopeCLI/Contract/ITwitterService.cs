using StatoScope.CLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Contract
{
    public interface ITwitterService
    {
        void SetTrackParameter(string track);
        TwitterStatistics GetStatistics();
    }
}
