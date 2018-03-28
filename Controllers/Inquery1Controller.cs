using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student5.Data;
using Student5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Xrm.Sdk;

namespace Student5.Controllers
{
    [Authorize]
    public class Inquery1Controller : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Inquery1Controller(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;    
        }

        // GET: Inquery1
        public async Task<IActionResult> Index()
        {
            var inquires = _context.Inquery1.Where(i => i.UserId == _userManager.GetUserId(User));
            return View(inquires);
            //return View(await _context.Inquery1.ToListAsync());
        }

        // GET: Inquery1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquery1 = await _context.Inquery1
                .SingleOrDefaultAsync(m => m.InquiryId == id);

            var service = CRM.CrmService.GetServiceProvider();
            Entity crmInquiry = service.Retrieve("drm_inquiry1", inquery1.InquiryId, new Microsoft.Xrm.Sdk.Query.ColumnSet("drm_response"));

            inquery1.Response = crmInquiry.GetAttributeValue<string>("drm_response");

            if (inquery1 == null)
            {
                return NotFound();
            }

            return View(inquery1);
        }

        // GET: Inquery1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquery1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InquiryId,Question,Response,UserId")] Inquery1 inquery1)
        {
            if (ModelState.IsValid)
            {
                inquery1.InquiryId = Guid.NewGuid();
                inquery1.UserId = _userManager.GetUserId(User);

                _context.Add(inquery1);
                await _context.SaveChangesAsync();

                Entity crmInquery = new Entity("drm_inquiry1");
                crmInquery.Id= inquery1.InquiryId;
                crmInquery["drm_question"] = inquery1.Question;
                crmInquery["drm_response"] = inquery1.Response;
                crmInquery["drm_contact"] = new EntityReference("contact",Guid.Parse(_userManager.GetUserId(User)));

                var service = CRM.CrmService.GetServiceProvider();
                service.Create(crmInquery);

                return RedirectToAction("Index");
            }
            return View(inquery1);
        }

        // GET: Inquery1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquery1 = await _context.Inquery1.SingleOrDefaultAsync(m => m.InquiryId == id);
            if (inquery1 == null)
            {
                return NotFound();
            }
            return View(inquery1);
        }

        // POST: Inquery1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InquiryId,Question,Response,UserId")] Inquery1 inquery1)
        {
            if (id != inquery1.InquiryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    inquery1.UserId = _userManager.GetUserId(User);
                    _context.Update(inquery1);

                    await _context.SaveChangesAsync();

                    var service = CRM.CrmService.GetServiceProvider();

                    var CrmInquiry = service.Retrieve("drm_inquiry1", inquery1.InquiryId, new Microsoft.Xrm.Sdk.Query.ColumnSet("drm_question"));
                    CrmInquiry["drm_question"] = inquery1.Question;

                    service.Update(CrmInquiry);

                       

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Inquery1Exists(inquery1.InquiryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(inquery1);
        }

        // GET: Inquery1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquery1 = await _context.Inquery1
                .SingleOrDefaultAsync(m => m.InquiryId == id);


        



            if (inquery1 == null)
            {
                return NotFound();
            }

            return View(inquery1);
        }

        // POST: Inquery1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var inquery1 = await _context.Inquery1.SingleOrDefaultAsync(m => m.InquiryId == id);
            _context.Inquery1.Remove(inquery1);

            var service = CRM.CrmService.GetServiceProvider();

            service.Delete("drm_inquiry1", inquery1.InquiryId);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool Inquery1Exists(Guid id)
        {
            return _context.Inquery1.Any(e => e.InquiryId == id);
        }
    }
}
