$(document).ready(function () {
    $("#playerDropdown").change(function () {
        var playerId = $(this).val();
        if (playerId) {
            $.ajax({
                url: '/api/Players/GetPlayerGames',
                type: 'GET',
                data: { playerId: playerId },
                success: function (data) {
                    $("#playerGames").html(data);
                }
            });
        } else {
            $("#playerGames").html("");
        }
    });
});
