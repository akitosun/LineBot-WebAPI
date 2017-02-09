namespace Domain
{
    public class ErrorResponseModel
    {
        public string message { get; set; }
        public Detail[] details { get; set; }
    }

    public class Detail
    {
        public string message { get; set; }
        public string property { get; set; }
    }

    public class Rootobject
    {
        public string message { get; set; }
    }
}