using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;

namespace library.api
{
    public static class ResultExtensions
    {
    
        public static IResult html(this IResultExtensions extension , string html)
        {
            return new htmlResult(html);
        }

       // private readonly string _html;
       

        private class htmlResult : IResult
        {
            private readonly string _html;
            public htmlResult(string html)
            {
                _html = html;
            }
            public Task ExecuteAsync(HttpContext httpcontext)
            {
                httpcontext.Response.ContentType = MediaTypeNames.Text.Html;
                httpcontext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
                return httpcontext.Response.WriteAsync(_html);

            }
        }

    }

   
}

