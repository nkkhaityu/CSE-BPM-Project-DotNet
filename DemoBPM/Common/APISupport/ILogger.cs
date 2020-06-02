using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoBPM.Common.APISupport
{
    public interface ILogger
    {
        void Info(string format, params string[] p);
        void Info(string message, Exception ex);
        void Info(string message);
        void Info(string message, object infor);
        void Info(object message);
        void Info(object message, Exception ex);


        void Error(string format, params string[] p);
        void Error(string message, Exception ex);
        void Error(string message);



        void Debug(string format, params string[] p);
        void Debug(string format, Exception ex);
        void Debug(Exception ex);


        void Critical(string format, params string[] p);
        void Critical(string format, Exception ex);
        void Critical(Exception ex);
    }
}