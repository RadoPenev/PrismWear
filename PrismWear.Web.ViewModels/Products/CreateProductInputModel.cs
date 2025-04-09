using Microsoft.AspNetCore.Http;
using PrismWear.Web.ViewModels.Products;
using PrismWear.Web.ViewModels.Sizes;
using System.ComponentModel.DataAnnotations;

namespace PrismWear.Web.ViewModels
{
    public class CreateProductInputModel:BaseProductInputModel
    {

        
        [Required(ErrorMessage = "Image path is required.")]
        public IEnumerable<IFormFile> Images { get; set; }


    }
}
