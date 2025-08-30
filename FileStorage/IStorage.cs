namespace MrX.Web.FileStorage
{
    public interface IStorage
    {
        public bool Exists(string Name, out bool exist);
        public bool DownloadFile(string Name, out string Text);
        public bool UploadFile(string Name, in string Text);
        public bool DeleteFile(string Name);
    }
}
