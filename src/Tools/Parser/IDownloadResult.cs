using System;

namespace ParserTool
{
    public interface IDownloadResult
    {
        Uri Source { get; }
    }

    public class FailedDownloadResult : IDownloadResult
    {
        public Uri Source { get; }
        public Exception Exception { get; }

        public FailedDownloadResult(Uri source, Exception exception = null)
        {
            Source = source;
            Exception = exception;
        }
    }

    public class HtmlDownloadResult : IDownloadResult
    {
        public Uri Source { get; }

        public string Content { get; }

        public HtmlDownloadResult(Uri source, string content)
        {
            Source = source;
            Content = content;
        }
    }

}