using BelgeYonetimi.Data;
using BelgeYonetimi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BelgeYonetimi.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ApplicationDbContext _db;

        public RequestsController(IHostingEnvironment hostingEnv, ApplicationDbContext db)
        {
            _hostingEnv = hostingEnv;
            _db = db;
        }
        public IActionResult NewRequest()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult NewRequest(RequestVM requestVM)
        {


            if (requestVM.File != null)
            {
                var fileName = Path.GetFileName(requestVM.File.FileName);
                //judge if it is pdf file
                string ext = Path.GetExtension(requestVM.File.FileName);
                if (ext.ToLower() != ".pdf")
                {
                    return View();
                }

                var filePath = Path.Combine(_hostingEnv.WebRootPath, "documents", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    requestVM.File.CopyToAsync(fileSteam);
                }
                //your logic to save filePath to database, for example

                UserRequest user = new();
                user.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user.UserName = requestVM.UserName;
                user.UserLastName = requestVM.UserLastName;
                user.Explanation = requestVM.Explanation;
                user.Document = filePath;
                user.DocumentName = fileName;
                _db.UserRequests.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
    }
}
