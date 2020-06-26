using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace WPBPlugins
{
    public class TaskCreate : IPlugin
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

                    var task = new Entity("task");
                    // Single line of text
                    task.Attributes.Add("subject", "Follow up");
                    task.Attributes.Add("description", "Please follow up with contact.");
                    // Date
                    task.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));
                    // Select option
                    task.Attributes.Add("prioritycode", new OptionSetValue(2));
                    // Parent
                    // task.Attributes.Add("regardingobjectid", new EntityReference("contact", contact.Id));
                    task.Attributes.Add("regardingobjectid", contact.ToEntityReference());

                    var taskGUID = service.Create(task);
                    tracingService.Trace("Task created with GUID {0}", taskGUID.ToString());
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
