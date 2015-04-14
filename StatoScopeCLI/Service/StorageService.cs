using StatoScope.CLI.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Service
{
    public class StorageService : IStorageService
    {
        private static string DEFAULT_DIRNAME = "pics";

        private string Dirname { get; set; }

        #region .ctor
        private StorageService()
        { }
        #endregion

        #region Create
        public static StorageService Create()
        {
            var dirname = DEFAULT_DIRNAME;
            if (!System.IO.Directory.Exists(dirname))
                System.IO.Directory.CreateDirectory(dirname);
            return new StorageService {
                Dirname = dirname,
            };
        }
        #endregion

        public string Store(byte[] data, string filename)
        {
            using (var ms = new MemoryStream(data))
                return Store(ms, filename);
        }

        public string Store(Stream inputStream, string filename)
        {
            var filepath = String.Format("{0}/{1}", Dirname, filename);
            var fileStream = new System.IO.FileStream(filepath, FileMode.CreateNew);
            inputStream.CopyTo(fileStream);
            return filepath;
        }
    }
}
