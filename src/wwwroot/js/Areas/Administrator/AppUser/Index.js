$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    var page = 0;

    appUserVM = {
        dtAppUser: null,
        init: function () {
            dtAppUser = $('#tblAppUser').DataTable({
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
                                title: 'Application User Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Application User Master',
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
                    url: $('#IndexData').data('appu-get-url'),
                    type: "GET",
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Application User Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { "data": "Id", "className": "boldColumn", "autoWidth": false },
                    { "data": "UserName", "autoWidth": false },
                    { "data": "Email", "autoWidth": false },
                    {
                        "autoWidth": true,
                        "render": function (data, type, appu, meta) {
                            return '<a id="viewAppUser" class="btn btn-default btn-sm" data-toggle="tooltip" title="View" href="AppUser/Details/' + appu.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>' +
                                '<a id="editAppUser" class="btn btn-default btn-sm" data-toggle="tooltip" title="Edit" href="AppUser/Edit/' + appu.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>' +
                                '<a id="delAppUser" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove" href="AppUser/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "30%", "targets": 0 },
                    { "width": "20%", "targets": 1 },
                    { "width": "40%", "targets": 2 },
                    { "width": "10%", "targets": 3, "orderable": false }
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

            //keep the current page after sorting
            dtAppUser.on('order', function () {
                if (dtAppUser.page() !== page) {
                    dtAppUser.page(page).draw('page');
                }
            });

            dtAppUser.on('page', function () {
                page = dtAppUser.page();
            });

            $('div.dataTables_filter input').addClass('form-control input-sm');
            $('div.dataTables_length select').addClass('form-control input-sm');

        },

        refresh: function () {
            dtAppUser.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtAppUser.order([]).draw(false);
        }
    }

    // initialize the datatables
    appUserVM.init();

    appUserVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtAppUser.page.info().page != 0) {
                dtAppUser.page('first').draw('page');
            }
        }, 200);
    }


    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add User
    $("#btnCreateAppUser").on("click", function (event) {
        

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {

                if (data) {
                    $('#newAppUserContainer').html(data);
                    $('#newAppUserModal').modal('show');
                } else {
                    global.authenExpire();
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseText = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseText.Message, 'Create Application User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });
   
    $("#newAppUserModal").on("shown.bs.modal", function () {
        $('#Name').focus();
    });
    //clear html data for create new;
    $("#newAppUserModal").on("hidden.bs.modal", function () {
        $('#newAppUserContainer').html("");
    });

    //view User
    $('#tblAppUser').on("click", "#viewAppUser", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewAppUserContainer').html(data);
                    $('#viewAppUserModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'View User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for View;
    $("#viewAppUserModal").on("hidden.bs.modal", function () {
        $('#viewAppUserContainer').html("");
    });

    //Edit
    $('#tblAppUser').on("click", "#editAppUser", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editAppUserContainer').html(data);
                    $('#editAppUserModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Edit User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for edit;
    $("#editAppUserModal").on("hidden.bs.modal", function () {
        $('#editAppUserContainer').html("");
    });

    //Delete
    $('#tblAppUser').on('click', '#delAppUser', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var appUserData = (dtAppUser.row(rowSelect).data());
        var appuId = appUserData["Id"];
        var Name = appUserData["UserName"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + Name + '\"',
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
                            data: addRequestVerificationToken({ id: appuId }),
                            success: function (response) {

                                if (response.success) {

                                    dtAppUser.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Application User', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Application User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);

                                toastr.error('Error: ' + reponseErr.message, 'Delete Application User', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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