// filepath: /home/sprudel/goblin-market/goblin-api/DTOs/ProductDto.cs
using System.ComponentModel.DataAnnotations;

namespace goblin_api.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(13)]
        public string? EAN { get; set; }

        [StringLength(100)]
        public string? StorageLocation { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? OpenedAt { get; set; }

        public int? ShelfLifeAfterOpening { get; set; } // In days
    }

    public class CreateProductDto
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(13)]
        public string? EAN { get; set; }

        [StringLength(100)]
        public string? StorageLocation { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? OpenedAt { get; set; }

        public int? ShelfLifeAfterOpening { get; set; } // In days
    }

    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(13)]
        public string? EAN { get; set; }

        [StringLength(100)]
        public string? StorageLocation { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? OpenedAt { get; set; }

        public int? ShelfLifeAfterOpening { get; set; } // In days
    }
}
