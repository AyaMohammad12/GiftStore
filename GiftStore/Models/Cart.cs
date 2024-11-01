using System.ComponentModel.DataAnnotations;

namespace GiftStore.Models
{
	public class Cart
	{
		[Key]
		public int CartId { get; set; }

		// العلاقة مع العناصر
		public List<CartItem> Items { get; set; } = new List<CartItem>();
	}
}
