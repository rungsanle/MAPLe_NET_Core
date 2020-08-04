$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#ProdTypeCode").on("focusout", function () {
        if ($("#ProdTypeCode").val() != '') {
            global.removeValidationErrors('ProdTypeCode');
        }
    });

    $("#ProdTypeName").on("focusout", function () {
        if ($("#ProdTypeName").val() != '') {
            global.removeValidationErrors('ProdTypeName');
        }
    });
    //End----check clear require---//
    var compCode = $('#EditData').data('viewbag-compcode');
    //$('#CompanyCode').inputpicker({
    //    url: "@Url.Action("GetCompany", "Company", new { Area = "Master" })",
    //    fields: [
    //        { name: 'CompanyCode', text: 'CODE', width: '30%' },
    //        { name: 'CompanyName', text: 'NAME', width: '70%' }
    //    ],
    //    width: '350px',
    //    autoOpen: true,
    //    selectMode: 'restore',
    //    headShow: true,
    //    fieldText: 'CompanyCode',
    //    fieldValue: 'CompanyCode'
    //});
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
            url: $('#EditData').data('prodtype-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                ProdTypeCode: $("#ProdTypeCode").val().toUpperCase(),
                ProdTypeName: $("#ProdTypeName").val(),
                ProdTypeDesc: $("#ProdTypeDesc").val(),
                ProdTypeSeq: $("#ProdTypeSeq").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#editProdTypeModal').modal('hide');
                    $('#editProdTypeContainer').html("");

                    $("#tblProdType").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Production Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});

