using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Interfaces;

namespace Project.Controllers
{
    public class TagController : Controller
    {
        private readonly IQuestionRepository qr;

        public TagController(IQuestionRepository q)
        {
            qr = q;
        }
        public async Task<ViewResult> Index()
        {
            var t = await qr.getAllTagsAsync();
            return View(t);
        }
    }
}
