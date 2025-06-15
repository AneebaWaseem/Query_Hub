using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repositories;
using Project.Models.Interfaces;
using Project.Models.Repositories;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository ur;
        public UserController(IUserRepository u)
        {
            ur = u;
        }

        public async Task<IActionResult> Index()
        {
            var users = await ur.GetAllUsersAsync();
            return View(users);
        }

    }
}
