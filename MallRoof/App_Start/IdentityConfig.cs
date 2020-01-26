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

namespace MallRoof
{
    public class EmailService : IIdentityMessageService
    {
        public static bool error = false;
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

            var task = Task.CompletedTask;
            

            try
            {
                 //return 
                   await client.SendMailAsync(mailMessage);
            }
            catch
            {
                //task = Task.FromResult(0);
            }
            
            await Task.CompletedTask;
            //return client.SendMailAsync("kai124@yandex.ru", message.Destination, message.Subject, message.Body);

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


