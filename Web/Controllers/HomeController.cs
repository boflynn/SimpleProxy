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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult DownloadFile(string url)
        {
            var fileName = GetFileNameFromUrl(url);
            var data = DownloadUrl(url);
            var mimeType = MimeMapping.GetMimeMapping(fileName);

            return File(data, mimeType, fileName);
        }

        public ActionResult ViewFile(string url)
        {
            var fileName = GetFileNameFromUrl(url);
            var data = DownloadUrl(url);
            var mimeType = MimeMapping.GetMimeMapping(fileName);
            var stream = new MemoryStream(data);

            return new FileStreamResult(stream, mimeType);
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
    }
}