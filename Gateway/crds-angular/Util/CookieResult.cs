using System;
using System.Collections.Generic;
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
    class CookieResult : IHttpActionResult
    {
        private readonly List<CookieHeaderValue> _cookies;
        private readonly IHttpActionResult _result;

        public CookieResult(List<CookieHeaderValue> cookies, IHttpActionResult result)
        {
            _cookies = cookies;
            _result = result;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var response = _result.ExecuteAsync(cancellationToken).Result;
                response.Headers.AddCookies(_cookies);
                return response;
            },
            cancellationToken);
        }
    }
}
