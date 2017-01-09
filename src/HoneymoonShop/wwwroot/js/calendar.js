var date = new Date();

var currentDate = new Date();

var selectedDay = new Date();

var selectedDay;

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

    $("#calender-currentday").text(MONTHS[date.getMonth()].concat(" " + date.getFullYear()));
    for (var i = 0; i < lastDay.getDate() ; i++) {
        var tableData = $(tableRows[Math.ceil((firstDay.getDate() + i + DAYS[firstDay.getDay()]) / 7) - 1]).children()[DAYS[(firstDay.getDay() + i) % 7]];
        var day = i + 1;
        if (date.getTime() < currentDate.getTime() || (day <= currentDate.getDate() && (date.getMonth() == currentDate.getMonth() && date.getFullYear() == currentDate.getFullYear()))) {
            $(tableData).html("<label class=\"nondate\">" + day + "<\label>");
        } else {
            $(tableData).html("<input class=\"dateinput\" type=\"radio\" id=\"date" + day + "\" name=\"date\" value=\"" + day + "\"> <label for=\"date" + day + "\">" + day + "</label>");
        }
    }
}

$(document).ready(function () {


    fillCalendar();

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
        $("#btn-stap2").show();
        //document.cookie = ;
    });

    $("#btntime").click(function () {
        $("#calendar").hide();
        $("#tijdstip").show();

        if ($('input[name=date]:checked').length > 0) {
            selectedDay = $('input[name=date]:checked').val();
            $("#calendertijdstip-currentday").text(selectedDay + " " + MONTHS[date.getMonth()] + " " + date.getFullYear());
        }
    });

    $("#btndatum").click(function () {
        $("#tijdstip").hide();
        $("#btn-stap2").hide();
        $("#calendar").show();
    });

    $("#btn-stap2").click(function () {
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

        if ($("input[name=tijdstip]:checked").length > 0) {
            date.setDate(selectedDay);
            $("#chosendate").text(DAYSNAME[date.getDay()] + " " + $("#calendertijdstip-currentday").text() + " Om " + $("input[name=tijdstip]:checked").val() + " U");
        }
    });

    $("#btn-stap3").click(function () {

        $("#stap2-label").toggleClass("staplabel-proceeded");
        $("#stap2-text").toggleClass("staptext-proceeded");
        $("#stap2-path").toggleClass("stappath-proceeded");

        $("#stap3-label").toggleClass("staplabel-disabled");
        $("#stap3-text").toggleClass("staptext-disabled");

        if ($("#stap2-path").text().length > 1) {
            $("#stap2-path").html("I<br>");
        }

        $("#chosendate-panel").hide();
        $("#userinfo").hide();
        $("#userinfo-validation-info").show();
        $("#userinfo-validation").show();
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