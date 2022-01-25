namespace tkovdev.captcha.component.Models
{
    public class CaptchaRequest
    {
        public string Secret { get; set; }
        public string EncodedSecret { get; set; }
    }
}