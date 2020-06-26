using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace WPBPlugins
{
    public class HelloWord : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins. If you are not registering
            //     the plug-in in the sandbox, then you do not have to add any tracing service related code.  
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity contact)
            {
                try
                {
                    /**** Other examples 
                        context.SharedVariables.Add("Key1", "Some Info"); // save
                        var key = context.SharedVariables["Key1"].ToString(); // read in same pipeline only
                    ****/

                    /**** Plug-in business logic goes here. ****/

                    var firstName = string.Empty;
                    if (contact.Attributes.Contains("firstname"))
                    {
                        firstName = contact.Attributes["firstname"].ToString();
                    }

                    var fullName = contact.Attributes["fullname"].ToString();
                    contact.Attributes.Add("description", "Hello World, " + fullName);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in Hello Word-in.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("Hello Word: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
