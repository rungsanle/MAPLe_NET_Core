$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#RawMatTypeCode").on("focusout", function () {
        if ($("#RawMatTypeCode").val() != '') {
            global.removeValidationErrors('RawMatTypeCode');
        }
    });

    $("#RawMatTypeName").on("focusout", function () {
        if ($("#RawMatTypeName").val() != '') {
            global.removeValidationErrors('RawMatTypeName');
        }
    });
    //End----check clear require---//

    var compCode = $('#CreateData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        $(function () {
            $(".inputpicker-input:last").attr("disabled", true); // or removeAttr("disabled")
        });
    }

    //global.applyBSwitchStyle($("#Is_Active").prop("id"), true, false, "small", "Yes", "No");
    global.applyIsActiveSwitch(true, false);

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
            url: $('#CreateData').data('rawmattype-add-url'),
            data: addRequestVerificationToken({
                RawMatTypeCode: $("#RawMatTypeCode").val().toUpperCase(),
                RawMatTypeName: $("#RawMatTypeName").val(),
                RawMatTypeDesc: $("#RawMatTypeDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newRawMatTypeModal').modal('hide');
                    $('#newRawMatTypeContainer').html("");

                    $("#tblRawMatType").DataTable().ajax.reload(null, false);
                    $("#tblRawMatType").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Raw MAT. Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

