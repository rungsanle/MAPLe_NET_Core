$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#btnSaveCreate").on("click", SaveCrate);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveCrate(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('role-add-url'),
            data: addRequestVerificationToken({
                Name: $("#Name").val()
            }),
            success: function (response) {

                if (response.success) {

                    $('#newRoleModal').modal('hide');
                    $('#newRoleContainer').html("");

                    $("#tblRole").DataTable().ajax.reload(null, false);
                    $("#tblRole").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Role', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Create Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    }

});

