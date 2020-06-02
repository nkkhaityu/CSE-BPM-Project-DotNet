using DemoBPM.Common.APISupport;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    public class TbRoleController : TBBaseController<Entities, tbRole>
    {
        public TbRoleController()
            : base("TbRoleController")
        { }

        public override Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<tbRole> Get()
        {
            throw new NotImplementedException();
        }

        public override SingleResult<tbRole> Get([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRole> patch)
        {
            throw new NotImplementedException();
        }

        public override Task<IHttpActionResult> PostEntity(tbRole se)
        {
            throw new NotImplementedException();
        }
    }
}