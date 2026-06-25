using System.Diagnostics.CodeAnalysis;

namespace ConstructEase.WebApp.ViewModels
{
    public class UserVm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FullName { get; internal set; }
    }
}
