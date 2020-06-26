using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ServiceModel;

namespace WPBPlugins
{
    public class DuplicateCheck : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins. If you are not registering
            //     the plug-in in the sandbox, then you do not have to add any tracing service related code.  
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for web service calls.  
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity contact)
            {
                try
                {
                    /**** Plug-in business logic goes here. ****/

                    var email = String.Empty;
                    if (contact.Contains("emailaddress1"))
                    {
                        email = contact.Attributes["emailaddress1"].ToString();

                        // select emailaddress1 from contact where emailaddress1 = 'email'
                        var query = new QueryExpression("contact")
                        {
                            ColumnSet = new ColumnSet(new String[] { "emailaddress1" }) // select emailaddress1
                        }; // from contact
                        query.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, email); // where emailaddress1 = 'email'

                        var collection = service.RetrieveMultiple(query);
                        if (collection.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Contact with email already exists!");
                        }
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
