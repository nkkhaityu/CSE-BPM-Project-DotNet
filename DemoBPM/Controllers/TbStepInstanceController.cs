using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbStepInstanceController : TBBaseController<Entities, tbStepInstance>
    {
        public TbStepInstanceController()
            : base("TbStepInstanceController")
        { }

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbStepInstance> Get()
        {
            return _db.tbStepInstances.AsQueryable();
        }

        public override SingleResult<tbStepInstance> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbStepInstances.Where(tbStepInstance => tbStepInstance.ID == key));
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetStepInstanceDetails()
        {
            var result = _db.sp_GetStepInstanceDetails();

            return Ok(result);
        }

        public override async Task<IHttpActionResult> PostEntity(tbStepInstance se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbStepInstances.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbStepInstance> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stepInstance = _db.tbStepInstances.Find(key);
            if (stepInstance == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(stepInstance);

            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbStepInstance stepInstance = _db.tbStepInstances.Find(key);
            if (stepInstance == null)
            {
                return NotFound();
            }

            _db.tbStepInstances.Remove(stepInstance);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}