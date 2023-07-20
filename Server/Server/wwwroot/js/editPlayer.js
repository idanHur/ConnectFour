$(document).ready(function () {
    $('#playerActionsContainer').addClass('d-none'); // initially hide all buttons
    console.log('Script loaded');

    $('#passwordCheckForm').on('submit', function (e) {
        e.preventDefault(); // prevent the form from submitting normally
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
