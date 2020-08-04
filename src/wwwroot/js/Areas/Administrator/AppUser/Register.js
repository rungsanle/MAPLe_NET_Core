$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '30%' // optional
    });

    setTimeout(function () {

        $('#NameIdentifier').val('');
        $('#Email').val('');
        $('#Password').val('');

    }, 300);

    $("#NameIdentifier").on("focusout", function () {
        if ($("#NameIdentifier").val().length >= 5) {
            global.removeValidationErrors('NameIdentifier');
        }
    });

    $("#Email").on("focusout", function () {
        if (isValidEmailAddress($("#Email").val())) {
            global.removeValidationErrors('Email');
        }
    });

    $("#Password").on("focusout", function () {
        if ($("#Password").val().length >= 6) {
            global.removeValidationErrors('Password');
        }
    });

    $("#ConfirmPassword").on("focusout", function () {
        if ($("#Password").val() == $("#ConfirmPassword").val()) {
            global.removeValidationErrors('ConfirmPassword');
        }
    });
   

    $("#btnRegister").on("click", Register);

    function isValidEmailAddress(emailAddress) {
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        return pattern.test(emailAddress);
    };

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function Register(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#RegisterData').data('user-regis-url'),
            data: addRequestVerificationToken({
                NameIdentifier: $("#NameIdentifier").val(),
                Email: $("#Email").val(),
                Password: $("#Password").val(),
                ConfirmPassword: $('#ConfirmPassword').val(),
                IsAgree: $('#IsAgree').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newAppUserModal').modal('hide');
                    $('#newAppUserContainer').html("");

                    $("#tblAppUser").DataTable().ajax.reload(null, false);
                    $("#tblAppUser").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Register Application User', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                } else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Register Application User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Register Application User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };
});
