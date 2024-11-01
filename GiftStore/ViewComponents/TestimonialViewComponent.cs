using GiftStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace GiftStore.ViewComponents
{
    public class TestimonialViewComponent:ViewComponent
    {
        private MyDbContext db;
        public TestimonialViewComponent(MyDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            var Testimonial = db.testimonials.ToList();
            return View(Testimonial);
        }
    }
}
