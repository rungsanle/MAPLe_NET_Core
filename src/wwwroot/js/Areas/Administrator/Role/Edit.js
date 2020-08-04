$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#btnSaveEdit").on("click", SaveEdit);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveEdit(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('role-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                Name: $("#Name").val()
            }),
            success: function (response) {

                if (response.success) {


                    $('#editRoleModal').modal('hide');
                    $('#editRoleContainer').html("");

                    $("#tblRole").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Role', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Edit Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    }

});


