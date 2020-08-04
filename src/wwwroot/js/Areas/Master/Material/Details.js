$(function () {

    /*-------------- BEGIN RAW MATERIAL TYPE --------------*/
    $('#RawMatType').inputpicker({
        url: $('#DetailsData').data('rawmattype-get-url'),
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
        url: $('#DetailsData').data('loc-get-url'),
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

    

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

    var num = Math.random();
    var imgSrc = $('#imageMaterial').attr("src") + "?v=" + num;
    $('#imageMaterial').attr("src", imgSrc);

});