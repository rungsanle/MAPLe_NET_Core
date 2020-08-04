$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');


    //Begin----check clear require---//
    $("#CustomerCode").on("focusout", function () {
        if ($("#CustomerCode").val() != '') {
            global.removeValidationErrors('CustomerCode');
        }
    });

    $("#CustomerName").on("focusout", function () {
        if ($("#CustomerName").val() != '') {
            global.removeValidationErrors('CustomerName');
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

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('cust-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                CustomerCode: $("#CustomerCode").val().toUpperCase(),
                CustomerName: $("#CustomerName").val(),
                AddressL1: $("#AddressL1").val(),
                AddressL2: $("#AddressL2").val(),
                AddressL3: $("#AddressL3").val(),
                AddressL4: $("#AddressL4").val(),
                Telephone: $("#Telephone").val(),
                Fax: $("#Fax").val(),
                CustomerEmail: $("#CustomerEmail").val(),
                CustomerContact: $("#CustomerContact").val(),
                CreditTerm: $("#CreditTerm").val(),
                PriceLevel: $("#PriceLevel").val(),
                CustomerTaxId: $("#CustomerTaxId").val(),
                Remark: $("#Remark").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#editCustModal').modal('hide');
                    $('#editCustContainer').html("");

                    $("#tblCust").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Customer', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Customer', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Customer', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

