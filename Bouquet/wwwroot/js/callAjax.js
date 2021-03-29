

$(document).ready(function () {
    var url = "/Admin/Product/GetAll";

    $.getJSON(url, function (data) {

        var html = "";
        $.each(data.items, function (i, item) {
            html += "<h2>" + item.name + "</h2>";
        });
        $("#photos").html(html);
    });

});

$(document).ready(function () {
	var url = "http://api.flickr.com/services/feeds/photos_public.gne?" +
		"id=82407828@N07&format=json&jsoncallback=?&tags=vectacorp";

	$.getJSON(url, function (data) {
		var html = "";

		$.each(data.items, function (i, item) {
			html += "<h3>" + item.title + "</h3>";
			html += item.description;

			// Remove the first paragraph of the description
			html = html.replace(
				"<p><a href=\"http://www.flickr.com/people/82407828@N07/\">" +
				"zakruvalcaba<\/a> posted a photo:<\/p>", "");
		});

		$("#team").html(html);
	});
});



