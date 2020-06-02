using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DemoBPM.Common.APISupport
{
    public interface ISEController<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : Common.APISupport.SEObject
    {
        IQueryable<TEntity> Get();
        SingleResult<TEntity> Get([FromODataUri] int key);

        Task<IHttpActionResult> Post(TEntity se);
        Task<IHttpActionResult> PostEntity(TEntity se);

        Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TEntity> patch);
        Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<TEntity> patch);

        Task<IHttpActionResult> Delete([FromODataUri] int key);
        Task<IHttpActionResult> DeleteEntity([FromODataUri] int key);

        Task<IHttpActionResult> Put([FromODataUri] int key, TEntity update);


    }
}