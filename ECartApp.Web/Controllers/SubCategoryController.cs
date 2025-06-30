using ECartApp.Data.Entity;
using ECartApp.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ECartApp.Controllers
{
    public class ViewModel1
    {
        public IEnumerable<Category> Category { get; set; }
        public IQueryable<SubCategory> SubCategory { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public string nameSortParm { get; set; }
        public string categorySortParm { get; set; }
        public string createDateSortParm { get; set; }
        public string currentFilter { get; set; }
        public string currentSortOrder { get; set; }
    }
    public class SubCategoryController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SubCategory> _subCategoryRepository;
        private readonly IGenericRepository<Items> _itemsRepository;

        public SubCategoryController(IGenericRepository<Category> categoryRepository, IGenericRepository<SubCategory> subCategoryRepository, IGenericRepository<Items> itemsRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _itemsRepository = itemsRepository;
        }
        public IActionResult Index(string sortOrder, string searchString, int? pageNo, int? pageSize)
        {
            var vm = new ViewModel1();
            int pgNo = (pageNo ?? 1);
            int pgSize = vm.pageSize = (pageSize ?? 5);
            IQueryable<SubCategory> subCategories = null;
            int? totalPage = null;
            vm.currentSortOrder = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                vm.currentFilter = searchString;
                subCategories = _subCategoryRepository.Search(x => x.IsDeleted == false && x.SubCategoryName.ToLower().Contains(searchString.ToLower()) || x.Category.CategoryName.ToLower().Contains(searchString.ToLower()));
                totalPage = (int)Math.Ceiling((decimal)subCategories.Count() / pgSize);
            }
            else
            {
                subCategories = _subCategoryRepository.Search(x => x.IsDeleted == false);
                totalPage = (int)Math.Ceiling((decimal)subCategories.Count() / pgSize);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    subCategories = subCategories.OrderByDescending(p => p.SubCategoryName);
                    break;
                case "category":
                    subCategories = subCategories.OrderBy(p => p.Category.CategoryName);
                    break;
                case "category_desc":
                    subCategories = subCategories.OrderByDescending(s => s.Category.CategoryName);
                    break;
                case "createDate":
                    subCategories = subCategories.OrderBy(s => s.CreatedOn);
                    break;
                case "Cdate_desc":
                    subCategories = subCategories.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    subCategories = subCategories.OrderBy(s => s.SubCategoryName);
                    break;
            }
            subCategories = subCategories.Skip((pgNo - 1) * pgSize).Take(pgSize);
            vm.SubCategory = subCategories;
            vm.totalPage = (int)totalPage;
            vm.nameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            vm.categorySortParm = sortOrder == "category" ? "category_desc" : "category";
            vm.createDateSortParm = sortOrder == "createDate" ? "Cdate_desc" : "createDate";
            vm.Category = _categoryRepository.Search(x => x.IsDeleted == false);
            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult Index(SubCategory model)
        {
            if (model.SubCategoryName != null)
            {
                _subCategoryRepository.Insert(model);
                _subCategoryRepository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
