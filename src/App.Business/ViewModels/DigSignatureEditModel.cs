namespace App.Business.ViewModels
{
    public class DigSignatureEditModel
    {
        public string FileBase64 { get; set; }
        public string SignaturedFile { get; set; }
        public long ApplicationId { get; set; }
        public string ActionReturn { get; set; }
        public string ControllerReturn { get; set; }
    }
}
