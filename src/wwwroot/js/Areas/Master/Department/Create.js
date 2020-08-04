$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#DeptCode").on("focusout", function () {
        if ($("#DeptCode").val() != '') {
            global.removeValidationErrors('DeptCode');
        }
    });

    $("#DeptName").on("focusout", function () {
        if ($("#DeptName").val() != '') {
            global.removeValidationErrors('DeptName');
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
            url: $('#CreateData').data('dept-add-url'),
            data: addRequestVerificationToken({
                DeptCode: $("#DeptCode").val().toUpperCase(),
                DeptName: $("#DeptName").val(),
                DeptDesc: $("#DeptDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newDeptModal').modal('hide');
                    $('#newDeptContainer').html("");

                    $("#tblDept").DataTable().ajax.reload(null, false);
                    $("#tblDept").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Department', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                } else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Department', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Department', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});


