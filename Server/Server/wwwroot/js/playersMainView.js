$(document).ready(function () {
    $('#selectData').on('change', function () {
        var value = $(this).val();

        // Show/hide the checkbox depending on the selected value
        if (value === 'AllPlayers') {
            $('#sortByNameCheck').show();
        } else {
            $('#sortByNameCheck').hide();
            $('#sortByName').prop('checked', false); // Uncheck the checkbox
        }

        // Set the url based on the checkbox state and selected value
        var url = '/api/Players/';
        if ($('#sortByName').is(':checked')) {
            url += 'SortByName';
        } else if (value === 'SelectPlayer') {
            url = '/api/Players/GetPlayerDropdown'; // new API endpoint to fetch the player dropdown partial view
        } else {
            url += value;
        }
        $('#dataContainer').load(url);
    });

    // Event for the checkbox
    $('#sortByName').on('change', function () {
        var url = '/api/Players/';
        if ($(this).is(':checked')) {
            url += 'SortByName';
        } else {
            url += 'AllPlayers';
        }
        $('#dataContainer').load(url);
    });

    // Trigger the change event for the dropdown when the page loads
    $('#selectData').val('AllPlayers').trigger('change');
});