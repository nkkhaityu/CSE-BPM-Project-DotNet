using DemoBPM.Common.APISupport;
using DemoBPM.Database;
using Microsoft.AspNet.OData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoBPM.Controllers
{
    //[SEAuthorize]
    public class TbRequestInstanceController : TBBaseController<Entities, tbRequestInstance>
    {
        public TbRequestInstanceController()
            : base("TbRequestInstanceController")
        { }

        [EnableQuery(PageSize = 100)]
        public override IQueryable<tbRequestInstance> Get()
        {
            return _db.tbRequestInstances.AsQueryable();
        }

        public override SingleResult<tbRequestInstance> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.tbRequestInstances.Where(tbRequestInstance => tbRequestInstance.ID == key));
        }

        public override async Task<IHttpActionResult> PostEntity(tbRequestInstance se)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            se.CreatedDate = TimeZoneInfo.ConvertTime((DateTime)se.CreatedDate, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            se.FinishedDate = TimeZoneInfo.ConvertTime((DateTime)se.FinishedDate, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            _db.tbRequestInstances.Add(se);
            await _db.SaveChangesAsync();

            return Ok(se);
        }

        public override async Task<IHttpActionResult> PatchEntity([FromODataUri] int key, Delta<tbRequestInstance> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestInstance = _db.tbRequestInstances.Find(key);
            if (requestInstance == null)
            {
                return NotFound();
            }
            Validate(patch.GetInstance());

            patch.Patch(requestInstance);
            await _db.SaveChangesAsync();

            SendNotification(key);

            return Ok();
        }

        public override async Task<IHttpActionResult> DeleteEntity([FromODataUri] int key)
        {
            tbRequestInstance requestInstance = _db.tbRequestInstances.Find(key);
            if (requestInstance == null)
            {
                return NotFound();
            }

            _db.tbRequestInstances.Remove(requestInstance);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetRequestInstance()
        {
            var ret = _db.sp_GetRequestInstance();

            return Ok(ret);
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetRequestInstanceDetails()
        {
            var ris = _db.sp_GetRequestInstance().ToList();

            List<RequestInstanceDetails> resRIs = new List<RequestInstanceDetails>();
            foreach (sp_GetRequestInstance_Result ri in ris)
            {
                RequestInstanceDetails r = new RequestInstanceDetails();

                r.ID = ri.ID;
                r.UserID = ri.UserID;
                r.RequestID = ri.RequestID;
                r.DefaultContent = ri.DefaultContent;
                r.CurrentStepIndex = ri.CurrentStepIndex;
                r.Status = ri.Status;
                r.ApproverID = ri.ApproverID;
                r.CreatedDate = ri.CreatedDate;
                r.FinishedDate = ri.FinishedDate;
                r.Keyword = ri.Keyword;
                r.RequestName = ri.RequestName;
                r.RequestDescription = ri.RequestDescription;
                r.ResponseMessage = ri.ResponseMessage;
                r.NumOfSteps = ri.NumOfSteps;
                r.UserName = ri.UserName;
                r.Mail = ri.Mail;
                r.Phone = ri.Phone;
                r.FullName = ri.FullName;
                r.Code = ri.Code;

                var ips = _db.sp_GetRequestInstanceExpan(r.ID).ToList();
                if (ips.Count > 0)
                {
                    List<sp_GetRequestInstanceExpan_Result> ipExpans = new List<sp_GetRequestInstanceExpan_Result>();
                    foreach (sp_GetRequestInstanceExpan_Result ip in ips)
                    {
                        sp_GetRequestInstanceExpan_Result i = new sp_GetRequestInstanceExpan_Result();
                        i.Title = ip.Title;
                        i.TextAnswer = ip.TextAnswer;
                        i.FileUrl = ip.FileUrl;
                        i.InputFieldTypeID = ip.InputFieldTypeID;
                        ipExpans.Add(i);
                    }

                    r.InputFieldInstances = ipExpans;
                }
                resRIs.Add(r);
            }

            return Ok(resRIs);
        }

        //[EnableQuery]
        //[HttpGet]
        //public IHttpActionResult GetRequestInstanceDetails()
        //{
        //    var res = _db.tbRequestInstances.ToList();

        //    List<RequestInstanceDetails> resRIs = new List<RequestInstanceDetails>();

        //    foreach (tbRequestInstance r in res)
        //    {
        //        RequestInstanceDetails instanceDetails = new RequestInstanceDetails();
        //        instanceDetails.RequestInstance = r;
        //        instanceDetails.StepInstances = new List<StepInstanceDetails>();

        //        instanceDetails.InputFieldInstances = r.tbInputFieldInstances.ToList();
        //        instanceDetails.User = r.tbUser;

        //        resRIs.Add(instanceDetails);
        //    }

        //    return Ok(resRIs);
        //}

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetNumOfRequestInstance()
        {
            var ret = _db.sp_GetNumOfRequestInstance();

            return Ok(ret);
        }

        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetNumOfRequestInstanceToday()
        {
            var ret = _db.sp_GetNumOfRequestInstanceToday();

            return Ok(ret);
        }

        public void SendNotification(int key)
        {
            List<String> deviceIdList = new List<String>();
            var list = _db.tbDeviceTokens.Where(rs => rs.UserID == 4 && rs.IsLogin == true).ToList();
            if (list.Count > 0)
            {
                foreach (tbDeviceToken deviceToken in list)
                {
                    deviceIdList.Add(deviceToken.DeviceToken);
                }
                string[] deviceIds = deviceIdList.ToArray();

                sp_GetRequestInstance_Result requestInstance = _db.sp_GetRequestInstance().Where(rs => rs.ID == key).FirstOrDefault() as sp_GetRequestInstance_Result;
                if (requestInstance == null)
                {
                    return;
                }

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAApRTPiS0:APA91bHI50S86Fe_ZGgf-7JMfj2T5DJrhx-0wlMtUYdyarP9KWPdBO47q--JpSAJppsA6esVuN9UsSXZLUUByOmo8qn0oGp5Xzr2Vp2yyIHzrsg6OSr_frb672Xa361-5ivwNPDFHcoo"));
                //Sender Id - From firebase project setting  
                //tRequest.Headers.Add(string.Format("Sender: id={0}", "XXXXX.."));
                tRequest.ContentType = "application/json";
                var status = "";
                switch (requestInstance.Status)
                {
                    case "active":
                        status = "Trạng thái: " + "Đang thực hiện";
                        break;
                    case "done":
                        status = "✅ Trạng thái: " + "Thành công";
                        break;
                    case "failed":
                        status = "❌ Trạng thái: " + "Thất bại";
                        break;
                    default:
                        status = "Trạng thái: " + "Mới";
                        break;
                }
                var payload = new
                {
                    registration_ids = deviceIds,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        title = "Yêu cầu " + requestInstance.Keyword.Trim() + " - " + "mã " + requestInstance.ID,
                        body = "Bước hiện tại: " + requestInstance.CurrentStepIndex + "/" + requestInstance.NumOfSteps + System.Environment.NewLine + 
                               status,
                        badge = 1
                    },
                    data = new
                    {
                        requestInstance = new
                        {
                            ID = requestInstance.ID,
                            UserID = requestInstance.UserID,
                            RequestID = requestInstance.RequestID,
                            DefaultContent = requestInstance.DefaultContent,
                            CurrentStepIndex = requestInstance.CurrentStepIndex,
                            Status = requestInstance.Status,
                            ApproverID = requestInstance.ApproverID,
                            CreatedDate = requestInstance.CreatedDate,
                            FinishedDate = requestInstance.FinishedDate,
                            Keyword = requestInstance.Keyword,
                            RequestName = requestInstance.RequestName,
                            RequestDescription = requestInstance.RequestDescription,
                            ResponseMessage = requestInstance.ResponseMessage,
                            NumOfSteps = requestInstance.NumOfSteps,
                            UserName = requestInstance.UserName,
                            Mail = requestInstance.Mail,
                            Phone = requestInstance.Phone,
                            FullName = requestInstance.FullName,
                            Code = requestInstance.Code
                        },
                        click_action = "FLUTTER_NOTIFICATION_CLICK"
                    }

                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }
            }
        }
    }

    public class RequestInstanceDetails
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> RequestID { get; set; }
        public string DefaultContent { get; set; }
        public Nullable<int> CurrentStepIndex { get; set; }
        public string Status { get; set; }
        public Nullable<int> ApproverID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> FinishedDate { get; set; }
        public string Keyword { get; set; }
        public string RequestName { get; set; }
        public string RequestDescription { get; set; }
        public string ResponseMessage { get; set; }
        public Nullable<int> NumOfSteps { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }

        public IEnumerable<sp_GetRequestInstanceExpan_Result> InputFieldInstances { get; set; }
    }

    //public partial class RequestInstanceDetails
    //{
    //    public tbRequestInstance RequestInstance{ get; set; }
    //    public IEnumerable<StepInstanceDetails> StepInstances { get; set; }
    //    public IEnumerable<tbInputFieldInstance> InputFieldInstances { get; set; }
    //    public tbUser User { get; set; }
    //}

    //public partial class StepInstanceDetails
    //{
    //    public tbStepInstance StepInstance { get; set; }
    //    public IEnumerable<tbInputFieldInstance> InputFieldInstances { get; set; }
    //}
}