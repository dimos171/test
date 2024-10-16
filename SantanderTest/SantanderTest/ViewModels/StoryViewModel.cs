namespace SantanderTest.ViewModels
{
    public class StoryViewModel
    {
        public string PostedBy { get; set; }

        public int Score { get; set; }

        public string Title { get; set; }

        public string Uri { get; set; }

        //TODO Gett more info about date format to return
        public DateTime Time { get; set; }

        //TODO Get more info what is it
        public int CommentCount { get; set; }
    }
}

