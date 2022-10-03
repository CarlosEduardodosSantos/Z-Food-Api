using System;

namespace APIAlturas.ViewModels
{
    public class Wishlist
    {
        public int WishlistId { get; set; }
        public Guid UsuarioId { get; set; }
        public int RestauranteId { get; set; }
    }
}