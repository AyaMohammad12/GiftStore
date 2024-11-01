using Microsoft.AspNetCore.Identity;

namespace GiftStore.Models
{
	public class ApplicationUser: IdentityUser
	{
		// يمكنك إضافة خصائص إضافية للمستخدم هنا إذا كنت بحاجة إلى ذلك
		public string FirstName { get; set; }
		public string LastName { get; set; }
		// مثال على خصائص إضافية
	}
}
