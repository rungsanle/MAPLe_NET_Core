$(function () {
    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

    //for clear cache image
    var num = Math.random();
    var imgSrc = $('#imageLogo').attr("src") + "?v=" + num;
    $('#imageLogo').attr("src", imgSrc);

});