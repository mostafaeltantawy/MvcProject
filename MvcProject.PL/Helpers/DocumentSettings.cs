using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MvcProject.PL.Helpers
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //string FolderPath =  Directory.CreateDirectory(FolderName) + "\\wwwroot\\Files\\" + FolderName ;

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName); 

            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            string FilePath= Path.Combine(FolderPath, FileName);

            using var Fs = new FileStream(FilePath , FileMode.Create);
            file.CopyTo(Fs);

            return FileName;

        }


        // Delete

        public static void DeleteFile(string FileName, string FolderName) 
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files" , FolderName , FileName);

            if (File.Exists(FolderPath)) 
            {
                File.Delete(FolderPath);
            }
        }

    }
}
