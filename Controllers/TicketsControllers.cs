using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CarWorkshop.Models;

namespace CarWorkshop.Controllers
{
    public class TicketsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            var Tickets = _context.Tickets
                .AsNoTracking();
            return View(await Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
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

            var ticket = await _context.Tickets
                .Include(c => c.Parts)
                .Include(c => c.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarBrand,CarModel,Year,LicensePlate,Description,PaidAmount,TicketStatus,Estimation")] TicketModel ticket)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.Remove("Id");
            ModelState.Remove("Parts");
            ModelState.Remove("Tasks");

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
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

            var ticket = await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, [Bind("Id,CarBrand,CarModel,Year,LicensePlate,Description,PaidAmount,TicketStatus,Estimation")] TicketModel ticket)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            ModelState.Remove("Parts");
            ModelState.Remove("Tasks");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
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

            var ticket = await _context.Tickets
                .Include(c => c.Parts)
                .Include(c => c.Tasks)
                .ThenInclude(c => c.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
