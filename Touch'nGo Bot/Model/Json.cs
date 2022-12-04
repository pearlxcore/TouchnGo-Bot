namespace Touch_nGo_Bot.Model
{
    internal class Json
    {
        public class Empty
        {
            public string buttonTitle { get; set; }
            public string title { get; set; }
            public string buttonUrl { get; set; }
        }

        public class Module
        {
            public int cartNum { get; set; }
            public List<object> cookies { get; set; }
            public Empty empty { get; set; }
            public bool success { get; set; }
            public string errorTitle { get; set; }
            public string msgInfo { get; set; }
            public List<object> items { get; set; }
        }

        public class Root
        {
            public bool success { get; set; }
            public Module module { get; set; }
        }
    }
}
