using BelgeYonetimi.Data;
using BelgeYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BelgeYonetimi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("IndexPublic");
            }
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (User.Identity.IsAuthenticated && userEmail == "Admin@example.com")
            {
                return RedirectToAction("IndexAdmin","Home");
            }
            else
            {
                return View(_db.UserRequests.Where(x => x.UserId==userId).ToList());
            }
        }

        public IActionResult IndexPublic()
        {
            return View();
        }
        public IActionResult IndexAdmin(string inf)
        {
            IQueryable<UserRequest> requests = _db.UserRequests;
            AdminVM vm = new();

            if (inf == "all")
            {
                vm.AllRequest = requests.ToList();
                return View(vm.AllRequest);
            }
            else if (inf == "true")
            {
                vm.Approved = requests.Where(x => x.ConsiderationStatus == true).ToList();
                return View(vm.Approved);
            }
            else
            {
                vm.Unapproved = requests.Where(x => x.ConsiderationStatus == false).ToList();
            }

            return View(vm.Unapproved);
        }
        
        public IActionResult ShowDocument(string infpath,string infId)
        {

          

            byte[] pdfByte = GetBytesFromFile(infpath);
            return File(pdfByte, "application/pdf");
         
        }

        private byte[] GetBytesFromFile(string filepath)
        {
            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(filepath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        public IActionResult EvaluateRequest(int id)
        {
            var userReq = _db.UserRequests.Find(id);
            if (userReq == null)
            {
                return NotFound();
            }
            RequestEvaluateVM vm = new();
            vm.Id = userReq.Id;
            vm.UserName = userReq.UserName;
            vm.UserLastName = userReq.UserLastName;
            vm.Explanation = userReq.Explanation;
            
            return View(vm);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult EvaluateRequest(RequestEvaluateVM vm)
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
