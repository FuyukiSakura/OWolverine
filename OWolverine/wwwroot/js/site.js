/* $(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/StellarView/GetServers",
        dataType: 'json',
        success: function (data) {
            $('#query-result').empty();
            if (data['status'] === 'success') {
                $.each(data['data'], function (index, value) {
                    $('#query-result').append(value['coords'] + ' - ' + value['name'] + "<br />");
                });
            } else {
                $('#query-result').html(data['data']);
            }
        }

    });
}); */

$('#player-request-form').on('submit', function (e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: "/StellarView/GetUser",
        data: {
            playerName: $('#user-name').val(),
            server: $("#server-name").val()
        },
        dataType: 'json',
        success: function (data) {
            $('#query-result').empty();
            if (data['status'] === 'success') {
                $.each(data['data'], function (index, value) {
                    $('#query-result').append(value['coords'] + ' - ' + value['name'] + "<br />");
                });
            } else {
                $('#query-result').html(data['data']);
            }
        }

    });
});