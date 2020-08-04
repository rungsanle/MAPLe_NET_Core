
$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#ArrivalTypeCode").on("focusout", function () {
        if ($("#ArrivalTypeCode").val() != '') {
            global.removeValidationErrors('ArrivalTypeCode');
        }
    });

    $("#ArrivalTypeName").on("focusout", function () {
        if ($("#ArrivalTypeName").val() != '') {
            global.removeValidationErrors('ArrivalTypeName');
        }
    });
    //End----check clear require---//

    /*-------------- BEGIN COMPANY CODE --------------*/
    var compCode = $('#CreateData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/

    global.applyIsActiveSwitch(true, false);

    $("#btnSaveCreate").on("click", SaveCrate);

    function onFocusOut(ctl) {

        if (ctl.val() != '') {
            document.querySelectorAll('.text-danger li')[0].remove();
        }

    }

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
            url: $('#CreateData').data('arrtype-add-url'),
            data: addRequestVerificationToken({
                ArrivalTypeCode: $("#ArrivalTypeCode").val().toUpperCase(),
                ArrivalTypeName: $("#ArrivalTypeName").val(),
                ArrivalTypeDesc: $("#ArrivalTypeDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newArrivalTypeModal').modal('hide');
                    $('#newArrivalTypeContainer').html("");

                    $("#tblArrivalType").DataTable().ajax.reload(null, false);
                    $("#tblArrivalType").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Arrival Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Create Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };


});


