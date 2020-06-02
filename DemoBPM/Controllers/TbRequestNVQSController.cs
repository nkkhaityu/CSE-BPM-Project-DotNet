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
    public class TbRequestNVQSController : TBBaseController<Entities, tbRequestNVQ>
    {
        public TbRequestNVQSController()
            : base("TbRequestNVQSController")
        { }

        public override Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<tbRequestNVQ> Get()
        {
            throw new NotImplementedException();
        }

        public override SingleResult<tbRequestNVQ> Get([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRequestNVQ> patch)
        {
            throw new NotImplementedException();
        }

        public override Task<IHttpActionResult> PostEntity(tbRequestNVQ se)
        {
            throw new NotImplementedException();
        }
    }
}