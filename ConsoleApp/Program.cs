using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;
using System.Linq;

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

                    // Fetch XML
                    var query = @"
                        <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='contact'>
                            <attribute name='fullname' />
                            <attribute name='parentcustomerid' />
                            <attribute name='telephone1' />
                            <attribute name='emailaddress1' />
                            <attribute name='contactid' />
                            <order attribute='fullname' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                              <condition attribute='address1_city' operator='eq' value='Redmond' />
                            </filter>
                          </entity>
                        </fetch>
                     ";
                    var collection = service.RetrieveMultiple(new FetchExpression(query));
                    foreach (var item in collection.Entities)
                    {
                        Console.WriteLine(item.Attributes["fullname"].ToString());
                    }

                    query = @"
                        <fetch version='1.0' distinct='false' mapping='logical' aggregate='true'>
                          <entity name='lead'>
                            <attribute name='leadid' aggregate='count' alias='NumberOfLeads' />
                          </entity>
                        </fetch>
                    ";
                    collection = service.RetrieveMultiple(new FetchExpression(query));
                    foreach (var item in collection.Entities)
                    {
                        Console.WriteLine(((AliasedValue)item.Attributes["NumberOfLeads"]).Value.ToString());
                    }

                    // LINQ
                    using (var context = new OrganizationServiceContext(service))
                    {
                        // Pull contacts
                        var records = (from c in context.CreateQuery("contact")
                                       where c["address1_city"].Equals("Redmond")
                                       select c);
                        foreach (var record in records)
                        {
                            if (record.Attributes.Contains("fullname"))
                                Console.WriteLine("{0} - {1}", record.Attributes["fullname"].ToString(), record.Attributes["address1_city"].ToString());
                        }
                    }

                    using (var context = new OrganizationServiceContext(service))
                    {
                        // Pull contacts
                        var records = (from c in context.CreateQuery("contact")
                                       join a in context.CreateQuery("account")
                                       on c["parentcustomerid"] equals a["accountid"]
                                       where c["parentcustomerid"] != null
                                       select new
                                       {
                                           fullname = c["fullnmae"],
                                           accountname = a["name"]
                                       });
                        foreach(var record in records)
                        {
                            Console.WriteLine("{0} - {1}", record.fullname, record.accountname);
                        }
                    }
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
