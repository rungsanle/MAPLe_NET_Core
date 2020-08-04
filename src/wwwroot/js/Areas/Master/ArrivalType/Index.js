$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    arrTypeVM = {
        dtArrivalType: null,
        init: function () {
            dtArrivalType = $('#tblArrivalType').DataTable({
                dom: "<'row'<'col-sm-4'B><'col-sm-2'l><'col-sm-6'f>>" +
                     "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-6'i><'col-sm-6'p>>",
                buttons: [
                    {
                        text: '<i class="fa fa-refresh">&nbsp;<p class="setfont">Refresh</p></i>',
                        titleAttr: 'Refresh',
                        action: function (e, dt, node, config) {
                            dt.ajax.reload(null, false);
                        }
                    },
                    {
                        extend: 'collection',
                        text: '<i class="fa fa-file-o">&nbsp;<p class="setfont">Export</p></i>',
                        titleAttr: 'Export Option',
                        autoClose: true,
                        buttons: [
                            {
                                extend: 'excelHtml5',
                                text: '<i class="fa fa-file-excel-o">&nbsp;<p class="setfont">Export XLS</p></i>',
                                title: 'Arrival Type Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Arrival Type Master',
                                titleAttr: 'CSV'
                            }
                        ]
                    }
                ],
                initComplete: function () {
                    var btns = $('.dt-button');
                    btns.addClass('btn btn-default btn-sm');
                    btns.removeClass('dt-button');
                },
                processing: true, // for show progress bar
                autoWidth: false,
                ajax: {
                    url: $('#IndexData').data('arrtype-get-url'),    //"/Customer/GetCustomers",
                    type: "GET",
                    async: true,
                    datatype: "json"
                },
                columns: [
                    { "data": "ArrivalTypeCode", "className": "boldColumn", "autoWidth": false },
                    { "data": "ArrivalTypeName", "autoWidth": false },
                    { "data": "ArrivalTypeDesc", "autoWidth": false },
                    { "data": "CompanyCode", "className": "boldColumn", "autoWidth": false },
                    {
                        "data": "Is_Active",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                //return '<input type="checkbox" disabled="disabled" class="chkIs_Active">';
                                if (data) {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_yes.png" />';;
                                } else {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_no.png" />';
                                }
                            }
                            return data;
                        }
                    },
                    {
                        "autoWidth": true,
                        "render": function (data, type, arrType, meta) {
                            return '<a id="viewArrivalType" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="ArrivalType/Details/' + arrType.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="editArrivalType" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="ArrivalType/Edit/' + arrType.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="delArrivalType" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="ArrivalType/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "15%", "targets": 0 },
                    { "width": "26%", "targets": 1 },
                    { "width": "29%", "targets": 2 },
                    { "width": "13%", "targets": 3 },
                    { "className": "dt-center", "width": "7%", "targets": 4, "orderable": false },
                    { "width": "10%", "targets": 5, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                stateSave: true,
                stateDuration: -1 //force the use of Session Storage
            });

            //dt.on('draw', function () {
            //    global.applyIcheckStyle();
            //});

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        refresh: function () {
            dtArrivalType.ajax.reload();
        }
    }

    // initialize the datatables
    arrTypeVM.init();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtArrivalType.page.info().page != 0) {
                dtArrivalType.page('first').draw('page');
            }
        }, 250);
    }

    //set width of input search.
    //$('.dataTables_filter input[type="search"]').css({ 'width': '350px' });

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add
    $("#btnCreateArrivalType").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newArrivalTypeContainer').html(data);
                    $('#newArrivalTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseText = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseText.Message, 'Create Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    $("#newArrivalTypeModal").on("shown.bs.modal", function () {
        //$('input[type=text]:visible:first').focus();
        $('#ArrivalTypeCode').focus();
    });
    //clear html data for create new;
    $("#newArrivalTypeModal").on("hidden.bs.modal", function () {
        $('#newArrivalTypeContainer').html("");
    });

    //View
    $('#tblArrivalType').on("click", "#viewArrivalType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewArrivalTypeContainer').html(data);
                    $('#viewArrivalTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for View;
    $("#viewArrivalTypeModal").on("hidden.bs.modal", function () {
        $('#viewArrivalTypeContainer').html("");
    });

    //Edit
    $('#tblArrivalType').on("click", "#editArrivalType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editArrivalTypeContainer').html(data);
                    $('#editArrivalTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    //clear html data for edit;
    $("#editArrivalTypeModal").on("hidden.bs.modal", function () {
        $('#editArrivalTypeContainer').html("");
    });

    //Delete
    $('#tblArrivalType').on('click', '#delArrivalType', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var arrTypeData = (dtArrivalType.row(rowSelect).data());
        var arrTypeId = arrTypeData["Id"];
        var arrTypeName = arrTypeData["ArrivalTypeName"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + arrTypeName + '\"',
            buttons: {
                confirm: {
                    text: 'Confirm',
                    btnClass: 'btn-confirm',
                    keys: ['shift', 'enter'],
                    action: function () {
                            $.ajax({
                                type: 'POST',
                                url: api,
                                async: true,
                                data: addRequestVerificationToken({ id: arrTypeId }),
                                success: function (response) {

                                    if (response.success) {

                                        //arrTypeVM.refresh();
                                        dtArrivalType.row(rowSelect).remove().draw(false);

                                        toastr.success(response.message, 'Delete Arrival Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout});
                                    }
                                    else {
                                        toastr.error(response.message, 'Delete Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                    }
                                },
                                error: function (xhr, txtStatus, errThrown) {

                                    var reponseErr = JSON.parse(xhr.responseText);
                                    
                                    toastr.error('Error: ' + reponseErr.message, 'Delete Arrival Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            });
                    }
                },
                cancel: {
                    text: 'Cancel',
                    btnClass: 'btn-cancel',
                    keys: ['enter'],
                    action: function () {
                    }
                }
            }
        });
        



        //var con = confirm("Are you sure you want to delete this " + arrTypeName)
        //if (con) {

        //    $.ajax({
        //        type: 'POST',
        //        url: api,
        //        async: true,
        //        data: addRequestVerificationToken({ id: arrTypeId }),
        //        success: function (response) {

        //            if (response.success) {

        //                arrTypeVM.refresh();

        //                toastr.success(response.message, 'Delete Arrival Type');
        //            }
        //            else {
        //                toastr.error(response.message, 'Delete Arrival Type', { closeButton: true, timeOut: 0, extendedTimeOut: 0 });
        //            }
        //        },
        //        error: function (xhr, txtStatus, errThrown) {
        //            toastr.error('Error: ' + xhr.statusText, 'Delete Arrival Type', { closeButton: true, timeOut: 0, extendedTimeOut: 0 });
        //        }
        //    });
        //}
        //else {
        //    //deptVM.refresh();
        //}





    });


});