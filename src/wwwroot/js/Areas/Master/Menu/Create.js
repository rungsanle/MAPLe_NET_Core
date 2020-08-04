$(function () {

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $('#parentName').inputpicker({
        url: $('#CreateData').data('menu-parent-url'),
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

    //global.applyIcheckStyle();
    global.applyBSwitchStyle($("#status").prop("id"), true, false, "small", "Yes", "No");
    global.applyBSwitchStyle($("#isParent").prop("id"), false, false, "small", "Yes", "No");

    global.applyIsActiveSwitch(true, false);

    $("#btnSaveCreate").on("click", SaveCrate);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function SaveCrate(event) {

        event.preventDefault();

        global.resetValidationErrors();

        $.ajax({
            async: true,
            type: "POST",
            url: $('#CreateData').data('menu-add-url'),
            data: addRequestVerificationToken({
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

                    $('#newMenuModal').modal('hide');
                    $('#newMenuContainer').html("");

                    $("#tblMenu").DataTable().ajax.reload(null, false);
                    $("#tblMenu").DataTable().page('last').draw('page');

                    toastr.success(response.message, 'Create Menu', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                }
                else {

                    if (response.errors != null) {
                        global.displayValidationErrors(response.errors);
                    } else {
                        toastr.error(response.message, 'Create Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    };

});


