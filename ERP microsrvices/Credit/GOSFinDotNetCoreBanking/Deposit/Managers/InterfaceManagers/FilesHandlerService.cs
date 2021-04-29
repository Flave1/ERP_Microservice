using Deposit.Data;
using GODP.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Deposit.Contracts.Response;
using Deposit.Managers.Interface.temp;

namespace Deposit.Managers.InterfaceManagers
{
    public class FilesHandlerService : IFilesHandlerService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _env;
        public FilesHandlerService(IHttpContextAccessor accessor, DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _dataContext = dataContext;
            _accessor = accessor;
        }

        Deposit_req_response IFilesHandlerService.SaveSingleFile(IFormFile file)
        {
            var response = new Deposit_req_response();
            if (file != null)
            {

                if (file.Length > 0)
                {
                    if (file.FileName.Split('.').Length > 2)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Character detected in file Name";
                        return response;
                    }

                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(folderName);
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    }
                    var guid = Guid.NewGuid();

                    var fileName = $"{guid}-{DateTime.Now.ToString().Split(" ")[1].Replace(':', '-')}.{file.FileName.Split('.')[1]}";

                    var filePath = Path.Combine(pathToSave, fileName);
                     
                    using (FileStream filestrem = File.Create(filePath))
                    {
                        file.CopyTo(filestrem);
                        filestrem.Flush();
                    }
                    response.Status.IsSuccessful = true;
                    response.Status.Message.SearchResultMessage = filePath;
                    return response;
                }
                else
                {
                    response.Status.Message.FriendlyMessage = "No file selected";
                    response.Status.IsSuccessful = true;
                    return response;
                }

            }
            response.Status.Message.FriendlyMessage = "File Not Found";
            return response;
        }

 

        public async Task<string> Save_ustomer_thumbs_Async(long cutomerid)
        {
            try
            {
                var db_item = new Deposit_customer_thumbs();
                var files = _accessor.HttpContext.Request.Form.Files;

                foreach (var upload in files)
                {
                    if (upload.FileName.Split('.').Length > 2)
                        return "Invalid characters found in file name";

                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var guid = new Guid();
                    var fileName = $"{guid}{DateTime.UtcNow.Date.ToString("MMM/DD/YYY")}";
                    var type = upload.ContentType;

                    string path = Path.Combine(_env.WebRootPath, "Resources");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fullPath = _env.WebRootPath + "/Resources/" + fileName;

                    using (FileStream filestrem = File.Create(fullPath))
                    {
                        await upload.CopyToAsync(filestrem);
                        await filestrem.FlushAsync();
                    }

                    db_item.FilePath = fullPath;
                    db_item.FileName = "/Resources/" + fileName;
                    db_item.Type = type;
                    db_item.Extention = upload.FileName.Split('.')[1];
                    db_item.CustomerId = cutomerid;

                    _dataContext.Deposit_customer_thumbs.Add(db_item);
                    await _dataContext.SaveChangesAsync();
                }
                return await Task.Run(() => "success");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }






}