$(function () {

    $('#WarehouseName').inputpicker({
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
    });

    $('#WarehouseName').val($("#WarehouseId").val());

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

});