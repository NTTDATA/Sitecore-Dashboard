function Widget(id, apiUrl, options) {
    this.id = id;
    this.apiUrl = apiUrl;
    Dashboard.ViewModel[id] = ko.observable();
    this.update(options);
}

Widget.prototype.update = function (options) {
    var self = this,
        settings = $j.extend({
            data: {},
            success: function () { },
            error: function () { }
        }, options);

    $j.ajax({
        type: "GET",
        data: settings.data,
        dataType: "json",
        cache: false,
        url: self.apiUrl,
        success: function (data) {
            Dashboard.ViewModel[self.id](JSON.parse(data));
            settings.success.call(data);
            return data;
        },
        error: function (data) {
            console.log('Error: ' + data.responseText);
            settings.error.call(data);
        }
    });
};