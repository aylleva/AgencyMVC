using AgencyMVC.DAL;
using AgencyMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgencyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM()
            {
                Projects=await _context.Projects.Include(p=>p.Category).Include(p=>p.ProjectImages).Take(10)
                 .ToListAsync(),
                Employees=await _context.Employees.Include(e=>e.Position).Include(e=>e.EmployeeImages).Take(8)
                .ToListAsync()
            };
            return View(vm);    
        }
    }
}
