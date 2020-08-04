$(function () {

    $('#DeptName').inputpicker({
        url: $('#DetailsData').data('dept-get-url'),
        fields: [
            { name: 'DeptCode', text: 'CODE', width: '30%' },
            { name: 'DeptName', text: 'NAME', width: '70%' }
        ],
        width: '350px',
        autoOpen: true,
        selectMode: 'empty',
        headShow: true,
        fieldText: 'DeptName',
        fieldValue: 'Id'
    });

    $('#DeptName').val($("#DeptId").val());

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), true);

    //for clear cache image
    var num = Math.random();
    var imgSrc = $('#imageUser').attr("src") + "?v=" + num;
    $('#imageUser').attr("src", imgSrc);

});