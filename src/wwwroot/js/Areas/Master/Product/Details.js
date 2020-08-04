$(function () {

    
    /*-------------- BEGIN PRODUCTION TYPE --------------*/
    $('#ProductionType').inputpicker({
        url: $('#DetailsData').data('prodtype-get-url'),
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
        url: $('#DetailsData').data('unit-get-url'),
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
        url: $('#DetailsData').data('wh-get-url'),
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
        url: $('#DetailsData').data('loc-get-url'),
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

    /*-- BEGIN TABLE PRODUCT PROCESS --*/

    prodProcessVM = {
        dtProdprocess: null,
        init: function () {
            var url = $('#DetailsData').data('proc-get-url') + "/" + $('#Id').val();
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
                                    return '<img src="' + $('#DetailsData').data('image-url') + '/checkbox/checkbox_checked.gif" class="gridCheckbox" />';
                                } else {
                                    return '<img src="' + $('#DetailsData').data('image-url') + '/checkbox/checkbox_uncheck.gif" class="gridCheckbox" />';
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

    /*-- END TABLE PRODUCT PROCESS --*/

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

    var num = Math.random();
    var imgSrc = $('#imageProduct').attr("src") + "?v=" + num;
    $('#imageProduct').attr("src", imgSrc);

});