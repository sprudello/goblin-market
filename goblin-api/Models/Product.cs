// filepath: /home/sprudel/goblin-market/goblin-api/Models/Product.cs
using System.ComponentModel.DataAnnotations;

namespace goblin_api.Models
{
    public class Product
    {
        public long Id { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(13)] // EAN-13
        public string? EAN { get; set; }

        [StringLength(100)]
        public string? StorageLocation { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? OpenedAt { get; set; }

        public int? ShelfLifeAfterOpening { get; set; } // In days
    }
}
