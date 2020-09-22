using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MallRoof.Models;
using MallRoof.DAL;
using SendGrid.Helpers.Mail;
using MallRoof;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.ComponentModel;
using Limilabs.Client.IMAP;

namespace MallRoof
{
    public class EmailService : IIdentityMessageService
    {
        public static bool error = false;

        //public async Task SendAsync(IdentityMessage message)
        //{
        //    Microsoft.Exchange.WebServices.Data.ExchangeService service = new Microsoft.Exchange.WebServices.Data.ExchangeService(Microsoft.Exchange.WebServices.Data.ExchangeVersion.Exchange2007_SP1);

        //    // In case you have a dodgy SSL certificate:
        //    System.Net.ServicePointManager.ServerCertificateValidationCallback =
        //                delegate (Object obj, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        //                {
        //                    return true;
        //                };

        //    service.Credentials = new Microsoft.Exchange.WebServices.Data.WebCredentials
        //        (
        //        ConfigurationManager.AppSettings["mailAccount"]
        //        , ConfigurationManager.AppSettings["mailPassword"]
        //        );
        //    service.AutodiscoverUrl(message.Destination);
        //    //service.Url = new Uri("https://exchangebox/EWS/Exchange.asmx");

        //    Microsoft.Exchange.WebServices.Data.EmailMessage em = new Microsoft.Exchange.WebServices.Data.EmailMessage(service);
        //    em.Subject = message.Subject;
        //    em.Body = new Microsoft.Exchange.WebServices.Data.MessageBody(message.Body);
        //    em.Sender = new Microsoft.Exchange.WebServices.Data.EmailAddress(ConfigurationManager.AppSettings["mailAccount"]);
        //    em.ToRecipients.Add(new Microsoft.Exchange.WebServices.Data.EmailAddress(message.Destination));

        //    // Send the email and put it into the SentItems:
        //    em.SendAndSaveCopy(Microsoft.Exchange.WebServices.Data.WellKnownFolderName.SentItems);

        //}


        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.

            //return Task.FromResult(0);

            SmtpClient client = new SmtpClient();

            //ConfigurationManager.AppSettings["mailAccount"]

            client.Port = Int32.Parse(ConfigurationManager.AppSettings["mailPort"]);
            client.Host = ConfigurationManager.AppSettings["mailHost"];
            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"]
                , ConfigurationManager.AppSettings["mailPassword"]
                );

            MailMessage mailMessage = new MailMessage(ConfigurationManager.AppSettings["mailAccount"], message.Destination);
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mailAccount"], "Комманда Kenseler");
            mailMessage.Subject = message.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message.Body;




            //var res = await client.SendMailAsync(mailMessage);
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            mailMessageC = mailMessage;

            
            //var task = Task.CompletedTask;


            try
            {
                 //return 
                   await client.SendMailAsync(mailMessage);
                //using (Imap imap = new Imap())
                //{
                //    imap.Connect(ConfigurationManager.AppSettings["iMap"]);     // or ConnectSSL for SSL
                //    imap.UseBestLogin(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailPassword"]);
                    
                //    FolderInfo sent = new CommonFolders(imap.GetFolders()).Sent;
                //    imap.UploadMessage(sent, (Limilabs.Mail.IMail)mailMessageC);

                //    imap.Close();
                //}
            }
            catch
            {
                //task = Task.FromResult(0);
            }
            
            await Task.CompletedTask;
            //return client.SendMailAsync("kai124@yandex.ru", message.Destination, message.Subject, message.Body);

        }

        static MailMessage mailMessageC;

        static Limilabs.Mail.IMail FromMailToLMail(MailMessage mailMessage)
        {
            Limilabs.Mail.MailBuilder builder = new Limilabs.Mail.MailBuilder();
            builder.Subject = mailMessage.Subject;
            builder.Html = mailMessage.Body;
            builder.From.Add(new Limilabs.Mail.Headers.MailBox(mailMessage.From.Address, mailMessage.From.DisplayName));
            builder.To.Add(new Limilabs.Mail.Headers.MailBox(mailMessage.To.ToString(), ""));
            Limilabs.Mail.IMail email = builder.Create();
            return email;
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            //String token = (string)e.UserState;
            var t = e.UserState.ToString();

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", "token");
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", "token", e.Error.ToString());
                error = true;
                
            }
            else
            {
                Console.WriteLine("Message sent.");
                using (Imap imap = new Imap())
                {
                    imap.Connect(ConfigurationManager.AppSettings["iMap"]);     // or ConnectSSL for SSL
                    imap.UseBestLogin(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailPassword"]);
                    
                    FolderInfo sent = new CommonFolders(imap.GetFolders()).Sent;
                    //imap.UploadMessage(sent, (Limilabs.Mail.IMail) mailMessageC);
                    imap.UploadMessage(sent, FromMailToLMail(mailMessageC));
                    imap.Close();
                }
            }
        }

        //private Task configSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();
        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new EmailAddress("kai124@yandex.ru", "Kai Kai");

        //    myMessage.Subject = message.Subject;

        //    myMessage.PlainTextContent = message.Body;
        //    myMessage.HtmlContent = message.Body;

        //    var credentials = new NetworkCredential(
        //               ConfigurationManager.AppSettings["mailAccount"],
        //               ConfigurationManager.AppSettings["mailPassword"]
        //               );

        //    // Create a Web transport for sending email.
        //    var transportWeb = new Web(credentials);


        //    // Send the email.
        //    if (transportWeb != null)
        //    {
        //        return transportWeb.DeliverAsync(myMessage);
        //    }
        //    else
        //    {
        //        return Task.FromResult(0);
        //    }
        //}
    }


    
}

public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {

        }


        public static ApplicationUserManager CreateDefault(MallContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<User>(context));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            var manager = new ApplicationUserManager(new UserStore<User>(context.Get<MallContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            // pass validation
            manager.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,

                RequiredLength = 0,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            ///It is based on the same context as the ApplicationUserManager
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<MallContext>()));

            return appRoleManager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }


