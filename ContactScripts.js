var Sdk = window.Sdk || {};
(
    function() {
        //this.formOnLoad = function (executionContext) {
        //    var formContext = executionContext.getFormContext();
        //    var firstName = formContext.getAttribute("firstname").getValue();
        //    alert("Hello " + firstName);
        //}
        this.formOnLoad = function(executionContext) {
            var formContext = executionContext.getFormContext();
            var lookupAccount = formContext.getAttribute("parentcustomerid").getValue();

            if (Array.isArray(lookupAccount) && lookupAccount.length) {
                var guid = lookupAccount[0].id;
                var name = lookupAccount[0].name;
                var entityType = lookupAccount[0].entityType;

                var accountInfo = `Account's GUID: ${guid}, Name: ${name}, Entity type: ${entityType}`;
                formContext.ui.setFormNotification(accountInfo, "INFO", "accountName");
            }
        }


        this.shippingMethodOnchange = function(context) {
            var formContext = context.getFormContext();
            var shippingMethod = formContext.getAttribute("address1_shippingmethodcode").getText();

            if (shippingMethod === "FedEx") {
                formContext.getControl("address1_freighttermscode").setDisabled(true);
            } else {
                formContext.getControl("address1_freighttermscode").setDisabled(false);
            }
        }
    }
).call(Sdk);