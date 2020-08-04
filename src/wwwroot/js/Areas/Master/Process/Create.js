$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');


    //Begin----check clear require---//
    $("#ProcessCode").on("focusout", function () {
        if ($("#ProcessCode").val() != '') {
            global.removeValidationErrors('ProcessCode');
        }
    });

    $("#ProcessName").on("focusout", function () {
        if ($("#ProcessName").val() != '') {
            global.removeValidationErrors('ProcessName');
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
            url: $('#CreateData').data('proc-add-url'),
            data: addRequestVerificationToken({
                ProcessCode: $("#ProcessCode").val().toUpperCase(),
                ProcessName: $("#ProcessName").val(),
                ProcessDesc: $("#ProcessDesc").val(),
                ProcessSeq: $("#ProcessSeq").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newProcessModal').modal('hide');
                    $('#newProcessContainer').html("");

                    $("#tblProcess").DataTable().ajax.reload(null, false);
                    $("#tblProcess").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Process', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Process', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Process', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

