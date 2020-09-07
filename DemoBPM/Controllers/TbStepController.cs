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
    public class TbStepController : TBBaseController<Entities, tbStep>
    {
        public TbStepController()
            : base("TbStepController")
        { }

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbStep> Get()
        {
            return _db.tbSteps.AsQueryable();
        }

        public override SingleResult<tbStep> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbSteps.Where(tbStep => tbStep.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbStep se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbSteps.Add(se);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbStep> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var step = _db.tbSteps.Find(key);
            if (step == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(step);

            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbStep step = _db.tbSteps.Find(key);
            if (step == null)
            {
                return NotFound();
            }

            _db.tbSteps.Remove(step);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}