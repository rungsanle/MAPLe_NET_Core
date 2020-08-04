$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#MatTypeCode").on("focusout", function () {
        if ($("#MatTypeCode").val() != '') {
            global.removeValidationErrors('MatTypeCode');
        }
    });

    $("#MatTypeName").on("focusout", function () {
        if ($("#MatTypeName").val() != '') {
            global.removeValidationErrors('MatTypeName');
        }
    });
    //End----check clear require---//

    /*-------------- BEGIN COMPANY CODE --------------*/
    var compCode = $('#EditData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/

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
            url: $('#EditData').data('mattype-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                MatTypeCode: $("#MatTypeCode").val().toUpperCase(),
                MatTypeName: $("#MatTypeName").val(),
                MatTypeDesc: $("#MatTypeDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#editMatTypeModal').modal('hide');
                    $('#editMatTypeContainer').html("");

                    $("#tblMatType").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Material Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };
});

