using System;

namespace AuebUnofficial.Helpers
{
    public class Article
    {
        private string title;
        private string description;
        private Uri link;
        private string pubDate;
        private int thesis;

        public int Thesis
        {
            get { return thesis; }
            set { thesis = value; }
        }

        public String PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }
        public Uri Link
        {
            get { return link; }
            set { link = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public Article() { }
        public Article(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

    }
}
