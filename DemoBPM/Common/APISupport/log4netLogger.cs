using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoBPM.Common.APISupport
{
    public class log4netLogger : ILogger
    {
        log4net.ILog log;
        public log4netLogger()
        {

        }
        public log4netLogger(string identifier)
            : this()
        {
            log = log4net.LogManager.GetLogger(identifier);
        }
        public virtual void Info(string message)
        {
            log.Info(message);
        }
        public virtual void Info(string message, Exception ex)
        {
            log.Info(message, ex);
        }

        public virtual void Info(string message, object infor)
        {
            this.Info($"Message: {message}; Infor: {Newtonsoft.Json.JsonConvert.SerializeObject(infor)}");
        }
        public virtual void Info(object message)
        {
            log.Info(message);
        }
        public virtual void Info(object message, Exception ex)
        {
            log.Info(message, ex);
        }
        public virtual void Info(string format, params string[] p)
        {
            log.InfoFormat(format, p);
        }




        public virtual void Error(string message)
        {
            log.Error(message);
        }
        public virtual void Error(string message, Exception ex)
        {
            log.Error(message);
        }
        public virtual void Error(string format, params string[] p)
        {
            log.ErrorFormat(format, p);
        }


        public virtual void Debug(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        public virtual void Debug(string format, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(format, ex);
        }
        public virtual void Debug(string format, params string[] p)
        {
            System.Diagnostics.Debug.WriteLine(format, p);
        }



        public virtual void Critical(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        public virtual void Critical(string format, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(format, ex);
        }
        public virtual void Critical(string format, params string[] p)
        {
            System.Diagnostics.Debug.WriteLine(format, p);
        }
    }
}