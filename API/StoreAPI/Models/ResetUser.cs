#region RESET USER JIRA STOREREPO-14 START
namespace StoreAPI.Models
{
    public class ResetUser
    {
        public string UserName { get; set; }
        public string VerificationCode { get; set; }
        public string NewPassword { get; set; }
    }
}

#endregion RESET USER JIRA STOREREPO-14 END