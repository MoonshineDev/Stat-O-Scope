using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatoScope.CLI.Contract
{
    public interface IDownloadService
    {
        Task<Bitmap> DownloadBitmapAsync(string uriString);
        Task<Bitmap> DownloadBitmapAsync(Uri uri);
        Task<byte[]> DownloadAsync(string uriString);
        Task<byte[]> DownloadAsync(Uri uri);

        string ToFileExt(string uriString);
    }
}
