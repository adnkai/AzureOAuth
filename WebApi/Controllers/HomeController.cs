using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<String> _notes;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            if (_notes == null) {
                _notes = new List<String> {"","",""};
            }
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(_notes);
        }

        [HttpPost]
        public IActionResult Add(String note) {
            _notes.Add(note);
            if (_notes.Count > 3) {
                _notes.RemoveAt(0);
            }
            return RedirectToAction("Index");
        }
 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
