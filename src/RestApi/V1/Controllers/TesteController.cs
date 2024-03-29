﻿using Asp.Versioning;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using RestApi.Controllers;

namespace RestApi.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;

        public TesteController(INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
    
        }

        [HttpGet]
        public string Valor()
        {

            return "Sou a V1";
        }
    }
}