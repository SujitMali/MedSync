using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MedSync_ClassLibraries.Helpers
{
    public class EmailHelper
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;
        private readonly bool enableSsl;

        public EmailHelper(string server, int port, string user, string pass, bool ssl = true)
        {
            smtpServer = server;
            smtpPort = port;
            smtpUser = user;
            smtpPass = pass;
            enableSsl = ssl;
        }

        public bool SendEmail(string to, string subject, string body, int CurrentUserId = 1)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(smtpUser, "MedSync Notifications");
                    message.To.Add(to);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient(smtpServer, smtpPort))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        client.EnableSsl = enableSsl;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;

                        client.Send(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return false;
            }
        }

        public string LoadEmailTemplate(string templateFileName, dynamic data, bool forDoctor = false, int CurrentUserId = 1)
        {
            try
            {
                string templatePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Views",
                    "Emails",
                    templateFileName
                );

                if (!File.Exists(templatePath))
                    throw new FileNotFoundException($"Email template not found: {templatePath}");

                string body = File.ReadAllText(templatePath);

                string appointmentDateStr = "";
                if (DateTime.TryParse(Convert.ToString(data.AppointmentDate), out DateTime appointmentDate))
                    appointmentDateStr = appointmentDate.ToString("dd MMM yyyy");

                string startTimeStr = "";
                if (TimeSpan.TryParse(Convert.ToString(data.StartTime), out TimeSpan startTime))
                    startTimeStr = startTime.ToString(@"hh\:mm");

                string endTimeStr = "";
                if (TimeSpan.TryParse(Convert.ToString(data.EndTime), out TimeSpan endTime))
                    endTimeStr = endTime.ToString(@"hh\:mm");

                body = body.Replace("{{PatientName}}", Convert.ToString(data.PatientName) ?? "")
                           .Replace("{{DoctorName}}", Convert.ToString(data.DoctorName) ?? "")
                           .Replace("{{AppointmentDate}}", appointmentDateStr)
                           .Replace("{{StartTime}}", startTimeStr)
                           .Replace("{{EndTime}}", endTimeStr)
                           .Replace("{{CancellationReason}}", Convert.ToString(data.CancellationReason) ?? "");

                return body;
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return string.Empty;
            }
        }
    
    }
}
