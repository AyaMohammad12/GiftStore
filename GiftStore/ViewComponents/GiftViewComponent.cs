using GiftStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace GiftStore.ViewComponents
{
	public class GiftViewComponent:ViewComponent
	{

		private MyDbContext db;
		public GiftViewComponent(MyDbContext _db)
		{
			db = _db;
		}
		public IViewComponentResult Invoke()
		{
			var gifts = db.gifts.Take(8).ToList();
			return View(gifts);
		}
	}
}
