namespace BackEndProject.Utils;

public static class Extension
{
    public static bool CheckFileSize(this IFormFile file, int size) => file.Length / 1024 < size;

    public static bool CheckFileType(this IFormFile file, string fileType) => file.ContentType.Contains($"{fileType}/");

    public async static Task<string> SaveImg(this IFormFile file, string root, string folder, string folder2, string folder3 )
    {
        string path = Path.Combine(root, folder, folder2, folder3);
        string fileName = Guid.NewGuid().ToString() + file.FileName;
        string resultPath = Path.Combine(path, fileName);
        using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return fileName;
    }
}
