using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BreadSpread.Web;

namespace BreadSpread.Web.Controllers
{
	[RequireHttps]
	[Authorize]
    public class GroupController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();

        // GET: Group
        public async Task<ActionResult> Index()
        {
            var userGroups = db.UserGroups.Include(u => u.OwnerUser);
            return View(await userGroups.ToListAsync());
        }

        // GET: Group/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = await db.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,PhotoId,OwnerUserId,CreatedTime")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                db.UserGroups.Add(userGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", userGroup.OwnerUserId);
            return View(userGroup);
        }

        // GET: Group/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = await db.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", userGroup.OwnerUserId);
            return View(userGroup);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,PhotoId,OwnerUserId,CreatedTime")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", userGroup.OwnerUserId);
            return View(userGroup);
        }

        // GET: Group/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = await db.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            UserGroup userGroup = await db.UserGroups.FindAsync(id);
            db.UserGroups.Remove(userGroup);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
