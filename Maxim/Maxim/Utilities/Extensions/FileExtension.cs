using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Maxim.Utilities.Extensions
{
    public static class FileExtension
    {
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType == type;
        }

        public static bool CheckLength(this IFormFile file, int length)
        {
            return file.Length < length * 1024;
        }

        public static string CreateFile(this IFormFile file, string root, string folder)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(root,folder,filename);

            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filename;
        }

        public static void DeleteFile(this string file, string root, string folder)
        {
            string path = Path.Combine(root, folder, file);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
