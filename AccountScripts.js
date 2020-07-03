var Sdk = window.Sdk || {};
(
    function() {
        this.formOnLoad = function(context) {
            var formContext = context.getFormContext();
            var formType = formContext.ui.getFormType();

            if (formType == 1) {
                formContext.ui
                    .setFormNotification("User is creating account", "INFO", "type_1");
            } else if (formType == 2) {
                formContext.ui
                    .setFormNotification("User is opening existing account", "INFO", "type_2");
            } else if (formType == 3) {
                formContext.ui
                    .setFormNotification("User doesn't have permissions to edit the record", "INFO", "type_3");
            }
        }
        
        this.formOnSave = function(context) {
            var eventArgs = context.getEventsArgs();
            if (eventArgs.getSaveMode() == 70) {
                eventArgs.preventDefault();
            }
        }

        this.mainPhoneOnChange = function(context) {
            var formContext = context.getFormContext();
            var phoneNumber = formContext.getAttribute("telephone1").getValue();

            var patt = new RegExp(/((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}/);
            if (!patt.test(phoneNumber)) {
                formContext.getControl("telephone1")
                    .setNotification("Enter phone number in US format!", "telephonemsg");

                formContext.ui
                    .setFormNotification("Enter phone number in US format!", "INFO", "telephonemsg_1");
            } else {
                formContext.getControl("telephone1")
                    .clearNotification("telephonemsg");

                formContext.ui
                    .clearFormNotification("telephonemsg_1");
            }
        }
    }
).call(Sdk);