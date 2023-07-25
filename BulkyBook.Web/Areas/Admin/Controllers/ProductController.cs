using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {

            ProductViewModel productViewModel = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll()
                .Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //Create Product
                return View(productViewModel);
            }
            else
            {
                //Update Product
                productViewModel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category
                .GetAll()
                .Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                return View(productViewModel);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
