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
    public class TbDeviceTokenController : TBBaseController<Entities, tbDeviceToken>
    {
        public TbDeviceTokenController()
            : base("TbDeviceTokenController")
        { }

        [EnableQuery(PageSize = 100)]
        public override IQueryable<tbDeviceToken> Get()
        {
            return _db.tbDeviceTokens.AsQueryable();
        }

        public override async Task<IHttpActionResult> PostEntity(tbDeviceToken se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceToken = _db.tbDeviceTokens.Where(tbDeviceToken => (tbDeviceToken.UserID == se.UserID && tbDeviceToken.DeviceToken == se.DeviceToken)).FirstOrDefault();

            if (deviceToken != null)
            {
                deviceToken.IsLogin = se.IsLogin;
                await _db.SaveChangesAsync();
                return Ok(deviceToken);
            }

            _db.tbDeviceTokens.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbDeviceToken> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceToken = _db.tbDeviceTokens.Find(key);
            if (deviceToken == null)
            {
                return NotFound();
            }

            Validate(patch.GetInstance());
            patch.Patch(deviceToken);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbDeviceToken deviceToken = _db.tbDeviceTokens.Find(key);
            if (deviceToken == null)
            {
                return NotFound();
            }

            _db.tbDeviceTokens.Remove(deviceToken);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override SingleResult<tbDeviceToken> Get([FromODataUri] int key)
        {
            throw new System.NotImplementedException();
        }
    }
}