//----------------------------------------------------------------
// Copyright (C) 2018 vino 版权所有
//
// 文件名：SmsRepository.cs
// 功能描述：短信 数据访问
//
// 创建者：kulend@qq.com
// 创建时间：2018-02-04 19:13
// 说明：此代码由工具自动生成，对此文件的更改可能会导致不正确的行为，
// 并且如果重新生成代码，这些更改将会丢失。
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Vino.Core.Infrastructure.Data;
using Vino.Core.CMS.Data.Common;
using Vino.Core.CMS.Domain.Entity.System;

namespace Vino.Core.CMS.Data.Repository.System
{
    /// <summary>
    /// 短信 仓储接口
    /// </summary>
    public partial interface ISmsRepository : IRepository<Sms>
    {
    }

    /// <summary>
    /// 短信 仓储接口实现
    /// </summary>
    public partial class SmsRepository : BaseRepository<Sms>, ISmsRepository
    {
        public SmsRepository(VinoDbContext dbcontext) : base(dbcontext)
        {
        }
    }
}
