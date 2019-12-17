using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Dasync.Collections;
namespace ParserTool
{
    public class Crawler
    {
        private readonly IDownloader[] _downloaders;
        
        private IUriFilter _filter = new AndUriFilter(new AcceptOnceUriFilter(), new CalorizatorProductsUriFilter());
        
        private IDownloadResultHandler[] _handlers;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public Crawler(IDownloader[] downloaders, IDownloadResultHandler[] handlers)
        {
            _downloaders = downloaders;

            _handlers = handlers;
        }

        public async Task Run(Uri start)
        {
            var _channel = Channel.CreateBounded<Uri>(100);
            
            var context = new Context(_cts, _channel);

            await _channel.Writer.WriteAsync(start);
            
            var downloads = RunDownloads(_channel.Reader.ReadAllAsync(_cts.Token));
            await downloads.ParallelForEachAsync(async downloadResult =>
            {
                Console.WriteLine(downloadResult.Source);
                foreach (var handler in _handlers)
                {
                    if (handler.Accept(downloadResult)) await handler.Handle(context, downloadResult);
                }
            }, _cts.Token)
                .ContinueWith(t => Task.CompletedTask);
        }

        private IAsyncEnumerable<IDownloadResult> RunDownloads(IAsyncEnumerable<Uri> uris)
        {
            var channel = Channel.CreateBounded<IDownloadResult>(10);
            var task = uris.Where(_filter.Accept).ParallelForEachAsync(async (t) =>
            {
          //      _cts.CancelAfter(10000);
                var result = await Download(t).ConfigureAwait(false);
                await channel.Writer.WriteAsync(result).ConfigureAwait(false);
            }, _cts.Token).ContinueWith(t =>
            {
                channel.Writer.Complete();
            });

            return channel.Reader.ReadAllAsync();

        }

        async Task<IDownloadResult> Download(Uri uri)
        {
            foreach (var downloader in _downloaders)
            {
                if (downloader.Accept(uri))
                {
                    return await downloader.Get(uri, _cts.Token);
                }
            }

            return new FailedDownloadResult(uri);
        }

        class Context : ICrawlingContext 
        {
            private readonly CancellationTokenSource _tokenSource;
            private readonly Channel<Uri> _channel;

            public Context(CancellationTokenSource tokenSource, Channel<Uri> channel)
            {
                _tokenSource = tokenSource;
                _channel = channel;
            }
            public async Task AddUri(Uri uri)
            {
                await _channel.Writer.WriteAsync(uri, _tokenSource.Token);
            }

            public Task Cancel()
            {
                _tokenSource.Cancel();
                return Task.CompletedTask;
            }
        }

    }
}