// JavaScript code to populate the countries in the select element
// Function to populate the select element with country options
function populateCountrySelect(countryData) {
    var selectElement = document.getElementById('country');

    for (var countryCode in countryData) {
        var option = document.createElement('option');
        option.value = countryCode;
        option.text = countryData[countryCode];
        selectElement.add(option);
    }
}

// AJAX request to fetch the JSON data
var xhr = new XMLHttpRequest();
xhr.open('GET', 'data/countries.json', true);
xhr.onload = function () {
    if (xhr.status === 200) {
        var countryData = JSON.parse(xhr.responseText);
        populateCountrySelect(countryData);
    }
};
xhr.send();
