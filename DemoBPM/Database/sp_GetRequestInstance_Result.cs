//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoBPM.Database
{
    using System;
    
    public partial class sp_GetRequestInstance_Result
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> RequestID { get; set; }
        public string DefaultContent { get; set; }
        public Nullable<int> CurrentStepIndex { get; set; }
        public string Status { get; set; }
        public Nullable<int> NumOfSteps { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}