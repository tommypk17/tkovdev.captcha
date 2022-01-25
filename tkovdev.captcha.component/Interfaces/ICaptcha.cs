using System.Drawing;
using tkovdev.captcha.component.Enums;
using tkovdev.captcha.component.Models;

namespace tkovdev.captcha.component.Interfaces
{
    public interface ICaptcha
    {
        public string Secret { get; set; }
        public string EncodedSecret { get; set; }
        public byte[] Image { get; set; }
        public string Salt { get; set; }

        public string CreateSecret(int length);
        public string EncodeSecret();
        public string DecodeSecret();
        public byte[] CreateImage();
    }
}