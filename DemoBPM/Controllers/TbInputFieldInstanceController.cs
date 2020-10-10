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

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbInputFieldInstance> Get()
        {
            return _db.tbInputFieldInstances.AsQueryable();
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

            return Ok();
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

        //[HttpPost]
        //public IHttpActionResult UploadFileInputFieldInstance(int id, string type, MultipartDataMediaFormatter.Infrastructure.FormData formData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("ModelState invalid");
        //    }

        //    try
        //    {
        //        if (formData == null)
        //        {
        //            return BadRequest();
        //        }
        //        var fileValue = formData.Files.FirstOrDefault();
        //        if (fileValue == null)
        //        {
        //            return BadRequest();
        //        }

        //        var file = fileValue.Value;
        //        var name = file.FileName;
        //        var data = file.Buffer;
        //        _db.spv2_Org_UploadFile(id, data, name, (int)SESession.Current.UserId, DateTime.UtcNow, type);
        //        return StatusCode(HttpStatusCode.NoContent);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}