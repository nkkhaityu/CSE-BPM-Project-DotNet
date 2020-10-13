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
    public class TbRoleController : TBBaseController<Entities, tbRole>
    {
        public TbRoleController()
            : base("TbRoleController")
        { }

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbRole> Get()
        {
            return _db.tbRoles.AsQueryable();
        }

        public override SingleResult<tbRole> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbRoles.Where(tbRole => tbRole.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbRole se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbRoles.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRole> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = _db.tbRoles.Find(key);
            if (role == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(role);

            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbRole role = _db.tbRoles.Find(key);
            if (role == null)
            {
                return NotFound();
            }

            _db.tbRoles.Remove(role);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}