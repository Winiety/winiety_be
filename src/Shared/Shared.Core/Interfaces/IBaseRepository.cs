﻿using Shared.Core.BaseModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);

        IQueryable<T> GetQueryable();

        Task<T> GetAsync(
            int id,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<T> GetByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllAsync(
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllByAsync(
            Expression<Func<T, bool>> getBy,
            bool withTracking = false,
            params Expression<Func<T, object>>[] includes);

        Task<IPagedList<T>> GetPagedByAsync(
            IQueryable<T> query, 
            int pageNumber,
            int pageSize);

        Task<IPagedList<T>> GetPagedByAsync(
         Expression<Func<T, bool>> getBy,
         int pageNumber,
         int pageSize,
         bool withTracking = false,
         params Expression<Func<T, object>>[] includes);

        Task<IPagedList<T>> GetPagedByAsync<TKey>(
           Expression<Func<T, bool>> getBy,
           Expression<Func<T, TKey>> orderBy,
           int pageNumber,
           int pageSize,
           bool withTracking = false,
           bool orderByDescending = true,
           params Expression<Func<T, object>>[] includes);

        Task<IPagedList<T>> GetPagedAsync(
           int pageNumber,
           int pageSize,
           bool withTracking = false,
           params Expression<Func<T, object>>[] includes);

        Task<bool> ExistAsync(
            Expression<Func<T, bool>> getBy,
            params Expression<Func<T, object>>[] includes);
    }
}
