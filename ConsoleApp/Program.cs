using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient service = null;
            try
            {
                // Authentication
                service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRMServer"].ConnectionString);
                if (service != null && service.IsReady)
                {
                    // Create a contact in dynamics 365
                    var contact = new Entity("contact");
                    contact.Attributes.Add("lastname", "Console App");
                    Guid guid = service.Create(contact);
                    Console.WriteLine(guid);
                }
                else
                {
                    // Display the last error.  
                    Console.WriteLine("An error occurred: {0}", service.LastCrmError);

                    // Display the last exception message if any.   
                    Console.WriteLine(service.LastCrmException.Message);
                    Console.WriteLine(service.LastCrmException.Source);
                    Console.WriteLine(service.LastCrmException.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (service != null)
                    service.Dispose();
                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }
        }
    }
}
