namespace GiftStore.Models.ViewModels
{
    public class GiftViewModel
    {
        public IEnumerable<Gift> gifts { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Slider> sliders { get; set; }

		public List<Gift> RelatedGifts { get; set; } // الهدايا المشابهة

		public string SearchTerm { get; set; } // إضافة خاصية البحث
        public decimal? Price { get; set; }

		public int GiftId { get; set; } // Store the selected gift ID

		public Gift Gift { get; set; } // تفاصيل الهدية

		public int SelectedQuantity { get; set; } // لاختيار عدد العناصر
		public string SelectedColor { get; set; } // لاختيار اللون من قائمة
		public string CustomMessage { get; set; } // لإضافة رسالة مخصصة

		// Dropdown for available colors
		public List<string> AvailableColors { get; set; }
	}
}
