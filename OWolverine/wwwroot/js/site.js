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
        success: function (result) {
            $('#query-result').empty();
            if (result['status'] === 'success') {
                let data = result['data'][0];
                $('#query-result').append("艦隊數目：" + data['ships'] + "<br /><br />");
                $.each(data['planets'], function (index, value) {
                    $('#query-result').append(value['coords'] + ' - ' + value['name'] + "<br />");
                });
            } else {
                $('#query-result').html(result['message']);
            }
        },
        error: function () {
            $('#query-result').html(data['內部錯誤']);
        }
    });
});