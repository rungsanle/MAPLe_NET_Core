$(function () {
    $('#MachineProdTypeName').inputpicker({
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
        fieldValue: 'Id',
        //responsive: true
    });

    $('#MachineProdTypeName').val($("#MachineProdType").val());

    global.applyBSwitchStyle($("#Is_Active").prop("id"), $('#Is_Active').is(':checked'), true, "small", "Yes", "No");
});