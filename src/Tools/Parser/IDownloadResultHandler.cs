using System;
using System.Threading.Tasks;

namespace ParserTool
{
    public interface ICrawlingContext
    {
        Task AddUri(Uri uri);

        Task Cancel();
    }

    public interface IDownloadResultHandler
    {
        bool Accept(IDownloadResult download);

        Task Handle(ICrawlingContext context, IDownloadResult download);
    }
}