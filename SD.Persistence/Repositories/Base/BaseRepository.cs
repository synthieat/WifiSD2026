using SD.Core.Repositories;
using SD.Persistence.Repositories.DBContext;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SD.Persistence.Repositories.Base
{
    public class BaseRepository : IBaseRepository
    {
        private MovieDbContext movieDbContext;

        #region ctor | dtor
        public BaseRepository()
        {
            this.movieDbContext = new MovieDbContext();
        }

        public BaseRepository(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }
        #endregion


        #region [C]REATE
        void IBaseRepository.Add<T>(T entity, bool saveImmediately)
        {
            if (entity == null)
            {
                return;
            }

            /* Als hinzugefügtes Entity vorgemerkt, aber noch nicht in der Datenbank gespeichert */
            this.movieDbContext.Add(entity);

            if(saveImmediately)
            {
                this.movieDbContext.SaveChanges();
            }
            
        }

        Task IBaseRepository.AddAsync<T>(T entity, bool saveImmediately, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region [R}EAD
        IQueryable<T> IBaseRepository.QueryFrom<T>(Expression<Func<T, bool>> whereFilter)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region [U]PDATE
        T IBaseRepository.Update<T>(T entity, object key, bool saveImmediately)
        {
            throw new NotImplementedException();
        }

        Task<T> IBaseRepository.UpdateAsync<T>(T entity, object key, bool saveImmediately, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region [D]ELETE
        void IBaseRepository.Remove<T>(T entity, bool saveImmediately)
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository.RemoveAsync<T>(T entity, bool saveImmediately, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IBaseRepository.RemoveByKey<T>(object key, bool saveImmediately)
        {
            throw new NotImplementedException();
        }

        Task IBaseRepository.RemoveByKeyAsync<T>(object key, bool saveImmediately, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
