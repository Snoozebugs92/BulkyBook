using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class Category : Controller
    {
        private readonly ApplicationDbContext _db;
        public Category(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryModel> objCategoryList = _db.Categories;
            return View(objCategoryList);
        }
    }
}
