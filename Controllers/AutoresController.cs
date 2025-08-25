using System;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    [HttpGet]
    public String Get()
    {
        return "autores";
    }

}
