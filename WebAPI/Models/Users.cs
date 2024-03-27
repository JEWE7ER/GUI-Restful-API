namespace WebAPI.Models
{
    public class Users
    {
        internal int id { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        internal bool read { get; set; } = false;
        internal bool write { get; set; } = false;
    }
}
