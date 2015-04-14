using StatoScope.CLI.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StatoScope.CLI.Service
{
    public class DownloadService : IDownloadService
    {
        #region .ctor
        private DownloadService()
        { }
        #endregion

        #region Create
        public static DownloadService Create()
        {
            return new DownloadService();
        }
        #endregion

        public async Task<Bitmap> DownloadBitmapAsync(string uriString)
        { return await DownloadBitmapAsync(new Uri(uriString)); }

        public async Task<Bitmap> DownloadBitmapAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var content = response.Content;
                var stream = await content.ReadAsStreamAsync();
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }

        public async Task<byte[]> DownloadAsync(string uriString)
        { return await DownloadAsync(new Uri(uriString)); }

        public async Task<byte[]> DownloadAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var content = response.Content;
                var data = await content.ReadAsByteArrayAsync();
                return data;
            }
        }

        public string ToFileExt(string uriString)
        {
            var fileExt = Regex.Replace(uriString, @"\?.*$", "");
            fileExt = Regex.Replace(fileExt, @"^(.*\.|[^.]*)", "");
            fileExt = fileExt.Any() ? "." + fileExt : fileExt;
            return fileExt;
        }
    }
}
