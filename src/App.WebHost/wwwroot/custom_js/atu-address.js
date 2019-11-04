$(document).ready(function () {
    // ATU
    $('#CityName').focusout(function () {
        if ($(this).val() === '') {
            $('#CityId').val('');
            $('#StreetName').val('');
            $('#StreetId').val('');
        }
        updateStreetUrl();
    });
    updateStreetUrl();
    function updateStreetUrl() {
        var id = $('#CityId').val();
        $('#btn-street').attr('data-tab-container-url',
            '/atu/AtuModalStreet?cityId=' + id);
    }

    $('#CityName').change(function () {
        if ($(this).val() !== $('#OldCityName').val()) {
            $(this).val('');
            $('#CityId').val('');
            $('#StreetName').val('');
            $('#StreetId').val('');
        }
    });
    $('#StreetName').change(function () {
        if ($(this).val() !== $('#OldStreetName').val()) {
            $(this).val('');
            $('#StreetId').val('');
        }
    });
});
function OnSelectCity(item) {
    $('#OldCityName').val(item.value);
}
function OnSelectStreet(item) {
    $('#OldStreetName').val(item.value);
}