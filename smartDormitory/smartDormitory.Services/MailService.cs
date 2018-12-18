//using MailKit.Net.Smtp;
//using MimeKit;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services
{
    public class MailService : IMailService
    {
        private readonly StringBuilder emailMessage = new StringBuilder();
        private readonly string emailName = "ICB Smart Dormitory";
        private readonly string emailFromAddress = "smart.dormitory.mb@gmail.com";
        private readonly string emailSubject = "Sensors out of range";
        private readonly string emailAuthenticatePassword = "smartdormitory123";
        private readonly string smtp = "smtp.gmail.com";
        private readonly int smtpPort = 587;

        public async Task<bool> SendEmail(IEnumerable<UserSensors> userSensors, string username, string email)
        {
            if (userSensors == null)
            {
                throw new ArgumentNullException("User sensors cannot be null!");
            }

            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null!");
            }

            if (email == null)
            {
                throw new ArgumentNullException("Email cannot be null!");
            }

            if (userSensors.Count() != 0)
            {
                foreach (var sensor in userSensors)
                {
                    if ((sensor.Alarm == true && sensor.IsDeleted == false) && (sensor.Sensor.Value < sensor.MinValue || sensor.Sensor.Value > sensor.MaxValue))
                    {
                        emailMessage.Append($"Your sensor with name {sensor.Name} is out of range!\n");
                    }
                }
             
                MailAddress recipientMail = new MailAddress(email);
                MailAddress senderMail = new MailAddress(emailFromAddress);
                MailMessage mailMsg = new MailMessage(senderMail, recipientMail);

                mailMsg.Subject = emailSubject;
                mailMsg.Body = emailMessage.ToString();

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential(emailFromAddress, emailAuthenticatePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mailMsg);
                    return true;
                }
            }
            return false;
        }
    }
}
