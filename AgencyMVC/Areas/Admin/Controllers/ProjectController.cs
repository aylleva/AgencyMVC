using AgencyMVC.Areas.Admin.ViewModels.Employees;
using AgencyMVC.Areas.Admin.ViewModels.Projects;
using AgencyMVC.DAL;
using AgencyMVC.Models;
using AgencyMVC.Utilities.Enums;
using AgencyMVC.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgencyMVC.Areas.Admin.Controllers
{
    public class ProjectController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string roots = Path.Combine("assets", "images", "img", "team");

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProjectVM> vm = await _context.Projects.Include(p=>p.Category).Include(e => e.ProjectImages)
                .Select(e => new GetProjectVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryName = e.Category.Name,
                    Image = e.ProjectImages[0].Image
                })
                .ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateProjectVM vm = new CreateProjectVM()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectVM vm)
        {
            vm.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProjectVM.Photo), "Wrong Type");
                return View(vm);
            }

            if (!vm.Photo.ValidateSize(FileSizes.MB, 2))
            {

                ModelState.AddModelError(nameof(CreateProjectVM.Photo), "Wrong Size");
                return View(vm);
            }

            bool result = await _context.Categories.AnyAsync(p => p.Id == vm.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProjectVM.CategoryId), "WRONG");
                return View(vm);
            }

            ProjectImage image = new ProjectImage()
            {
                Image = await vm.Photo.CreateFileAsync(_env.WebRootPath, roots)
            };

            Project project = new Project()
            {
               
                Name = vm.Name,
                CategoryId= vm.CategoryId,
                ProjectImages = new List<ProjectImage> { image }
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();
           Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project is null) return NotFound();


            UpdateProjectVM vm = new UpdateProjectVM()
            {
                Name = project.Name,
                CategoryId = project.CategoryId,
                Categories = await _context.Categories.ToListAsync(),
                Images = project.ProjectImages

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProjectVM vm, int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
           var existed = await _context.Projects.FirstOrDefaultAsync(p => p.Id == Id);
            if (existed is null) return NotFound();

            vm.Categories = await _context.Categories.ToListAsync();
            vm.Images = existed.ProjectImages;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Photo is not null)
            {
                if (!vm.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateProjectVM.Photo), "Wrong Type");
                    return View(vm);
                }

                if (!vm.Photo.ValidateSize(FileSizes.MB, 2))
                {

                    ModelState.AddModelError(nameof(UpdateProjectVM.Photo), "Wrong Size");
                    return View(vm);
                }
            }

            if (vm.CategoryId != existed.CategoryId)
            {
                bool result = await _context.Positions.AnyAsync(p => p.Id == vm.CategoryId);
                if (!result)
                {
                    ModelState.AddModelError(nameof(UpdateProjectVM.CategoryId), "WRONG");
                    return View(vm);
                }
            }

            if (vm.Photo is not null)
            {
                string file = await vm.Photo.CreateFileAsync(_env.WebRootPath, roots);

                var image = existed.ProjectImages.FirstOrDefault();
                image.Image.DeleteFile(_env.WebRootPath, roots);
                existed.ProjectImages.Remove(image);

                existed.ProjectImages.Add(new ProjectImage
                {
                    Image = file
                });
            }

            existed.Name = vm.Name;
            existed.CategoryId = vm.CategoryId.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project is null) return NotFound();

            _context.Projects.Remove(project);
            return RedirectToAction(nameof(Index));
        }
    }
}
