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

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), false);

    $("#btnSaveEdit").on("click", SaveEdit);



    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveEdit(event) {

        event.preventDefault();

        global.resetValidationErrors();

        var strCompCode = $("#CompanyCode").val().toUpperCase();
        var logoFileName = $("#CompanyLogoPath").val();

        var fileLength = $("#imgCompanyLogo").get(0).files.length;
        if (fileLength > 0) {

            var selFilename = $("#imgCompanyLogo").get(0).files[0].name;
            var extension = selFilename.substring(selFilename.lastIndexOf('.') + 1);

            logoFileName = strCompCode + '_LOGO.' + extension;
        }

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('comp-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
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

                    $('#editCompModal').modal('hide');
                    $('#editCompContainer').html("");

                    $("#tblComp").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Company', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Company', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Company', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    }

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
            url: $('#EditData').data('comp-upload-url'),
            dataType: "json",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (response) {
                if (response.success) {
                    //file name = response.data
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Company', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    }

    function displayValidationErrors(errors) {
        $.each(errors, function (idx, errorMessage) {
            var res = errorMessage.split("|");
            $("[data-valmsg-for='" + res[0] + "']").append('<li>' + res[1] + '</li>');
        });
    }

    function resetValidationErrors() {

        var listItems = document.querySelectorAll('.text-danger li');
        for (let i = 0; i < listItems.length; i++) {
            if (listItems[i].textContent != null)
                listItems[i].remove();
        };

    }

    //for clear cache image
    var num = Math.random();
    var imgSrc = $('#imageLogo').attr("src") + "?v=" + num;
    $('#imageLogo').attr("src", imgSrc);

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

