namespace ChatGPT
{
    public class Data
    {
        public string url { get; set; }
    }

    public class Images
    {
        public int created { get; set; }
        public List<Data> data { get; set; }
    }
}