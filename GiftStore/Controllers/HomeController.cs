using GiftStore.Data;
using GiftStore.Models;
using GiftStore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GiftStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(MyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //private readonly MyDbContext _context;

        //public HomeController(MyDbContext context)
        //{
        //	_context = context;
        //}

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Gifts(int? categoryId, string searchTerm, decimal? price)
        {
            var gifts = await _context.gifts.Include(p => p.Category).ToListAsync();

            // تطبيق الفلترة حسب الفئة
            if (categoryId.HasValue)
            {
                gifts = gifts.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // تطبيق البحث باستخدام مصطلح البحث
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                gifts = gifts.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // تطبيق البحث باستخدام السعر (السعر الأقصى فقط)
            if (price.HasValue)
            {
                gifts = gifts.Where(p => p.Price <= price.Value).ToList();
            }

            var viewModel = new GiftViewModel
            {
                gifts = gifts,
                Categories = await _context.categories.ToListAsync(),
                SearchTerm = searchTerm, // تمرير مصطلح البحث
                Price = price // تمرير السعر
            };

            return View(viewModel);
        }
        [HttpGet]
		public IActionResult ContactUs()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ContactUs(ContactUs contactUs)
		{
			if (ModelState.IsValid)
			{
				contactUs.CreatedAt = DateTime.Now; // سجل تاريخ الرسالة
				_context.contactUs.Add(contactUs);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index"); // توجيه المستخدم إلى صفحة شكر
			}
			return View("ContactUs");
		}

        public async Task<IActionResult> AboutUs()
        {
            var messages = await _context.aboutUs.ToListAsync();
            return View(messages);
        }



        // وظيفة لإضافة الهدية إلى المفضلة
        public async Task<IActionResult> AddToFavorites(int giftId)
        {
            var userId = _userManager.GetUserId(User); // الحصول على UserId الحالي
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلاً دخول
            }

            // تحقق مما إذا كانت الهدية موجودة في المفضلة بالفعل
            var favoriteExists = await _context.Favorites
                .AnyAsync(f => f.GiftId == giftId && f.UserId == userId);

            if (!favoriteExists)
            {
                var favorite = new Favorite { GiftId = giftId, UserId = userId };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            //return View();
            return RedirectToAction("Gifts", "Home"); // إعادة التوجيه إلى صفحة الهدايا
        }

        public async Task<IActionResult> Favorites()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var favoriteGifts = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Gift) // تضمين تفاصيل الهدية
                .ToListAsync();

            return View(favoriteGifts);
        }
		[HttpPost]
		public async Task<IActionResult> RemoveFromFavorites(int giftId)
		{
			var userId = _userManager.GetUserId(User); // الحصول على UserId الحالي
			if (userId == null)
			{
				return RedirectToAction("Login", "Account"); // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلاً دخول
			}

			// العثور على الهدية في المفضلة
			var favorite = await _context.Favorites
				.FirstOrDefaultAsync(f => f.GiftId == giftId && f.UserId == userId);

			if (favorite != null)
			{
				_context.Favorites.Remove(favorite); // إزالة الهدية من المفضلة
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Favorites");

			//return RedirectToAction("Index", "Home");
			//// إعادة التوجيه إلى صفحة المفضلة
			//                                  //return View(favorite);
			//return Ok(); // إرجاع رد ناجح
		}


		public async Task<IActionResult> GiftDetails(int id)
		{
			var gift = await _context.gifts
				.Include(g => g.Category) // تأكد من تضمين معلومات الفئة
				.FirstOrDefaultAsync(g => g.GiftId == id);

			if (gift == null)
			{
				return NotFound();
			}

			// تحويل السلسلة إلى قائمة
			var availableColors = gift.AvailableColors?.Split(',').ToList() ?? new List<string>();

			var relatedGifts = await _context.gifts
		.Where(g => g.CategoryId == gift.CategoryId && g.GiftId != gift.GiftId)
		.ToListAsync();

			var viewModel = new GiftViewModel
			{
				Gift = gift,
				AvailableColors = availableColors ,// تمرير الألوان المتاحة
				RelatedGifts = relatedGifts // تمرير الهدايا المشابهة

			};

			return View(viewModel);
		}


        [HttpPost]
        public async Task<IActionResult> AddToCart(int giftId, string selectedColor, int quantity, string message)
        {
			var userId = _userManager.GetUserId(User); // الحصول على UserId الحالي
			if (userId == null)
			{
				return RedirectToAction("Login", "Account"); // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلاً دخول
			}
			// من هنا يمكنك إضافة الكود لإضافة العنصر إلى السلة
			var cartItem = new CartItem
            {
                GiftId = giftId,
                SelectedColor = selectedColor,
                Quantity = quantity,
                Message = message,
				UserId = userId // تأكد من تعيين UserId هنا
			};

            //أضف cartItem إلى قاعدة البيانات أو إلى السلة المخزنة في الذاكرة
            _context.cartItems.Add(cartItem);
            await _context.SaveChangesAsync();
			return RedirectToAction("ViewCart");

			//return RedirectToAction("Index", "Home"); // عد إلى صفحة السلة أو الصفحة الرئيسية
		}


		public async Task<IActionResult> ViewCart()
        {
			var userId = _userManager.GetUserId(User); // الحصول على UserId الحالي
			if (userId == null)
			{
				return RedirectToAction("Login", "Account"); // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلاً دخول
			}
			//var cartItems = await _context.cartItems
			//             .Include(ci => ci.Gift) // تضمين تفاصيل الهدية المرتبطة
			//             .ToListAsync();

			// جلب عناصر السلة الخاصة بالمستخدم الحالي فقط
			var cartItems = await _context.cartItems
				.Where(ci => ci.UserId == userId) // تصفية حسب UserId
				.Include(ci => ci.Gift) // تضمين تفاصيل الهدية المرتبطة
				.ToListAsync();


			return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
			var userId = _userManager.GetUserId(User); // الحصول على UserId الحالي
			if (userId == null)
			{
				return RedirectToAction("Login", "Account"); // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلاً دخول
			}
			//var cartItem = await _context.cartItems.FindAsync(id);
			//         if (cartItem != null)
			//         {
			//             _context.cartItems.Remove(cartItem);
			//             await _context.SaveChangesAsync();
			//         }
			var cartItem = await _context.cartItems
				 .Where(ci => ci.Id == id && ci.UserId == userId) // التحقق من أن العنصر يخص المستخدم الحالي
				 .FirstOrDefaultAsync();

			if (cartItem != null)
			{
				_context.cartItems.Remove(cartItem);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("ViewCart");
        }


        private async Task<bool> UserHasTestimonialAsync(string userId)
        {
            return await _context.testimonials.AnyAsync(t => t.UserId == userId);
        }

        // عرض صفحة إضافة testimonial
        public async Task<IActionResult> AddTestimonial()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (await UserHasTestimonialAsync(userId))
            {
                return RedirectToAction("TestimonialError");


            }

            return View();
        }
        public IActionResult TestimonialError()
        {
            ViewBag.Message = "You have Already added your testimonial.";
            return View(); // يمكنك إنشاء عرض خاص لهذا
        }
        // حفظ testimonial جديد
        [HttpPost]
        public async Task<IActionResult> AddTestimonial(Testimonial model)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (await UserHasTestimonialAsync(userId))
            {
                ViewBag.Message = "لقد قمت بإضافة تقييم بالفعل.";
                return View("Error");
            }


            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {


                    ModelState.AddModelError(" ", error.ErrorMessage);

                }
            }
            model.UserId = userId;
            model.DateAdded = DateTime.Now;
            model.IsApproved = false;
            _context.testimonials.Add(model);
            await _context.SaveChangesAsync();

            ViewBag.Message = "تمت إضافة التقييم بنجاح.";
            return RedirectToAction("Index", "Home");
        }












    }
}