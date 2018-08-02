using Omnicx.WebStore.Models.Site;
using System;
using System.Collections.Generic;
using System.Net;

namespace Omnicx.WebStore.Models
{
    [Serializable]
    public class ResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Status { get; set; }

        public string Error { get; set; }

        public string ErrorId { get; set; }

        public string Message { get; set; }

        public string MessageCode { get; set; }

        public T Result { get; set; }

        public List<SnippetModel> Snippets { get; set; }
    }
}
