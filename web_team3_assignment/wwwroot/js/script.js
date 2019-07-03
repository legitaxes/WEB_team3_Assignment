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

    $('#passwordForm [type="password"]').keyup(function () {
        var password = $('#NewPassword');
        var confirm = $('#ConfirmPassword');
        var message = $('#warning');
        var good_color = "#66cc66";
        var bad_color = "#ff6666";

        if (password.val() === confirm.val()) {
            confirm.css('background-color', good_color);
            message.css('color', good_color).html("Passwords Match!");
        }
        else {
            confirm.css('background-color', bad_color);
            message.css('color', bad_color).html("Passwords Do Not Match!");
        }
    });


    //$('#NewPassword, #ConfirmPassword').on('keyup', function () {
    //    if ($('#NewPassword').val() == $('#ConfirmPassword').val()) {
    //        $('#warning').html('Matching').css('color', 'green');
    //    } else
    //        $('#warning').html('Not Matching').css('color', 'red');
    //});

});

$(window).on(function() {

});
