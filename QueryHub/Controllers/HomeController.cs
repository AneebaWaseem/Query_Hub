using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Data;
using Project.Models.Interfaces;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IQuestionRepository qr;
        private readonly IAnswerRepository ar;
        private readonly IUserRepository ur;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IQuestionRepository q, IAnswerRepository ar, IUserRepository u)
        {
            _logger = logger;
            _context = context;
            qr = q;
            this.ar = ar;
            ur = u;
        }

        [Authorize(Policy = "Policy")]
        public async Task<IActionResult> Index()
        {
            var questions = await qr.GetAllQuestionsAsync();
            return View(questions);
        }

        public ViewResult NotLoggedIn()
        {
            return View();
        }

        [Authorize(Policy = "Roles")]
        public IActionResult Owner()
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
