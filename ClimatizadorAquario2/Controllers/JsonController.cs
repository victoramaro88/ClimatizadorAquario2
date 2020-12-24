using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ClimatizadorAquario2.Models;

namespace ClimatizadorAquario2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        //private IConfiguration configuration;
        //public JsonController(IConfiguration iConfig)
        //{
        //    configuration = iConfig;
        //}

        private readonly IOptions<MySettingsModel> appSettings;
        public JsonController(IOptions<MySettingsModel> app)
        {
            appSettings = app;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            //string dbConn = configuration.GetSection("MySettings").GetSection("DbConnection").Value;
            //string dbConn2 = configuration.GetValue<string>("MySettings:DbConnection");

            var dbValue = appSettings.Value.Email;

            var list = new List<string>();
            list.Add("Victor");
            list.Add("Amaro");
            list.Add(dbValue);
            return Ok(list);
        }

        [HttpGet]
        public bool InsereJson(string arquivoJson)
        {
            try
            {
                

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
