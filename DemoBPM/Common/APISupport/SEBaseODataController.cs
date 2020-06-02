using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DemoBPM.Common.APISupport
{
    public class SEBaseODataController<TDbContext> : ODataController
        where TDbContext : DbContext
    {
        protected ILogger _log = null;
        protected TDbContext _db;

        public SEBaseODataController(string logIdentifier)
        {
            this._log = new log4netLogger(logIdentifier);

            try
            {
                _db = (TDbContext)System.Activator.CreateInstance(typeof(TDbContext));
            }
            catch (Exception ex)
            {
                _log.Critical("Cannot open database context. Details: {0}.", ex);

                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}