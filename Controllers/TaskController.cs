using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CarWorkshop.Models;
using NpgsqlTypes;

namespace CarWorkshop.Controllers
{
    public class TasksController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }

            var EmployeeId = (await _userManager.GetUserAsync(User))!.Id;
            var Tasks = _context.Tasks
                .Where(c => c.EmployeeId == EmployeeId)
                .OrderBy(c => c.WorkTime.LowerBound)
                .Include(c => c.Employee)
                .Include(c => c.Ticket)
                .AsNoTracking();
            ViewBag.EmployeeId = EmployeeId;
            return View(await Tasks.ToListAsync());

        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(c => c.Employee)
                .Include(c => c.Ticket)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create(Guid? ticketId)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ticketId == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = ticketId;
            var ticket = await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == ticketId);
            if (ticket == null)
            {
                return NotFound();
            }
            var employeeId = (await _userManager.GetUserAsync(User))!.Id;
            ViewData["EmployeeId"] = employeeId;
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,TicketId,Description,WorkTime,PricePerHour")] TaskInputModel task)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.Remove("Id");
            ModelState.Remove("Employee");
            ModelState.Remove("Ticket");

            ViewData["EmployeeId"] = task.EmployeeId;
            ViewData["TicketId"] = task.TicketId;

            TaskModel task1 = new TaskModel
            {
                Id = Guid.NewGuid(),
                EmployeeId = task.EmployeeId,
                TicketId = task.TicketId,
                Description = task.Description,
                WorkTime = new NpgsqlRange<DateTime>(task.WorkTime.LowerBound.ToUniversalTime(), task.WorkTime.UpperBound.ToUniversalTime()),
                PricePerHour = task.PricePerHour,
                Employee = task.Employee,
                Ticket = task.Ticket
            };

            if (ModelState.IsValid)
            {
                var tasks = _context.Tasks
                    .Where(c => c.EmployeeId == task.EmployeeId && !c.WorkTime.Intersect(task1.WorkTime).IsEmpty)
                    .AsNoTracking();
                if (tasks.Any())
                {
                    ModelState.AddModelError("WorkTime", "The task intersects with another task");
                    return View(task1);
                }

                _context.Add(task1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task1);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, [Bind("Id,EmployeeId,TicketId,Description,WorkTime,PricePerHour")] TaskInputModel task)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            ModelState.Remove("Employee");
            ModelState.Remove("Ticket");

            TaskModel task1 = new TaskModel
            {
                Id = task.Id,
                EmployeeId = task.EmployeeId,
                TicketId = task.TicketId,
                Description = task.Description,
                WorkTime = new NpgsqlRange<DateTime>(task.WorkTime.LowerBound.ToUniversalTime(), task.WorkTime.UpperBound.ToUniversalTime()),
                PricePerHour = task.PricePerHour,
                Employee = task.Employee,
                Ticket = task.Ticket
            };

            if (ModelState.IsValid)
            {
                try
                {
                    var tasks = _context.Tasks
                        .Where(c => c.EmployeeId == task.EmployeeId && !c.WorkTime.Intersect(task1.WorkTime).IsEmpty)
                        .AsNoTracking();
                    if (tasks.Any())
                    {
                        ModelState.AddModelError("WorkTime", "The task intersects with another task");
                        return View(task1);
                    }

                    _context.Update(task1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task1);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(c => c.Ticket)
                .Include(c => c.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(Guid id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
