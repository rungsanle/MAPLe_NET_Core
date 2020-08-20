$(function () {

    $('input').attr('autocomplete', 'off');

    var arrRemoveDetails = new Array();

    global.applyDatepicker($("#ArrivalDate").prop("id"), false);

    global.applyDatepicker($("#DocRefDate").prop("id"), false);

    global.applyInputPicker(
        $("#ArrivalTypeName").prop("id"),
        $('#EditData').data('arrtype-get-url'),
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

    global.applyInputPicker(
        $("#RawMatTypeName").prop("id"),
        $('#EditData').data('rawmattype-get-url'),
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

        return false;
    });

    $("#VendorCode").keyup(function (e) {
        if (e.which == 13) {
            // Enter key pressed

            var api = $('#EditData').data('vend-get-url');

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
                    "url": $('#EditData').data('vend-get-url'),
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

            var api = $('#EditData').data('mat-get-url'); // + "/" + $(this).val();

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
                    "url": $('#EditData').data('mat-get-url'),
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

    /*-------------- BEGIN ARRIVAL DETAIL --------------*/

    arrDtlVM = {
        dtArrDtl: null,
        init: function () {
            var url = $('#EditData').data('arrdtl-get-url');  // + "/xxx";

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
                    { "data": "Id", "autoWidth": false },
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
                    { "data": "MaterialName", "autoWidth": false },
                    { "data": "MaterialDesc", "autoWidth": false },
                    //{ "data": "OrderQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    {
                        "data": "OrderQty", "autoWidth": false,
                        "render": function (data, type, detail, meta) {
                            //return "<input type='text' value='" + ditem + "'/>"; ditem
                            if (detail.GenLabelStatus != 'C') {
                                return "<input class='form-control input-sm text-align-right text-box single-line' style='width:85px' id='OrderQty' min='0' type='number' value='" + detail.OrderQty + "' autocomplete='off' />";
                            } else {
                                return detail.OrderQty;
                            }
                        }
                    },
                    { "data": "RecvQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 4, '') },
                    { "data": "LotNo", "autoWidth": false },
                    {
                        "data": "LotDate", "autoWidth": false, "type": "date", "render": function (value) {

                            return global.localDate(value);

                            //if (value === null) return "";
                            //var pattern = /Date\(([^)]+)\)/;
                            //var results = pattern.exec(value);

                            //if (results) {
                            //    if (results.length == 2) {

                            //        var dt = new Date(parseFloat(results[1]));
                            //        return (("0" + dt.getDate()).slice(-2) + "-" + ("0" + (dt.getMonth() + 1)).slice(-2) + "-" + dt.getFullYear());
                            //    }
                            //    else {
                            //        return value;
                            //    }
                            //} else {
                            //    return value;
                            //}

                            //var dt = new Date(parseFloat(results[1]));
                            //return (("0" + dt.getDate()).slice(-2) + "-" + ("0" + (dt.getMonth() + 1)).slice(-2) + "-" + dt.getFullYear());
                        }
                    },
                    { "data": "DetailRemark", "autoWidth": false },
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
                    { "width": "0%", "targets": 0, "visible": false },  //Id
                    { "width": "0%", "targets": 1, "visible": false },  //ArrivalId
                    { "className": "dt-right", "width": "3%", "targets": 2 },  //LineNo
                    { "width": "0%", "targets": 3, "visible": false },  //PoLineNo
                    { "width": "0%", "targets": 4, "visible": false },  //MaterialId
                    { "className": "dt-bold-left", "width": "15%", "targets": 5 },  //MaterialCode
                    { "width": "21%", "targets": 6 },   //MaterialName
                    { "width": "0%", "targets": 7, "visible": false },  //MaterialDesc
                    { "className": "dt-bold-right", "width": "8%", "targets": 8 },  //OrderQty
                    { "className": "dt-right", "width": "8%", "targets": 9 },   //RecvQty
                    { "width": "11%", "targets": 10 },   //LotNo
                    { "className": "dt-center", "width": "8%", "targets": 11 }, //LotDate
                    { "width": "12%", "targets": 12 },  //DetailRemark
                    { "className": "dt-right", "width": "4%", "targets": 13 },  //NoOfLabel
                    { "className": "dt-center", "width": "6%", "targets": 14 }, //GenLabelStatus
                    { "width": "0%", "targets": 15, "visible": false },  //CompanyCode
                    { "width": "0%", "targets": 16, "visible": false },  //RecordFlag
                    { "width": "4%", "targets": 17 }    //Action
                ],
                scrollY: '295px',
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
                responsive: true
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

            if (rflag == 1 || rflag == 3) {

                //alert(global.ConvertLocalDate(ItemData["LotDate"]));

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
                arrRemoveDtl.LotDate = global.localDate(ItemData["LotDate"]); //global.convertNETDateTime(ItemData["LotDate"]);
                arrRemoveDtl.DetailRemark = ItemData["DetailRemark"];
                arrRemoveDtl.GenLabelStatus = ItemData["GenLabelStatus"];
                arrRemoveDtl.NoOfLabel = ItemData["NoOfLabel"];
                arrRemoveDtl.CompanyCode = ItemData["CompanyCode"];
                arrRemoveDtl.RecordFlag = 0; //Delete Record
                arrRemoveDtl.Is_Active = true;

                arrRemoveDetails.push(arrRemoveDtl);
            }

            dtArrDtl.row(row).remove().draw(false);

            dtArrDtl.column(2, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);
        }
        else {

        }
    });

    $("#tblArrivalDtl").on("keyup", "input[type='number']", function (e) {

        var rData = dtArrDtl.rows($(this).closest("tr")).data()[0];
        rData.OrderQty = $(this).val();

        if (rData.RecordFlag == 1) {
            rData.RecordFlag = 3; //Edit Qty
        }

    });

    /*-------------- END ARRIVAL DETAIL --------------*/

    global.applyDatepicker($("#LotDate").prop("id"), true);

    $("#btnAddArrDtl").on("click", function (event) {

        event.preventDefault();

        var itemCode = $('#ItemCode').val();
        var poLine = 0;

        var noExist = CheckDuplicateArrDtl(itemCode, poLine);

        if (noExist) {

            dtArrDtl.row.add({
                Id: 0,
                ArrivalId: $('#Id').val(),
                LineNo: dtArrDtl.data().length + 1,
                PoLineNo: 0,
                MaterialId: $('#ItemId').val(),
                MaterialCode: itemCode,
                MaterialName: $('#ItemName').val(),
                MaterialDesc: '',
                OrderQty: $('#ItemQty').val(),
                RecvQty: 0,
                LotNo: $('#LotNo').val(),
                LotDate: global.convertJsonDate($('#LotDate').val()),
                DetailRemark: $('#ItemRemark').val(),
                NoOfLabel: 0,
                GenLabelStatus: 'N',
                CompanyCode: $("#CompanyCode").val(),
                RecordFlag: 2 //new record
            }).draw(false);

            dtArrDtl.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            }).draw(false);

            $('.dataTables_scrollBody').scrollTop($('.dataTables_scrollBody')[0].scrollHeight);
        }

        ClearPanelItemDetail();

    });

    $("#btnSaveEdit").on("click", function (event) {

        event.preventDefault();

        var apiView = $('#EditData').data('arrival-get-url') + '/' + $("#Id").val();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('arrival-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                ArrivalNo: $("#ArrivalNo").val().toUpperCase(),
                ArrivalDate: global.localDate($("#ArrivalDate").val()),
                ArrivalTypeId: $("#ArrivalTypeName").val(),
                RawMatTypeId: $("#RawMatTypeName").val(),
                VendorId: $("#VendorId").val(),
                PurchaseOrderNo: $("#PurchaseOrderNo").val(),
                DocRefNo: $("#DocRefNo").val(),
                DocRefDate: global.localDate($("#DocRefDate").val()),
                ArrivalRemark: $("#ArrivalRemark").val(),
                CompanyCode: $("#CompanyCode").val(),
                Is_Active: $("#Is_Active").is(':checked'),
                ArrivalDetails: GetArrivalItemDetails(arrRemoveDetails)
            }),
            success: function (response) {

                if (response.success) {


                    $('#editArrivalModal').modal('hide');
                    $('#editArrivalContainer').html("");

                    $("#tblArrival").DataTable().ajax.reload(null, false);
                    //$("#tblArrival").DataTable().page('last').draw('page');

                    

                    setTimeout(function () {
                        
                        $.ajax({
                            type: "GET",
                            url: apiView, 
                            async: true,
                            success: function (data) {
                                if (data) {
                                    $('#viewArrivalContainer').html(data);
                                    $('#viewArrivalModal').modal('show');
                                } else {
                                    global.authenExpire();
                                }
                            }, error: function (xhr) {
                                alert('View Error : ' + xhr);

                            }
                        });
                        
                    }, 500);

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

    });

});


