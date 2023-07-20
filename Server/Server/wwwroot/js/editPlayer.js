$(document).ready(function () {

    $('#deleteGameButton, #editPlayerButton').on('click', function () {
        $('#playerActionsContainer').addClass('d-none'); // hide the action buttons
    });

    $('#editPlayerButton').on('click', function () {
        $('#editPlayerDiv').removeClass('d-none'); // show edit form
        $('#playerDetailsDiv').addClass('d-none'); // show edit form
    });

    $('#deleteGameButton').on('click', function () {
        $('#PlayerGamesDiv').removeClass('d-none'); // hide the action buttons
    });
});
