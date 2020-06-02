using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    [SEAuthorize]
    public class TbUserController : TBBaseController<Entities, tbUser>
    {
        public TbUserController()
            : base("tbUserController")
        { }

        public override IQueryable<tbUser> Get()
        {
            return _db.tbUsers.AsQueryable();
        }

        public override SingleResult<tbUser> Get([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var u = parameters["user"];
            if (!(u is tbUser))
            {
                return BadRequest();
            }

            tbUser user = u as tbUser;

            try
            {
                var strLogin = Auth.Login(user.UserName, user.Password);
                if (strLogin.ToLower() != "true")
                {
                    return BadRequest(strLogin);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Logout()
        {
            SessionExtensions.Clear();

            //FormsAuthentication.SignOut();

            return Ok();
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbUser> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tbUser = _db.tbUsers.Find(key);
            if (tbUser == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(tbUser);

            await _db.SaveChangesAsync();

            return Ok(tbUser);
        }

        public override async Task<IHttpActionResult> PostEntity(tbUser se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.tbUsers.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IHttpActionResult GetUserRole()
        {
            //var userId = AuthSession.Current.UserId;
            var ret = _db.sp_GetUserRole();

            return Ok(ret);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}