function addRequestVerificationToken(data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

function ClearPanelItemDetail() {
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
    var lineIndex = 0;
    var jsonData = JSON.parse(JSON.stringify($('#tblArrivalDtl').dataTable().fnGetData()));

    for (var obj in jsonData) {

        if (jsonData.hasOwnProperty(obj)) {

            var arrDtl = {};

            arrDtl.Id = jsonData[obj]["Id"];
            arrDtl.ArrivalId = jsonData[obj]["ArrivalId"];
            if (jsonData[obj]["RecordFlag"] > 0) {
                lineIndex++;
                arrDtl.LineNo = lineIndex;
            } else {
                alert(jsonData[obj]["LineNo"]);
                arrDtl.LineNo = jsonData[obj]["LineNo"];
            }
            
            arrDtl.PoLineNo = jsonData[obj]["PoLineNo"];
            arrDtl.MaterialId = jsonData[obj]["MaterialId"];
            arrDtl.MaterialCode = jsonData[obj]["MaterialCode"];
            arrDtl.MaterialName = jsonData[obj]["MaterialName"];
            arrDtl.MaterialDesc = jsonData[obj]["MaterialDesc"];
            arrDtl.OrderQty = jsonData[obj]["OrderQty"];
            arrDtl.RecvQty = jsonData[obj]["RecvQty"];
            arrDtl.LotNo = jsonData[obj]["LotNo"];
            arrDtl.LotDate = global.localDate(jsonData[obj]["LotDate"]); 
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
