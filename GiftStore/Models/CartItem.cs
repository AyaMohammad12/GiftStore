namespace GiftStore.Models
{
	public class CartItem
	{
		public int Id { get; set; } // معرف فريد لكل عنصر في السلة

		public int GiftId { get; set; } // معرف الهدية المرتبطة بالعنصر

		public string SelectedColor { get; set; } // اللون الذي اختاره المستخدم

		public int Quantity { get; set; } // كمية العنصر

		public string? Message { get; set; } // الرسالة المخصصة


		// الربط مع نموذج الهدية (اختياري)
		public virtual Gift Gift { get; set; }

		// ربط المستخدم بالمفضلة
		public string UserId { get; set; }  // UserId سيكون من جدول المستخدمين في Identity
		public ApplicationUser User { get; set; } // علاقة بالمستخدم
	}
}
