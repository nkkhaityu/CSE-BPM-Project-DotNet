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
                var payload = new
                {
                    registration_ids = deviceIds,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        title = "Cập nhật yêu cầu",
                        body = "Nhấp để mở chi tiết.",
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
                            RequestName = requestInstance.RequestName,
                            RequestDescription = requestInstance.RequestDescription,
                            NumOfSteps = requestInstance.NumOfSteps,
                            UserName = requestInstance.UserName,
                            Mail = requestInstance.Mail,
                            Phone = requestInstance.Phone,
                            FullName = requestInstance.FullName
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
}