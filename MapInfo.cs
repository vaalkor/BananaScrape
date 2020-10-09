namespace BananaScrape
{
    public class MapInfo
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int PostCount { get; set; }
        public bool WasFeatured { get; set; }

        public MapInfo(string name, string link, int viewCount, int likeCount, int postCount, bool wasFeatured)
        {
            Name = name;
            Link = link;
            ViewCount = viewCount;
            LikeCount = likeCount;
            PostCount = postCount;
            WasFeatured = wasFeatured;
        }
    }
}
