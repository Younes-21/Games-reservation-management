using DEBUT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Cors;
using System.Globalization;

namespace DEBUT.Controllers
{ [RoutePrefix("api/booking")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookingController : ApiController
    {
            private SGBD db = new SGBD();

            public OkNegotiatedContentResult<DataTable> y { get; private set; }

            [HttpGet]
            [Route("")]
            public IHttpActionResult Select()
            {
                return Ok(db.Cmd("exec getbooking"));
            }
            [HttpGet]
            [Route("{id}")]
            public IHttpActionResult Selectid(int id)
            {
                return Ok(
                    db.Cmd("exec getbookingbyid @id", new Dictionary<string, object> { { "id", id } })

                    );
            }
        [HttpGet]
        [Route("byperson/{PersonID}")]
        public IHttpActionResult Selectbypersonid(string PersonID)
        {
            return Ok(
                db.Cmd("exec getbookingbyperson @PersonID", new Dictionary<string, object> { { "@PersonID", PersonID} })

                );
        }

        [HttpDelete]
            [Route("delete/{id}")]
            public IHttpActionResult Delete(int id)
            {
                y = Ok(
                     db.Cmd("exec deletebookingbyid @id", new Dictionary<string, object> { { "id", id } }));

                return y;
            }
        [HttpPut]
        [Route("update/{BookingID}/{ScheduleID}")]
        public IHttpActionResult Update(int BookingID , int ScheduleID)
        {
            return Ok(

                db.Cmd("exec updatebooking @BookingID,@ScheduleID", new Dictionary<string, object> { {"BookingID",BookingID},{ "ScheduleID", ScheduleID } })

                );
        }

        [HttpPost]
            [Route("insert/{ScheduleID}/{PersonID}/{BookingDate}")]
        public IHttpActionResult Insert(int ScheduleID, string PersonID, DateTime BookingDate)
            {
                return Ok(
             db.Cmd("exec insertbooking @ScheduleID,@PersonID,@BookingDate", new Dictionary<string, object> { { "ScheduleID", ScheduleID }, { "PersonID", PersonID }, { "BookingDate", BookingDate } })

              );
            }
       
        [HttpGet]
        [Route("bookingbysite/{SiteID}")]
        public IHttpActionResult Selectbysite(string SiteID)
        {
            return Ok(
                db.Cmd("exec getbookingbysite @SiteID", new Dictionary<string, object> { { "SiteID", SiteID } })
                );
        }
        [HttpGet]
        [Route("nbrbooking/{id}")]
        public IHttpActionResult NbrBooking(int id)
        {
            return Ok(
                db.Cmd("exec nbrBooking @id", new Dictionary<string, object> { { "id", id } })

                );
        }
        [HttpGet]
        [Route("bookingexist/{ScheduleID}/{PersonID}")]
        public IHttpActionResult NbrBooking(int ScheduleID, string PersonID)
        {
            return Ok(
                db.Cmd("exec BookingExists @ScheduleID,@PersonID", new Dictionary<string, object> { { "ScheduleID", ScheduleID }, { "PersonID", PersonID } })

                );
        }
        [HttpGet]
        [Route("findbyname/{GameName}/{SiteID}")]
        public IHttpActionResult findbyname(string GameName , string SiteID)
        {
            return Ok(
                db.Cmd("exec findbyname @GameName,@SiteID", new Dictionary<string, object> { { "GameName", GameName },{"SiteID",SiteID} })

                );
        }
        [HttpGet]
        [Route("findbydate/{BeginDate}/{SiteID}")]
        public IHttpActionResult findbydate(String BeginDate, string SiteID)
        {
           
            return Ok(
                db.Cmd("exec findbydate @BeginDate,@SiteID", new Dictionary<string, object> { { "BeginDate", BeginDate }, { "SiteID", SiteID } })

                );
        }
        [HttpGet]
        [Route("findbydateandname/{BeginDate}/{GameName}/{SiteID}")]
        public IHttpActionResult findbydateandname(String BeginDate , string GameName , string SiteID)
        {

            return Ok(
                db.Cmd("exec findbydateandname @BeginDate,@GameName,@SiteID", new Dictionary<string, object> { { "BeginDate", BeginDate },{"GameName",GameName}, { "SiteID", SiteID } })

                );
        }


    }
    }