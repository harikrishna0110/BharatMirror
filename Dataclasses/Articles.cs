using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BharatMirror.Dataclasses
{
    public class Articles
    {
        public Source source { get; set; }
        public String author { get; set; }
        public String description { get; set; }

        public String title { get; set; }

        public String url { get; set; }

        public String urlToImage { get; set; }

        public String publishedAt { get; set; }

        public string content { get; set; }
    }
}
