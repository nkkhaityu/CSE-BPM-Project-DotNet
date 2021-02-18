using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbDropdownOptionController : TBBaseController<Entities, tbDropdownOption>
    {
        public TbDropdownOptionController()
            : base("TbDropdownOptionsController")
        { }

        [EnableQuery(PageSize = 100, MaxNodeCount =1000)]
        public override IQueryable<tbDropdownOption> Get()
        {
            return _db.tbDropdownOptions.AsQueryable();
        }

        public override SingleResult<tbDropdownOption> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbDropdownOptions.Where(tbDropdownOption => tbDropdownOption.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbDropdownOption se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbDropdownOptions.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public async Task<IHttpActionResult> PostListEntity(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string jsonStr = (string)parameters["value"];

            var dropdownOptions = JsonConvert.DeserializeObject<List<tbDropdownOption>>(jsonStr);

            if (dropdownOptions.Count > 0)
            {
                _db.tbDropdownOptions.AddRange(dropdownOptions);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbDropdownOption> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dropdownOption = _db.tbDropdownOptions.Find(key);
            if (dropdownOption == null)
            {
                return NotFound();
            }

            Validate(patch.GetInstance());
            patch.Patch(dropdownOption);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbDropdownOption dropdownOption = _db.tbDropdownOptions.Find(key);
            if (dropdownOption == null)
            {
                return NotFound();
            }

            _db.tbDropdownOptions.Remove(dropdownOption);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}