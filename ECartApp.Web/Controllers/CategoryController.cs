using ECartApp.Models;
using ECartApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ECartApp.Controllers
{
    public class ViewModel
    {
        public IQueryable<Category> Category { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string dateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SubCategory> _subCategoryRepository;
        private readonly IGenericRepository<Items> _itemsRepository;

        public CategoryController(IGenericRepository<Category> categoryRepository, IGenericRepository<SubCategory> subCategoryRepository, IGenericRepository<Items> itemsRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _itemsRepository = itemsRepository;
        }
        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<Category> categories = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                categories = _categoryRepository.Search((x => x.IsDeleted == false && x.CategoryName.ToLower().Contains(searchString.ToLower())));
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            else
            {
                categories = _categoryRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)categories.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.CategoryName);
                    break;
                case "Date":
                    categories = categories.OrderBy(s => s.CreatedOn);
                    break;
                case "date_desc":
                    categories = categories.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    categories = categories.OrderBy(s => s.CategoryName);
                    break;
            }
            categories = categories.Skip((pgNo - 1) * pgSize).Take(pgSize);

            vm.Category = categories;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.dateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult Index(Category model)
        {
            if (model.CategoryName != null)
            {
                _categoryRepository.Insert(model);
                _categoryRepository.Save();

            }
            return RedirectToAction("Index");
        }
    }
}
