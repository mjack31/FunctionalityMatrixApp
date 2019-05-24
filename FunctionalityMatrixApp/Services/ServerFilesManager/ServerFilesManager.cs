using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionalityMatrixApp.Services.ServerFilesManager
{
    public class ServerFilesManager : IServerFilesManager
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ServerFilesManager(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>> UploadFilesToServer(IFormFile[] filesToUpload, string filePath)
        {
            var fileNames = new List<string>();
            foreach (var fileToUpload in filesToUpload)
            {
                var uniqueName = GetUniqueFileName(fileToUpload.FileName);
                var path = Path.Combine(filePath, uniqueName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fileToUpload.CopyToAsync(fileStream);
                }

                fileNames.Add(uniqueName);
            }

            return fileNames;
        }

        public void DeleteFilesFromServer(IEnumerable<string> picturesNamesToDelete, string filePath)
        {
            foreach (var picture in picturesNamesToDelete)
            {
                var path = Path.Combine(filePath, picture);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        public string GetEnvFullPath(string path)
        {
            return webHostEnvironment.WebRootPath + path;
        }

        private string GetUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            var guid = Guid.NewGuid().ToString().Substring(0, 10);

            return $"{fileName}_{guid}{extension}";
        }
    }
}
