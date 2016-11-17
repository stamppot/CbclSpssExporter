using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// temporarily here. Should be moved to eLab.Presentation or similar (since it's used in views?!)
namespace General.Helpers.Datastructures
{

    /// <summary>
    /// contains data for a user from the DB
    /// </summary>
    public class UserData
    {
        //public UserData() { }

        public UserData(int userId, string clientIp) {
            UserID = userId;
            ClientIP = clientIp;
        }
        
        /*
        public UserData(int userId, int instituteId, string userName, string password, bool academic) : this(userId, instituteId, userName, password, "", DateTime.Now, true, academic) { }

        public UserData(int userId, int instutiteId, string userName, string password, string email, DateTime lastLogin, bool active, bool academic)
        {
            UserId = userId;
            InstituteId = instutiteId;
            UserName = userName;
            Password = password;
            Email = email;
            LastLogin = lastLogin;
            Active = active;
            IsAcademic = academic;
        }

        public int UserId { get; private set; }
        public int InstituteId { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public DateTime LastLogin { get; private set; }
        public bool Active { get; private set; }
        public bool IsAcademic { get; private set; }*/

        public String ClientIP { get; set; }
        public List<int> GroupIDs { get; set; }
        public List<int> GroupIDsNonBlocked { get; set; }
        public String GroupIDsString { get; set; }
        public String GroupIDsStringNonBlocked { get; set; }
        public List<int> GroupOwnedIDs { get; set; }
        public String GroupOwnedIDsString { get; set; }
        public bool IsLoggedIn { get; set; }
        public int PrimaryGroupID { get; set; }
        public int PrimarySubGroupID { get; set; }
        public String UserEmail { get; set; }
        public String UserFullName { get; set; }
        public int UserID { get; set; }
        public String UserName { get; set; }

    }
}