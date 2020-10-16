using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbInputFieldInstanceController : TBBaseController<Entities, tbInputFieldInstance>
    {
        public TbInputFieldInstanceController()
            : base("TbInputFieldInstanceController")
        { }

        [EnableQuery(PageSize = 100)]
        public override IQueryable<tbInputFieldInstance> Get()
        {
            return _db.tbInputFieldInstances.AsQueryable();
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetInputFieldInstance()
        {
            var ret = _db.sp_GetInputFieldInstance();

            return Ok(ret);
        }

        public override SingleResult<tbInputFieldInstance> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbInputFieldInstances.Where(tbStepInputFieldInstance => tbStepInputFieldInstance.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbInputFieldInstance se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbInputFieldInstances.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbInputFieldInstance> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stepInputFieldInstance = _db.tbInputFieldInstances.Find(key);
            if (stepInputFieldInstance == null)
            {
                return NotFound();
            }

            Validate(patch.GetInstance());
            patch.Patch(stepInputFieldInstance);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbInputFieldInstance stepInputFieldInstance = _db.tbInputFieldInstances.Find(key);
            if (stepInputFieldInstance == null)
            {
                return NotFound();
            }

            _db.tbInputFieldInstances.Remove(stepInputFieldInstance);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}