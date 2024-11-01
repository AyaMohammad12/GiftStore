using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStore.Models
{
    public class Gift
    {
        public int GiftId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string?ImageUrl { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
		public string AvailableColors { get; set; } // مثل "أحمر;أزرق;أخضر"
		public decimal discount { get; set; }
		public int numberOfItems { get; set; }
	}
}
