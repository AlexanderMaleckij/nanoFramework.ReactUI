using nanoFramework.ResourcesMetadata;
using nanoFramework.Runtime.Native;
using System;
using System.Diagnostics;
using System.Net;
using System.Resources;
using System.Threading;

namespace nanoFramework.ReactUI
{
    public sealed class ResourcesServer
    {
        private const int ResourceChunkReadSize = 8192;
        private readonly HttpListener _listener;
        private readonly ResourceManager _resourceManager;
        private CancellationTokenSource _tokenSource;
        private Thread _serverThread;

        public ResourcesServer()
        {
            _listener = new HttpListener("http", 80);
            _resourceManager = new ResourceManager("nanoFramework.ReactUI.Resources", typeof(ResourcesServer).Assembly);
        }

        public void Start()
        {
            Stop();

            _tokenSource = new CancellationTokenSource();

            _listener.Start();

            _serverThread = new Thread(() => Serve(_listener, _resourceManager, _tokenSource.Token));

            _serverThread.Start();
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            _serverThread?.Join();
            _listener.Stop();
        }

        private static void Serve(HttpListener listener, ResourceManager resourceManager, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var context = listener.GetContext();

                    try
                    {
                        var uriPath = GetUriPath(context.Request);
                        var resourceMetadata = ResourcesMetadataProvider.FindByUriPath(uriPath);

                        if (resourceMetadata is null)
                        {
                            context.Response.StatusCode = 404;
                        }
                        else
                        {
                            context.Response.ContentLength64 = resourceMetadata.Size;
                            context.Response.ContentType = resourceMetadata.ContentType;

                            if (resourceMetadata.ContentEncoding != null)
                            {
                                context.Response.Headers.Set("Content-Encoding", resourceMetadata.ContentEncoding);
                            }

                            for (var position = 0; position < resourceMetadata.Size; position += ResourceChunkReadSize)
                            {
                                var chunk = (byte[])ResourceUtility.GetObject(resourceManager, resourceMetadata.Id, position, ResourceChunkReadSize);
                                context.Response.OutputStream.Write(chunk, 0, chunk.Length);
                            }

                            context.Response.OutputStream.Flush();
                        }
                    }
                    finally
                    {
                        context.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Resources Server: {ex.Message}\r\nStack = {ex.StackTrace}");
                }
            }
        }

        private static string GetUriPath(HttpListenerRequest request)
        {
            var resourceName = request.RawUrl;
            var hashIndex = resourceName.IndexOf('#');

            if (hashIndex != -1)
            {
                resourceName = resourceName.Substring(0, hashIndex);
            }
            else if (resourceName.EndsWith("/"))
            {
                resourceName += "index.html";
            }

            return resourceName;
        }
    }
}
