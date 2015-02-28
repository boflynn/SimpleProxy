using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SimpleProxy.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult DownloadFile(string url, bool isBase64Encoded = false)
        {
            var result = ProcessRequest(url, isBase64Encoded, "Download");

            return result;
        }

        public ActionResult ViewFile(string url, bool isBase64Encoded = false)
        {
            var result = ProcessRequest(url, isBase64Encoded, "View");

            return result;
        }

        private ActionResult ProcessRequest(string url, bool isBase64Encoded, string action)
        {
            if (isBase64Encoded)
            {
                url = DecodeBase64(url);
            }

            var fileName = GetFileNameFromUrl(url);
            var data = DownloadUrl(url);
            var mimeType = MimeMapping.GetMimeMapping(fileName);

            if (action == "Download")
            {
                return File(data, mimeType, fileName);
            }
            else
            {
                var stream = new MemoryStream(data);

                return new FileStreamResult(stream, mimeType);
            }
        }

        private string GetFileNameFromUrl(string url)
        {
            return url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
        }

        private byte[] DownloadUrl(string url)
        {
            byte[] data;

            using (var client = new WebClient())
            {
                data = client.DownloadData(url);
            }

            return data;
        }

        private string DecodeBase64(string url)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(url);

                url = System.Text.Encoding.Default.GetString(bytes);
            }
            catch (FormatException)
            {
                // Not base64 string, return original URL
            }

            return url;
        }
    }
}