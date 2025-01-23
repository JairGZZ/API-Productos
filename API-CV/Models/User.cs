using System.Diagnostics.CodeAnalysis;

namespace API_CV.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
