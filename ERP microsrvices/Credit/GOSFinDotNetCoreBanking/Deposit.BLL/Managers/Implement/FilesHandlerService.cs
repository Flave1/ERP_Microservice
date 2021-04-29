using Deposit.DomainObjects.Deposit;
using Deposit.Handlers.Deposit.Signature_thumb_upload;
using Deposit.Data;
using GODP.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Deposit.Repository.Implement.Deposit
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

        public async Task<string> HandleForSignatures_thumbs_Async(long item_id, List<Uploads> uploads)
        { 
            var db_item = _dataContext.deposit_file_uploads.FirstOrDefault(e => e.TargetId == item_id);
            if (db_item == null)
                db_item = new deposit_file_uploads();
            var files = _accessor.HttpContext.Request.Form.Files;


            var uploads_and_names = files.Zip(uploads, (n, w) => new { File = n, Name = w });
            foreach (var upload in uploads_and_names)
            { 
                if (upload.File.FileName.Split('.').Length > 2)
                    return "Invalid characters found in file name";

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var guid = new Guid();
                var fileName = $"{guid}-{item_id}-{upload.Name.Name}";
                var type = upload.File.ContentType;
                 
                var fullPath = _env.WebRootPath + "/Resources/" + fileName;
                var dbPath = _env.WebRootPath + "/Resources/" + fileName;

                using (FileStream filestrem = File.Create(fullPath))
                {
                    await upload.File.CopyToAsync(filestrem);
                    await filestrem.FlushAsync();
                }

                db_item.FullPath = fullPath;
                db_item.FileName = "/Resources/" + fileName;
                db_item.DbPath = dbPath;
                db_item.Type = type;
                db_item.Extention = upload.File.FileName.Split('.')[1];
                db_item.TargetId = item_id;

                if (db_item.ID < 1)
                    _dataContext.Add(db_item);

                await _dataContext.SaveChangesAsync();
            }
            return await Task.Run(() => "success");
        }

    }


    public interface IFilesHandlerService 
    {
        Task<string> HandleForSignatures_thumbs_Async(long item_id, List<Uploads> uploads);
    }

}