namespace ErrorImage
{
    public class Error
    {
        public object code { get; set; }
        public string message { get; set; }
        public object param { get; set; }
        public string type { get; set; }
    }

    public class ImageError
    {
        public Error error { get; set; }
    }


}