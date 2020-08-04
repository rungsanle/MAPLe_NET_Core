$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#WarehouseCode").on("focusout", function () {
        if ($("#WarehouseCode").val() != '') {
            global.removeValidationErrors('WarehouseCode');
        }
    });

    $("#WarehouseName").on("focusout", function () {
        if ($("#WarehouseName").val() != '') {
            global.removeValidationErrors('WarehouseName');
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
            url: $('#CreateData').data('wh-add-url'),
            data: addRequestVerificationToken({
                WarehouseCode: $("#WarehouseCode").val().toUpperCase(),
                WarehouseName: $("#WarehouseName").val(),
                WarehouseDesc: $("#WarehouseDesc").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newWHModal').modal('hide');
                    $('#newWHContainer').html("");

                    $("#tblWH").DataTable().ajax.reload(null, false);
                    $("#tblWH").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Warehouse', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Warehouse', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Warehouse', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});



