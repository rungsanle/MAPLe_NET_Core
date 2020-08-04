$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    var reqColumns = (['Vendor ID', 'Vendor Name', 'Address-Line One', 'Address-Line Two', 'City', 'Country', 'Telephone 1', 'Fax Number', 'Vendor E-mail', 'Contact', 'Due Days']);
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
                        toastr.error('Format is incorrect!!', 'Upload Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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
        var vendors = new Array();
        var jsonData = JSON.parse(JSON.stringify($('#tblUpload').dataTable().fnGetData()));

        for (var obj in jsonData) {
            if (jsonData.hasOwnProperty(obj)) {
                //console.log(jsonData[obj]['Customer ID']);
                var vendor = {};
                vendor.Id = 0;
                vendor.VendorCode = jsonData[obj][reqColumns[0]];
                vendor.VendorName = jsonData[obj][reqColumns[1]];
                vendor.AddressL1 = jsonData[obj][reqColumns[2]];
                vendor.AddressL2 = jsonData[obj][reqColumns[3]];
                vendor.AddressL3 = jsonData[obj][reqColumns[4]];
                vendor.AddressL4 = jsonData[obj][reqColumns[5]];
                vendor.Telephone = jsonData[obj][reqColumns[6]];
                vendor.Fax = jsonData[obj][reqColumns[7]];
                vendor.VendorEmail = jsonData[obj][reqColumns[8]];
                vendor.VendorContact = jsonData[obj][reqColumns[9]];
                vendor.CreditTerm = jsonData[obj][reqColumns[10]];
                vendor.PriceLevel = 0;
                vendor.VendorTaxId = 'VAT0';
                vendor.Remark = '';
                vendor.CompanyCode = compCode;
                vendor.Is_Active = true;

                vendors.push(vendor);
            }
        }

        //var postData = { lstCust: customers };

        $.ajax({
            async: true,
            type: "POST",
            url: $('#UploadData').data('vend-upload-url'),
            processing: true, // for show progress bar
            data: addRequestVerificationToken({ lstVend: vendors }),   //JSON.stringify(customers),
            success: function (response) {

                if (response.success) {

                    $('#uploadVendModal').modal('hide');
                    $('#uploadVendContainer').html("");

                    $("#tblVend").DataTable().ajax.reload(null, false);
                    $("#tblVend").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Upload Vendor', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Upload Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Upload Vendor', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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