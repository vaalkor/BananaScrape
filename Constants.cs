namespace BananaScrape
{
    public static class Constants
    {
        public static readonly string ClickLoadMoreContentScript =
@"var collection = document.getElementsByClassName(""ExtendedContentButton"");
if(collection.length){collection[0].click();return true;}
else return false
";
    }
}
