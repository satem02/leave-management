namespace LeaveManagement.Application.Common.Models
{
    public class AppConfigurations
    {
        public bool UseInMemoryDatabase { get; set; }
        public JwtOptions Jwt { get; set; }
    }

    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireHours { get; set; }
        public string Key { get; set; }

    }
}