using GiftStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace GiftStore.ViewComponents
{
	public class SliderViewComponent:ViewComponent
	{
		private MyDbContext db;
		public SliderViewComponent(MyDbContext _db)
		{
			db = _db;
		}
		public IViewComponentResult Invoke()
		{
			var sliders= db.sliders.ToList();
			return View(sliders);
		}
	}
}
