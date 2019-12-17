using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ParserTool
{
    public interface IDownloader
    {
        bool Accept(Uri uri);

        Task<IDownloadResult> Get(Uri uri, CancellationToken cancellationToken);
    }


    public class HttpDownloader : IDownloader
    {
        private HttpClient _client = new HttpClient();

        public bool Accept(Uri uri)
        {
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }

        public async  Task<IDownloadResult> Get(Uri uri, CancellationToken token)
        {
            try
            {
                var response = await _client.GetAsync(uri, token);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var contentType = response.Content.Headers.ContentType;
                    if (contentType.MediaType == System.Net.Mime.MediaTypeNames.Text.Html)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return new HtmlDownloadResult(uri, content);
                    }
                }

                return new FailedDownloadResult(uri);
            }
            catch (HttpRequestException exception)
            {
                return new FailedDownloadResult(uri, exception);
            }
        }
    }
}