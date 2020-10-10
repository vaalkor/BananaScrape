namespace BananaScrape
{
    public static class Constants
    {
        //Attempts to click on the Load More content button and returns true if the button was there.
        public static readonly string ClickLoadMoreContentScript =
@"var collection = document.getElementsByClassName(""ExtendedContentButton"");
if(collection.length){collection[0].click();return true;}
else return false
";
    }
}
