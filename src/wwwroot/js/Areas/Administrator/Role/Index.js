$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    var page = 0;

    roleVM = {
        dtRole: null,
        init: function () {
            dtRole = $('#tblRole').DataTable({
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
                                title: 'Role Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Role Master',
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
                    url: $('#IndexData').data('role-get-url'),
                    type: "GET",
                    async: false,
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Role Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { "data": "Id", "className": "boldColumn", "autoWidth": false },
                    { "data": "Name", "autoWidth": false },
                    {
                        "autoWidth": true,
                        "render": function (data, type, role, meta) {
                            return '<a id="viewRole" class="btn btn-default btn-sm" data-toggle="tooltip" title="View" href="Role/Details/' + role.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="editRole" class="btn btn-default btn-sm" data-toggle="tooltip" title="Edit" href="Role/Edit/' + role.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="delRole" class="btn btn-default btn-sm" data-toggle="tooltip" title="Remove" href="Role/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "30%", "targets": 0 },
                    { "width": "60%", "targets": 1 },
                    { "width": "10%", "targets": 2, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                teSave: true,
                stateDuration: -1 //force the use of Session Storage
            });

            //dt.on('draw', function () {
            //    global.applyIcheckStyle();
            //});

            //keep the current page after sorting
            dtRole.on('order', function () {
                if (dtRole.page() !== page) {
                    dtRole.page(page).draw('page');
                }
            });

            dtRole.on('page', function () {
                page = dtRole.page();
            });

            $('div.dataTables_filter input').addClass('form-control input-sm');
            $('div.dataTables_length select').addClass('form-control input-sm');

        },

        refresh: function () {
            dtRole.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtRole.order([]).draw(false);
        }
    }

    // initialize the datatables
    roleVM.init();

    roleVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtRole.page.info().page != 0) {
                dtRole.page('first').draw('page');
            }
        }, 200);
    }


    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add User
    $("#btnCreateRole").on("click", function (event) {
        

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {

                if (data) {
                    $('#newRoleContainer').html(data);
                    $('#newRoleModal').modal('show');
                } else {
                    global.authenExpire();
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseText = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseText.Message, 'Create Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });
   
    $("#newRoleModal").on("shown.bs.modal", function () {
        $('#Name').focus();
    });
    //clear html data for create new;
    $("#newRoleModal").on("hidden.bs.modal", function () {
        $('#newRoleContainer').html("");
    });

    //view User
    $('#tblRole').on("click", "#viewRole", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewRoleContainer').html(data);
                    $('#viewRoleModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'View Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for View;
    $("#viewRoleModal").on("hidden.bs.modal", function () {
        $('#viewRoleContainer').html("");
    });

    //Edit
    $('#tblRole').on("click", "#editRole", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editRoleContainer').html(data);
                    $('#editRoleModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);

                toastr.error('Error: ' + reponseErr.message, 'Edit Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for edit;
    $("#editRoleModal").on("hidden.bs.modal", function () {
        $('#editRoleContainer').html("");
    });

    //Delete
    $('#tblRole').on('click', '#delRole', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var roleData = (dtRole.row(rowSelect).data());
        var roleId = roleData["Id"];
        var roleName = roleData["Name"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + roleName + '\"',
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
                            data: addRequestVerificationToken({ id: roleId }),
                            success: function (response) {

                                if (response.success) {

                                    dtRole.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Role', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);

                                toastr.error('Error: ' + reponseErr.message, 'Delete Role', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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