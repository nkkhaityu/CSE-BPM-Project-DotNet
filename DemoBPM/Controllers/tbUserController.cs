using DemoBPM.Common.APISupport;
using DemoBPM.Common.Security;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    [SEAuthorize]
    public class TbUserController : TBBaseController<Entities, tbUser>
    {
        public TbUserController()
            : base("tbUserController")
        { }

        [EnableQuery(PageSize = 20)]
        public override IQueryable<tbUser> Get()
        {
            return _db.tbUsers.AsQueryable();
        }

        [EnableQuery]
        public override SingleResult<tbUser> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbUsers.Where(tbUser => tbUser.ID == key));
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetCurrentUser()
        {
            return Ok(_db.tbUsers.Where(tbUser => tbUser.ID == AuthSession.Current.UserId));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Login(ODataActionParameters parameters)
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

            // Return current user ID and role
            var userID = AuthSession.Current.UserId;

            var userRole = _db.tbUserRoles.Where(tbUserRole => tbUserRole.UserId == userID);

            return Ok(userRole);
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
            tbUser user = _db.tbUsers.Find(key);
            if (user == null)
            {
                return NotFound();
            }

            _db.tbUsers.Remove(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetUserRole()
        {
            var result = _db.sp_GetUserRole();

            return Ok(result);
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