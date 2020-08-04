$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    var reqColumns = (['MAT_TYPE_CODE', 'MAT_TYPE_NAME', 'MAT_TYPE_DESC']);
    var tblUpload;

    $("#btnUploadData").prop('disabled', true);

    $("#fileButton").on("click", function (event) {

        event.preventDefault();

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
                    } else {
                        toastr.error('Format is incorrect!!', 'Upload Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }

                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    global.applyIcheckStyle();

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

    $(function () {
        bs_input_file();
    });

    $("#btnUploadData").on("click", UploadData);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function UploadData(event) {

        //console.log(JSON.stringify($('#tblUpload').dataTable().fnGetData()))

        event.preventDefault();

        var compCode = $('#CompanyCode').val();
        var materialTypes = new Array();
        var jsonData = JSON.parse(JSON.stringify($('#tblUpload').dataTable().fnGetData()));

        for (var obj in jsonData) {
            if (jsonData.hasOwnProperty(obj)) {
                //console.log(jsonData[obj]['Customer ID']);
                var matType = {};
                matType.Id = 0;
                matType.MatTypeCode = jsonData[obj]['MAT_TYPE_CODE'];
                matType.MatTypeName = jsonData[obj]['MAT_TYPE_NAME'];
                matType.MatTypeDesc = jsonData[obj]['MAT_TYPE_DESC'];
                matType.CompanyCode = compCode;
                matType.Is_Active = true;

                materialTypes.push(matType);
            }
        }

        //var postData = { lstCust: customers };
        var api = $('#UploadData').data('mattype-upload-url');

        $.ajax({
            async: true,
            type: "POST",
            url: api,
            processing: true, // for show progress bar
            data: addRequestVerificationToken({ lstMatType: materialTypes }),   //)  JSON.stringify(customers),
            success: function (response) {

                if (response.success) {

                    $('#uploadMatTypeModal').modal('hide');
                    $('#uploadMatTypeContainer').html("");

                    $("#tblMatType").DataTable().ajax.reload(null, false);
                    $("#tblMatType").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Upload Material Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Upload Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Material Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }

        });
    }

    /*-------------- BEGIN COMPANY CODE --------------*/
    var compCode = $('#UploadData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/

});