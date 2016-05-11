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
				+ "Group name:"
				+ groupName
				+ "<br/><br/>"
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

		public ActionResult AnswerInvite(string invitationId)
		{
			HttpCookie cookie = new HttpCookie("InviteId");
			cookie.Values.Add(null, invitationId);
			HttpContext.Response.SetCookie(cookie);
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