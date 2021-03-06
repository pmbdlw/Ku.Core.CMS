﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ku.Core.CMS.Domain.Enum.Mall
{
    /// <summary>
    /// 积分赠送规则
    /// </summary>
    public enum EmPointsGainRule : short
    {
        [Display(Name = "不赠送")]
        Null = 0,

        [Display(Name = "按订单")]
        Order = 1,

        [Display(Name = "按商品")]
        Product = 2,

        [Display(Name = "按商品SKU")]
        ProductSku = 3,
    }
}
