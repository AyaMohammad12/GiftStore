namespace GiftStore.Models
{
	public class Favorite
	{

		public int Id { get; set; }

		// ربط الهدية بالمفضلة
		public int GiftId { get; set; }
		public Gift Gift { get; set; }

		// ربط المستخدم بالمفضلة
		public string UserId { get; set; }  // UserId سيكون من جدول المستخدمين في Identity
		public ApplicationUser User { get; set; } // علاقة بالمستخدم


	}
}
