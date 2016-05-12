using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BreadSpread.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BreadSpread.Web.Controllers
{
	[Authorize]
    public class InvitationController : Controller
    {
		public static class Settings
		{
			public static readonly TimeSpan InvitationExpiry = TimeSpan.FromDays(7);
		}

		private ApplicationIdentityDbContext db;

		/// <summary>
		/// User manager - attached to application DB context
		/// </summary>
		protected UserManager<User> UserManager { get; set; }

		public InvitationController()
		{
			db = new ApplicationIdentityDbContext();
			UserManager = new UserManager<User>(new UserStore<User>(db));
		}

		//// GET: Invitation
		//public ActionResult Index()
		//{
		//    return View();
		//}

		private async Task SendEmailInvite(string emailAddress, Invitation invitation, string groupName, string fromUsername)
		{
			// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
			// Send an email with this link

			var callbackUrl =
				Url.Action(
					"AnswerInvite",
					"Invitation",
					new { invitationId = invitation.Id },
					protocol: Request.Url.Scheme);

			string message =
				"BreadSpread Group Invitation<br/><br/>"
				+ "You've been invited to join a group on BreadSpread by user "
				+ fromUsername
				+ ".<br/><br/>"
				+ "Group name: <b>"
				+ groupName
				+ "</b><br/><br/>"
				+ "Click <a href=\""
				+ callbackUrl
				+ "\">here</a> to join the group.<br/><br/>"
				+ "If the above link doesn't work, please copy and paste "
				+ "the following into your address bar and press Enter:<br/><br/>"
				+ callbackUrl;

			await EmailService.SendEmailAsync(emailAddress, "deep.cosmic.mysteries@gmail.com", "BreadSpread Group Invitation", message);
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
			invitation.CreatedTime = DateTime.Now;
			db.Invitations.Add(invitation);
			await db.SaveChangesAsync();

			string destEmail = model.Username;
			var destUser = await UserManager.FindByNameAsync(model.Username);
			if (destUser != null)
				destEmail = destUser.Email;

			await SendEmailInvite(destEmail, invitation, group.Name, model.FromUsername);

			return RedirectToAction("InviteSent");
		}

		public ActionResult InviteSent()
		{
			return View();
		}

		private void RemoveCookie(HttpCookie cookie, string invitationId)
		{
			if (cookie != null)
			{
				for (int i = cookie.Values.Count - 1; i >= 0; --i)
					if (cookie.Values[i] == invitationId)
						cookie.Values.Remove(invitationId);
			}
		}

		private string[] GetCookies(HttpCookie cookie)
		{
			List<string> results = new List<string>();
			if (cookie != null)
			{
				for (int i = 0; i < cookie.Values.Count; ++i)
					results.Add(cookie.Values[i]);
			}
			return results.ToArray();
		}

		private void AddCookie(HttpCookie cookie, string invitationId)
		{
			if (cookie != null)
			{
				RemoveCookie(cookie, invitationId);
				cookie.Values.Add(invitationId, invitationId);
				cookie.Expires = DateTime.Now.AddDays(1);
			}
		}

		[AllowAnonymous]
		public async Task<ActionResult> AnswerInvite(string invitationId)
		{
			if (invitationId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			Invitation invite =
				await db.Invitations
					.Include(i => i.Group)
					.Include(i => i.Group.OwnerUser)
					.FirstOrDefaultAsync(i => i.Id == invitationId);

			if (invite == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			DateTime expiryDate = invite.CreatedTime + Settings.InvitationExpiry;
			if (expiryDate < DateTime.Now)
				return RedirectToAction(
					"Expired",
					new { expiryDate = expiryDate.ToString("yyyy-MM-dd hh:mm tt")});

			var cookie = HttpContext.Request.Cookies["InviteId"];
			if (cookie == null)
				cookie = new HttpCookie("InviteId");
			AddCookie(cookie, invitationId);
			HttpContext.Response.SetCookie(cookie);

			if (!HttpContext.Request.IsAuthenticated)
				return RedirectToAction("Login", "Account");

			InvitationViewModel model =
				new InvitationViewModel
				{
					InvitationId = invite.Id,
					GroupId = invite.Group.Id,
					GroupName = invite.Group.Name,
					OwnerName = invite.Group.OwnerUser.UserName
				};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> AnswerInvite(string button, InvitationViewModel model)
		{
			bool accept = (button == "yes");

			Invitation invite = await db.Invitations.FirstOrDefaultAsync(i => i.Id == model.InvitationId);
			Group group = await db.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == model.GroupId);

			if (invite == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			DateTime expiryTime = invite.CreatedTime + Settings.InvitationExpiry;

			db.Invitations.Remove(invite);
			await db.SaveChangesAsync();

			if (expiryTime < DateTime.Now)
				return RedirectToAction(
					"Expired",
					new { expiryDate = expiryTime.ToString("yyyy-MM-dd hh:mm tt") });

			if (group == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			if (accept)
			{
				User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (!group.Users.Contains(user))
					group.Users.Add(user);
				await db.SaveChangesAsync();
			}

			return RedirectToAction("Index", "Group");
		}

		[AllowAnonymous]
		public ActionResult Expired(string expiryDate)
		{
			ViewBag.ExpiryDate = expiryDate;
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