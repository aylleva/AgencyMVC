using AgencyMVC.Utilities.Enums;

namespace AgencyMVC.Utilities.Extentions
{
    public static class FileValidation
    {
        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file, FileSizes filesize, int size)
        {
            switch (filesize)
            {
                case FileSizes.KB:
                    return file.Length <= size * 1024;
                case FileSizes.MB:
                    return file.Length <= size * 1024 * 1024;
            }
            return false;
        }

        public static string CreatePath(this string file, params string[] roots)
        {
            string path = string.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, file);
            return path;
        }

        public static async Task<string> CreateFileAsync(this IFormFile file, params string[] roots)
        {
            string filename = file.FileName;

            string File = string.Concat(Guid.NewGuid().ToString(), filename.Substring(filename.LastIndexOf(".")));

            using (FileStream fileStream = new FileStream(File.CreatePath(roots), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return File;
        }

        public static void DeleteFile(this string file, params string[] roots)
        {
            File.Delete(file.CreatePath(roots));
        }
    }
}
