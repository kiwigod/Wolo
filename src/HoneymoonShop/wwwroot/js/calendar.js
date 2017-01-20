var date = new Date();

var currentDate = new Date();

var selectedDay = new Date();

var selectedDay;

var namefieldValue;
var namefieldSplit;
var mailfieldValue;
var remailfieldValue;
var datefieldValue;
var phonenumberfieldValue;

var nonAvailableDays;

function requestDate() {
    $.ajax({
        method: "GET",
        async: false,
        url: "DatesUnavailableInMonth",
        data: { month: (date.getMonth() + 1), year: date.getFullYear() }
    })
      .done(function (msg) {
          nonAvailableDays = msg.split(",");
      });
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function requestTime() {
    var day = selectedDay.toLocaleString('en-US', { minimumIntegerDigits: 2 });
    var month = (date.getMonth() + 1).toLocaleString('en-US', { minimumIntegerDigits: 2 });
    var year = date.getFullYear();
    var request = year + "-" + month + "-" + day;
    $.ajax({
        method: "GET",
        url: "timeAvailableAtDate",
        data: { date: request }
    })
      .done(function (msg) {
          addTimes(msg);
      });
}

function postUserinfo() {
    var day = selectedDay.toLocaleString('en-US', { minimumIntegerDigits: 2 });
    var month = (date.getMonth() + 1).toLocaleString('en-US', { minimumIntegerDigits: 2 });
    var year = date.getFullYear();
    var request = year + "-" + month + "-" + day;

    $.ajax({
        method: "POST",
        url: "validate",
        data: { mdate: datefieldValue, time: $("input[name=tijdstip]:checked").val(), phone: phonenumberfieldValue, mail: mailfieldValue, name: namefieldValue, date: request, newsletter: $("#nieuwsbrief").is(":checked") }
    })
      .done(function (msg) {
          location.href = 'complete';
      });
}

function validatePhone(phone) {
    var vast_nummer = /^(((0)[1-9]{2}[0-9][-]?[1-9][0-9]{5})|((\\+31|0|0031)[1-9][0-9][-]?[1-9][0-9]{6}))$/;
    var mobiel_nummer = /^(((\\+31|0|0031)6){1}[1-9]{1}[0-9]{7})$/i;
    return (vast_nummer.test(phone) || mobiel_nummer.test(phone));
}

var resetFields = function () {
    $("#namefield").css("box-shadow", "");
    $("#mailfield").css("box-shadow", "");
    $("#remailfield").css("box-shadow", "");
    $("#datefield").css("box-shadow", "");
}

var fieldsFilled = function () {
    namefieldValue = document.getElementById("namefield").value;
    namefieldSplit = namefieldValue.split(" ");
    mailfieldValue = document.getElementById("mailfield").value;
    remailfieldValue = document.getElementById("remailfield").value;
    datefieldValue = document.getElementById("datefield").value;
    phonenumberfieldValue = document.getElementById("phonenumberfield").value;

    resetFields();

    if (!(namefieldSplit.length > 1 && namefieldSplit[1].length > 0)) {
        $("#namefield").css("box-shadow", "rgb(255, 0, 0) 0px 0px 15px");
    }
    if (!(datefieldValue.length > 0)) {
        $("#datefield").css("box-shadow", "rgb(255, 0, 0) 0px 0px 15px");
    }
    if (!validateEmail(mailfieldValue)) {
        $("#mailfield").css("box-shadow", "rgb(255, 0, 0) 0px 0px 15px");
    }
    if (mailfieldValue != remailfieldValue) {
        $("#remailfield").css("box-shadow", "rgb(255, 0, 0) 0px 0px 15px");
    }


    return namefieldSplit.length > 1 && namefieldSplit[1].length > 0 && datefieldValue.length > 0 && validateEmail(mailfieldValue) && mailfieldValue == remailfieldValue;
}

function addTimes(msg) {

    $("#tijdstip-form").empty();

    var times = msg.split(",");

    for (var i = 0; i < times.length; i++) {
        if (times[i].length > 0) {
            $("#tijdstip-form").append('<input class="radio-white" id="appointment' + i + '" type="radio" name="tijdstip" value="' + times[i] + '" hidden/><label class="radio-white-label" for="appointment' + i + '"></label><label for="appointment' + i + '">' + times[i] + ' uur</label><br/>');
        }
    }

}

var tableRows = ["#tr1", "#tr2", "#tr3", "#tr4", "#tr5", "#tr6"];

var MONTHS = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
];

var DAYS = [
    6,
    0,
    1,
    2,
    3,
    4,
    5
];

var DAYSNAME = [
    "Zo",
    "Ma",
    "Di",
    "Woe",
    "Do",
    "Vr",
    "Za"
];

var removeEntries = function () {
    for(var i of tableRows) {
        for(var x of $(i).children()) {
            $(x).html("");
        }
    }
}

var fillCalendar = function () {
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0)

    removeEntries();

    requestDate();



    $("#calender-currentday").text(MONTHS[date.getMonth()].concat(" " + date.getFullYear()));
    for (var i = 0; i < lastDay.getDate() ; i++) {
        var tableData = $(tableRows[Math.ceil((firstDay.getDate() + i + DAYS[firstDay.getDay()]) / 7) - 1]).children()[DAYS[(firstDay.getDay() + i) % 7]];
        var day = i + 1;

        if (date.getTime() < currentDate.getTime() || (day <= currentDate.getDate() && (date.getMonth() == currentDate.getMonth() && date.getFullYear() == currentDate.getFullYear()))) {
            $(tableData).html("<label class=\"nondate\">" + day + "<\label>");
        }
        else if (nonAvailableDays.indexOf(day.toString()) > -1) {
            $(tableData).html("<label class=\"occupied\">" + day + "<\label>");
        }
        else {
            $(tableData).html("<input class=\"dateinput\" type=\"radio\" id=\"date" + day + "\" name=\"date\" value=\"" + day + "\"> <label for=\"date" + day + "\">" + day + "</label>");
        }
    }
}

