$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#LocationCode").on("focusout", function () {
        if ($("#LocationCode").val() != '') {
            global.removeValidationErrors('LocationCode');
        }
    });

    $("#LocationName").on("focusout", function () {
        if ($("#LocationName").val() != '') {
            global.removeValidationErrors('LocationName');
        }
    });
    //End----check clear require---//

    $('#WarehouseName').inputpicker({
        url: $('#CreateData').data('wh-get-url'),
        fields: [
            { name: 'WarehouseCode', text: 'CODE', width: '30%' },
            { name: 'WarehouseName', text: 'NAME', width: '70%' }
        ],
        selectMode: 'restore',
        headShow: true,
        fieldText: 'WarehouseName',
        fieldValue: 'Id',
        autoOpen: true,
        width: '350px'
    });

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
            url: $('#CreateData').data('loc-add-url'),
            data: addRequestVerificationToken({
                LocationCode: $("#LocationCode").val().toUpperCase(),
                LocationName: $("#LocationName").val(),
                LocationDesc: $("#LocationDesc").val(),
                WarehouseId: $("#WarehouseName").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newLocationModal').modal('hide');
                    $('#newLocationContainer').html("");

                    $("#tblLocation").DataTable().ajax.reload(null, false);
                    $("#tblLocation").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Location', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Location', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Location', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

