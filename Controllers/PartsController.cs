using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CarWorkshop.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace CarWorkshop.Controllers
{
    public class PartsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public PartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Parts/Details/5
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

            var part = await _context.Parts
                .Include(p => p.Ticket)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
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
            // ModelState.SetModelValue("TicketId", new ValueProviderResult(ticketId.ToString(), CultureInfo.InvariantCulture));
            ViewData["TicketId"] = ticketId;
            var ticket = await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == ticketId);
            if (ticket == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Quantity,TicketId")] PartModel part)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.Remove("Id");
            ModelState.Remove("Ticket");

            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tickets", new { id = part.TicketId });
            }
            return View(part);
        }

        // GET: Parts/Edit/5
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

            var part = await _context.Parts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Guid? id, [Bind("Id,Name,Description,Price,Quantity,TicketId")] PartModel part)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            ModelState.Remove("Ticket");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Tickets", new { id = part.TicketId });
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(part);
        }

        // GET: Parts/Delete/5
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

            var part = await _context.Parts
                .Include(p => p.Ticket)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!_signInManager.IsSignedIn(User) || !((await _userManager.GetUserAsync(User))?.Type == ApplicationUser.UserType.Employee))
            {
                return RedirectToAction("Index", "Home");
            }
            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tickets", new { id = part.TicketId });
        }

        private bool PartExists(Guid id)
        {
            return _context.Parts.Any(e => e.Id == id);
        }
    }
}
