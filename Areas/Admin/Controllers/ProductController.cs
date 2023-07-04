using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Dto;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.Enum;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("admin"))]
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        private readonly IPhotoService photoService;

        public ProductController(IProductRepository repository, IPhotoService photoService)
        {
            this.repository = repository;
            this.photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await repository.GetProducts();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Create product failed");
                return View(productDto);
            }
            var photoResult1 = await photoService.AddPhoto(productDto.Image1);
            var photoResult2 = await photoService.AddPhoto(productDto.Image2);
            var photoResult3 = await photoService.AddPhoto(productDto.Image3);
            var product = new Product
            {
                Title = productDto.Title,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                Brand = productDto.Brand,
                Gender = productDto.Gender,
                Sale = productDto.Sale,
                Image = new Image
                {
                    First = photoResult1.Url.ToString(),
                    Second = photoResult2.Url.ToString(),
                    Third = photoResult3.Url.ToString()
                },
                Quantity = productDto.Quantity,
                Status = productDto.Status,
                CreateDate = productDto.CreateDate
            };
            repository.Add(product);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var p = await repository.getProductByIdAsNoTracking(id);
            if (p == null)
            {
                return View("Error");
            }
            var productDto = new ProductDto
            {
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                Brand = p.Brand,
                Gender = p.Gender,
                Status = p.Status,
                Sale = p.Sale,
                Quantity = p.Quantity,
            };
            return View(productDto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = await repository.getProductByIdAsNoTracking(id);
                if (product != null)
                {
                    try
                    {
                        photoService.DeletePhoto(product.Image.First);
                        photoService.DeletePhoto(product.Image.Second);
                        photoService.DeletePhoto(product.Image.Third);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Delete photo failed");
                        return View(productDto);
                    }
                    var resultPhoto1 = await photoService.AddPhoto(productDto.Image1);
                    var resultPhoto2 = await photoService.AddPhoto(productDto.Image2);
                    var resultPhoto3 = await photoService.AddPhoto(productDto.Image3);

                    // Update the properties of the existing 'product' entity instead of creating a new 'productNew' entity
                    product.Title = productDto.Title;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                    product.Category = productDto.Category;
                    product.Brand = productDto.Brand;
                    product.Gender = productDto.Gender;
                    product.Image.First = resultPhoto1.Url.ToString();
                    product.Image.Second = resultPhoto2.Url.ToString();
                    product.Image.Third = resultPhoto3.Url.ToString();
                    product.Quantity = productDto.Quantity;
                    product.Sale = productDto.Sale;
                    product.Status = productDto.Status;
                    product.CreateDate = productDto.CreateDate;

                    repository.Update(product);
                    return RedirectToAction("Index", "Product", new { area = "Admin" });
                }
                else
                {
                    return View(productDto);
                }
            }

            ModelState.AddModelError("", "Update failed");
            return View(productDto);
        }

        public IActionResult Delete(int id)
        {
            if (repository.Delete(id)) return RedirectToAction("Index");
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            Product product = await repository.getProductByIdAsNoTracking(id);
            if (product != null)
            {
                if (product.Status == ProductStatus.Active)
                {
                    product.Status = ProductStatus.InActive;
                }
                else
                {
                    product.Status = ProductStatus.Active;
                }

                repository.Update(product);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
