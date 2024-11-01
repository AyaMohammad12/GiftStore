using GiftStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftStore.Data
{
    public class MyDbContext: IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
		public DbSet<ContactUs> contactUs { get; set; }
        public DbSet<Gift> gifts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<AboutUs> aboutUs { get; set; }
        public DbSet<Slider> sliders  { get; set; }
		public DbSet<Favorite> Favorites { get; set; }

		public DbSet<CartItem> cartItems { get; set; }
        public DbSet<Testimonial> testimonials { get; set; }

        public DbSet<Cart> carts { get; set; }



	}

}
