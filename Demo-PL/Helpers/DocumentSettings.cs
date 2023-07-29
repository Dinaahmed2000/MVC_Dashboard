using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo_PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            //1.get located folder path

            //string folderPath = "D:\\Back_End\\ASP.net core MVC\\Assignments\\assignment5\\assign5\\Demo-PL\\Demo-PL\\wwwroot\\files\\";
            //string folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;
            string folderPath =Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            //2.get file name and make it unique

            string fileName= $"{Guid.NewGuid()}{file.FileName}";
            //3.get file path

            string filePath=Path.Combine(folderPath, fileName);
            //4.save file as streams[Data per time]

            using var fs =new FileStream(filePath,FileMode.Create);
            file.CopyToAsync(fs);
            return fileName;
        }

        public static void deleteFile(string fileName,string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName,fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
