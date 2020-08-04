$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');


    //Begin----check clear require---//
    $("#ProductCode").on("focusout", function () {
        if ($("#ProductCode").val() != '') {
            global.removeValidationErrors('ProductCode');
        }
    });

    $("#ProductName").on("focusout", function () {
        if ($("#ProductName").val() != '') {
            global.removeValidationErrors('ProductName');
        }
    });

    $("#GLSalesAccount").on("focusout", function () {
        if ($("#GLSalesAccount").val() != '') {
            global.removeValidationErrors('GLSalesAccount');
        }
    });

    $("#GLCogsAccount").on("focusout", function () {
        if ($("#GLCogsAccount").val() != '') {
            global.removeValidationErrors('GLCogsAccount');
        }
    });

    //End----check clear require---//

    /*-------------- BEGIN PRODUCTION TYPE --------------*/
    $('#ProductionType').inputpicker({
        url: $('#EditData').data('prodtype-get-url'),
        fields: [
            { name: 'ProdTypeCode', text: 'CODE', width: '30%' },
            { name: 'ProdTypeName', text: 'NAME', width: '70%' }
        ],
        width: '350px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'ProdTypeName',
        fieldValue: 'Id'
        //responsive: true
    });

    $('#ProductionType').val($("#ProductionTypeId").val());
    /*-------------- END PRODUCTION TYPE --------------*/

    /*-------------- BEGIN UNIT --------------*/
    $('#Unit').inputpicker({
        url: $('#EditData').data('unit-get-url'),
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
        url: $('#EditData').data('wh-get-url'),
        fields: [
            { name: 'WarehouseCode', text: 'CODE', width: '30%' },
            { name: 'WarehouseName', text: 'NAME', width: '70%' }
        ],
        width: '350px',
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
        url: $('#EditData').data('loc-get-url'),
        fields: [
            { name: 'LocationCode', text: 'CODE', width: '30%' },
            { name: 'LocationName', text: 'NAME', width: '70%' }
        ],
        width: '350px',
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
    var compCode = $('#EditData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 100);
    }
    /*-------------- END COMPANY CODE --------------*/
    global.applyDraggable();

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), false);

    /*-- BEGIN SEARCH MATERIAL TYPE --*/

    $("#btnSearchMatType").on("click", function (event) {

        event.preventDefault();

        //$('#searchIdContainer').html('');
        $('#searchMatTypeIdModal').modal('show');

        //Initial Datatable
        matTypeVM.init();

    });

    $("#btnRemoveMatType").on("click", function (event) {

        event.preventDefault();

        $('#MaterialTypeId').val(null);
        $('#MaterialType').val('');

    });

    $("#searchMatTypeIdModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        matTypeVM.tbdestroy();

        return false;
    });

    matTypeVM = {
        dtMatType: null,
        init: function () {

            dtMatType = $('#tblMatType').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": $('#EditData').data('mattype-get-url'),
                    "type": "GET",
                    "datatype": "json"
                },
                columns: [
                    { "data": "MatTypeCode", "autoWidth": false },
                    { "data": "MatTypeName", "autoWidth": false },
                    { "data": "MatTypeDesc", "autoWidth": false },
                    {
                        "render": function (data, type, matType, meta) {
                            return '<a id="selectMatType" class="btn btn-default btn-sm" data-toggle="tooltip" title="Select" onclick="selectMatType(\'' + matType.Id + '\',\'' + matType.MatTypeName + '\');" href="javascript:void(0);"><span class="glyphicon glyphicon-hand-up" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "20%", "targets": 0 },
                    { "width": "45%", "targets": 1 },
                    { "width": "30%", "targets": 2 },
                    { "width": "5%", "targets": 3, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: 10
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        tbdestroy: function () {
            dtMatType.destroy();
        },

        refresh: function () {
            dtMatType.ajax.reload();
        }
    }

    /*-- END SEARCH MATERIAL TYPE --*/

    /*-- BEGIN SEARCH MACHINE --*/

    $("#btnSearchMc").on("click", function (event) {

        event.preventDefault();

        //$('#searchIdContainer').html('');
        $('#searchMcIdModal').modal('show');

        //Initial Datatable
        mcVM.init();

    });

    $("#btnRemoveMc").on("click", function (event) {

        event.preventDefault();

        $('#MachineId').val(null);
        $('#Machine').val('');

    });

    $("#searchMcIdModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        mcVM.tbdestroy();

        return false;
    });



    mcVM = {
        dtMc: null,
        init: function () {
            var url = $('#EditData').data('mc-get-url') + "/" + $('#ProductionType').val();
            dtMc = $('#tblMc').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": url,
                    "type": "GET",
                    "datatype": "json"
                },
                columns: [
                    { "data": "MachineCode", "autoWidth": false },
                    { "data": "MachineName", "autoWidth": false },
                    { "data": "MachineSize", "autoWidth": false },
                    {
                        "render": function (data, type, mc, meta) {
                            return '<a id="selectMc" class="btn btn-default btn-sm" data-toggle="tooltip" title="Select" onclick="selectMc(\'' + mc.Id + '\',\'' + mc.MachineCode + '\');" href="javascript:void(0);"><span class="glyphicon glyphicon-hand-up" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "20%", "targets": 0 },
                    { "width": "45%", "targets": 1 },
                    { "width": "30%", "targets": 2 },
                    { "width": "5%", "targets": 3, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: 10
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        tbdestroy: function () {
            dtMc.destroy();
        },

        refresh: function () {
            dtMc.ajax.reload();
        }
    }

    /*-- END SEARCH MACHINE --*/

    /*-- BEGIN TABLE PRODUCT PROCESS --*/

    prodProcessVM = {
        dtProdprocess: null,
        init: function () {
            var url = $('#EditData').data('proc-get-url') + "/" + $('#Id').val();
            dtProdprocess = $('#tblProdProcess').DataTable({
                processing: true,
                autoWidth: false,
                ajax: {
                    "url": url,
                    "type": "GET",
                    "datatype": "json"
                },
                columns: [
                    { "data": "ProductId", "autoWidth": false },
                    { "data": "ProcessId", "autoWidth": false },
                    {
                        data: "Is_Active",
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data == 1) {
                                    return '<input id="chkProcess" type="checkbox" class="gridCheckbox" checked>';
                                } else {
                                    return '<input id="chkProcess" type="checkbox" class="gridCheckbox" >';
                                }
                            }
                            return data;
                        }
                    },
                    { "data": "ProcessSeq", "autoWidth": false },
                    { "data": "ProcessName", "autoWidth": false }
                ],
                columnDefs: [
                    { "width": "0%", "targets": 0, "visible": false },
                    { "width": "0%", "targets": 1, "visible": false },
                    { "className": "dt-center", "width": "20%", "targets": 2 },
                    { "className": "dt-center", "width": "15%", "targets": 3 },
                    { "width": "65%", "targets": 4 },
                ],
                paging: false,
                searching: false,
                ordering: false,
                info: false,
                lengthChange: false,
                responsive: true
            });
        },

        tbdestroy: function () {
            dtProdprocess.destroy();
        },

        refresh: function () {
            dtProdprocess.ajax.reload();
        }
    }

    // initialize the datatables
    prodProcessVM.init();

    //$("#tblProdProcess").on('draw.dt', function () {
        
    //    $('input[id$="chkProcess"]').iCheck({
    //        checkboxClass: 'icheckbox_square-blue',    //'input[id$="chkProcess"]'
    //        radioClass: 'iradio_square-blue',
    //        increaseArea: '20%' // optional
    //    });

    //});

    $("#tblProdProcess").on("click", "td input[type=checkbox]", function (event) {   //"td input[type=checkbox]"

        event.preventDefault();
        
        //var isChecked = this.checked;
        var dtapi = $("#tblProdProcess").DataTable();
        // set the data item associated with the row to match the checkbox
        var dtRow = dtapi.rows($(this).closest("tr"));


        //check first process can't click
        if (dtRow.data()[0].ProcessSeq == 1) {
            return;
        }


        dtapi.cell(dtRow, 2).data(this.checked).draw();

    });

    /*-- END TABLE PRODUCT PROCESS --*/
    $("#btnSaveEdit").on("click", SaveEdit);

    

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveEdit(event) {

        event.preventDefault();

        global.resetValidationErrors();

        var strProductCode = $("#ProductCode").val().toUpperCase();
        var strCompanyCode = $("#CompanyCode").val();
        var productFileName = $("#ProductImagePath").val();

        var fileLength = $("#imgProduct").get(0).files.length;
        if (fileLength > 0) {

            var selFilename = $("#imgProduct").get(0).files[0].name;
            var extension = selFilename.substring(selFilename.lastIndexOf('.') + 1);

            productFileName = strCompanyCode + '_' + strProductCode + '.' + extension;
        }

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('prod-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                ProductCode: $("#ProductCode").val().toUpperCase(),
                ProductName: $("#ProductName").val(),
                ProductNameRef: $("#ProductNameRef").val(),
                ProductDesc: $("#ProductDesc").val(),
                MaterialTypeId: $("#MaterialTypeId").val(),
                ProductionTypeId: $("#ProductionType").val(),
                MachineId: $("#MachineId").val(),
                UnitId: $("#Unit").val(),
                PackageStdQty: $("#PackageStdQty").val(),
                SalesPrice1: $("#SalesPrice1").val(),
                SalesPrice2: $("#SalesPrice2").val(),
                SalesPrice3: $("#SalesPrice3").val(),
                SalesPrice4: $("#SalesPrice4").val(),
                SalesPrice5: $("#SalesPrice5").val(),
                GLSalesAccount: $("#GLSalesAccount").val(),
                GLInventAccount: $("#GLInventAccount").val(),
                GLCogsAccount: $("#GLCogsAccount").val(),
                RevisionNo: $("#RevisionNo").val(),
                WarehouseId: $("#Warehouse").val(),
                LocationId: $("#Location").val(),
                CompanyCode: $("#CompanyCode").val(),
                ProductImagePath: productFileName,
                Is_Active: $("#Is_Active").is(':checked'),
                ProdProcess: GetProductionProcess()
            }),
            success: function (response) {

                if (response.success) {

                    //Update Product Image.
                    if (fileLength > 0) {
                        UploadProductImage(productFileName);
                    }

                    //UpdateProductProcess(event);

                    $('#editProdModal').modal('hide');
                    $('#editProdContainer').html("");

                    $("#tblProduct").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Product', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        function GetProductionProcess() {

            var prodpros = new Array();
            var jsonData = JSON.parse(JSON.stringify($('#tblProdProcess').dataTable().fnGetData()));

            for (var obj in jsonData) {
                if (jsonData.hasOwnProperty(obj)) {

                    var prodpro = {};
                    prodpro.ProductId = jsonData[obj]["ProductId"];
                    prodpro.ProcessId = jsonData[obj]["ProcessId"];
                    prodpro.Is_Active = jsonData[obj]["Is_Active"];

                    prodpros.push(prodpro);
                }
            }


            return prodpros;
        }

        function UploadProductImage(strName) {

            var files = $("#imgProduct").get(0).files;
            var fileData = new FormData();
            fileData.append("fileName", strName);
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }

            $.ajax({
                async: true,
                type: "POST",
                url: $('#EditData').data('prod-upload-url'),
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
                    
                    toastr.error('Error: ' + reponseErr.message, 'Edit Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
            });
        }
    }

    function displayValidationErrors(errors) {

        global.displayValidationErrors(errors);

        if ($("#ProductCode").val() === '' || $("#ProductName").val() === '') {
            $('.nav-tabs a[href="#tab_1"]').tab('show');
            return;
        }

        if ($("#GLSalesAccount").val() === '' || $("#GLCogsAccount").val() === '') {
            $('.nav-tabs a[href="#tab_2"]').tab('show');
            return;
        }
    }

    //for clear cache image
    var num = Math.random();
    var imgSrc = $('#imageProduct').attr("src") + "?v=" + num;
    $('#imageProduct').attr("src", imgSrc);

});

/*-- BEGIN SEARCH MATERIAL TYPE --*/
function selectMatType(id, name) {

    $("#MaterialTypeId").val(id);
    $("#MaterialType").val(name);

    $('#searchMatTypeIdModal').modal('hide');

}

function closeMatTypePopup() {
    $('#searchMatTypeIdModal').modal('hide');
}
/*-- END SEARCH MATERIAL TYPE --*/

/*-- BEGIN SEARCH MACHINE --*/
function selectMc(id, code) {

    $("#MachineId").val(id);
    $("#Machine").val(code);

    $('#searchMcIdModal').modal('hide');

}

function closeMcPopup() {
    $('#searchMcIdModal').modal('hide');
}
/*-- END SEARCH MACHINE --*/

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imageProduct')
                .attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

