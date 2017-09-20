// Write your JavaScript code.
$(document).ready(function () {

});

$('#player-request').on('click', function () {
    $.ajax({
        type: "POST",
        url: "/StellarView/GetUser",
        data: {
            player: $('#user-name').val(),
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