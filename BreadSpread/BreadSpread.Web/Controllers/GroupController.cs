using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BreadSpread.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BreadSpread.Web.Controllers
{
	[Authorize]
    public class GroupController : Controller
    {
        private ApplicationIdentityDbContext db = new ApplicationIdentityDbContext();

		/// <summary>
		/// User manager - attached to application DB context
		/// </summary>
		protected UserManager<User> UserManager { get; set; }
		
		public GroupController()
		{
			UserManager = new UserManager<User>(new UserStore<User>(db));
		}

		private GroupViewModel CreateGroupViewModel(Group g)
		{
			return new GroupViewModel
			{
				Id = g.Id,
				CreatedTime = g.CreatedTime,
				Name = g.Name,
				OwnerName = g.OwnerUser.UserName,
				PhotoId = g.PhotoId,
				UserCount = g.Users.Count,
				IsOwner = g.OwnerUser.Id == User.Identity.GetUserId(),
			};
		}

		private GroupDetailViewModel CreateGroupDetailViewModel(Group g)
		{
			return
				new GroupDetailViewModel(CreateGroupViewModel(g))
				{
					Users =
						g.Users
							.OrderBy(u => u.UserName)
							.Select(
								u =>
								new GroupUserViewModel {
									Id = u.Id,
									Name = u.UserName,
									IsOwner = (u == g.OwnerUser) })
							.ToArray()
				};
		}

		// GET: Group
		public async Task<ActionResult> Index()
        {
			User user = await GetCurrentUser();

			var q =
				user.Groups
					.AsQueryable()
					.OrderByDescending(g => g.CreatedTime)
					.Include(g => g.OwnerUser)
					.Include(g => g.Users);

			var groups = q.ToList();

			var results =
				groups.Select(g => CreateGroupViewModel(g));

			return View(results);
        }

        // GET: Group/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
			GroupDetailViewModel viewModel = CreateGroupDetailViewModel(group);

            return View(viewModel);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            return View();
        }

		private async Task<User> GetCurrentUser()
		{
			var result = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			return result;
		}

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,PhotoId,CreatedTime")] Group group)
        {
            if (ModelState.IsValid)
            {
				User user = await GetCurrentUser();

				group.Id = Guid.NewGuid().ToString();
				group.CreatedTime = DateTime.Now;
				group.OwnerUser = user;
				if (group.Users == null)
					group.Users = new List<User>();
				group.Users.Add(user);

                db.Groups.Add(group);
				await db.SaveChangesAsync();

				return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Group/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
			if (group.OwnerUser != await GetCurrentUser())
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			return View(CreateGroupDetailViewModel(group));
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,PhotoId,CreatedTime")] GroupDetailViewModel groupModel)
        {
            if (ModelState.IsValid)
            {
				Group group = await db.Groups.FirstOrDefaultAsync(g => g.Id == groupModel.Id);
				User user = await GetCurrentUser();
				if (group == null || group.OwnerUser != user)
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

				group.Name = groupModel.Name;
				group.PhotoId = groupModel.PhotoId;

				db.Entry(group).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(groupModel);
        }

		// GET: Group/Delete/5
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Group group = await db.Groups.FindAsync(id);
			if (group.OwnerUser.Id != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (group == null)
			{
				return HttpNotFound();
			}
			return View(CreateGroupViewModel(group));
		}

		// POST: Group/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			Group group = await db.Groups.FindAsync(id);
			if (group == null)
			{
				return HttpNotFound();
			}
			if (group.OwnerUser.Id != User.Identity.GetUserId())
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			db.Groups.Remove(group);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		// GET: Group/Leave/5
		public async Task<ActionResult> Leave(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Group group = await db.Groups.FindAsync(id);
			User user = await GetCurrentUser();
			if (!user.Groups.Contains(group))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (group == null)
			{
				return HttpNotFound();
			}
			return View(CreateGroupViewModel(group));
		}

		// POST: Group/Leave/5
		[HttpPost, ActionName("Leave")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> LeaveConfirmed(string id)
		{
			Group group = await db.Groups.FindAsync(id);
			User user = await GetCurrentUser();
			if (!group.Users.Contains(user))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			group.Users.Remove(user);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public ActionResult TestAction()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> RemoveMember(string userId, string groupId)
		{
			var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
			var group = await db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

			if (user == null || group == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			if (!group.Users.Contains(user))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			if (group.OwnerUser != await GetCurrentUser())
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			group.Users.Remove(user);
			await db.SaveChangesAsync();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		public ActionResult Invite(string groupId, string groupName)
		{
			GroupInviteViewModel model = new GroupInviteViewModel();

			model.GroupId = groupId;
			model.GroupName = groupName;
			model.FromUsername = User.Identity.GetUserName();

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Invite(GroupInviteViewModel model)
		{
			Invitation invitation = new Invitation();
			Group group = await db.Groups.FirstOrDefaultAsync(g => g.Id == model.GroupId);
			if (group == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			invitation.Group = group;
			invitation.Id = Guid.NewGuid().ToString();
			db.Invitations.Add(invitation);
			await db.SaveChangesAsync();

			return RedirectToAction("InviteSent");
		}

		public ActionResult InviteSent()
		{
			return View();
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
