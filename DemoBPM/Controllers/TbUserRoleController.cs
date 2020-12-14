using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbUserRoleController : TBBaseController<Entities, tbUserRole>
    {
        public TbUserRoleController()
           : base("TbUserRoleController")
        { }

        public override Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<tbUserRole> Get()
        {
            throw new NotImplementedException();
        }

        public override SingleResult<tbUserRole> Get([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbUserRole> patch)
        {
            throw new NotImplementedException();
        }

         public override async Task<IHttpActionResult> PostEntity(tbUserRole se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbUserRoles.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }
    }
}