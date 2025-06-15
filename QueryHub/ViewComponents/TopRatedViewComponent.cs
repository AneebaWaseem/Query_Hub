using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Microsoft.EntityFrameworkCore;

namespace Project.ViewComponents
{
    public class TopRatedViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TopRatedViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.Rating)
                .Take(10)
                .ToListAsync();

            return View(users);
        }
    }

}
