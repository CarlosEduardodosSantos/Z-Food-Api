using System;

namespace APIAlturas.ViewModels
{
    public class PasswordChangeRequest
    {
        public Guid PasswordChangeRequestId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DataHoraExpira { get; set; }
        public bool Reset { get; set; }
    }
}