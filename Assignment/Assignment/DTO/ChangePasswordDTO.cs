namespace Assignment.DTO
{
    public class ChangePasswordDTO
    {
        public int AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
