using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FunctionalityMatrixApp.Services.ServerFilesManager
{
    public interface IServerFilesManager
    {
        void DeleteFilesFromServer(IEnumerable<string> picturesNamesToDelete, string filePath);
        Task<List<string>> UploadFilesToServer(IFormFile[] filesToUpload, string filePath);
        string GetEnvFullPath(string path);
    }
}