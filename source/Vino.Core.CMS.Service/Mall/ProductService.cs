//----------------------------------------------------------------
// Copyright (C) 2018 vino 版权所有
//
// 文件名：ProductService.cs
// 功能描述：商品 业务逻辑处理类
//
// 创建者：kulend@qq.com
// 创建时间：2018-02-24 10:50
//
//----------------------------------------------------------------

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vino.Core.CMS.Data.Repository.Mall;
using Vino.Core.CMS.Domain.Dto.Mall;
using Vino.Core.CMS.Domain.Entity.Mall;
using Vino.Core.CMS.IService.Mall;
using Vino.Core.Infrastructure.Exceptions;
using Vino.Core.Infrastructure.Extensions;
using Vino.Core.Infrastructure.IdGenerator;

namespace Vino.Core.CMS.Service.Mall
{
    public partial class ProductService : BaseService, IProductService
    {
        protected readonly IProductRepository _repository;

        #region 构造函数

        public ProductService(IProductRepository repository)
        {
            this._repository = repository;
        }

        #endregion

        #region 自动生成的方法

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns>List<ProductDto></returns>
        public async Task<List<ProductDto>> GetListAsync(ProductSearch where, string sort)
        {
            var data = await _repository.QueryAsync(where.GetExpression(), sort ?? "CreateTime desc");
            return Mapper.Map<List<ProductDto>>(data);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns>count：条数；items：分页数据</returns>
        public async Task<(int count, List<ProductDto> items)> GetListAsync(int page, int size, ProductSearch where, string sort)
        {
            var data = await _repository.PageQueryAsync(page, size, where.GetExpression(), sort ?? "CreateTime desc");
            return (data.count, Mapper.Map<List<ProductDto>>(data.items));
        }

        /// <summary>
        /// 根据主键取得数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<ProductDto> GetByIdAsync(long id)
        {
            return Mapper.Map<ProductDto>(await this._repository.GetByIdAsync(id));
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task SaveAsync(ProductDto dto)
        {
            Product model = Mapper.Map<Product>(dto);
            if (model.Id == 0)
            {
                //新增
                model.Id = ID.NewID();
                model.CreateTime = DateTime.Now;
                model.IsDeleted = false;
                await _repository.InsertAsync(model);
            }
            else
            {
                //更新
                var item = await _repository.GetByIdAsync(model.Id);
                if (item == null)
                {
                    throw new VinoDataNotFoundException("无法取得商品数据！");
                }

                //TODO:这里进行赋值

                _repository.Update(item);
            }
            await _repository.SaveAsync();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }

        #endregion

        #region 其他方法

        #endregion
    }
}