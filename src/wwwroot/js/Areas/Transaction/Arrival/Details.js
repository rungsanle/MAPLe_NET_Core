$(function () {

    $('input').attr('autocomplete', 'off');

    

    var arrRemoveSubDetails = new Array();

    global.applyDatepicker($("#ArrivalDate").prop("id"), false);
    $("#ArrivalDate").prop('disabled', true);

    global.applyDatepicker($("#DocRefDate").prop("id"), false);
    $("#DocRefDate").prop('disabled', true);

    global.applyInputPicker(
        $("#ArrivalTypeName").prop("id"),
        $('#DetailsData').data('arrtype-get-url'),
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

    $('#ArrivalTypeName').val($("#ArrivalTypeId").val());

    global.applyInputPicker(
        $("#RawMatTypeName").prop("id"),
        $('#DetailsData').data('rawmattype-get-url'),
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

    $('#RawMatTypeName').val($("#RawMatTypeId").val());

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

        return false;
    });

    $("#VendorCode").keyup(function (e) {
        if (e.which == 13) {
            // Enter key pressed

            var api = $('#DetailsData').data('vend-get-url');

            $.ajax({
                type: "GET",
                url: api,
                async: true,
                dataType: 'json',
                contentType: "application/json",
                data: { vcode: $(this).val() },
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
                        alert(response.message);
                    }

                }, error: function (xhr) {
                    alert('Get Data Error : ' + xhr);

                }
            });
        }
    });

    vendorVM = {
        dtVendor: null,
        init: function () {

            dtVendor = $('#tblVendor').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": $('#DetailsData').data('vend-get-url'),
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

    $("#btnSearchMaterial").on("click", function (event) {

        event.preventDefault();
        $('#searchMaterialModal').modal('show');
        matVM.init();

    });

    $("#searchMaterialModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        matVM.tbdestroy();


        if ($('#ItemCode').val() === '') {
            $('#ItemCode').focus();
        }

        return false;
    });

    $("#ItemCode").keyup(function (e) {
        if (e.which == 13) {
            // Enter key pressed
            if ($(this).val() === '') {
                $('#ItemCode').focus();
                return;
            }

            var api = $('#DetailsData').data('mat-get-url'); // + "/" + $(this).val();

            $.ajax({
                type: "GET",
                url: api,
                async: true,
                dataType: 'json',
                contentType: "application/json",
                data: {
                    vcode: $(this).val(),
                    rawMatTypeId: $("#RawMatTypeName").val(),
                    companyCode: $("#CompanyCode").val()
                },
                success: function (response) {

                    if (response.success) {

                        $('#ItemId').val(response.data.Id);
                        $('#ItemName').val(response.data.MaterialName);

                        $('#ItemQty').focus();

                    } else {
                        //alert(response.message);
                        $("#btnSearchMaterial").click();
                    }

                }, error: function (xhr) {
                    alert('Get Data Error : ' + xhr);

                }
            });
        }
    });

    matVM = {
        dtMaterial: null,
        init: function () {

            dtMaterial = $('#tblMaterial').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": $('#DetailsData').data('mat-get-url'),
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

    //Select from Vendor LOV
    $('#tblMaterial').on("click", "#selectMaterial", function (event) {

        event.preventDefault();

        var rowSelect = $(this).parents('tr')[0];
        var itemData = (dtMaterial.row(rowSelect).data());

        $('#searchMaterialModal').modal('hide');

        $('#ItemId').val(itemData["Id"]);
        $('#ItemCode').val(itemData["MaterialCode"]);
        $('#ItemName').val(itemData["MaterialName"]);

        $('#ItemQty').focus();

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
    
    global.applyDraggable();

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

    /*-------------- BEGIN ARRIVAL DETAIL --------------*/

    arrDtlVM = {
        dtArrDtl: null,
        tgIndex: 0,
        init: function () {

            tgIndex = 0;

            var url = $('#DetailsData').data('arrdtl-get-url');  // + "/xxx";

            dtArrDtl = $('#tblArrivalDtl').DataTable({
                processing: true,
                ajax: {
                    "url": url,
                    "type": "GET",
                    "datatype": "json",
                    data: {
                        arrId: $('#Id').val()
                    }
                },
                columns: [
                    { "data": "Id" },
                    { "data": "ArrivalId", "autoWidth": false },
                    //{
                    //    "data": "LineNo",
                    //    render: function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},
                    { "data": "LineNo", "autoWidth": false },
                    { "data": "PoLineNo", "autoWidth": false },
                    { "data": "MaterialId", "autoWidth": false },
                    { "data": "MaterialCode", "autoWidth": false },
                    { "data": "MaterialName", "autoWidth": false },
                    { "data": "MaterialDesc", "autoWidth": false },
                    { "data": "OrderQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    { "data": "RecvQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    { "data": "LotNo", "autoWidth": false },
                    {
                        "data": "LotDate", "autoWidth": false, "type": "date", "render": function (value) {

                            return global.localDate(value);
                        }
                    },
                    { "data": "DetailRemark", "autoWidth": false },
                    { "data": "NoOfLabel", "autoWidth": false },
                    { "data": "GenLabelStatus", "autoWidth": false },
                    { "data": "CompanyCode", "autoWidth": false },
                    { "data": "RecordFlag", "autoWidth": false },
                    { "data": "PackageStdQty", "autoWidth": false },
                    {
                        "render": function (data, type, item, meta) {
                            return '<a id="labelOption" class="btn btn-default btn-sm" data-toggle="tooltip" title="Label Option" href="Arrival/GetLabelOption/"><span class="glyphicon glyphicon-tags" aria-hidden="true"></span></a>&nbsp;';
                        }
                    }
                ],
                columnDefs: [
                    {
                        "targets": tgIndex++,
                        "searchable": false,
                        "orderable": false,
                        "width": "3%",
                        "className": "dt-center",
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox">';
                        }
                    }, //Checkbox
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //ArrivalId
                    { "className": "dt-right", "width": "3%", "targets": tgIndex++ },  //LineNo
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //PoLineNo
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //MaterialId
                    { "className": "dt-bold-left", "width": "15%", "targets": tgIndex++ },  //MaterialCode
                    { "width": "21%", "targets": tgIndex++ },   //MaterialName
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //MaterialDesc
                    { "className": "dt-bold-right", "width": "7%", "targets": tgIndex++ },  //OrderQty   
                    { "className": "dt-right", "width": "7%", "targets": tgIndex++ },   //RecvQty
                    { "width": "11%", "targets": tgIndex++ },   //LotNo
                    { "className": "dt-center", "width": "7%", "targets": tgIndex++ }, //LotDate
                    { "width": "12%", "targets": tgIndex++ },  //DetailRemark
                    { "className": "dt-right", "width": "4%", "targets": tgIndex++ },  //NoOfLabel
                    { "className": "dt-center", "width": "6%", "targets": tgIndex++ }, //GenLabelStatus
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //CompanyCode
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //RecordFlag
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //PackageStdQty
                    { "width": "4%", "targets": tgIndex++ }    //Action
                ],
                scrollY: '235px',
                scrollX: false,
                scrollCollapse: true,
                autoWidth: false,
                paging: false,
                searching: false,
                order: [],
                ordering: false,
                info: true,
                language: {
                    info: "Total _TOTAL_ records",
                    infoEmpty: "No records available"
                },
                lengthChange: false,
                responsive: true,
                select: {
                    style: "multi"
                },
                rowCallback: function (row, data, dataIndex) {
                    // Get row ID
                    var rowId = data[0];

                    // If row ID is in the list of selected row IDs
                    if ($.inArray(rowId, rows_selected) !== -1) {
                        $(row).find('input[type="checkbox"]').prop('checked', true);
                        $(row).addClass('selected');
                    }
                }
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

    setTimeout(function () {
        dtArrDtl.columns.adjust().draw();
    }, 200);

    function updateDataTableSelectAllCtrl(table) {
        var $table = table.table().node();
        var $chkbox_all = $('tbody input[type="checkbox"]', $table);
        var $chkbox_checked = $('tbody input[type="checkbox"]:checked', $table);
        var chkbox_select_all = $('thead input[id="select_all"]', $table).get(0);

        // If none of the checkboxes are checked
        if ($chkbox_checked.length === 0) {
            chkbox_select_all.checked = false;
            if ('indeterminate' in chkbox_select_all) {
                //chkbox_select_all.indeterminate = false;
                $("#select_all").prop("indeterminate", false);
            }
            $("#select_all").prop("checked", false);
            // If all of the checkboxes are checked
        } else if ($chkbox_checked.length === $chkbox_all.length) {
          
            chkbox_select_all.checked = true;
            if ('indeterminate' in chkbox_select_all) {
                //chkbox_select_all.indeterminate = false;
                $("#select_all").prop("indeterminate", false);
                
            }
            $("#select_all").prop("checked", true);
            // If some of the checkboxes are checked
        } else {
            //chkbox_select_all.checked = true;
            $("#select_all").prop("checked", true);
            if ('indeterminate' in chkbox_select_all) {
                //chkbox_select_all.indeterminate = true;
                $("#select_all").prop("indeterminate", true);
            }
        }
    }

    var rows_selected = [];

    // Handle click on checkbox
    $('#tblArrivalDtl tbody').on('click', 'input[type="checkbox"]', function (e) {
        var $row = $(this).closest('tr');

        // Get row data
        var data = dtArrDtl.row($row).data();

        // Get row ID
        var rowId = data[0];

        // Determine whether row ID is in the list of selected row IDs 
        var index = $.inArray(rowId, rows_selected);

        // If checkbox is checked and row ID is not in list of selected row IDs
        if (this.checked && index === -1) {
            rows_selected.push(rowId);

            // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
        } else if (!this.checked && index !== -1) {
            rows_selected.splice(index, 1);
        }

        if (this.checked) {
            $row.addClass('selected');
        } else {
            
            $row.removeClass('selected');
        }

        // Update state of "Select all" control
        updateDataTableSelectAllCtrl(dtArrDtl);

        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

    // Handle click on table cells with checkboxes
    //$('#tblArrivalDtl').on('click', 'tbody td, thead th:first-child', function (e) {
    //    $(this).parent().find('input[type="checkbox"]').trigger('click');
    //});

    // Handle click on "Select all" control
    $('thead input[id="select_all"]', dtArrDtl.table().container()).on('click', function (e) {

        if (this.checked) {
            $('#tblArrivalDtl tbody input[type="checkbox"]:not(:checked)').trigger('click');
        } else {
            $('#tblArrivalDtl tbody input[type="checkbox"]:checked').trigger('click');
        }

        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

    // Handle table draw event
    $('#tblArrivalDtl').on('draw', function () {
        // Update state of "Select all" control
        updateDataTableSelectAllCtrl(dtArrDtl);
    });


    //Label Option Open
    var arrdtlsubapi;

    var rArrId;
    var rLineNo;
    var rMaterialId;
    var rCompanyCode;

    var subLabelQty = 0;
    var subTotalQty = 0;

    $('#tblArrivalDtl').on('click', '#labelOption', function (event) {

        event.preventDefault();
        //Clear all Checkbox
        $('#tblArrivalDtl tbody input[type="checkbox"]:checked').trigger('click');

        arrdtlsubapi = $(this).attr("href");
        var row = $(this).parents('tr')[0];
        var arrDtlData = (dtArrDtl.row(row).data());

        rArrId = arrDtlData["ArrivalId"];
        rLineNo = arrDtlData["LineNo"];
        rMaterialId = arrDtlData["MaterialId"];
        rCompanyCode = arrDtlData["CompanyCode"];

        $('#ItemCode').val(arrDtlData["MaterialCode"]);
        $('#ItemName').val(arrDtlData["MaterialName"]);
        $('#PackageStdQty').val(global.numberWithCommas(arrDtlData["PackageStdQty"].toFixed(2)));
        $('#TotalQty').val(global.numberWithCommas(arrDtlData["OrderQty"].toFixed(4)));

        $('#viewLbOptionModal').modal('show');

        arrDtlSubVM.init();
        //$.ajax({
        //    type: "GET",
        //    url: api,
        //    async: true,
        //    dataType: 'json',
        //    contentType: "application/json",
        //    data: { arrId: rArrId, lineNo: rLineNo },
        //    success: function (data) {
        //        if (data) {


        //        } else {
        //            global.authenExpire();
        //        }
        //    }, error: function (xhr) {
        //        alert('View Error : ' + xhr);

        //    }
        //});


    });

    arrDtlSubVM = {
        dtArrDtlSub: null,
        tgIndex: 0,
        init: function () {
            tgIndex = 0;
            dtArrDtlSub = $('#tblArrDtlSub').DataTable({
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    "url": arrdtlsubapi,
                    "type": "GET",
                    "datatype": "json",
                    data: {
                        arrId: rArrId,
                        lineNo: rLineNo
                    }
                },
                columns: [
                    { "data": "Id", "autoWidth": false },
                    { "data": "ArrivalId", "autoWidth": false },
                    { "data": "DtlLineNo", "autoWidth": false },
                    { "data": "MaterialId", "autoWidth": false },
                    {
                        "data": "SubLineNo",
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    //{ "data": "LabelQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    {
                        "data": "LabelQty", "autoWidth": false,
                        "render": function (ditem) {
                            //alert(global.numberWithCommas(ditem.toFixed(4)));
                            return "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='LabelQty' min='0' type='number' value='" + ditem + "' autocomplete='off' />";
                        }
                    },
                    {
                        "render": function (data, type, vendor, meta) {
                            return 'X';
                        }
                    },
                    //{ "data": "NoOfLabel", "autoWidth": false },
                    {
                        "data": "NoOfLabel", "autoWidth": false,
                        "render": function (ditem) {
                            return "<input class='form-control input-sm text-align-right text-box single-line' style='width:100%' id='NoOfLabel' min='0' type='number' value='" + ditem + "' autocomplete='off' />";
                        }
                    },
                    { "data": "TotalQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    //{ "data": "SubDetail", "autoWidth": false },
                    {
                        "data": "SubDetail", "autoWidth": false,
                        "render": function (ditem) {
                            return "<input class='form-control input-sm text-box single-line' style='width:100%' id='SubDetail' type='text' value='" + (ditem == null ? '' : ditem) + "' autocomplete='off' />";
                        }
                    },
                    {
                        "render": function (data, type, vendor, meta) {
                            return '<a id="delItem" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    },
                    { "data": "CompanyCode", "autoWidth": false },
                    { "data": "Is_Active", "autoWidth": false },
                    { "data": "RecordFlag", "autoWidth": false }
                ],
                columnDefs: [
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //Id
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //ArrivalId
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //DtlLineNo
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //MaterialId
                    { "className": "dt-right", "width": "3%", "targets": tgIndex++ },  //SubLineNo
                    {
                        "className": "dt-right", "width": "19%", "targets": tgIndex++,
                        "createdCell": function (td, cellData, rowData, row, col) {
                            $(td).css('padding', '0px 3px 0px 3px')
                        }
                    },   //Label Qty
                    { "className": "dt-center", "width": "4%", "targets": tgIndex++ },   //X
                    {
                        "className": "dt-right", "width": "15%", "targets": tgIndex++,
                        "createdCell": function (td, cellData, rowData, row, col) {
                            $(td).css('padding', '0px 3px 0px 3px')
                        }
                    },   //Number Of Label
                    { "className": "dt-right", "width": "20%", "targets": tgIndex++ },   //Total Qty
                    {
                        "width": "30%", "targets": tgIndex++,
                        "createdCell": function (td, cellData, rowData, row, col) {
                            $(td).css('padding', '0px 3px 0px 3px')
                        }
                    },   //Sub Detail
                    {
                        "width": "9%", "targets": (tgIndex++).toString(), "orderable": false, 
                        "createdCell": function (td, cellData, rowData, row, col) {
                            $(td).css('padding', '0px 3px 0px 3px')
                        }
                    },    //Action 
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //CompanyCode
                    { "width": "0%", "targets": tgIndex++, "visible": false },  //Is_Active
                    { "width": "0%", "targets": tgIndex++, "visible": false }  //RecordFlag
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };

                    // Total over all pages
                    //total = api
                    //    .column(3)
                    //    .data()
                    //    .reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    });

                    // Total over this page
                    subLabelQty = api
                        .column(7, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0)

                    // Update footer
                    $(api.column(7).footer()).html(
                        global.numberWithCommas(subLabelQty)
                    );

                    subTotalQty = api
                        .column(8, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(8).footer()).html(
                        global.numberWithCommas(subTotalQty.toFixed(4))
                    );
                },
                processing: true,
                autoWidth: false,
                paging: false,
                searching: false,
                ordering: false,
                info: false,
                lengthChange: false,
                responsive: true
            });
        },

        tbdestroy: function () {
            dtArrDtlSub.destroy();
        },

        refresh: function () {
            dtArrDtlSub.ajax.reload();
        }
    }

    $("#tblArrDtlSub").on("keyup", "input[id='LabelQty']", function (e) {

        event.preventDefault();

        var dtapi = $("#tblArrDtlSub").DataTable();
        var dtRow = dtapi.rows($(this).closest("tr"));

        dtRow.data()[0].LabelQty = $(this).val();

        if (dtRow.data()[0].RecordFlag == 1) {
            dtRow.data()[0].RecordFlag = 3; //Edit Qty
        }

        var resultTotal = ($(this).val() * dtRow.data()[0].NoOfLabel);

        dtArrDtlSub.cell(dtRow, 8).data(resultTotal).draw();
        $(this).focus();


        //var rData = dtArrDtlSub.rows($(this).closest("tr")).data()[0];
        //var resultTotal = ($(this).val() * rData.NoOfLabel);

        //rData.TotalQty = resultTotal;

        //dtArrDtlSub.cell(rData, 4).data(resultTotal).draw();
        //$(this).focus();


    });

    $("#tblArrDtlSub").on("keyup", "input[id='NoOfLabel']", function (e) {

        event.preventDefault();

        var dtapi = $("#tblArrDtlSub").DataTable();
        var dtRow = dtapi.rows($(this).closest("tr"));
        dtRow.data()[0].NoOfLabel = $(this).val();

        if (dtRow.data()[0].RecordFlag == 1) {
            dtRow.data()[0].RecordFlag = 3; //Edit Qty
        }

        var resultTotal = (dtRow.data()[0].LabelQty * $(this).val());

        dtArrDtlSub.cell(dtRow, 8).data(resultTotal).draw();
        $(this).focus();


        //var rData = dtArrDtlSub.rows($(this).closest("tr")).data()[0];
        //var resultTotal = ($(this).val() * rData.NoOfLabel);

        //rData.TotalQty = resultTotal;

        //dtArrDtlSub.cell(rData, 4).data(resultTotal).draw();
        //$(this).focus();


    });

    $("#tblArrDtlSub").on("keyup", "input[id='SubDetail']", function (e) {

        event.preventDefault();

        var dtapi = $("#tblArrDtlSub").DataTable();
        var dtRow = dtapi.rows($(this).closest("tr"));
        dtRow.data()[0].SubDetail = $(this).val();

        if (dtRow.data()[0].RecordFlag == 1) {
            dtRow.data()[0].RecordFlag = 3; //Edit Qty
        }
        $(this).focus();

    });

    $('#tblArrDtlSub').on("click", "#btnAddNewLine", function (event) {

        event.preventDefault();

        var balanceQty = (parseFloat($('#TotalQty').val().replace(/,/g, '')) - subTotalQty).toFixed(4);

        if (balanceQty > 0) {
            dtArrDtlSub.row.add({
                Id: 0,
                ArrivalId: rArrId,
                DtlLineNo: rLineNo,
                MaterialId: rMaterialId,
                SubLineNo: 0,
                LabelQty: eval(balanceQty),
                NoOfLabel: 1,
                TotalQty: balanceQty,
                SubDetail: '',
                CompanyCode: rCompanyCode,
                Is_Active: true,
                RecordFlag: 2  //new record
            }).draw(false);
        } else {
            alert('Cannot add new line!!');
        }
    });

    $('#tblArrDtlSub').on("click", "#delItem", function (event) {

        event.preventDefault();

        var row = $(this).parents('tr')[0];
        var ItemData = (dtArrDtlSub.row(row).data());
        var rflag = ItemData["RecordFlag"];
        //var itemCode = ItemData["MaterialCode"];
        var con = confirm("Are you sure you want to delete this Line?");
        if (con) {

            if (rflag == 1 || rflag == 3) {

                //alert(global.ConvertLocalDate(ItemData["LotDate"]));

                var arrRemoveSubDtl = {};

                arrRemoveSubDtl.Id = ItemData["Id"];
                arrRemoveSubDtl.ArrivalId = ItemData["ArrivalId"];
                arrRemoveSubDtl.DtlLineNo = ItemData["DtlLineNo"];
                arrRemoveSubDtl.MaterialId = ItemData["MaterialId"];
                arrRemoveSubDtl.SubLineNo = ItemData["SubLineNo"];
                arrRemoveSubDtl.LabelQty = ItemData["LabelQty"];
                arrRemoveSubDtl.NoOfLabel = ItemData["NoOfLabel"];
                arrRemoveSubDtl.TotalQty = ItemData["TotalQty"];
                arrRemoveSubDtl.SubDetail = ItemData["SubDetail"];
                arrRemoveSubDtl.CompanyCode = ItemData["CompanyCode"];
                arrRemoveSubDtl.Is_Active = true;
                arrRemoveSubDtl.RecordFlag = 0; //Delete Record
                
                arrRemoveSubDetails.push(arrRemoveSubDtl);
            }

            dtArrDtlSub.row(row).remove().draw(false);

            dtArrDtlSub.column(4, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);
        }
        else {

        }
        
    });

    /*-------------- END ARRIVAL DETAIL --------------*/

    global.applyDatepicker($("#LotDate").prop("id"), true);


    $("#viewLbOptionModal").on("hidden.bs.modal", function (event) {

        event.preventDefault();

        //Destroy Datatable
        arrDtlSubVM.tbdestroy();

        return false;
    });

    $("#btnSaveLabelOption").on("click", function (event) {

        event.preventDefault();

        if (parseFloat($('#TotalQty').val().replace(/,/g, '')) == subTotalQty) {

            $.ajax({
                async: true,
                type: "POST",
                url: $('#DetailsData').data('arrdtlsub-update-url'),
                data: addRequestVerificationToken({
                    lstArrDtlSub: GetArrivalDetailSub(arrRemoveSubDetails)
                }),
                success: function (response) {

                    arrRemoveSubDetails = [];

                    if (response.success) {

                        $('#viewLbOptionModal').modal('hide');
                        global.successAlert(response.message);
                    }
                    else {
                        if (response.errors != null) {
                            displayValidationErrors(response.errors);
                        } else {
                            global.dangerAlert(response.message, 5000);
                        }
                    }

                },
                error: function () {
                    //alert("error");
                    global.dangerAlert("error", 5000);
                }
            });

        } else {
            alert('Not Equal');
        }
    });

    $("#btnCloseLbOptionModel").on("click", function (event) {
        event.preventDefault();

        arrRemoveSubDetails = [];

        $('#viewLbOptionModal').modal('hide');
    });

    $("#btnCancelLbOptionModel").on("click", function (event) {
        event.preventDefault();

        arrRemoveSubDetails = [];

        $('#viewLbOptionModal').modal('hide');
    });

    $("a[name='lnkGenerateLabel']").on("click", function (event) {
        event.preventDefault();
        //alert('Generate Label');
        //var rows = dtArrDtl.rows({ selected: true }).data();
        data = dtArrDtl.rows('.selected').data();

        console.log('Total Selected : ' + data.length);

        if (data.length > 0) {

            var arrDetails = new Array();

            data.each(function (value, index) {
                console.log('Data in index: ' + index + ' is: ' + value.Id);

                var arrDtl = {};

                arrDtl.Id = value.Id;
                arrDtl.ArrivalId = value.ArrivalId;
                arrDtl.LineNo = value.LineNo;
                arrDtl.PoLineNo = value.PoLineNo;
                arrDtl.MaterialId = value.MaterialId;
                arrDtl.MaterialCode = value.MaterialCode;
                arrDtl.MaterialName = value.MaterialName;
                arrDtl.MaterialDesc = value.MaterialDesc;
                arrDtl.OrderQty = value.OrderQty;
                arrDtl.RecvQty = value.RecvQty;
                arrDtl.LotNo = value.LotNo;
                arrDtl.LotDate = value.LotDate;
                arrDtl.DetailRemark = value.DetailRemark;
                arrDtl.GenLabelStatus = value.GenLabelStatus;
                arrDtl.NoOfLabel = value.NoOfLabel;
                arrDtl.CompanyCode = value.CompanyCode;
                arrDtl.RecordFlag = value.RecordFlag;
                arrDtl.PackageStdQty = value.PackageStdQty;
                arrDtl.Is_Active = value.Is_Active;

                arrDetails.push(arrDtl);
            });

            $.ajax({
                async: true,
                type: "POST",
                url: $('#DetailsData').data('arrdtl-genlabel-url'),
                data: addRequestVerificationToken({
                    lstArrDtl: arrDetails
                }),
                success: function (response) {

                    if (response.success) {

                        //Clear all Checkbox
                        $('#tblArrivalDtl tbody input[type="checkbox"]:checked').trigger('click');

                        global.successAlert(response.message);
                    }
                    else {
                        if (response.errors != null) {
                            displayValidationErrors(response.errors);
                        } else {
                            global.dangerAlert(response.message, 5000);
                        }
                    }

                },
                error: function () {
                    global.dangerAlert("error", 5000);
                }
            });

        } else {
            alert('Please select data to Generate!!');
        }
        
    });
});

function addRequestVerificationToken(data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function GetArrivalDetailSub(arrDetailSubs) {

    if (!arrDetailSubs) {

        arrDetailSubs = new Array();
    }
    //var arrDetails = new Array();
    var jsonData = JSON.parse(JSON.stringify($('#tblArrDtlSub').dataTable().fnGetData()));

    for (var obj in jsonData) {

        if (jsonData.hasOwnProperty(obj)) {

            var arrDtlSub = {};

            arrDtlSub.Id = jsonData[obj]["Id"];
            arrDtlSub.ArrivalId = jsonData[obj]["ArrivalId"];
            arrDtlSub.DtlLineNo = jsonData[obj]["DtlLineNo"];
            arrDtlSub.MaterialId = jsonData[obj]["MaterialId"];
            arrDtlSub.SubLineNo = jsonData[obj]["SubLineNo"];
            arrDtlSub.LabelQty = jsonData[obj]["LabelQty"];
            arrDtlSub.NoOfLabel = jsonData[obj]["NoOfLabel"];
            arrDtlSub.TotalQty = jsonData[obj]["TotalQty"];
            arrDtlSub.SubDetail = jsonData[obj]["SubDetail"];
            arrDtlSub.CompanyCode = jsonData[obj]["CompanyCode"];
            arrDtlSub.Is_Active = true;
            arrDtlSub.RecordFlag = jsonData[obj]["RecordFlag"];

            arrDetailSubs.push(arrDtlSub);
        }
    }

    return arrDetailSubs;
}
