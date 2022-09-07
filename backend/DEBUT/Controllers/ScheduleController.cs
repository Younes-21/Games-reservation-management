using DEBUT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Cors;
using System.Globalization;

namespace DEBUT.Controllers
{
    [RoutePrefix("api/schedule")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ScheduleController : ApiController
    {
        private SGBD db = new SGBD();

        public OkNegotiatedContentResult<DataTable> y { get; private set; }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Select()
        {
            return Ok(db.Cmd("exec getschedule"));
        }
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Selectid(int id)
        {
            return Ok(
                db.Cmd("exec getschedulebyid @id", new Dictionary<string, object> { { "id", id } })

                );
        }
        
        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            y = Ok(
                 db.Cmd("exec deleteschedulebyid @id", new Dictionary<string, object> { { "id", id } }));

            return y;
        }



        [HttpPut]
        [Route("update/{ScheduleID}/{BeginDate}/{EndDate}/{Availableseats}/{GameID}")]
        public IHttpActionResult Update(int ScheduleID, string BeginDate, string EndDate, string Availableseats, int GameID)
        {
            var startDate = BuildDateTimeFromYAFormat(BeginDate);
            var endDate = BuildDateTimeFromYAFormat(EndDate);
            return Ok(

                db.Cmd("exec updateschedule @ScheduleID , @BeginDate,@EndDate, @Availableseats, @GameID", new Dictionary<string, object> { { "ScheduleID", ScheduleID }, { "BeginDate", startDate }, { "EndDate", endDate }, { "Availableseats", Availableseats }, { "GameID", GameID } })

                );
        }
        [HttpPut]
        [Route("updatetime/{ScheduleID}/{BeginDate}/{EndDate}")]
        public IHttpActionResult UpdateTime(int ScheduleID , string BeginDate, string EndDate)
        {
            var startDate = BuildDateTimeFromYAFormat(BeginDate);
            var endDate = BuildDateTimeFromYAFormat(EndDate);
            return Ok(

                db.Cmd("exec updatescheduletime  @ScheduleID,@BeginDate,@EndDate ", new Dictionary<string, object> { {"ScheduleID" , ScheduleID},{ "BeginDate", startDate }, { "EndDate", endDate } })

                );
        }
      
        [HttpPost]
        [Route("insert/{AddDate}/{BeginDate}/{EndDate}/{Availableseats}/{PersonID}/{GameID}")]
        public IHttpActionResult Insert(DateTime AddDate, String BeginDate, String EndDate, int Availableseats, string PersonID, int GameID)
        {
            var startDate = BuildDateTimeFromYAFormat(BeginDate);
            var endDate = BuildDateTimeFromYAFormat(EndDate);
            return Ok(
         db.Cmd("exec insertschedule @AddDate,@BeginDate,@EndDate,@Availableseats,@PersonID,@GameID", new Dictionary<string, object> { { "AddDate", AddDate }, { "BeginDate", startDate }, { "EndDate", endDate }, { "Availableseats", Availableseats }, { "PersonID", PersonID },  { "GameID", GameID } })

          );
        }
        [HttpGet]
        [Route("schedulebysite/{SiteID}")]
        public IHttpActionResult Selectbysite(string SiteID)
        {
            return Ok(
                db.Cmd("exec getschedulebysite @SiteID", new Dictionary<string, object> { { "SiteID", SiteID } })
               );
        }

        private DateTime BuildDateTimeFromYAFormat(string dateString)
        {

            var format = "ddMMyyyyTHHmm";
            CultureInfo provider = new CultureInfo("en-US");
            DateTime dt;


            dt = DateTime.ParseExact(dateString, format, provider);
            Console.WriteLine("{0} converts to {1}.", dateString, dt.ToString());
            return dt;

        }
    }
}