$(document).ready(function () {

    fillCalendar();
    $("#btn-bevestig").click(function () {
        postUserinfo();
    });

    $("#next").click(function () {
        var currentMonth = date.getMonth();
        if (currentMonth > 11) {
            date.setMonth(0);
            date.setFullYear(date.getFullYear() + 1);
            fillCalendar();
        }
        else {
            date.setMonth(currentMonth + 1);
            fillCalendar();
        }
    });

    $("#previous").click(function () {
        var currentMonth = date.getMonth();
        if (currentMonth < 1) {
            date.setMonth(11);
            date.setFullYear(date.getFullYear() - 1);
            fillCalendar();
        }
        else {
            date.setMonth(currentMonth - 1);
            fillCalendar();
        }
    });


    $("#btntime").click(function () {
        if ($('input[name=date]:checked').length > 0) {
            $("#calendar").hide();
            $("#tijdstip").show();
            $("#btn-stap2").show();
            selectedDay = $('input[name=date]:checked').val();
            $("#calendertijdstip-currentday").text(selectedDay + " " + MONTHS[date.getMonth()] + " " + date.getFullYear());
            requestTime();
        } else {
            alert("Date is required!");
        }
    });

    $("#btndatum").click(function () {
        $("#tijdstip").hide();
        $("#btn-stap2").hide();
        $("#calendar").show();
    });

    $("#btn-stap2").click(function () {
        if ($("input[name=tijdstip]:checked").length > 0) {
            $("#tijdstip").hide();
            $("#btn-stap2").hide();
            $("#stap-datum-selectie").hide();
            $("#chosendate-panel").show();
            $("#userinfo").show();

            if ($("#stap1-path").text().length > 1) {
                $("#stap1-path").html("I<br>");
            }

            if ($("#stap2-path").text().length == 1) {
                $("#stap2-path").html("I<br>I<br>I<br>I<br>I<br>I<br>");
            }

            $("#stap1-label").toggleClass("staplabel-proceeded");
            $("#stap1-path").toggleClass("stappath-proceeded");
            $("#stap1-text").toggleClass("staptext-proceeded");

            $("#stap2-label").toggleClass("staplabel-disabled");
            $("#stap2-text").toggleClass("staptext-disabled");

            date.setDate(selectedDay);
            $("#chosendate").text(DAYSNAME[date.getDay()] + " " + $("#calendertijdstip-currentday").text() + " Om " + $("input[name=tijdstip]:checked").val() + " U");

        } else {
            alert("Time is required!");
        }
    });

    $("#btn-stap3").click(function () {
        if (fieldsFilled()) {
            $("#stap2-label").toggleClass("staplabel-proceeded");
            $("#stap2-text").toggleClass("staptext-proceeded");
            $("#stap2-path").toggleClass("stappath-proceeded");

            $("#stap3-label").toggleClass("staplabel-disabled");
            $("#stap3-text").toggleClass("staptext-disabled");

            if ($("#stap2-path").text().length > 1) {
                $("#stap2-path").html("I<br>");
            }
            $("#userinfo-validation-datetime").text($("#chosendate").text());
            $("#tdNaam").text(namefieldValue);
            $("#tdDatum").text(datefieldValue);
            $("#tdTelefoon").text(phonenumberfieldValue);
            $("#tdMail").text(mailfieldValue);

            $("#chosendate-panel").hide();
            $("#userinfo").hide();
            $("#userinfo-validation-info").show();
            $("#userinfo-validation").show();
        } else {
            //alert("Fields is not filled!");
        }
    });

    $("#btn-stap1").click(function () {
        $("#stap1-label").toggleClass("staplabel-proceeded");
        $("#stap1-path").toggleClass("stappath-proceeded");
        $("#stap1-text").toggleClass("staptext-proceeded");

        $("#stap2-label").toggleClass("staplabel-disabled");
        $("#stap2-text").toggleClass("staptext-disabled");

        if ($("#stap1-path").text().length == 1) {
            $("#stap1-path").html("I<br>I<br>I<br>I<br>I<br>I<br>");
        }

        if ($("#stap2-path").text().length > 1) {
            $("#stap2-path").html("I<br>");
        }

        $("#chosendate-panel").hide();
        $("#userinfo").hide();
        $("#stap-datum-selectie").show();
        $("#calendar").show();
    });

    $("#btn-stap2-back").click(function () {
        $("#stap2-label").toggleClass("staplabel-proceeded");
        $("#stap2-text").toggleClass("staptext-proceeded");
        $("#stap2-path").toggleClass("stappath-proceeded");

        $("#stap3-label").toggleClass("staplabel-disabled");
        $("#stap3-text").toggleClass("staptext-disabled");

        if ($("#stap2-path").text().length == 1) {
            $("#stap2-path").html("I<br>I<br>I<br>I<br>I<br>I<br>");
        }

        $("#tijdstip").hide();
        $("#btn-stap2").hide();
        $("#stap-datum-selectie").hide();
        $("#chosendate-panel").show();
        $("#userinfo").show();
        $("#userinfo-validation-info").hide();
        $("#userinfo-validation").hide();
    });

    //    $("#calendar-form").change(function(){
    //        if ($('input[name=date]:checked').length > 0) {
    //            console.log($('input[name=date]:checked').val() + " " + MONTHS[date.getMonth()] + " " + date.getFullYear());
    //        }
    //    });


});