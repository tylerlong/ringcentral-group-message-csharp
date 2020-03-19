using System;
using System.Threading.Tasks;
using RingCentral;

namespace RingCentralSendGroupMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var rc = new RestClient(
                Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_ID"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_SECRET"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_SERVER_URL")
            ))
            {
                Task.Run(async () =>
                {
                    await rc.Authorize(
                        Environment.GetEnvironmentVariable("RINGCENTRAL_USERNAME"),
                        Environment.GetEnvironmentVariable("RINGCENTRAL_EXTENSION"),
                        Environment.GetEnvironmentVariable("RINGCENTRAL_PASSWORD")
                    );

                    var r = await rc.Restapi().Account().Extension().Sms().Post(new CreateSMSMessage
                    {
                        from = new MessageStoreCallerInfoRequest
                        {
                            phoneNumber = Environment.GetEnvironmentVariable("RINGCENTRAL_USERNAME")
                        },
                        // multiple recipient, so this is actually an MMS
                        // user received this message will be able to see all the recipient numbers
                        // so this is not a good way to send massive notifications.
                        to = new []
                        {
                            new MessageStoreCallerInfoRequest
                            {
                                phoneNumber = Environment.GetEnvironmentVariable("RINGCENTRAL_RECIPIENT_1")
                            },
                            new MessageStoreCallerInfoRequest
                            {
                                phoneNumber = Environment.GetEnvironmentVariable("RINGCENTRAL_RECIPIENT_2")
                            }
                        },
                        text = "hello world"
                    });
                    Console.WriteLine(r);
                }).GetAwaiter().GetResult();
            }
        }
    }
}