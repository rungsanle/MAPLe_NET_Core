$(function () {

    $('#parentName').inputpicker({
        url: $('#DetailsData').data('menu-parent-url'),
        fields: [
            { name: 'nameOption', text: 'NAME', width: '100%' }
        ],
        selectMode: 'restore',
        headShow: false,
        fieldText: 'nameOption',
        fieldValue: 'Id',
        autoOpen: true,
        width: '200px',
    });

    $('#parentName').val($("#parentId").val());

    //global.applyIcheckStyle();
    global.applyBSwitchStyle($("#status").prop("id"), $('#status').is(':checked'), true, "small", "Yes", "No");


    global.applyBSwitchStyle($("#isParent").prop("id"), $('#isParent').is(':checked'), true, "small", "Yes", "No");

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

});