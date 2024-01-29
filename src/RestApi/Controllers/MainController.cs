using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/manha-controller")]
    public abstract class MainController : ControllerBase //abstract - ela SÒ pode ser herdada, por isso não apareceu no swagger
    {
        //Validacao de notificacoes de erro

        //validação de modelstate

        //validacao da opertacao de negocios
    }
}
