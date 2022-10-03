using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace APIAlturas.Service
{
    public class TwilioService
    {
        public string EnviarSms(string msg, string to)
        {
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            const string accountSid = "ACaabde8fc2c99d2da55daea592d0d908f";
            const string authToken = "13cffd59995d8cb5abaa1744e56c9c6e";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: msg,
                from: new Twilio.Types.PhoneNumber($"+14805265873"),
                to: new Twilio.Types.PhoneNumber($"+55{to}")
            );


            return message.Sid;
        }
    }
}