﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreadSpread.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
		public DateTime CreatedTime { get; set; }

		public string PhotoId { get; set; }

		public virtual ICollection<Group> Groups { get; set; }

		public virtual ICollection<Group> OwnedGroups { get; set; }

		public virtual ICollection<Invitation> Invitations { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
			
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}