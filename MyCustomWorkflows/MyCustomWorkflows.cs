using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Linq;

namespace MyCustomWorkflows
{
    public class GetTaxWorkflow : CodeActivity
    {
        [RequiredArgument]
        [Input("Key")]
        public InArgument<string> Key { get; set; }

        [Output("Value")]
        public OutArgument<string> Value { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var tracingService = context.GetExtension<ITracingService>();

            var workflowContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();

            // Use the context service to create an instance of IOrganizationService.             
            var service = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);

            var keyValue = Key.Get(context);

            // Get data from Configuraction Entity
            // Call organization web service

            var query = new QueryByAttribute("wpb_configuration");
            query.ColumnSet = new ColumnSet(new string[] { "wpb_value" });
            query.AddAttributeValue("wpb_name", keyValue);

            var collection = service.RetrieveMultiple(query);

            if (collection.Entities.Count != 1)
            {
                tracingService.Trace("Something is wrong with configuration");
            }

            var config = collection.Entities.FirstOrDefault();
            Value.Set(context, config.Attributes["wpb_value"].ToString());
            tracingService.Trace("Value: " + Value.Get(context));
        }
    }
}
