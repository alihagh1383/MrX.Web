using System.Text;

namespace MrX.Web.FileStorage;

public class LocalStorage : IStorage
{
    private string path;

    public LocalStorage(string Path)
    {
        Directory.CreateDirectory(Path);
        path = Path;
    }

    private string Path(string name) => System.IO.Path.Combine(path, name);

    public bool Exists(string name, out bool exist)
    {
        exist = File.Exists(Path(name));
        return true;
    }

    public bool DownloadFile(string name, out string text)
    {
        if (Exists(name, out var ex) && !ex)
        {
            text = "";
            return false;
        }

        text = File.ReadAllText(Path(name));
        return true;
    }

    public bool UploadFile(string name, in string text)
    {
        File.WriteAllText(Path(name), text, Encoding.UTF8);
        return true;
    }

    public bool DeleteFile(string name)
    {
        if (Exists(name, out var ex) && !ex)
            return false;
        File.Delete(Path(name));
        return true;
    }
}