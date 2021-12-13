using BelgeYonetimi.Data;
using BelgeYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            UserRequest userR = _db.UserRequests.Find(infId);
            var docName = userR.DocumentName;
            if (infpath == userR.Document)
            {
                return File("~/documents/docName", "application/pdf");
            }

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
