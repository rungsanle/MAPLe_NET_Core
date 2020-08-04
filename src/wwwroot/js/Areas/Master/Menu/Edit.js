$(function () {
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $('#parentName').inputpicker({
        url: $('#EditData').data('menu-parent-url'),
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
    global.applyBSwitchStyle($("#status").prop("id"), $('#status').is(':checked'), false, "small", "Yes", "No");
    global.applyBSwitchStyle($("#isParent").prop("id"), $('#isParent').is(':checked'), false, "small", "Yes", "No");

    global.applyIsActiveSwitch($('#Is_Active').is(':checked'), false);

    $("#btnSaveEdit").on("click", SaveEdit);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveEdit(event) {

        event.preventDefault();

        //var info = $('#tblMenu').DataTable().page.info();
        $.ajax({
            async: true,
            type: "POST",
            url: $('#EditData').data('menu-edit-url'),
            data: addRequestVerificationToken({
                Id: $("#Id").val(),
                nameOption: $("#nameOption").val(),
                controller: $("#controller").val(),
                action: $("#action").val(),
                imageClass: $("#imageClass").val(),
                status: $('#status').is(':checked'),
                isParent: $('#isParent').is(':checked'),
                parentId: $("#parentName").val() || 0,
                area: $("#area").val(),
                menuseq: $("#menuseq").val(),
                Is_Active: $('#Is_Active').is(':checked')
            }),
            success: function (response) {

                if (response.success) {

                    $('#editMenuModal').modal('hide');
                    $('#editMenuContainer').html("");

                    $("#tblMenu").DataTable().ajax.reload(null, false);

                    toastr.success(response.message, 'Edit Menu', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {
                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Edit Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };
});

