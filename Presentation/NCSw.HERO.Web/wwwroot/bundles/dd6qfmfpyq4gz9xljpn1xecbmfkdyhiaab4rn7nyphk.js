$(document).ready(function () {

	/*append select2*/
	$('.select2').select2();

	$('.select2-selection__arrow').append('<i class="fa fa-chevron-down" style="padding-top: 7px;"></i>');
	$('.select2-selection__arrow b').remove();


});

$(document).ready(function () {

	var today = new Date();
	var dd = String(today.getDate()).padStart(2, '0');
	var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
	var yyyy = today.getFullYear();
	today = mm + '/' + dd + '/' + yyyy;

	$("#BirdthDay").datetimepicker({
	    format: "DD/MM/YYYY",
	    locale: 'vi',
	    icons: {
	    	date: "fa fa-calendar"
	    }
	});


	$("#DateExam").datetimepicker({
	    format: "DD/MM/YYYY",
	    locale: 'vi'
	});

	$("#HourExam").datetimepicker({
	    format: "HH:mm",
	    locale: 'vi'
	});

	$("#DateSlotItem").datetimepicker({
		format: "DD/MM/YYYY",
	    locale: 'vi',
	    defaultDate: today
	});

	$('input[type="checkbox"]').click(function(){
	    if($(this).prop("checked") == true){
	        $(".checkingHeilthInsuranceShow").css("display", "inherit");
	    }
	    else if($(this).prop("checked") == false){
	        $(".checkingHeilthInsuranceShow").css("display", "none");
	    }
	});

});