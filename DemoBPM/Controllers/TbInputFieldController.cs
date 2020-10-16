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
    public class TbInputFieldController : TBBaseController<Entities, tbInputField>
    {
        public TbInputFieldController()
            : base("TbInputFieldController")
        { }

        [EnableQuery(PageSize = 100)]
        public override IQueryable<tbInputField> Get()
        {
            return _db.tbInputFields.AsQueryable();
        }

        public override SingleResult<tbInputField> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbInputFields.Where(tbStepInputField => tbStepInputField.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbInputField se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbInputFields.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbInputField> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stepInputField = _db.tbInputFields.Find(key);
            if (stepInputField == null)
            {
                return NotFound();
            }

            Validate(patch.GetInstance());
            patch.Patch(stepInputField);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbInputField stepInputField = _db.tbInputFields.Find(key);
            if (stepInputField == null)
            {
                return NotFound();
            }

            _db.tbInputFields.Remove(stepInputField);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}