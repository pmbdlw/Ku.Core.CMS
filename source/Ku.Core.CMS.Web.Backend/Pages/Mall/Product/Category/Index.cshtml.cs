using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ku.Core.CMS.Domain.Dto.Mall;
using Ku.Core.CMS.Domain.Entity.Mall;
using Ku.Core.CMS.IService.Mall;
using Ku.Core.CMS.Web.Base;
using Ku.Core.CMS.Web.Security;

namespace Ku.Core.CMS.Web.Backend.Pages.Mall.Product.Category
{
    /// <summary>
    /// 商品类目 列表页面
    /// </summary>
    [Auth("mall.product.category")]
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : BasePage
    {
        private readonly IProductCategoryService _service;

        public IndexModel(IProductCategoryService service)
        {
            this._service = service;
        }

        public List<ProductCategoryDto> Parents { set; get; }

        [Auth("view")]
        public async Task OnGetAsync(long? parentId)
        {
            Parents = new List<ProductCategoryDto>();
            if (parentId.HasValue)
            {
                Parents = await _service.GetWithParentsAsync(parentId.Value);
            }
            ViewData["ParentId"] = parentId;
        }

        /// <summary>
        /// 取得列表数据
        /// </summary>
        [Auth("view")]
        public async Task<IActionResult> OnPostDataAsync(int page, int rows, ProductCategorySearch where)
        {
            var data = await _service.GetListAsync(page, rows, where, null);
            return PagerData(data.items, page, rows, data.count);
        }

        /// <summary>
        /// 删除
        /// </summary>
        [Auth("delete")]
        public async Task<IActionResult> OnPostDeleteAsync(params long[] id)
        {
            await _service.DeleteAsync(id);
            return JsonData(true);
        }

        /// <summary>
        /// 恢复
        /// </summary>
        [Auth("restore")]
        public async Task<IActionResult> OnPostRestoreAsync(params long[] id)
        {
            await _service.RestoreAsync(id);
            return Json(true);
        }

        /// <summary>
        /// 取得Json数据
        /// </summary>
        public async Task<IActionResult> OnGetJsonAsync()
        {
            var data = await _service.GetJsonDataAsync();
            return new OriginJsonResult(data);
        }
    }
}
