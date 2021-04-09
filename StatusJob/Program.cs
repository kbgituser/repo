using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using System.Data.Entity.Core;








//using Microsoft.AspNet.Identity.Owin;


namespace StatusJob
{
    class Program
    {
        private static MallRoof.DAL.MallContext mallContext;
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

            //Console.WriteLine("Hello World!");
            //Console.ReadKey();

            mallContext = MallRoof.DAL.MallContext.Create();

            Console.WriteLine("Hello World!");
            Console.WriteLine("Началась смена статусов заявок с 'Active' на 'Done'!");

            var demands = mallContext.Demands.Where(d => d.DemandStatus == MallRoof.Models.DemandStatus.Active
                && d.EndDate <= DateTime.Now
            );

            foreach (var dem in demands)
            {
                dem.DemandStatus = MallRoof.Models.DemandStatus.Done;
                Console.WriteLine("Статус запроса созданного " + dem.CreateDate.ToShortDateString()+ " изменен!" );
            }
            mallContext.Configuration.ValidateOnSaveEnabled = false;
            mallContext.SaveChanges();

            Console.WriteLine("Изменение статуса заявок с 'Active' на 'Done' закончено!");
            
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
