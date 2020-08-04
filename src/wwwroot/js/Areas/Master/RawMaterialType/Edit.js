$(function () {

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

    var compCode = $('#EditData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }

    //global.applyBSwitchStyle($("#Is_Active").prop("id"), $('#Is_Active').is(':checked'), false, "small", "Yes", "No");
    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), false);

    $("#btnSaveEdit").on("click", SaveEdit);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveEdit(event) {

        event.preventDefault();

        global.resetValidationErrors();

        //var info = $('#tblMenu').DataTable().page.info();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('rawmattype-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                RawMatTypeCode: $("#RawMatTypeCode").val().toUpperCase(),
                RawMatTypeName: $("#RawMatTypeName").val(),
                RawMatTypeDesc: $("#RawMatTypeDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#editRawMatTypeModal').modal('hide');
                    $('#editRawMatTypeContainer').html("");

                    $("#tblRawMatType").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Raw MAT. Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Edit Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

