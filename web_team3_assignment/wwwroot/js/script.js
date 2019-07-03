$(document).ready(function () {
    $("#lecturerform").hide();
    $("#studentform").hide();
    //lecturerform show
    $("#lecturerbtn").click(function () {
        $("#buttonmenu").hide(1000).delay(100);
        $("#lecturerform").show(1000);
    });

    $("#choose").click(function () {
        $("#lecturerform").hide(1000).delay(100);
        $("#buttonmenu").show(1000);
    });

    //studentform show
    $("#studentbtn").click(function () {
        $("#buttonmenu").hide(1000).delay(100);
        $("#studentform").show(1000);
    });

    $("#choosemenu").click(function () {
        $("#studentform").hide(1000).delay(100);
        $("#buttonmenu").show(1000);
    });
});
