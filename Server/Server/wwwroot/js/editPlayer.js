$(document).ready(function () {

    $('#passwordCheckForm').on('submit', function (e) {
        console.log('Form submitted');
        $('#playerActionsContainer').removeClass('d-none'); // show all buttons
    });

    $('#deleteGameButton, #editPlayerButton').on('click', function () {
        $('#playerActionsContainer').addClass('d-none'); // hide the action buttons
    });

    $('#editPlayerButton').on('click', function () {
        $('#editPlayerDiv').removeClass('d-none'); // show edit form
    });
});
