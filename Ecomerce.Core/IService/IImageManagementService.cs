using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.IService
{
    public interface IImageManagementService
    {
        Task<List<string>> AddImgAsync(IFormFileCollection form, string foldername);
        void DeleteImgAsync(string src);
    }
}
