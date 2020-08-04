$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#UnitCode").on("focusout", function () {
        if ($("#UnitCode").val() != '') {
            global.removeValidationErrors('UnitCode');
        }
    });

    $("#UnitName").on("focusout", function () {
        if ($("#UnitName").val() != '') {
            global.removeValidationErrors('UnitName');
        }
    });
    //End----check clear require---//

    global.applyIsActiveSwitch(true, false);

    $("#btnSaveCreate").on("click", SaveCrate);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    }

    function SaveCrate(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('unit-add-url'),
            data: addRequestVerificationToken({
                UnitCode: $("#UnitCode").val().toUpperCase(),
                UnitName: $("#UnitName").val(),
                UnitDesc: $("#UnitDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newUnitModal').modal('hide');
                    $('#newUnitContainer').html("");

                    $("#tblUnit").DataTable().ajax.reload(null, false);
                    $("#tblUnit").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Unit', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Unit', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Unit', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

