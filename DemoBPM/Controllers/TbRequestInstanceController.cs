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
    public class TbRequestInstanceController : TBBaseController<Entities, tbRequestInstance>
    {
        public TbRequestInstanceController()
            : base("TbRequestInstanceController")
        { }

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbRequestInstance> Get()
        {
            return _db.tbRequestInstances.AsQueryable();
        }

        public override SingleResult<tbRequestInstance> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbRequestInstances.Where(tbRequestInstance => tbRequestInstance.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbRequestInstance se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbRequestInstances.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRequestInstance> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestInstance = _db.tbRequestInstances.Find(key);
            if (requestInstance == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(requestInstance);

            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbRequestInstance requestInstance = _db.tbRequestInstances.Find(key);
            if (requestInstance == null)
            {
                return NotFound();
            }

            _db.tbRequestInstances.Remove(requestInstance);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetRequestInstance()
        {
            var ret = _db.sp_GetRequestInstance();

            return Ok(ret);
        }
    }
}