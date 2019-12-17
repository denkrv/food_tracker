using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace ParserTool
{
    public class HtmlDownloadParser : IDownloadResultHandler
    {
        private readonly Func<ICrawlingContext, IDownloadResult, IHtmlDocument, Func<Task>, Task> _pipeline;
        private readonly HtmlParser parser = new HtmlParser();

        public HtmlDownloadParser(Func<ICrawlingContext, IDownloadResult, IHtmlDocument, Func<Task>, Task> pipeline)
        {
            _pipeline = pipeline;
        }
        public bool Accept(IDownloadResult download) => download is HtmlDownloadResult;

        public async Task Handle(ICrawlingContext context, IDownloadResult download)
        {

            var contentResult = (HtmlDownloadResult)download;
            var doc = await parser.ParseDocumentAsync(contentResult.Content);

            var closure = new Func<Task>(() => PublishLinks(context, download, doc));

            await ((_pipeline != null) ? _pipeline.Invoke(context, download, doc, closure) : closure.Invoke());

        }

        private async Task PublishLinks(ICrawlingContext context, IDownloadResult download, IHtmlDocument doc)
        {
            var links = doc.QuerySelectorAll("a[href]").Select(el => el.GetAttribute("href")).ToList();
            var root = new Uri(download.Source.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped), UriKind.Absolute);
            var abs = links.Where(l => CanMakeAbsoluteUri(root, l))
                .Select(l => ToAsboluteUri(root, l));
            foreach (var link in abs)
                await context.AddUri(link);
        }

        public static Uri ToAsboluteUri(Uri root, string rawUri)
        {
            return Uri.IsWellFormedUriString(rawUri, UriKind.Absolute) ? new Uri(rawUri, UriKind.Absolute) : new Uri(root, rawUri);
        }

        public static bool CanMakeAbsoluteUri(Uri root, string rawUri)
        {
            if (Uri.IsWellFormedUriString(rawUri, UriKind.Absolute))
                return true;
            try
            {
                var absUri = new Uri(root, rawUri);
                var returnVal = absUri.Scheme.Equals(Uri.UriSchemeHttp) || absUri.Scheme.Equals(Uri.UriSchemeHttps);
                return returnVal;
            }
            catch
            {
                return false;
            }
        }
    }
}