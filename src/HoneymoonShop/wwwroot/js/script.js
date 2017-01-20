$(document).ready(function(){
    
    var text = ["Winkel", "Onze specialisten", "Naai-atelier", "Servicepunten"];
    var dropdowns = ["#first-feature"];
    
    $(".button-transparent").click(function(){
        var buttonText = $(this).find("div").text();
        $("#hashtagbutton").text(buttonText);
        $("#hashtagbutton2").text(buttonText);
        $("#text1").text(text[parseInt(buttonText) - 1]);
        $("#text2").text(text[parseInt(buttonText) - 1]);
        $("#text3").text(text[parseInt(buttonText) - 1]);
    });

    for (var i = 0; i < $("#thumbnail").children().length; i++) {
        $($("#thumbnail").children()[i]).click(function () {
            //$("#main-image").fadeOut(250);
            $("#main-image").find("img").attr("src", $(this).attr("src"));
            //$("#main-image").fadeIn(250);
        });
    }
});
