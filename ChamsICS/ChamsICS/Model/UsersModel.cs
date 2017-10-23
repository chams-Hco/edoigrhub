using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class UserLoginRes : Response
    {
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int PasswordStatus { get; set; }
        public int AccountStatus { get; set; }

        public UserDashBoardData UserDashBoardData { get; set;}
    }

    public class UserDashBoardData
    {
        public int ClientId { get; set; }
        public string RoleName { get; set; }
        public string ClientName { get; set; }
        public string ClientLogoUrl { get; set; }
        public int UserTypeParentId { get; set; }

    }

    public class UserLoginReq
    {
        public string Email { get; set; }
        public string UserPassword { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public int UserTypeParentId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
        public int? PasswordStatus { get; set; }
        public int? Status { get; set; }
        public int? ClientId { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class FindUserRes : Response
    {
        public User User { get; set; }
    }

    public class GetAllUserReq
    {
       public  int roleId { get; set; } 
       public  int UserTypeParentId { get; set; } 
    }

    public class GetAllUserRes:Response
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class ChangeUserPasswordReq
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class ResetUserPasswordReq
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class RoleRes: Response
    {
        public IEnumerable<Role> Role { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
    }

    public class CreateUserRes: Response
    {
        public int userId { get; set; }
    }
}
