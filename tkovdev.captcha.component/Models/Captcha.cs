using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using tkovdev.captcha.component.Enums;
using tkovdev.captcha.component.Interfaces;

namespace tkovdev.captcha.component.Models
{
    public class Captcha : ICaptcha
    {
        public string Secret { get; set; }
        public string EncodedSecret { get; set; } = null;
        public byte[] Image { get; set; }
        public string Salt { get; set; }
        public DateTime TimeStamp { get; set; }


        public Captcha(){}
        public Captcha(string salt, int length = 6, int timeout = 20)
        {
            Salt = salt;
            TimeStamp = DateTime.Now.AddMinutes(timeout);
            Secret = CreateSecret(length);
            EncodedSecret = EncodeSecret();
            Image = CreateImage();
        }
        public string CreateSecret(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public string EncodeSecret()
        {
            if (Salt == null || Salt.Trim() == "") throw new NullReferenceException("Captcha.Salt cannot be null.");
            if (Secret == null || Secret.Trim() == "") throw new NullReferenceException("Captcha.Secret cannot be null.");
            if (TimeStamp == default) throw new NullReferenceException("Captcha.TimeStamp must be set.");
            
            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(Secret, Salt, TimeStamp.ToString("O")));
            byte[] hashedBytes = hasher.ComputeHash(textWithSaltBytes);
            hasher.Clear();
            
            return Convert.ToBase64String(hashedBytes);
        }
        
        public string DecodeSecret()
        {
            if (Salt == null || Salt.Trim() == "") throw new NullReferenceException("Captcha.Salt cannot be null.");
            if (Secret == null || Secret.Trim() == "") throw new NullReferenceException("Captcha.Secret cannot be null.");
            if (TimeStamp == default) throw new NullReferenceException("Captcha.TimeStamp must be set.");

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(Secret, Salt, TimeStamp.ToString("O")));
            byte[] hashedBytes = hasher.ComputeHash(textWithSaltBytes);
            hasher.Clear();
            
            return Convert.ToBase64String(hashedBytes);
        }

        public byte[] CreateImage()
        {
            if (Secret == null || Secret.Trim() == "")
            {
                throw new NullReferenceException("Captcha.Secret is null or blank, set a value prior to calling CreateImage()");
            }
            Bitmap bitmap = new Bitmap(1, 1);
            Font font = new Font("Arial", 45, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics graphics = Graphics.FromImage(bitmap);
            
            int width = (int) graphics.MeasureString(Secret, font).Width;
            int height = (int) graphics.MeasureString(Secret, font).Height;
            
            bitmap = new Bitmap(bitmap, new Size(width, height));
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            graphics.DrawString(Secret, font, new SolidBrush(Color.FromArgb(255, 0, 0)), 0, 0);
            graphics.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Transparent), graphics.ClipBounds);
            graphics.Flush();
            graphics.Dispose();
            
            MemoryStream ms = new MemoryStream();  
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        
        public static ECaptchaValid ValidateCaptcha(ICaptcha captchaToValidate)
        {
            string toValidateEncoded = captchaToValidate.DecodeSecret();
            if (toValidateEncoded == captchaToValidate.EncodedSecret)
            {
                if (captchaToValidate.TimeStamp <= DateTime.Now)
                {
                    return ECaptchaValid.Timeout;
                }
                return ECaptchaValid.Valid;
            }

            return ECaptchaValid.Mismatch;
        }
    }
}