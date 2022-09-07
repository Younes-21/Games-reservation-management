using DEBUT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Cors;

namespace DEBUT.Controllers
{
    [RoutePrefix("api/person")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PersonController : ApiController

    {
        private SGBD db = new SGBD();

        public OkNegotiatedContentResult<DataTable> y { get; private set; }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Select()
        {
            return Ok(db.Cmd("exec getperson"));
        }
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Selectid(String id)
        {
            return Ok(
                db.Cmd("exec getpersonbyid @id", new Dictionary<string, object> { { "id", id } })

                );
        }

    }
}
