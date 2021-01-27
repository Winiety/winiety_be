namespace Pictures.Core.Model.Requests
{
    public class AnalyzeRequest
    {
        public int PictureId { get; set; }
        public bool IsRecognized { get; set; }
        public string PlateNumber { get; set; }
    }
}
