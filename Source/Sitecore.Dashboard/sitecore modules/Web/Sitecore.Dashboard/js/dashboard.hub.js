(function ($) {
    var dashboard = $.connection.dashboardHub;

    dashboard.client.connected = function (id, date) {
        //console.log("connected: " + id + " : " + date);
    };

    dashboard.client.disconnected = function (id, date) {
        //console.log("disconnected: " + id + " : " + date);
    };

    dashboard.client.broadcastMessage = function (message) {
        //console.log(message);
    };

    dashboard.client.raiseServerEvent = function (eventName) {
        //console.log(eventName + " server event raised");
        $(document).trigger(eventName);
    };

    $.connection.hub.start().done(function () {
        //console.log("connection started");
    });
})(jQuery);