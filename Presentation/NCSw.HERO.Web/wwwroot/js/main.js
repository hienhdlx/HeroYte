
$(document).ready(function () {

	/*append select2*/
    $('.select2').select2();

	$('.select2-selection__arrow').append('<i class="fa fa-chevron-down" style="padding-top: 7px;"></i>');
    $('.select2-selection__arrow b').remove();

    $('[data-toggle="tooltip"]').tooltip();

    $('.panel.panel-default.panel-search .form-input-item').css("width", "calc(100% - " + ($('label[for="SearchName"]').width() + 5) + "px)");
});