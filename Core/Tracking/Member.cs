using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class Member {
		private const string RoleAdmin = "Admin";
		private const string RoleEngineer = "Race Engineer";
		private const string RoleDriver = "Driver";

		public string Name { get; set; }
		public string Badge { get; set; }
		public string Nationality { get; set; }
		public bool IsDriver { get; set; }
		public bool IsEngineer { get; set; }
		public bool IsAdmin { get; set; }

		public Member() { }

		public Member(string name, MultiplayerTeamMember member) {
			Name = name;
			Badge = string.IsNullOrEmpty(member.badge) ? null : member.badge;
			Nationality = string.IsNullOrEmpty(member.nationality) ? null : member.nationality;
			if(member.roles != null) {
				foreach(string role in member.roles) {
					switch(role) {
						case RoleAdmin:
							IsAdmin = true;
							break;
						case RoleDriver:
							IsDriver = true;
							break;
						case RoleEngineer:
							IsEngineer = true;
							break;
					}
				}
			}
		}

		public List<string> Roles() {
			List<string> roles = new List<string>();
			if(IsAdmin)
				roles.Add(RoleAdmin);
			if(IsEngineer)
				roles.Add(RoleEngineer);
			if(IsDriver)
				roles.Add(RoleDriver);
			return roles;
		}

		public bool IsSameMember(Member otherMember) {
			return Name == otherMember.Name &&
				Badge == otherMember.Badge &&
				Nationality == otherMember.Nationality &&
				IsAdmin == otherMember.IsAdmin &&
				IsDriver == otherMember.IsDriver &&
				IsEngineer == otherMember.IsEngineer;
		}
	}
}
