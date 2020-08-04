$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    bs_input_file();

    var reqColumns = (['Item ID', 'Item Description', 'Description for Sales', 'Sales Price 1', 'Sales Price 2', 'Sales Price 3', 'Sales Price 4', 'Sales Price 5', 'G/L Sales Account', 'G/L Inventory Account', 'G/L COGS/Salary Acct']);
    var tblUpload;

    $("#btnUploadData").prop('disabled', true);

    $("#fileButton").on("click", function (event) {

        event.preventDefault();

        if ($('#RawMatType').val() === '') {
            alert('Please select RAW MAT. TYPE!!');
            $('#RawMatType').focus();
            return false;
        }

        var fileLength = $("#fileInput").get(0).files.length;
        if (fileLength === 0) {
            alert('Please choose a file first!!');
            return false;
        }

        if (tblUpload) {
            tblUpload.clear().destroy();
            $('#tblUpload').empty()
        }


        var api = $(this).data("url");

        var files = $("#fileInput").get(0).files;
        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append("files", files[i]);
        }

        $.ajax({
            async: true,
            type: "POST",
            url: api,
            dataType: "json",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (response) {
                if (response.success) {
                    //alert(response.message);
                    var jsonData = JSON.parse(response.data);

                    var dataColumns = [];
                    var columns = [];


                    for (var obj in jsonData) {
                        if (jsonData.hasOwnProperty(obj)) {

                            for (var prop in jsonData[obj]) {

                                if (jsonData[obj].hasOwnProperty(prop)) {
                                    dataColumns.push(prop);
                                    columns.push({ data: prop.replace(/\./g, '\\.'), title: prop });
                                }
                            }
                        }
                        break; //only one round
                    }

                    var colsExist = global.containsArray(reqColumns, dataColumns);

                    if (colsExist) {

                        tblUpload = $('#tblUpload').DataTable({
                            destroy: true,
                            processing: true,
                            data: jsonData,
                            columns: columns,
                            scrollX: true,
                            order: [],
                            lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                            scroller: true,
                            iDisplayLength: 10
                        });

                        $("#btnUploadData").prop('disabled', false);
                    }
                    else {

                        toastr.error('Format is incorrect!!', 'Upload Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //function containsAll(needles, haystack) {
    //    for (var i = 0, len = needles.length; i < len; i++) {
    //        if ($.inArray(needles[i], haystack) == -1) return false;
    //    }
    //    return true;
    //}

    $(document).ajaxStart(function () {
        $("#overlay").show();
        $("#fileButton").prop('disabled', true);
    });

    $(document).ajaxStop(function () {
        $("#overlay").hide();
        $("#fileButton").prop('disabled', false);
    });

    function bs_input_file() {
        $(".input-file").before(
            function () {
                if (!$(this).prev().hasClass('input-ghost')) {
                    var element = $("<input type='file' id='fileInput' class='input-ghost' style='visibility:hidden; height:0' accept='.csv'>");
                    element.attr("name", $(this).attr("name"));
                    element.change(function () {
                        element.next(element).find('input').val((element.val()).split('\\').pop());
                    });
                    $(this).find("button.btn-choose").click(function () {
                        element.click();
                    });
                    $(this).find("button.btn-reset").click(function () {
                        element.val(null);
                        $(this).parents(".input-file").find('input').val('');
                    });
                    $(this).find('input').css("cursor", "pointer");
                    $(this).find('input').mousedown(function () {
                        $(this).parents('.input-file').prev().click();
                        return false;
                    });
                    return element;
                }
            }
        );
    }

    //$(function () {
    //    bs_input_file();
    //});

    $("#btnUploadData").on("click", UploadData);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function UploadData(event) {
        //console.log(JSON.stringify($('#tblUpload').dataTable().fnGetData()))
        event.preventDefault();

        var compCode = $('#CompanyCode').val();
        var rawMatTypeId = $('#RawMatType').val();
        var materials = new Array();
        var jsonData = JSON.parse(JSON.stringify($('#tblUpload').dataTable().fnGetData()));

        for (var obj in jsonData) {
            if (jsonData.hasOwnProperty(obj)) {

                var material = {};
                material.Id = 0;
                material.MaterialCode = jsonData[obj][reqColumns[0]];
                material.MaterialName = jsonData[obj][reqColumns[1]];
                material.MaterialDesc1 = jsonData[obj][reqColumns[2]];
                material.MaterialDesc2 = '';
                material.RawMatTypeId = rawMatTypeId;
                material.UnitId = '4';
                material.PackageStdQty = '1';
                material.WarehouseId = null;
                material.LocationId = null;
                material.CompanyCode = compCode;
                material.Is_Active = true;

                materials.push(material);
            }
        }

        //var postData = { lstCust: materials };

        $.ajax({
            async: true,
            type: "POST",
            url: $('#UploadData').data('mat-upload-url'),
            data: addRequestVerificationToken({ lstMat: materials }),   //JSON.stringify(materials),
            success: function (response) {

                if (response.success) {

                    $('#uploadMatModal').modal('hide');
                    $('#uploadMatContainer').html("");

                    $("#tblMaterial").DataTable().ajax.reload(null, false);
                    $("#tblMaterial").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Upload Material', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Upload Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }

        });
    }

    /*-------------- BEGIN COMPANY CODE --------------*/
    var compCode = $('#UploadData').data('viewbag-compcode');
    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:first").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/

    $('#RawMatType').inputpicker({
        url: $('#UploadData').data('rawmattype-get-url'),
        fields: [
            { name: 'RawMatTypeCode', text: 'CODE', width: '30%' },
            { name: 'RawMatTypeName', text: 'NAME', width: '70%' }
        ],
        width: '250px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'RawMatTypeName',
        fieldValue: 'Id',
        responsive: true
    });

});