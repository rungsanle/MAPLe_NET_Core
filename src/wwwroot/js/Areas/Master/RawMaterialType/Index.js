$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    var page = 0;

    rawmatTypeVM = {
        dtRawMatType: null,
        init: function () {
            dtRawMatType = $('#tblRawMatType').DataTable({
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
                                title: 'Raw Material Type Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Raw Material Type Master',
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
                    url: $('#IndexData').data('rawmattype-get-url'),    //"/Customer/GetCustomers",
                    type: "GET",
                    async: true,
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Raw Material Type Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { "data": "RawMatTypeCode", "className": "boldColumn", "autoWidth": false },
                    { "data": "RawMatTypeName", "autoWidth": false },
                    { "data": "RawMatTypeDesc", "autoWidth": false },
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
                        "render": function (data, type, rawmatType, meta) {
                            return '<a id="viewRawMatType" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="RawMaterialType/Details/' + rawmatType.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>' +
                                '<a id="editRawMatType" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="RawMaterialType/Edit/' + rawmatType.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>' +
                                '<a id="delRawMatType" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="RawMaterialType/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "16%", "targets": 0 },
                    { "width": "25%", "targets": 1 },
                    { "width": "30%", "targets": 2 },
                    { "width": "12%", "targets": 3 },
                    { "className": "dt-center", "width": "7%", "targets": 4, "orderable": false },
                    { "width": "10%", "targets": 5, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                stateSave: true,
                stateDuration: -1 //force the use of Session Storage
            });

            //keep the current page after sorting
            dtRawMatType.on('order', function () {
                if (dtRawMatType.page() !== page) {
                    dtRawMatType.page(page).draw('page');
                }
            });

            dtRawMatType.on('page', function () {
                page = dtRawMatType.page();
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        refresh: function () {
            dtRawMatType.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtRawMatType.order([]).draw(false);
        }
    }

    // initialize the datatables
    rawmatTypeVM.init();

    rawmatTypeVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtRawMatType.page.info().page != 0) {
                dtRawMatType.page('first').draw('page');
            }
        }, 200);
    }

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add Machine
    $("#btnCreateRawMatType").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newRawMatTypeContainer').html(data);
                    $('#newRawMatTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    $("#newRawMatTypeModal").on("shown.bs.modal", function () {
        $('#RawMatTypeCode').focus();
    });
    //clear html data for create new;
    $("#newRawMatTypeModal").on("hidden.bs.modal", function () {
        $('#newRawMatTypeContainer').html("");
    });

    //view Machine
    $('#tblRawMatType').on("click", "#viewRawMatType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewRawMatTypeContainer').html(data);
                    $('#viewRawMatTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        //$.get(url, function (data) {

        //    $('#viewMenuContainer').html(data);
        //    $('#viewMenuModal').modal('show');
        //});

    });

    //clear html data for View;
    $("#viewRawMatTypeModal").on("hidden.bs.modal", function () {
        $('#viewRawMatTypeContainer').html("");
    });

    //Edit Machine
    $('#tblRawMatType').on("click", "#editRawMatType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editRawMatTypeContainer').html(data);
                    $('#editRawMatTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    //clear html data for edit;
    $("#editRawMatTypeModal").on("hidden.bs.modal", function () {
        $('#editRawMatTypeContainer').html("");
    });

    //Delete
    $('#tblRawMatType').on('click', '#delRawMatType', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var rawmatTypeData = (dtRawMatType.row(rowSelect).data());
        var rawmatTypeId = rawmatTypeData["Id"];
        var rawmatTypeName = rawmatTypeData["RawMatTypeName"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + rawmatTypeName + '\"',
            buttons: {
                confirm: {
                    text: 'Confirm',
                    btnClass: 'btn-confirm',
                    keys: ['shift', 'enter'],
                    action: function () {

                        $.ajax({
                            type: 'POST',
                            async: true,
                            url: api,
                            data: addRequestVerificationToken({ id: rawmatTypeId }),
                            success: function (response) {

                                if (response.success) {

                                    //rawmatTypeVM.refresh();
                                    dtRawMatType.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Raw MAT. Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);
                                
                                toastr.error('Error: ' + reponseErr.message, 'Delete Raw MAT. Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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

    });


});