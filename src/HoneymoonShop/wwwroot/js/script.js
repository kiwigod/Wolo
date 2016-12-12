$(document).ready(function(){
    
    var text = ["Winkel", "Onze specialisten", "Naai-atelier", "Servicepunten"];
    var dropdowns = ["#first-feature"];
    
    $(".button-transparent").click(function(){
        var buttonText = $(this).find("div").text();
        $("#hashtagbutton").text(buttonText);

        $("#text1").text(text[parseInt(buttonText) - 1]);
        $("#text2").text(text[parseInt(buttonText) - 1]);
    });
});
