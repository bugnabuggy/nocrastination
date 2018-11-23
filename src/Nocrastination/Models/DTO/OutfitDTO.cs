using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Models.DTO
{
	public class OutfitDTO
	{
		public string ImageUrl{ get; set; }
		public string Name { get; set; }
		public Guid PurchaseId { get; set; }
		public bool IsSelected { get; set; }
		public int Points { get; set; }
	}
}
