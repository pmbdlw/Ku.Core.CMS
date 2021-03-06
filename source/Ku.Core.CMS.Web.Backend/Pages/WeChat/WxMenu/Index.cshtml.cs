using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ku.Core.CMS.Domain.Dto.WeChat;
using Ku.Core.CMS.Domain.Entity.WeChat;
using Ku.Core.CMS.IService.WeChat;
using Ku.Core.CMS.Web.Base;
using Ku.Core.CMS.Web.Security;

namespace Ku.Core.CMS.Web.Backend.Pages.WeChat.WxMenu
{
    /// <summary>
    /// 微信菜单 列表页面
    /// </summary>
    [Auth("wechat.wxmenu")]
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : BasePage
    {
        private readonly IWxMenuService _service;
        private readonly IWxAccountService _accountService;

        public IndexModel(IWxMenuService service, IWxAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        public IEnumerable<WxAccountDto> Accounts { set; get; }

        [Auth("view")]
        public async Task OnGetAsync()
        {
            Accounts = await _accountService.GetListAsync(null, "Name asc");
        }

        /// <summary>
        /// 取得列表数据
        /// </summary>
        [Auth("view")]
        public async Task<IActionResult> OnPostDataAsync(int page, int rows, WxMenuSearch where)
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
            return JsonData(true);
        }
    }
}
