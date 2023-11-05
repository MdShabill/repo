namespace ShopEase.DataModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime LastSuccessFulLoginDate { get; set; }
        public DateTime LastFailedLoginDate { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool IsLocked { get; set; }
    }
}
