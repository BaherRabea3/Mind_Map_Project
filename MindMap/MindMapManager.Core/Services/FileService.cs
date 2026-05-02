using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void DeleteFile(string relativeFile)
        {
            if (string.IsNullOrEmpty(relativeFile))
                return;

            var contentPath = _environment.WebRootPath;
            var fullPath = Path.Combine(contentPath, relativeFile);

            if(File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtension, string FolderName)
        {

            if (imageFile == null || imageFile.Length == 0)
                throw new BadRequestException("File is required");

            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedFileExtension.Contains(ext))
                throw new BadRequestException($"Only {string.Join(",", allowedFileExtension)} are allowed");


            if (imageFile.Length > 2 * 1024 * 1024)
                throw new BadRequestException("File size must not exceed 2MB");

            var ContentPath = _environment.WebRootPath;
            var UploadPath = Path.Combine(ContentPath, "Uploads", FolderName);
            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(UploadPath, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return $"Uploads/{FolderName}/{fileName}";
        }
    }
}
