namespace AppStoreWeb.Models
{
    public class iTunesApiResponse
    {
        public Feed feed { get; set; }
    }

    public class Feed
    {
        public List<Entry> entry { get; set; }
    }

    public class Entry
    {
        public Id id { get; set; }
        public Title title { get; set; }
        public ReleaseDate imReleaseDate { get; set; }
        public Category category { get; set; }
        public Link link { get; set; }
    }

    public class Id
    {
        public string label { get; set; }
    }

    public class Title
    {
        public string label { get; set; }
    }

    public class ReleaseDate
    {
        public string label { get; set; }
        public DateAttributes attributes { get; set; }
    }
    public class DateAttributes
    {
        public string label { get; set; }

    }

    public class Category
    {
        public CategoryAttributes attributes { get; set; }
    }
    public class CategoryAttributes
    {
        public string term { get; set; }
        public string label { get; set; }
        public string href { get; set; }
    }

    public class Link
    {
        public LinkAttributes attributes { get; set; }
    }

    public class LinkAttributes
    {
        public string term { get; set; }
        public string label { get; set; }
        public string href { get; set; }
    }



}
