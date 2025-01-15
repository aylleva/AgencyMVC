using AgencyMVC.Areas.Admin.ViewModels.Employees;
using AgencyMVC.DAL;
using AgencyMVC.Models;
using AgencyMVC.Utilities.Enums;
using AgencyMVC.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AgencyMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string roots = Path.Combine("assets", "images", "img", "team");

        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
          _context = context;
           _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> vm=await _context.Employees.Include(e=>e.Position).Include(e=>e.EmployeeImages)
                .Select(e=> new GetEmployeeVM
                {
                    Id = e.Id,
                    Name= e.Name,
                    PositionName=e.Position.Name,
                    Image = e.EmployeeImages[0].Image
                })
                .ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM vm = new CreateEmployeeVM()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM vm)
        {
            vm.Positions = await _context.Positions.ToListAsync();

            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "Wrong Type");
                return View(vm);
            }

            if (!vm.Photo.ValidateSize(FileSizes.MB, 2))
            {

                ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "Wrong Size");
                return View(vm);
            }

            bool result = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if(!result)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.PositionId), "WRONG");
                return View(vm);
            }

            EmployeeImage image = new EmployeeImage()
            {
                Image = await vm.Photo.CreateFileAsync(_env.WebRootPath, roots)
            };

            Employee employee = new Employee()
            {
                Name = vm.Name,
                PositionId = vm.PositionId,
                EmployeeImages=new List<EmployeeImage> { image }
            };

            await _context.Employees.AddAsync(employee);
           await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null  || id<1) return BadRequest();
            Employee? employee=await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if(employee is null) return NotFound();


            UpdateEmployeeVM vm = new UpdateEmployeeVM()
            {
                Name = employee.Name,
                PositionId = employee.PositionId,
                Positions = await _context.Positions.ToListAsync(),
                Images = employee.EmployeeImages

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeVM vm,int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Employee? existed = await _context.Employees.FirstOrDefaultAsync(p => p.Id == Id);
            if (existed is null) return NotFound();

            vm.Positions = await _context.Positions.ToListAsync();
            vm.Images=existed.EmployeeImages;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if(vm.Photo is not null)
            {
                if (!vm.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "Wrong Type");
                    return View(vm);
                }

                if (!vm.Photo.ValidateSize(FileSizes.MB, 2))
                {

                    ModelState.AddModelError(nameof(UpdateEmployeeVM.Photo), "Wrong Size");
                    return View(vm);
                }
            }

            if(vm.PositionId!=existed.PositionId)
            {
                bool result = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
                if (!result)
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVM.PositionId), "WRONG");
                    return View(vm);
                }
            }

            if (vm.Photo is not null)
            {
                string file = await vm.Photo.CreateFileAsync(_env.WebRootPath, roots);

                EmployeeImage? image= existed.EmployeeImages.FirstOrDefault();
                image.Image.DeleteFile(_env.WebRootPath,roots);
                existed.EmployeeImages.Remove(image);

                existed.EmployeeImages.Add(new EmployeeImage
                {
                    Image = file
                });
            }

            existed.Name = vm.Name;
            existed.PositionId = vm.PositionId.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (employee is null) return NotFound();

            _context.Employees.Remove(employee);
            return RedirectToAction(nameof(Index));
        }


    }
}
