$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //toastr.options.newestOnTop = false;
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
    var compCode = $('#EditData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/
    
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
            url: $('#EditData').data('dept-edit-url'),
            data: addRequestVerificationToken({
                DeptCode: $("#DeptCode").val().toUpperCase(),
                DeptName: $("#DeptName").val(),
                DeptDesc: $("#DeptDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response, textStatus, jqXHR) {

                if (response.success) {

                    $('#editDeptModal').modal('hide');
                    $('#editDeptContainer').html("");

                    $("#tblDept").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Department', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Department', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Department', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

//$(document).on("click", "#btnSaveEdit", function () {

//    resetValidationErrors();

//    alert('Hello, World');
//});


