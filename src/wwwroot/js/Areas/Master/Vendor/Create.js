﻿$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#VendorCode").on("focusout", function () {
        if ($("#VendorCode").val() != '') {
            global.removeValidationErrors('VendorCode');
        }
    });

    $("#VendorName").on("focusout", function () {
        if ($("#VendorName").val() != '') {
            global.removeValidationErrors('VendorName');
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
    };

    function SaveCrate(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('vend-add-url'),
            data: addRequestVerificationToken({
                VendorCode: $("#VendorCode").val().toUpperCase(),
                VendorName: $("#VendorName").val(),
                AddressL1: $("#AddressL1").val(),
                AddressL2: $("#AddressL2").val(),
                AddressL3: $("#AddressL3").val(),
                AddressL4: $("#AddressL4").val(),
                Telephone: $("#Telephone").val(),
                Fax: $("#Fax").val(),
                VendorEmail: $("#VendorEmail").val(),
                VendorContact: $("#VendorContact").val(),
                CreditTerm: $("#CreditTerm").val(),
                PriceLevel: $("#PriceLevel").val(),
                VendorTaxId: $("#VendorTaxId").val(),
                Remark: $("#Remark").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#newVendModal').modal('hide');
                    $('#newVendContainer').html("");

                    $("#tblVend").DataTable().ajax.reload(null, false);
                    $("#tblVend").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Vendor', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

