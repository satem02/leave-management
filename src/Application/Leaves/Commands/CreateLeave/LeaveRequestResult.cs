namespace LeaveManagement.Application.Leaves.Commands.CreateLeave
{
    public class LeaveRequestResult
    {
        public int Id { get; set; }
        public bool CanUseLeave { get; set; } = false;
        public int  LeaveCount { get; set; }
        public bool NeedManagerApproval { get; set; }
    }
}