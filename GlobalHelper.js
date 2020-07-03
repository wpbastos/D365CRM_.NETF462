var Helper = window.Helper || {};
(
    function() {
        this.doSomething = function(context) {
            alert("Message ....");
        }
    }    
).call(Helper);