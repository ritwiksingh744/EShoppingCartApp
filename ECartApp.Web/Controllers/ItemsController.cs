using ECartApp.Data.Entity;
using ECartApp.Data.Repository;
using ECartApp.Models.Item;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECartApp.Controllers
{
    public class ViewModel2
    {
        public IEnumerable<Category> Category { get; set; }
        public IQueryable<Items> Items { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string categorySortParm { get; set; }
        public string subCategorySortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    public class ItemsController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SubCategory> _subCategoryRepository;
        private readonly IItemRepository _itemsRepository;

        public ItemsController(IGenericRepository<Category> categoryRepository, IGenericRepository<SubCategory> subCategoryRepository, IItemRepository itemsRepository )
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _itemsRepository = itemsRepository;
        }
        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel2();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<Items> items = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                items = _itemsRepository.GetData(x => x.IsDeleted == false && x.ItemName.ToLower().Contains(searchString.ToLower()) || x.SubCategory.Category.CategoryName.ToLower().Contains(searchString.ToLower()) || x.SubCategory.SubCategoryName.ToLower().Contains(searchString.ToLower()));
                totalPage = (int)Math.Ceiling((decimal)items.Count() / pgSize);
            }
            else
            {
                items = _itemsRepository.GetData(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)items.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(p => p.ItemName);
                    break;
                case "category":
                    items = items.OrderBy(p => p.SubCategory.Category.CategoryName);
                    break;
                case "category_desc":
                    items = items.OrderByDescending(s => s.SubCategory.Category.CategoryName);
                    break;
                case "subCategory":
                    items = items.OrderBy(s => s.SubCategory.SubCategoryName);
                    break;
                case "subCategory_desc":
                    items = items.OrderByDescending(s => s.SubCategory.SubCategoryName);
                    break;
                case "price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:
                    items = items.OrderBy(s => s.ItemName);
                    break;
            }
            items = items.Skip((pgNo - 1) * pgSize).Take(pgSize);
            vm.Items = items;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.categorySortParm = sortOrder == "category" ? "category_desc" : "category";
            vm.subCategorySortParm = sortOrder == "subCategory" ? "subCategory_desc" : "subCategory";
            vm.Category = _categoryRepository.Search(x => x.IsDeleted == false);
            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult Index(ItemAddModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ItemName != null)
                {
                    var request = new Items();
                    request.ItemName = model.ItemName;
                    request.Price = model.Price;
                    request.ImageUrl = model.ImageUrl;
                    request.CategoryId = model.CategoryId;
                    request.SubCategoryId = model.SubCategoryId;
                    request.CreatedOn = DateTime.Now;
                    
                    _itemsRepository.Insert(request);
                    _itemsRepository.Save();
                }
            return RedirectToAction("Index");
            }
            var message = string.Join(" | ", ModelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage));
            throw new Exception($"Status Code: {HttpStatusCode.BadRequest}, Message: {message}");
        }

        [HttpGet]
        public IActionResult GetJson(int CategoryId)
        {
            var subCategoryList = _subCategoryRepository.Search(x=>x.IsDeleted == false && x.CategoryId == CategoryId);
            return Json(subCategoryList);
        }
    }
}
