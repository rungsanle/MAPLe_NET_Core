$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //Begin----check clear require---//
    $("#MaterialCode").on("focusout", function () {
        if ($("#MaterialCode").val() != '') {
            global.removeValidationErrors('MaterialCode');
        }
    });

    $("#MaterialName").on("focusout", function () {
        if ($("#MaterialName").val() != '') {
            global.removeValidationErrors('MaterialName');
        }
    });
    //End----check clear require---//

    /*-------------- BEGIN RAW MATERIAL TYPE --------------*/
    $('#RawMatType').inputpicker({
        url: $('#CreateData').data('rawmattype-get-url'),
        fields: [
            { name: 'RawMatTypeCode', text: 'CODE', width: '30%' },
            { name: 'RawMatTypeName', text: 'NAME', width: '70%' }
        ],
        width: '250px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'RawMatTypeName',
        fieldValue: 'Id'
        //responsive: true
    });

    $('#RawMatType').val($("#RawMatTypeId").val());
    /*-------------- END PRODUCTION TYPE --------------*/
    
    /*-------------- BEGIN UNIT --------------*/
    $('#Unit').inputpicker({
        url: $('#CreateData').data('unit-get-url'),
        fields: [
            { name: 'UnitName', text: 'NAME', width: '100%' }
        ],
        selectMode: 'restore',
        headShow: false,
        fieldText: 'UnitName',
        fieldValue: 'Id',
        autoOpen: true,
        width: '130px',
    });

    $('#Unit').val($("#UnitId").val());
    /*-------------- END UNIT --------------*/

    /*-------------- BEGIN WAREHOUSE --------------*/
    $('#Warehouse').inputpicker({
        url: $('#CreateData').data('wh-get-url'),
        fields: [
            { name: 'WarehouseCode', text: 'CODE', width: '30%' },
            { name: 'WarehouseName', text: 'NAME', width: '70%' }
        ],
        width: '300px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'WarehouseName',
        fieldValue: 'Id'
        //responsive: true
    });

    $('#Warehouse').val($("#WarehouseId").val());
    /*-------------- END WAREHOUSE --------------*/

    /*-------------- BEGIN LOCATION --------------*/
    $('#Location').inputpicker({
        url: $('#CreateData').data('loc-get-url'),
        fields: [
            { name: 'LocationCode', text: 'CODE', width: '30%' },
            { name: 'LocationName', text: 'NAME', width: '70%' }
        ],
        width: '300px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'LocationName',
        fieldValue: 'Id'
        //responsive: true
    });

    $('#Location').val($("#LocationId").val());
    /*-------------- END LOCATION --------------*/

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
    global.applyDraggable();

    global.applyIsActiveSwitch(true, false);

    $("#btnSaveCreate").on("click", SaveCreate);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveCreate(event) {


        event.preventDefault();

        global.resetValidationErrors();

        var strMaterialCode = $("#MaterialCode").val().toUpperCase();
        var strCompanyCode = $("#CompanyCode").val();
        var materialFileName;

        var fileLength = $("#imgMaterial").get(0).files.length;
        if (fileLength > 0) {

            var selFilename = $("#imgMaterial").get(0).files[0].name;
            var extension = selFilename.substring(selFilename.lastIndexOf('.') + 1);

            materialFileName = strCompanyCode + '_' + strMaterialCode + '.' + extension;
        }

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('mat-add-url'),
            data: addRequestVerificationToken({
                MaterialCode: $("#MaterialCode").val().toUpperCase(),
                MaterialName: $("#MaterialName").val(),
                MaterialDesc1: $("#MaterialDesc1").val(),
                MaterialDesc2: $("#MaterialDesc2").val(),
                RawMatTypeId: $("#RawMatType").val(),
                UnitId: $("#Unit").val(),
                PackageStdQty: $("#PackageStdQty").val(),
                WarehouseId: $("#Warehouse").val(),
                LocationId: $("#Location").val(),
                CompanyCode: $("#CompanyCode").val(),
                MaterialImagePath: materialFileName,
                Is_Active: $("#Is_Active").is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    //Update Material Image.
                    if (fileLength > 0) {
                        UploadMaterialImage(materialFileName);
                    }

                    $('#newMatModal').modal('hide');
                    $('#newMatContainer').html("");

                    $("#tblMaterial").DataTable().ajax.reload(null, false);
                    $("#tblMaterial").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Material', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        function UploadMaterialImage(strName) {

            var files = $("#imgMaterial").get(0).files;
            var fileData = new FormData();
            fileData.append("fileName", strName);
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }

            $.ajax({
                async: true,
                type: "POST",
                url: $('#CreateData').data('mat-upload-url'),
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
                    
                    toastr.error('Error: ' + reponseErr.message, 'Upload Material Image', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
            });
        }
    };

});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imageMaterial')
                .attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
};

