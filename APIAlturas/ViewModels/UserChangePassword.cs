using System;

namespace APIAlturas.ViewModels
{
    public class UserChangePassword
    {
        public Guid PasswordChangeRequestId { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public int RestauranteId { get; set; }
    }
}