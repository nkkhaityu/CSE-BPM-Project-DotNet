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
    public abstract class TBBaseController<TDbContext, TEntity> : SEBaseODataController<TDbContext>, ISEController<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : Common.APISupport.SEObject
    {
        public TBBaseController(string logIdentifier) : base(logIdentifier)
        {
        }

        public abstract IQueryable<TEntity> Get();
        public abstract SingleResult<TEntity> Get([FromODataUri] int key);

        public abstract Task<IHttpActionResult> PostEntity(TEntity se);
        public async Task<IHttpActionResult> Post(TEntity se)
        {
            try
            {
                return await PostEntity(se);
            }
            catch (Exception ex)
            {

                _log.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name}-Path-{ex.Message}", ex);
                return BadRequest(ex.Message);

            }
        }

        public abstract Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<TEntity> patch);
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TEntity> patch)
        {
            try
            {
                return await PatchEntity(key, patch);
            }
            catch (Exception ex)
            {

                _log.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name}-Path-{ex.Message}", ex);
                return BadRequest(ex.Message);

            }
        }

        public abstract Task<IHttpActionResult> DeleteEntity([FromODataUri] int key);
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                return await DeleteEntity(key);
            }
            catch (Exception ex)
            {

                _log.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name}-Path-{ex.Message}", ex);
                return BadRequest(ex.Message);

            }
        }


        public async Task<IHttpActionResult> Put([FromODataUri] int key, TEntity update)
        {
            throw new NotImplementedException();
        }
    }
}