﻿using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbRequestController : TBBaseController<Entities, tbRequest>
    {
        public TbRequestController()
            : base("TbRequestController")
        { }

        [EnableQuery(PageSize = 100)]
        public override IQueryable<tbRequest> Get()
        {
            return _db.tbRequests.AsQueryable();
        }

        public override SingleResult<tbRequest> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbRequests.Where(tbRequest => tbRequest.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbRequest se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existRequest = _db.tbRequests.Where(tbRequest => (tbRequest.Keyword == se.Keyword)).FirstOrDefault();

            if (existRequest != null)
            {
                return BadRequest();
            }

            se.CreatedTime = TimeZoneInfo.ConvertTime((DateTime)se.CreatedTime, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            se.StartDate = TimeZoneInfo.ConvertTime((DateTime)se.StartDate, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            se.DueDate = TimeZoneInfo.ConvertTime((DateTime)se.DueDate, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

            _db.tbRequests.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRequest> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = _db.tbRequests.Find(key);
            if (request == null)
            {
                return NotFound();
            }

            Validate(patch.GetInstance());
            patch.Patch(request);
            await _db.SaveChangesAsync();

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbRequest request = _db.tbRequests.Find(key);
            if (request == null)
            {
                return NotFound();
            }

            _db.tbRequests.Remove(request);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}