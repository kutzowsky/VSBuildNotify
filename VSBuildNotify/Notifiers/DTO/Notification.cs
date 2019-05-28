namespace VSBuildNotify.Notifiers.DTO
{
    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public Notification(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
