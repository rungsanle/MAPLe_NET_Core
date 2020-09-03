$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //$('input').attr('autocomplete', 'off');

    var arrRemoveDetails = new Array();

    $("#ArrivalNo").val('(AUTO)');

    global.applyDatepicker($("#ArrivalDate").prop("id"), true);
    
    global.applyDatepicker($("#DocRefDate").prop("id"), false);

    global.applyInputPicker(
        $("#ArrivalTypeName").prop("id"),
        $('#CreateData').data('arrtype-get-url'),
        [
            { name: 'ArrivalTypeName', text: 'ARRIVAL TYPE', width: '100%' }
        ],
        '210px',
        true,
        'restore',
        false,
        'Id',
        'ArrivalTypeName'
    );
    $('#ArrivalTypeName').val('1');

    global.applyInputPicker(
        $("#RawMatTypeName").prop("id"),
        $('#CreateData').data('rawmattype-get-url'),
        [
            { name: 'RawMatTypeName', text: 'RAW MAT TYPE', width: '100%' }
        ],
        '210px',
        true,
        'restore',
        false,
        'Id',
        'RawMatTypeName'
    );

    /*---------------BEGIN VENDOR SEARCH-----------------*/

    $("#btnSearchVendor").on("click", function (event) {

        event.preventDefault();
        $('#searchVendorModal').modal('show');
        vendorVM.init();

    });

    $("#searchVendorModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        vendorVM.tbdestroy();

        if ($('#VendorCode').val() === '') {
            $('#VendorCode').focus();
        }

        return false;
    });

    $("#VendorCode").keyup(function (e) {
        if (e.which == 13) {
            // Enter key pressed

            var api = $('#CreateData').data('vend-get-url');

            $.ajax({
                type: "GET",
                url: api,
                async: true,
                dataType: 'json',
                contentType: "application/json",
                data: { vcode: $(this).val().toUpperCase() },
                success: function (response) {

                    if (response.success) {
                        
                        $('#VendorId').val(response.data.Id);
                        $('#VendorName').val(response.data.VendorName);

                        var address = (response.data.AddressL1 === null ? '' : response.data.AddressL1 + " ");
                        address += (response.data.AddressL2 === null ? '' : response.data.AddressL2 + " ");
                        address += (response.data.AddressL3 === null ? '' : response.data.AddressL3 + " ");
                        address += (response.data.AddressL4 === null ? '' : response.data.AddressL4 + " ");

                        $('#VendorAddress').val(address);
                        

                        $('#PurchaseOrderNo').focus();
                        
                    } else {
                        toastr.error(response.message, 'Select Vendor Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }

                },
                error: function (xhr, txtStatus, errThrown) {

                    var reponseErr = JSON.parse(xhr.responseText);

                    toastr.error('Error: ' + reponseErr.message, 'Select Vendor Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
            });
        }
    });

    //$("#VendorCode").focusout(function (e) {
    //    if ($('#VendorCode').val() === '') {
    //        $(this).val('');
    //    }
    //});

    vendorVM = {
        dtVendor: null,
        init: function () {

            dtVendor = $('#tblVendor').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": $('#CreateData').data('vend-get-url'),
                    "type": "GET",
                    "datatype": "json"
                },
                columns: [
                    { "data": "VendorCode", "autoWidth": false },
                    { "data": "VendorName", "autoWidth": false },
                    { "data": "AddressL1", "autoWidth": false },
                    { "data": "AddressL2", "autoWidth": false },
                    { "data": "AddressL3", "autoWidth": false },
                    { "data": "AddressL4", "autoWidth": false },
                    { "data": "VendorContact", "autoWidth": false },
                    {
                        "render": function (data, type, vendor, meta) {
                            return '<a id="selectVendor" class="btn btn-default btn-sm" data-toggle="tooltip" title="Select" href="javascript:void(0);"><span class="glyphicon glyphicon-hand-up" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "15%", "targets": 0 },
                    { "width": "35%", "targets": 1 },
                    { "width": "20%", "targets": 2 },
                    { "width": "0%", "targets": 3, "visible": false },
                    { "width": "0%", "targets": 4, "visible": false },
                    { "width": "0%", "targets": 5, "visible": false },
                    { "width": "22%", "targets": 6 },
                    { "width": "8%", "targets": 7, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: 10
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');
        },

        tbdestroy: function () {
            dtVendor.destroy();
        },

        refresh: function () {
            dtVendor.ajax.reload();
        }
    }

    //Select from Vendor LOV
    $('#tblVendor').on("click", "#selectVendor", function (event) {

        event.preventDefault();



        var rowSelect = $(this).parents('tr')[0];
        var vendorData = (dtVendor.row(rowSelect).data());

        $('#searchVendorModal').modal('hide');

        $('#VendorId').val(vendorData["Id"]);
        $('#VendorCode').val(vendorData["VendorCode"]);
        $('#VendorName').val(vendorData["VendorName"]);

        var address = (vendorData["AddressL1"] === null ? '' : vendorData["AddressL1"] + " ");
        address += (vendorData["AddressL2"] === null ? '' : vendorData["AddressL2"] + " ");
        address += (vendorData["AddressL3"] === null ? '' : vendorData["AddressL3"] + " ");
        address += (vendorData["AddressL4"] === null ? '' : vendorData["AddressL4"] + " ");

        $('#VendorAddress').val(address);

        $('#PurchaseOrderNo').focus();
        
    });

    $("#btnCloseVendorModel").on("click", function (event) {
        event.preventDefault();
        $('#searchVendorModal').modal('hide');
    });

    $("#btnCancelVendorModel").on("click", function (event) {
        event.preventDefault();
        $('#searchVendorModal').modal('hide');
    });

    /*---------------END VENDOR SEARCH-----------------*/

    /*---------------BEGIN MATERIAL SEARCH-----------------*/

    $("#searchMaterialModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        matVM.tbdestroy();


        if ($('#ItemCode').val() === '') {
            $('#ItemCode').focus();
        }

        return false;
    });
    

    

    //$("#ItemCode").keyup(function (e) {

    //    if (e.which == 13) {
    //        // Enter key pressed
    //        if ($(this).val() === '') {
    //            $('#ItemCode').focus();
    //            return;
    //        }

    //        var api = $('#CreateData').data('mat-get-url'); // + "/" + $(this).val();

    //        alert(api);

    //        $.ajax({
    //            type: "GET",
    //            url: api,
    //            async: true,
    //            dataType: 'json',
    //            contentType: "application/json",
    //            data: {
    //                vcode: $(this).val(),
    //                rawMatTypeId: $("#RawMatTypeName").val(),
    //                companyCode: $("#CompanyCode").val()
    //            },
    //            success: function (response) {

    //                if (response.success) {

    //                    $('#ItemId').val(response.data.Id);
    //                    $('#ItemName').val(response.data.MaterialName);

    //                    $('#ItemQty').focus();

    //                } else {
    //                    //alert(response.message);
    //                    $("#btnSearchMaterial").click();
    //                }

    //            },
    //            error: function (xhr, txtStatus, errThrown) {

    //                var reponseErr = JSON.parse(xhr.responseText);

    //                toastr.error('Error: ' + reponseErr.message, 'Select Items Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
    //            }
    //        });
    //    }
    //});

    matVM = {
        dtMaterial: null,
        init: function () {

            dtMaterial = $('#tblMaterial').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": $('#CreateData').data('mat-get-url'),
                    "type": "GET",
                    "datatype": "json",
                    data: {
                        rawMatTypeId: $("#RawMatTypeName").val(),
                        companyCode: $("#CompanyCode").val()
                    }
                },
                columns: [
                    { "data": "MaterialCode", "autoWidth": false },
                    { "data": "MaterialName", "autoWidth": false },
                    { "data": "MaterialDesc1", "autoWidth": false },
                    { "data": "PackageStdQty", "autoWidth": false },
                    { "data": "Unit", "autoWidth": false },
                    {
                        "render": function (data, type, vendor, meta) {
                            return '<a id="selectMaterial" class="btn btn-default btn-sm" data-toggle="tooltip" title="Select" href="javascript:void(0);"><span class="glyphicon glyphicon-hand-up" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "15%", "targets": 0 },
                    { "width": "35%", "targets": 1 },
                    { "width": "20%", "targets": 2 },
                    { "width": "22%", "targets": 3 },
                    { "width": "22%", "targets": 4 },
                    { "width": "8%", "targets": 5, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: 10
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');
        },

        tbdestroy: function () {
            dtMaterial.destroy();
        },

        refresh: function () {
            dtMaterial.ajax.reload();
        }
    }


    //Select from Material LOV
    $('#tblMaterial').on("click", "#selectMaterial", function (event) {

        event.preventDefault();

        var rowSelect = $(this).parents('tr')[0];
        var itemData = (dtMaterial.row(rowSelect).data());

        $('#searchMaterialModal').modal('hide');

        //test by jack
        //var curAddRow = (dtArrDtl.row(':last').data());

        //console.log(curAddRow);

        //var curAddRow = (dtArrDtl.row(':last').data());
        //var curAddRow = ($('#tblArrivalDtl tfoot tr').data());

        //console.log(curAddRow);
        //curAddRow["ArrivalId"] = 1;
        //curAddRow["PoLineNo"] = 1;
        //curAddRow["MaterialId"] = 1;

        //curAddRow["MaterialCode"] = itemData["MaterialCode"];
        //curAddRow["MaterialName"] = itemData["MaterialName"];

        //dtArrDtl.row(':last').data(curAddRow).draw(false);
        //dtArrDtl.row($(this).closest('tr')).data(curAddRow).draw(false);

        //$('#tblArrivalDtl').dataTable().fnUpdate(curAddRow, 5, undefined, false);

        //console.log('OK');

        $('#hPoLine').val(0);
        $('#ItemId').val(itemData["Id"]);
        $('#ItemCode').val(itemData["MaterialCode"]);     

        $('#ItemName').val(itemData["MaterialName"]);

        $('#ItemQty').focus().select();

    });

    $("#btnCloseMaterialModel").on("click", function (event) {
        event.preventDefault();
        $('#searchMaterialModal').modal('hide');
    });

    $("#btnCancelMaterialModel").on("click", function (event) {
        event.preventDefault();
        $('#searchMaterialModal').modal('hide');
    });

    /*---------------END MATERIAL SEARCH-----------------*/




    /*-------------- BEGIN COMPANY CODE --------------*/
    var compCode = $('#CreateData').data('viewbag-compcode');

    global.applyCompanyCodeDropdown();

    if (compCode != 'ALL*') {
        $('#CompanyCode').val(compCode);

        setTimeout(function () {
            $(".inputpicker-input:last").attr("disabled", true);
        }, 300);
    }
    /*-------------- END COMPANY CODE --------------*/

    global.applyDraggable();

    global.applyIsActiveSwitch(true, false);

    /*-------------- BEGIN ARRIVAL DETAIL --------------*/

    arrDtlVM = {
        dtArrDtl: null,
        init: function () {
            var url = $('#CreateData').data('arrdtl-get-url') + "/0";

            dtArrDtl = $('#tblArrivalDtl').DataTable({
                //dom: 'frtip',
                processing: true,
                ajax: {
                    "url": url,
                    "type": "GET",
                    "datatype": "json"
                },
                columns: [
                    { "data": "ArrivalId", "autoWidth": false },
                    {
                        "data": "LineNo",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { "data": "PoLineNo", "autoWidth": false },
                    { "data": "MaterialId", "autoWidth": false },
                    { "data": "MaterialCode", "autoWidth": false },
                    //{
                    //    "data": "MaterialCode", "autoWidth": false,
                    //    render: function (data, type, prod, meta) {

                    //        if (prod.RecordFlag < 0) {
                    //            return '<div class="input-group" style="width:100%">' +
                    //                '<input class="form-control input-sm text-bold text-box single-line" id="ItemCode" name="ItemCode" type="text" value="">' +
                    //                '<span class="input-group-btn">' +
                    //                '<button type="button" class="btn btn-default btn-sm btn-flat" data-toggle="tooltip" title="Search Material" id="searchItem">' +
                    //                '<span class="glyphicon glyphicon-search" aria-hidden="true"></span>' +
                    //                '</button>' +
                    //                '</span>' +
                    //                '</div>';
                    //        }
                    //        else {
                    //            return ditem;
                    //        }
                    //    }
                    //},
                    { "data": "MaterialName", "autoWidth": false },
                    { "data": "MaterialDesc", "autoWidth": false },
                    { "data": "OrderQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    //{
                    //    "data": "OrderQty", "autoWidth": false,
                    //    "render": function (data, type, prod, meta) {

                    //        //return "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='OrderQty' min='0' type='number' value='" + ditem + "' autocomplete='off' />";

                    //        if (prod.RecordFlag < 0) {
                    //            return "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='ItemQty' name='ItemQty' min='0' type='number' value='0' autocomplete='off'>";
                    //        } else {
                    //            return "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='OrderQty' min='0' type='number' value='" + data + "' autocomplete='off' />";
                    //        }
                    //    }
                    //},
                    { "data": "RecvQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    { "data": "LotNo", "autoWidth": false },
                    //{
                    //    data: "LotNo", autoWidth: false,
                    //    render: function (ditem) {
                    //        return "<input class='form-control input-sm text-box single-line' style='width:100%' id='LotNo' name='LotNo' type='text' value=''>";
                    //    }
                    //},
                    {
                        "data": "LotDate", "autoWidth": false, "type": "date-eu", "render": function (value) {

                            return global.localDate(value);
                            
                        }
                    },
                    //{
                    //    "data": "LotDate", "autoWidth": false, "type": "date",
                    //    "render": function (value) {
                    //        var hCode = '<div class="input-group date">' +
                    //            '<input class="form-control input-sm text-box single-line" data-val="true" data-val-date="The field LOT DATE must be a date." id="LotDate" name="LotDate" type="datetime" value="">' +
                    //            '<span class="input-group-addon" id="showLotDate">' +
                    //                '<i class="fa fa-calendar"></i>' +
                    //            '</span>' +
                    //            '</div>';

                    //        return hCode;
                    //    }
                    //},
                    { "data": "DetailRemark", "autoWidth": false },
                    //{
                    //    "data": "DetailRemark", "autoWidth": false,
                    //    render: function (ditem) {
                    //        return "<input class='form-control input-sm text-box single-line' style='width:100%' id='ItemRemark' name='ItemRemark' type='text' value=''>";
                    //    }
                    //},
                    { "data": "NoOfLabel", "autoWidth": false },
                    { "data": "GenLabelStatus", "autoWidth": false },
                    { "data": "CompanyCode", "autoWidth": false },
                    { "data": "RecordFlag", "autoWidth": false },
                    {
                        "render": function (data, type, prod, meta) {
                            return '<a id="delItem" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "0%", "targets": 0, "visible": false },  //ArrivalId
                    { "className": "dt-right", "width": "3%", "targets": 1 },  //LineNo
                    { "width": "0%", "targets": 2, "visible": false },  //PoLineNo
                    { "width": "0%", "targets": 3, "visible": false },  //MaterialId
                    { "className": "dt-bold-left", "width": "12%", "targets": 4 },  //MaterialCode
                    { "width": "21%", "targets": 5 },   //MaterialName
                    { "width": "0%", "targets": 6, "visible": false },  //MaterialDesc
                    { "className": "dt-bold-right", "width": "8%", "targets": 7 },  //OrderQty   
                    { "className": "dt-right", "width": "8%", "targets": 8 },   //RecvQty
                    { "width": "11%", "targets": 9 },   //LotNo
                    { "className": "dt-center", "width": "11%", "targets": 10 }, //LotDate
                    { "width": "12%", "targets": 11 },  //DetailRemark
                    { "className": "dt-right", "width": "4%", "targets": 12 },  //NoOfLabel
                    { "className": "dt-center", "width": "4%", "targets": 13 }, //GenLabelStatus
                    { "width": "0%", "targets": 14, "visible": false },  //CompanyCode
                    { "width": "0%", "targets": 15, "visible": false },  //RecordFlag
                    { "width": "6%", "targets": 16 }    //Action
                ],
                scrollY: '350px',
                scrollX: false,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                searching: false,
                order: [],
                ordering: false,
                info: true,
                //language: {
                //    info: "Total _TOTAL_ records",
                //    infoEmpty: ""
                //},
                lengthChange: false,
                responsive: true
                /*,
                initComplete: function (row, data, start, end, display) {
                    var api = this.api();
                    //var footer = $(this).append('<tfoot><tr></tr></tfoot>');
                    var footer = $(this).append($('<tfoot/>').append($("#tblArrivalDtl thead tr").clone()));
                    //this.api().columns().every(function () {
                    //    var column = this;
                    //    $(footer).append('<th><input type="text" style="width:100%;"></th>');
                    //});

                    //LineNo
                    var htmlLineNo = dtArrDtl.rows().count() + 1;
                    $(footer).append($('<td>' + htmlLineNo + '</td>'));

                    //MaterialCode
                    var htmlMaterialCode = '<div class="input-group" style="width:100%">' +
                        '<input class="form-control input-sm text-bold text-box single-line" id="ItemCode" name="ItemCode" type="text" value="">' +
                        '<span class="input-group-btn">' +
                        '<button type="button" class="btn btn-default btn-sm btn-flat" data-toggle="tooltip" title="Search Material" id="searchItem">' +
                        '<span class="glyphicon glyphicon-search" aria-hidden="true"></span>' +
                        '</button>' +
                        '</span>' +
                        '</div>';
                    $(footer).append($('<td>' + htmlMaterialCode + '</td>'));

                    //MaterialName
                    $(footer).append($('<td></td>'));

                    //OrderQty
                    var htmlOrderQty = "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='ItemQty' name='ItemQty' min='0' type='number' value='0' autocomplete='off'>";
                    $(footer).append($('<td>' + htmlOrderQty + '</td>'));

                    //RecvQty
                    $(footer).append($('<td>0</td>'));

                    //LotNo
                    var htmlLotNo = "<input class='form-control input-sm text-box single-line' style='width:100%' id='LotNo' name='LotNo' type='text' value=''>";
                    $(footer).append($('<td>' + htmlLotNo + '</td>'));

                    //LotDate
                    var htmlLotDate = '<div class="input-group date">' +
                        '<input class="form-control input-sm text-box single-line" data-val="true" data-val-date="The field LOT DATE must be a date." id="LotDate" name="LotDate" type="datetime" value="">' +
                        '<span class="input-group-addon" id="showLotDate">' +
                        '<i class="fa fa-calendar"></i>' +
                        '</span>' +
                        '</div>';
                    $(footer).append($('<td>' + htmlLotDate + '</td>'));

                    //DetailRemark
                    var htmlDetailRemark = "<input class='form-control input-sm text-box single-line' style='width:100%' id='ItemRemark' name='ItemRemark' type='text' value=''>";
                    $(footer).append($('<td>' + htmlDetailRemark + '</td>'));

                    //Label 
                    $(footer).append($('<td>0</td>'));

                    //Status
                    $(footer).append($('<td>G</td>'));

                    //Button
                    var htmlButtonAdd = '<a id="delItem" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                    $(footer).append($('<td>' + htmlButtonAdd + '</td>'));
                }
                */

            });
        },

        tbdestroy: function () {
            dtArrDtl.destroy();
        },

        refresh: function () {
            dtArrDtl.ajax.reload();
        }
    }

    arrDtlVM.init();

    $("#tblArrivalDtl").append(
        $('<tfoot/>').append($("#tblArrivalDtl thead tr").clone())
    );

    
    //$("div.toolbar").html(
    //    '<table id="tblAddMaterial" style="width: 100%;" border="1">' +
    //    '<tbody>' +
    //    '<tr>' +
    //    '<td style="width:3%">No</td>' +
    //    '<td style="width:15%">' +
    //    '<input id="ItemId" name="ItemId" type="hidden" value="">' +
    //    '<div class="input-group">' +
    //        '<input class="form-control input-sm text-bold text-box single-line" id="ItemCode" name="ItemCode" type="text" value="">' +
    //            '<span class="input-group-btn">' +
    //                '<button type="button" class="btn btn-default btn-sm btn-flat" data-toggle="tooltip" title="Search Material" id="btnSearchMaterial">' +
    //                    '<span class="glyphicon glyphicon-search" aria-hidden="true"></span>' +
    //                '</button>' +
    //            '</span>' +
    //    '</div>' +
    //    '</td >' +
    //    '<td style="width:21%">&nbsp;</td>' +
    //    '<td style="width:8%">&nbsp;</td>' +
    //    '<td style="width:8%">&nbsp;</td>' +
    //    '<td style="width:11%">&nbsp;</td>' +
    //    '<td style="width:8%">&nbsp;</td>' +
    //    '<td style="width:12%">&nbsp;</td>' +
    //    '<td style="width:4%">&nbsp;</td>' +
    //    '<td style="width:6%">&nbsp;</td>' +
    //    '<td style="width:4%">&nbsp;</td>' +
    //    '</tr>' +
    //    '</tbody>' +
    //    '</table >');

    setTimeout(function () {
        dtArrDtl.columns.adjust().draw();
        CreateNewArrivalDtlRow();
    }, 200);


    //------------------------- ItemCode -------------------------------//
    //when tab
    $('#tblArrivalDtl').on("keydown", "#ItemCode", function (event) {

        if (event.keyCode == 9) var thisTab = $(":focus").attr("tabindex");
        if (event.keyCode == 9) {

            if ($(this).val() === '') {
                $('#ItemCode').focus();
                return;
            }

            

            var api = $('#CreateData').data('mat-get-url'); // + "/" + $(this).val();
            var itemCode = $(this).val();

            $.ajax({
                type: "GET",
                url: api,
                async: true,
                dataType: 'json',
                contentType: "application/json",
                data: {
                    vcode: itemCode,
                    rawMatTypeId: $("#RawMatTypeName").val(),
                    companyCode: $("#CompanyCode").val()
                },
                success: function (response) {

                    if (response.success) {

                        $('#ItemId').val(response.data.Id);
                        $('#ItemName').val(response.data.MaterialName);

                        $('#ItemQty').focus().select();

                    } else {

                        toastr.error('"' + itemCode + '" ' + response.message, 'Search Item', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                        $("#searchItem").click();
                    }

                },
                error: function (xhr, txtStatus, errThrown) {

                    var reponseErr = JSON.parse(xhr.responseText);

                    toastr.error('Error: ' + reponseErr.message, 'Select Items Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
            });
        }
    });

    //when enter
    $('#tblArrivalDtl').on("keyup", "#ItemCode", function (event) {

        event.preventDefault();


        if (event.which == 13) {
            // Enter key pressed
            if ($(this).val() === '') {
                $('#ItemCode').focus();
                return;
            }

            var api = $('#CreateData').data('mat-get-url'); // + "/" + $(this).val();
            var itemCode = $(this).val();

            $.ajax({
                type: "GET",
                url: api,
                async: true,
                dataType: 'json',
                contentType: "application/json",
                data: {
                    vcode: itemCode,
                    rawMatTypeId: $("#RawMatTypeName").val(),
                    companyCode: $("#CompanyCode").val()
                },
                success: function (response) {

                    if (response.success) {

                        $('#ItemId').val(response.data.Id);
                        $('#ItemName').val(response.data.MaterialName);

                        $('#ItemQty').focus().select();

                    } else {

                        toastr.error('"' + itemCode + '" ' + response.message, 'Search Item', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                        $("#searchItem").click();
                    }

                },
                error: function (xhr, txtStatus, errThrown) {

                    var reponseErr = JSON.parse(xhr.responseText);

                    toastr.error('Error: ' + reponseErr.message, 'Select Items Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
            });
        }
    });

    //when click button search
    $('#tblArrivalDtl').on('click', '#searchItem', function (event) {

        event.preventDefault();

        $('#searchMaterialModal').modal('show');

        matVM.init();

        //var row = $(this).parents('tr')[0];
        //activeCurrRow = (dtArrDtl.row(row).data());
    });

    //------------------------- ItemCode -------------------------------//

    //------------------------- ItemQty -------------------------------//

    //when tab
    $('#tblArrivalDtl').on("keydown", "#ItemQty", function (event) {

        if (event.keyCode == 9) var thisTab = $(":focus").attr("tabindex");
        if (event.keyCode == 9) {

            var itemQty = $('#ItemQty').val();
            

            if (isNaN(itemQty) || itemQty < 1) {

                setTimeout(function () {
                    $('#ItemQty').focus().select();
                }, 50);

                return;
            }

            setTimeout(function () {
                $('#LotNo').focus().select();
            }, 50);
           
        }
    });

    //when enter
    $('#tblArrivalDtl').on("keyup", "#ItemQty", function (event) {

        event.preventDefault();

        if (event.which == 13) {

            var itemQty = $('#ItemQty').val();
            // Enter key pressed
            if (isNaN(itemQty) || itemQty < 1) {

                $('#ItemQty').focus().select();

                return;
            }

            $('#LotNo').focus().select();
            
        }
    });

    //------------------------- ItemQty -------------------------------//

    //------------------------- LotNo -------------------------------//
    
    //when enter
    $('#tblArrivalDtl').on("keyup", "#LotNo", function (event) {

        event.preventDefault();

        if (event.which == 13) {
            $('#LotDate').focus().select();
        }
    });

    //------------------------- LotNo -------------------------------//

    //------------------------- LotDate -------------------------------//

    //when enter
    $('#tblArrivalDtl').on("keyup", "#LotDate", function (event) {

        event.preventDefault();

        if (event.which == 13) {
            $('#ItemRemark').focus().select();
        }
    });

    //------------------------- LotDate -------------------------------//

    //------------------------- ItemRemark -------------------------------//

    //when enter
    $('#tblArrivalDtl').on("keyup", "#ItemRemark", function (event) {

        event.preventDefault();

        if (event.which == 13) {
            $('#btnAddDtl').focus();
        }
    });

    //------------------------- ItemRemark -------------------------------//

    //Add Detail
    $('#tblArrivalDtl').on('click', '#btnAddDtl', function (event) {

        event.preventDefault();

        if (!ValidationRecordDetail()) return;

        var itemCode = $('#ItemCode').val();
        var poLine = 0;

        var noExist = true;   //CheckDuplicateArrDtl(itemCode, poLine);

        if (noExist) {

            dtArrDtl.row.add({
                Id: 0,
                ArrivalId: 0,
                LineNo: dtArrDtl.data().length + 1,
                PoLineNo: $('#hPoLine').val(), 
                MaterialId: $('#ItemId').val(),
                MaterialCode: $('#ItemCode').val(),
                MaterialName: $('#ItemName').val(),
                MaterialDesc: '',
                OrderQty: $('#ItemQty').val(),
                RecvQty: 0,
                LotNo: $('#LotNo').val(),
                LotDate: global.localDate($("#LotDate").val()),    //$("#LotDate").val(),                //global.localDate($("#LotDate").val()),
                DetailRemark: $('#ItemRemark').val(),
                NoOfLabel: 0,
                GenLabelStatus: 'N',
                CompanyCode: $("#CompanyCode").val(),
                RecordFlag: 2
            }).draw(false);

            dtArrDtl.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);

            $('.dataTables_scrollBody').scrollTop($('.dataTables_scrollBody')[0].scrollHeight);

            CreateNewArrivalDtlRow();

            $('#ItemCode').focus();
        }
    });

    //Cancel Detail
    $('#tblArrivalDtl').on('click', '#btnCancelDtl', function (event) {

        event.preventDefault();

        ClearPanelItemDetail();
    });


    //Delete
    $('#tblArrivalDtl').on('click', '#delItem', function (event) {

        event.preventDefault();

        var row = $(this).parents('tr')[0];
        var ItemData = (dtArrDtl.row(row).data());
        //var lineDel = ItemData["LineNo"];
        var rflag = ItemData["RecordFlag"];
        var itemCode = ItemData["MaterialCode"];
        var con = confirm("Are you sure you want to delete this " + itemCode)
        if (con) {

            if (rflag == 1) {

                var arrRemoveDtl = {};

                arrRemoveDtl.Id = ItemData["Id"];
                arrRemoveDtl.ArrivalId = ItemData["ArrivalId"];
                arrRemoveDtl.LineNo = ItemData["LineNo"];
                arrRemoveDtl.PoLineNo = ItemData["PoLineNo"];
                arrRemoveDtl.MaterialId = ItemData["MaterialId"];
                arrRemoveDtl.MaterialCode = ItemData["MaterialCode"];
                arrRemoveDtl.MaterialName = ItemData["MaterialName"];
                arrRemoveDtl.MaterialDesc = ItemData["MaterialDesc"];
                arrRemoveDtl.OrderQty = ItemData["OrderQty"];
                arrRemoveDtl.RecvQty = ItemData["RecvQty"];
                arrRemoveDtl.LotNo = ItemData["LotNo"];
                arrRemoveDtl.LotDate = global.localDate(ItemData["LotDate"]);   //ItemData["LotDate"];
                arrRemoveDtl.DetailRemark = ItemData["DetailRemark"];
                arrRemoveDtl.GenLabelStatus = ItemData["GenLabelStatus"];
                arrRemoveDtl.NoOfLabel = ItemData["NoOfLabel"];
                arrRemoveDtl.CompanyCode = ItemData["CompanyCode"];
                arrRemoveDtl.RecordFlag = 0; //Delete Record
                arrRemoveDtl.Is_Active = true;

                arrRemoveDetails.push(arrRemoveDtl);
            }

            dtArrDtl.row(row).remove().draw(false);

            dtArrDtl.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);

            CreateNewArrivalDtlRow();   
        }
        else {
            
        }
    });

    //$("#tblArrivalDtl").on("keyup", "input[type='number']", function (e) {

    //    var rData = dtArrDtl.rows($(this).closest("tr")).data()[0];
    //    rData.OrderQty = $(this).val();

    //});

    //$('#tblArrivalDtl').on('click', '#showLotDate', function (event) {

    //    event.preventDefault();

    //    var trLotDate = $('input[id$="LotDate"]', $(this).closest("tr"));
    //    global.showDatepicker(trLotDate.prop("id"));
    //});

    /*-------------- END ARRIVAL DETAIL --------------*/

    //var ctlId = $("#LotDate").prop("id");
    //alert(ctlId);

    //global.applyDatepicker($("#LotDate").prop("id"), true);

    //$("input[name$='LotDate']").inputmask('dd-mm-yyyy', { placeholder: 'dd-mm-yyyy' });
    //$("input[name$='LotDate']").datepicker({
    //    format: 'dd-mm-yyyy',
    //    autoclose: true,
    //    todayHighlight: true,
    //    todayBtn: "linked",
    //    language: "fr-FR"
    //});






    $("#btnAddArrDtl").on("click", function (event) {

        event.preventDefault();

        CreateNewArrivalDtlRow();

        

        //var itemCode = $('#ItemCode').val();
        //var poLine = 0;

        //var noExist = CheckDuplicateArrDtl(itemCode, poLine);

        //if (noExist) {

        //    dtArrDtl.row.add({
        //        Id: 0,
        //        ArrivalId: 0,
        //        LineNo: dtArrDtl.data().length + 1,
        //        PoLineNo: 0,
        //        MaterialId: $('#ItemId').val(),
        //        MaterialCode: itemCode,
        //        MaterialName: $('#ItemName').val(),
        //        MaterialDesc: '',
        //        OrderQty: $('#ItemQty').val(),
        //        RecvQty: 0,
        //        LotNo: $('#LotNo').val(),
        //        LotDate: global.convertJsonDate($('#LotDate').val()),
        //        DetailRemark: $('#ItemRemark').val(),
        //        NoOfLabel: 0,
        //        GenLabelStatus: 'G',
        //        CompanyCode: $("#CompanyCode").val(),
        //        RecordFlag: 2 //new record
        //    }).draw(false);

        //    dtArrDtl.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        //        cell.innerHTML = i + 1;
        //    }).draw(false);

        //    $('.dataTables_scrollBody').scrollTop($('.dataTables_scrollBody')[0].scrollHeight);
        //}

        //ClearPanelItemDetail();
        
        


    });

    $("#btnAddDtl").on("click", function (event) {

        event.preventDefault();

        var itemCode = $('#ItemCode').val();
        var poLine = 0;

        var noExist = true;   //CheckDuplicateArrDtl(itemCode, poLine);

        if (noExist) {

            dtArrDtl.row.add({
                Id: 0,
                ArrivalId: 0,
                LineNo: dtArrDtl.data().length + 1,
                PoLineNo: 0,
                MaterialId: -1,
                MaterialCode: '',
                MaterialName: '',
                MaterialDesc: '',
                OrderQty: 0,
                RecvQty: 0,
                LotNo: '',
                LotDate: null,
                DetailRemark: '',
                NoOfLabel: 0,
                GenLabelStatus: '',
                CompanyCode: $("#CompanyCode").val(),
                RecordFlag: -1 
            }).draw(false);

            dtArrDtl.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);

            $('.dataTables_scrollBody').scrollTop($('.dataTables_scrollBody')[0].scrollHeight);
        }

        ClearPanelItemDetail();

        

    });

    $("#btnSaveCreate").on("click", function (event) {

        event.preventDefault();

        //var valueValConvertToDate = new Date($("#ArrivalDate").val());

        //console.log(valueValConvertToDate);

        //var valueGetDate = $("#ArrivalDate").datepicker("getDate");

        //console.log(valueGetDate);

        //var arrDate = new Date(valueGetDate);

        //console.log(arrDate);

        //var api = $('#CreateData').data('arrival-add-url');
        //jQuery.validator.methods["date"] = function (value, element) {
        //    var result = true;
        //    try {
        //        $.datepicker.parseDate('dd-mm-yyyy', value);
        //    }
        //    catch (err) {
        //        result = false;
        //    }

        //    return result;
        //}
        console.log($("#ArrivalDate").val());

        console.log(global.localDate($("#ArrivalDate").val()));

        //var useDate = moment($("#ArrivalDate").val(), "DD-MM-YYYY").format("DD-MM-YYYY");

        //console.log(useDate);

        //alert(useDate);

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('arrival-add-url'),
            data: addRequestVerificationToken({
                ArrivalNo: $("#ArrivalNo").val().toUpperCase(),
                ArrivalDate: global.localDate($("#ArrivalDate").val()),
                ArrivalTypeId: $("#ArrivalTypeName").val(),
                RawMatTypeId: $("#RawMatTypeName").val(),
                VendorId: $("#VendorId").val().toUpperCase(),
                PurchaseOrderNo: $("#PurchaseOrderNo").val(),
                DocRefNo: $("#DocRefNo").val().toUpperCase(),
                DocRefDate: global.localDate($("#DocRefDate").val()),
                ArrivalRemark: $("#ArrivalRemark").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $("#Is_Active").is(':checked'),

                ArrivalDetails: GetArrivalItemDetails(arrRemoveDetails)
            }),
            success: function (response) {

                if (response.success) {

                    
                    $('#newArrivalModal').modal('hide');
                    $('#newArrivalContainer').html("");

                    $("#tblArrival").DataTable().ajax.reload(null, false);
                    $("#tblArrival").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Arrival', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });

                }
                else {
                    if (response.errors != null) {
                        //global.errorsAlert(response.errors, 5000);

                        console.log(response.errors);

                        toastr.error(response.errors, 'Create Arrival', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    } else {
                        toastr.error(response.message, 'Create Arrival', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Create Arrival', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });


    //$('#tblArrivalDtl_F').on("click", "#btnAddArrDtl", function (event) {

    //    event.preventDefault();

    //    alert('test');

    //});
    //$('#tblArrivalDtl').DataTable({
    //    columns: [
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false },
    //        { "autoWidth": false }
    //    ],
    //    columnDefs: [
    //        { "width": "6%" }, //Line No
    //        { "width": "10%" }, //Item Code
    //        { "width": "20%" }, // Item Name
    //        { "width": "0%", "visible": false }, //Item Description
    //        { "width": "8%" }, //Order Qty
    //        { "width": "8%" },   //Receive Qty
    //        { "width": "12%" },   //Lot Number
    //        { "width": "8%" },    //Lot Date
    //        { "width": "12%" },    //Remark
    //        { "width": "6%" },     //No Of Label
    //        { "width": "10%" },    //Gen Label Status
    //        { "width": "8%" }    //Action
    //    ],
    //    order: [],
    //    autoWidth: false,
    //    scrollY: '200px',
    //    scrollCollapse: true,
    //    paging: false,
    //    searching: false,
    //    ordering: false,
    //    info: false,
    //    lengthChange: false,
    //    responsive: true
    //});


});

function addRequestVerificationToken(data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function CreateNewArrivalDtlRow() {

    //LineNo (New)
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class=" dt-right">(new)</td>'));  //<span class="fa fa-asterisk" aria-hidden="true">
    
    //MaterialCode
    var htmlMaterialCode = '<div class="input-group" style="width:100%">' +
        '<input type="hidden" id="hPoLine" name="ItemId" value="">' +
        '<input type="hidden" id="ItemId" name="ItemId" value="">' +
        '<input class="form-control input-sm text-bold text-box single-line" id="ItemCode" name="ItemCode" type="text" data-toggle="tooltip" data-placement="bottom" title="This field is require!!" value="" autocomplete="off">' +
        '<span class="input-group-btn">' +
        '<button type="button" class="btn btn-default btn-sm btn-flat" data-toggle="tooltip" title="Search Material" id="searchItem">' +
        '<span class="glyphicon glyphicon-search" aria-hidden="true"></span>' +
        '</button>' +
        '</span>' +
        '</div>';
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class="dt-bold-left">' + htmlMaterialCode + '</td>'));

    //MaterialName
    var htmlMaterialName = "<input class='form-control input-sm text-box single-line' style='width:100%' id='ItemName' name='ItemName' readonly='readonly' type='text' value='' tabindex='-1' onfocus='this.blur();'>";
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td>' + htmlMaterialName + '</td>'));

    //OrderQty
    var htmlOrderQty = '<input class="form-control input-sm text-align-right text-box single-line" style="width:100%" id="ItemQty" name="ItemQty" min="0" type="number" data-toggle="tooltip" data-placement="bottom" title="Qty not valid!!" value="0" autocomplete="off">';
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class="dt-bold-right">' + htmlOrderQty + '</td>'));

    //RecvQty
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class="dt-right">0</td>'));

    //LotNo
    var htmlLotNo = "<input class='form-control input-sm text-box single-line' style='width:100%' id='LotNo' name='LotNo' type='text' value=''>";
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td>' + htmlLotNo + '</td>'));

    //LotDate
    var htmlLotDate = '<div class="input-group date">' +
        '<input class="form-control input-sm text-box single-line" data-val="true" data-val-date="The field LOT DATE must be a date." id="LotDate" name="LotDate" type="datetime" value="">' +
        '<span class="input-group-addon" onclick="global.showDatepicker(\'LotDate\');">' +
        '<i class="fa fa-calendar"></i>' +
        '</span>' +
        '</div>';
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td>' + htmlLotDate + '</td>'));

    //DetailRemark
    var htmlDetailRemark = "<input class='form-control input-sm text-box single-line' style='width:100%' id='ItemRemark' name='ItemRemark' type='text' value=''>";
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td>' + htmlDetailRemark + '</td>'));

    //Label 
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class="dt-right">0</td>'));

    //Status
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td class="dt-center">G</td>'));

    //Button
    //var htmlButtonAdd = '<a id="btnAddDtl" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></a>';
    var htmlAction = '<span class="input-group">' +
        '<button type="button" class="btn btn-default btn-sm btn-flat input-space" data-toggle="tooltip" title="Add Detail" id="btnAddDtl">' +
        '<span class="glyphicon glyphicon-plus" aria-hidden="true"></span>' +
        '</button>&nbsp;' +
        '<button type="button" class="btn btn-default btn-sm btn-flat input-space" data-toggle="tooltip" title="Cancel Detail" id="btnCancelDtl">' +
        '<span class="glyphicon glyphicon-repeat" aria-hidden="true"></span>' +
        '</button>' +
        '</span>';
    $('#tblArrivalDtl tfoot tr:first-child').append($('<td>' + htmlAction + '</td>'));


    global.applyDatepicker($("#LotDate").prop("id"), true);

    var numRows = dtArrDtl.rows().count();

    $("#lblArrDtlRecords").text("Total " + numRows + " Records");

}

function ValidationRecordDetail() {

    //Item Code
    var itemCode = $('#ItemCode').val();
    if (itemCode === '') {

        $('#ItemCode[data-toggle="tooltip"]').tooltip('show');

        $('#ItemCode').focus();

        return false;
    }

    //Item Code
    var itemQty = $('#ItemQty').val();

    if (isNaN(itemQty) || itemQty < 1) {

        $('#ItemQty[data-toggle="tooltip"]').tooltip('show');

        $('#ItemQty').focus().select();

        return false;
    }

    return true;
}

function ClearPanelItemDetail() {

    $('hPoLine').val('');
    $('#ItemId').val('');
    $('#ItemCode').val('');
    $('#ItemName').val('');
    $('#ItemQty').val('');
    $('#LotNo').val('');
    $('#ItemRemark').val('');

    $('#ItemCode').focus();
}

function CheckDuplicateArrDtl(chkCode, poLine) {

    var noExist = true;
    $("#tblArrivalDtl").DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();

        if (chkCode == data["MaterialCode"] && poLine == data["PoLineNo"]) {

            alert('Item already exist!!');

            noExist = false;

            return noExist;
        }
        
        // ... do something with data(), or this.node(), etc
    });

    return noExist;
}

function GetArrivalItemDetails(arrDetails) {

    if (!arrDetails) {
        
        arrDetails = new Array();
    }
    //var arrDetails = new Array();
    var jsonData = JSON.parse(JSON.stringify($('#tblArrivalDtl').dataTable().fnGetData()));

    for (var obj in jsonData) {

        if (jsonData.hasOwnProperty(obj)) {

            var arrDtl = {};

            arrDtl.Id = jsonData[obj]["Id"];
            arrDtl.ArrivalId = jsonData[obj]["ArrivalId"];
            arrDtl.LineNo = jsonData[obj]["LineNo"];
            arrDtl.PoLineNo = jsonData[obj]["PoLineNo"];
            arrDtl.MaterialId = jsonData[obj]["MaterialId"];
            arrDtl.MaterialCode = jsonData[obj]["MaterialCode"];
            arrDtl.MaterialName = jsonData[obj]["MaterialName"];
            arrDtl.MaterialDesc = jsonData[obj]["MaterialDesc"];
            arrDtl.OrderQty = jsonData[obj]["OrderQty"];
            arrDtl.RecvQty = jsonData[obj]["RecvQty"];
            arrDtl.LotNo = jsonData[obj]["LotNo"];
            arrDtl.LotDate = global.localDate(jsonData[obj]["LotDate"]);   //jsonData[obj]["LotDate"];
            arrDtl.DetailRemark = jsonData[obj]["DetailRemark"];
            arrDtl.GenLabelStatus = jsonData[obj]["GenLabelStatus"];
            arrDtl.NoOfLabel = jsonData[obj]["NoOfLabel"];
            arrDtl.CompanyCode = jsonData[obj]["CompanyCode"];
            arrDtl.RecordFlag = jsonData[obj]["RecordFlag"];
            arrDtl.Is_Active = true;

            arrDetails.push(arrDtl);
        }
    }

    return arrDetails;
}

//function showDatepicker(obj) {
//    alert(obj);
//    //$("#ArrivalDate").datepicker('show');
//}

//function onFocusOut(ctl) {

//    if (ctl.val() != '') {
//        document.querySelectorAll('.text-danger li')[0].remove();
//    }

//}

//function addRequestVerificationToken(data) {
//    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
//    return data;
//};

//function SaveCrate(event) {

//    event.preventDefault();

//    resetValidationErrors();

//    $.ajax({
//        async: true,
//        type: "POST",
//        url: $('#CreateData').data('cust-add-url'),
//        data: addRequestVerificationToken({
//            CustomerCode: $("#CustomerCode").val().toUpperCase(),
//            CustomerName: $("#CustomerName").val(),
//            AddressL1: $("#AddressL1").val(),
//            AddressL2: $("#AddressL2").val(),
//            AddressL3: $("#AddressL3").val(),
//            AddressL4: $("#AddressL4").val(),
//            Telephone: $("#Telephone").val(),
//            Fax: $("#Fax").val(),
//            CustomerEmail: $("#CustomerEmail").val(),
//            CustomerContact: $("#CustomerContact").val(),
//            CreditTerm: $("#CreditTerm").val(),
//            PriceLevel: $("#PriceLevel").val(),
//            CustomerTaxId: $("#CustomerTaxId").val(),
//            Remark: $("#Remark").val(),
//            CompanyCode: $("#CompanyCode").val(),
//            Is_Active: $('#Is_Active').is(':checked')
//        }),
//        success: function (response) {

//            if (response.success) {

//                $('#newCustModal').modal('hide');
//                $('#newCustContainer').html("");

//                $("#tblCust").DataTable().ajax.reload(null, false);
//                $("#tblCust").DataTable().page('last').draw('page');

//                global.successAlert(response.message);
//            }
//            else {

//                if (response.errors != null) {
//                    displayValidationErrors(response.errors);
//                } else {
//                    global.dangerAlert(response.message, 5000);
//                }
//            }

//        },
//        error: function () {
//            global.dangerAlert("error", 5000);
//        }
//    });

//}

//function displayValidationErrors(errors) {

//    $.each(errors, function (idx, errorMessage) {
//        var res = errorMessage.split("|");
//        $("#" + res[0] + "_validationMessage").append('<li>' + res[1] + '</li>');
//    });
//}

//function resetValidationErrors() {

//    var listItems = document.querySelectorAll('.text-danger li');
//    for (let i = 0; i < listItems.length; i++) {
//        if (listItems[i].textContent != null)
//            listItems[i].remove();
//    };

//}