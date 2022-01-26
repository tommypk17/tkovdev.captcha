using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using tkovdev.captcha.api.Models;
using tkovdev.captcha.component.Enums;
using tkovdev.captcha.component.Interfaces;
using tkovdev.captcha.component.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CaptchaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // GET: api/Captcha
        [HttpGet]
        public IActionResult Get()
        {
            ICaptcha x = new Captcha(_configuration.GetValue<string>("Captcha:Salt"));
            Response.Headers.Add("X-Captcha-Encoded", x.EncodedSecret);
            Response.Headers.Add("X-Captcha-TimeStamp", x.TimeStamp.ToString("O"));
            return File( x.Image, "image/jpeg");
        }
        
        // POST: api/Captcha
        [HttpPost]
        public IActionResult Post([FromBody] CaptchaRequest captchaRequest)
        {
            try
            {
                ICaptcha toValidate = new Captcha()
                {
                    Salt = _configuration.GetValue<string>("Captcha:Salt"),
                    Secret = captchaRequest.Secret,
                    EncodedSecret = captchaRequest.EncodedSecret,
                    TimeStamp = captchaRequest.TimeStamp
                };

                CaptchaResponse res =
                    new CaptchaResponse(Captcha.ValidateCaptcha(toValidate));

                return StatusCode(StatusCodes.Status200OK, res);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new CaptchaResponse(ECaptchaValid.Failure));
            }
        }
    }
}
