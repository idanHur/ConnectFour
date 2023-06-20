// JavaScript code to populate the countries in the select element
$(document).ready(function () {
    $.ajax({
        url: 'https://api.myjson.com/bins/codku', // Replace with your API endpoint or JSON file URL
        dataType: 'json',
        success: function (data) {
            var selectElement = $('#country');
            $.each(data, function (key, value) {
                var option = $('<option>').text(value.name);
                selectElement.append(option);
            });
            selectElement.selectpicker('refresh');
        }
    });
});