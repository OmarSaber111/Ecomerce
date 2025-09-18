using Ecomerce.Core.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Infrastructure.Services
{
    public class IMageManagementService : IImageManagementService
    {
        private readonly IFileProvider _fileProvider;
        public IMageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImgAsync(IFormFileCollection form, string foldername)
        {
            var SaveImagesSrc = new List<string>();
            var ImgDirectory = Path.Combine("wwwroot", "Images", foldername);
            var dictFlag = Directory.Exists(ImgDirectory);
            if (!Directory.Exists(ImgDirectory))
            {
                var dirInfo = Directory.CreateDirectory(ImgDirectory);
            }
            foreach (var item in form)
            {
                if (item != null && item.Length > 0)
                {
                    string? ImgName = item.FileName;
                    var ImgSrc = $"/Images/{foldername}/{ImgName}";
                    var root = Path.Combine(ImgDirectory, ImgName);
                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImagesSrc.Add(ImgSrc);
                }
            }
            return SaveImagesSrc;
        }

        public void DeleteImgAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);
        }
    }
}
