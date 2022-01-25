using tkovdev.captcha.component.Enums;

namespace tkovdev.captcha.api.Models
{
    public class CaptchaResponse
    {
        public CaptchaResponse(ECaptchaValid resultType)
        {
            ResultType = resultType;
            switch (resultType)
            {
                case ECaptchaValid.Mismatch:
                    Message = "Captcha did not match.";
                    Valid = false;
                    break;
                case ECaptchaValid.Timeout:
                    Message = "Captcha time extended beyond acceptable time limit.";
                    Valid = false;
                    break;
                case ECaptchaValid.Valid:
                    Message = "Captcha is valid";
                    Valid = true;
                    break;
                default:
                    Message = "Fatal error when parsing captcha response.";
                    Valid = false;
                    break;
            }
        }
        public string Message { get; set; }
        public ECaptchaValid ResultType { get; set; }
        public bool Valid { get; set; }
    }
}