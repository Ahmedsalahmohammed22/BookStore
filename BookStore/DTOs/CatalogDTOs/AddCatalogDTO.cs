using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.CatalogDTOs
{
    public class AddCatalogDTO
    {
        [Required]
        [StringLength(50)]
        public string Catalog_name { get; set; }
        [Required]
        [StringLength(150)]
        public string Catalog_description { get; set; }
    }
}
