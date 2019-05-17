$(document).ready(function () {

    $("#BirthDay").datetimepicker({
        //debug: true,
        locale: app.dateTime.locale(),
        maxDate: getCurrentDate(),
        defaultDate: getCurrentDate()
    }).on('dp.show', function () {
        if (!$(this).val()) {
            return $(this).data('DateTimePicker').defaultDate(getCurrentDateDefaultForr());
        }
    });

    function getCurrentDateDefaultForr() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = dd + '/' + mm + '/' + yyyy;
        return today;
    }

    function getCurrentDate() {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = mm + '/' + dd + '/' + yyyy;
        return today;
    }
});