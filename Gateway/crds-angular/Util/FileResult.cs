using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace crds_angular.Util
{
    internal class FileResult : IHttpActionResult
    {
        private readonly MemoryStream _stream;
        private readonly string _fileName;
        private readonly string _contentType;
        private readonly bool _asAttachment;

        public FileResult(MemoryStream stream, String fileName, String contentType = null, bool asAttachment = true)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _stream = stream;
            _fileName = fileName;
            _contentType = contentType;
            _asAttachment = asAttachment;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(_stream)
                };

                var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_fileName));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                if (_asAttachment)
                {
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = _fileName
                    };
                }

                return response;
            },
                            cancellationToken);
        }
    }
}