namespace ChatGPT
{
    public class Completion{
        public string id {get;set;}
        public string Object {get;set;}
        public int created {get;set;}
        public string model {get;set;}
        public Choice[] choices {get;set;}
        public Usage usage {get;set;}
    }
}