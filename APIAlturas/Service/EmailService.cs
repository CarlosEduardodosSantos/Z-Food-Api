﻿using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using APIAlturas.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace APIAlturas.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async  Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender.Replace("=", "@")));

                mimeMessage.To.Add(new MailboxAddress(email));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    

                    if (_env.IsDevelopment())
                    {
                        // The third parameter is useSSL (true if the client should make an SSL-wrapped
                        // connection to the server; otherwise, false).
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.None);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.None);
                    }

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}