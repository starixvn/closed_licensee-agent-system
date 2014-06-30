namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Reset Password Criteria.
    /// </summary>
    public class ResetPasswordCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int AgentId { get; set; }

        public string Password { get; set; }

        public string UserSalt { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ResetPassword_Agent";
        }

        public override bool IsValid()
        {
            return UserId > 0 && !string.IsNullOrEmpty(Password);
        }
    }
}