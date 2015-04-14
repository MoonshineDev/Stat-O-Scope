using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Contract
{
    public interface IStorageService
    {
        string Store(byte[] data, string filename);
        string Store(Stream inputStream, string filename);
    }
}
