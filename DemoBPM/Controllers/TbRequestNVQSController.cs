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

        [EnableQuery]
        public override IQueryable<tbRequestNVQ> Get()
        {
            return _db.tbRequestNVQS.AsQueryable();
        }

        public override SingleResult<tbRequestNVQ> Get([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRequestNVQ> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tbRequestNVQS = _db.tbRequestNVQS.Find(key);
            if (tbRequestNVQS == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(tbRequestNVQS);

            await _db.SaveChangesAsync();

            return Ok(tbRequestNVQS);
        }

        public override async Task<IHttpActionResult> PostEntity(tbRequestNVQ se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbRequestNVQS.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }
    }
}