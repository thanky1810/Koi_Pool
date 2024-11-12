using System.ComponentModel.DataAnnotations;

namespace KoiPool_Project.Models
{
    public class ProductModel
    {
        [Key]
        public string Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Vui long nhap ten san pham")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Vui long nhap mo ta san pham")]
        public string Description { get; set; }
        [Required, MinLength(4, ErrorMessage = "Vui long nhap gia san pham")]
        public decimal Price { get; set; }

    }
}
