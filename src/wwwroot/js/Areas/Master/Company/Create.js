$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#CompanyCode").on("focusout", function () {
        if ($("#CompanyCode").val() != '') {
            global.removeValidationErrors('CompanyCode');
        }
    });

    $("#CompanyName").on("focusout", function () {
        if ($("#CompanyName").val() != '') {
            global.removeValidationErrors('CompanyName');
        }
    });
    //End----check clear require---//

    global.applyIsActiveSwitch(true, false);

    $("#btnSaveCreate").on("click", SaveCrate);

    //function onFocusOut(ctl) {

    //    if (ctl.val() != '') {
    //        document.querySelectorAll('.text-danger li')[0].remove();
    //    }

    //}

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveCrate(event) {

        event.preventDefault();

        global.resetValidationErrors();

        var strCompCode = $("#CompanyCode").val().toUpperCase();
        var logoFileName;

        var fileLength = $("#imgCompanyLogo").get(0).files.length;
        if (fileLength > 0) {

            var selFilename = $("#imgCompanyLogo").get(0).files[0].name;
            var extension = selFilename.substring(selFilename.lastIndexOf('.') + 1);

            logoFileName = strCompCode + '_LOGO.' + extension;
        }

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('comp-add-url'),
            data: addRequestVerificationToken({
                CompanyCode: strCompCode,
                CompanyName: $("#CompanyName").val(),
                CompanyLogoPath: logoFileName,
                AddressL1: $("#AddressL1").val(),
                AddressL2: $("#AddressL2").val(),
                AddressL3: $("#AddressL3").val(),
                AddressL4: $("#AddressL4").val(),
                Telephone: $("#Telephone").val(),
                Fax: $("#Fax").val(),
                CompanyTaxId: $("#CompanyTaxId").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    if (fileLength > 0) {
                        UploadCompanyLogo(logoFileName);
                    }

                    $('#newCompModal').modal('hide');
                    $('#newCompContainer').html("");

                    $("#tblComp").DataTable().ajax.reload(null, false);
                    $("#tblComp").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Company', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Company', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Company', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    };

    function UploadCompanyLogo(strName) {

        var files = $("#imgCompanyLogo").get(0).files;
        var fileData = new FormData();
        fileData.append("fileName", strName);
        for (var i = 0; i < files.length; i++) {
            fileData.append("files", files[i]);
        }

        $.ajax({
            async: false,
            type: "POST",
            url: $('#CreateData').data('comp-upload-url'),
            dataType: "json",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (response) {
                if (response.success) {
                    //return result
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Company Logo', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    };

});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imageLogo')
                .attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

