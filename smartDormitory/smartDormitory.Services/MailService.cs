using MailKit.Net.Smtp;
using MimeKit;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailName, emailFromAddress));
                message.To.Add(new MailboxAddress(username, email));
                message.Subject = emailSubject;
                message.Body = new TextPart("plain")
                {
                    Text = emailMessage.ToString()
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, smtpPort, false);
                    client.Authenticate(emailFromAddress, emailAuthenticatePassword);
                    await client.SendAsync(message);
                    client.Disconnect(true);
                    return true;
                }
            }
            return false;

        }
    }
}
