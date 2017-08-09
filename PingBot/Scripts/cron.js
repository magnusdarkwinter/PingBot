if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {

    var ping = $.connection.pingHub,
        $clientView = $("#clientView"),
        itemTemplate = '<div id="{ClientID}" class="col-lg-2 col-md-4 col-sm-6 col-xs-12"><div class="panel panel-default card-view pa-0"><div class="panel-wrapper collapse in"><div class="panel-body pa-0"><div class="sm-data-box {IsUpColor}"><div class="container-fluid"><div class="row"><div class="col-xs-6 text-center pl-0 pr-0 data-wrap-left"><span class="txt-light block counter"><span class="counter-anim">{IpAddress}</span></span><span class="weight-500 uppercase-font txt-light block">{Name}<br>{Provider}</span></div><div class="col-xs-6 text-center  pl-0 pr-0 data-wrap-right"><i class="zmdi {IsUpIcon} txt-light data-right-rep-icon"></i></div></div></div></div></div></div></div></div>';

    function formatClient(client) {
        return $.extend(client, {
            IsUpColor: client.IsUp ? "bg-green" : "bg-red",
            IsUpIcon: client.IsUp ? "zmdi-devices" : "zmdi-devices-off"
        });
    }

    function init() {
        ping.server.getAllClients().done(function (clients) {
            $clientView.empty();
            $.each(clients, function () {
                var client = formatClient(this);
                
                $clientView.append(itemTemplate.supplant(client));
            });
        });
    }

    ping.client.updatePingStatus = function (client) {
        var displayClient = formatClient(client),
            $item = $(itemTemplate.supplant(displayClient));
        $("#" + client.IpAddress).replaceWith($item);
        $clientView.find("#" + client.ClientID).replaceWith($item);
    }

    $.connection.hub.start().done(init);
});