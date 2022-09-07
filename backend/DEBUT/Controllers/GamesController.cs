using DEBUT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Cors;


namespace DEBUT.Controllers
{
    [RoutePrefix("api/game")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GamesController : ApiController
    {
        private SGBD db = new SGBD();

        public OkNegotiatedContentResult<DataTable> y { get; private set; }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Select()
        {
            return Ok(db.Cmd("exec getgame"));
        }
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Selectid(int id)
        {
            return Ok(
                db.Cmd("exec getgamebyid @id", new Dictionary<string, object> { { "id", id } })

                );
        }
        [HttpGet]
        [Route("bysite/{id}")]
        public IHttpActionResult Selectbysite(String id)
        {
            return Ok(
                db.Cmd("exec getgamebysite @id", new Dictionary<string, object> { { "id", id } })

                );
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            y = Ok(
                 db.Cmd("exec deletegamebyid @id", new Dictionary<string, object> { { "id", id } }));

            return y;
        }
        [HttpPut]
        [Route("disable/{id}")]
        public IHttpActionResult Disable(int id)
        {
            return Ok(

                db.Cmd("exec disablegamebyid @id", new Dictionary<string, object> { { "id", id } })

                );
        }

        [HttpPut]
        [Route("unable/{id}")]
        public IHttpActionResult Unable(int id)
        {
            return Ok(

                db.Cmd("exec unablegamebyid @id", new Dictionary<string, object> { { "id", id } })

                );
        }


        [HttpPut]
        [Route("update/{GameID}/{GameName}/{SiteID}/{Status}")]
        public IHttpActionResult Update(int GameID, string GameName, string SiteID, Boolean Status)
        {
            return Ok(

                db.Cmd("exec updategame @GameID , @GameName, @SiteID, @Status", new Dictionary<string, object> { { "GameID", GameID }, { "GameName", GameName }, { "SiteID", SiteID }, { "Status", Status } })

                );
        }

        [HttpPost]
        [Route("insert/{GameName}/{SiteID}/{Status}/{PersonID}/{GameAddDate}")]
        public IHttpActionResult Insert(string GameName, string SiteID, string PersonID, Boolean Status, DateTime GameAddDate)
        {
            return Ok(
         db.Cmd("exec insertgame @GameName,@SiteID,@PersonID,@GameAddDate,@Status", new Dictionary<string, object> { { "GameName", GameName }, { "SiteID", SiteID }, { "PersonID", PersonID }, { "Status", Status }, { "GameAddDate", GameAddDate } })

          );
        }
    }
}