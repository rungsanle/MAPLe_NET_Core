//--Begin sidebar-collapse--//
//document.body.removeAttribute('class');
//document.body.className = 'hold-transition skin-black-light sidebar-collapse sidebar-mini';
//--End sidebar-collapse--//

$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    menuVM = {
        dtMenu: null,
        init: function () {
            dtMenu = $('#tblMenu').DataTable({
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
                                title: 'Menu Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Menu Master',
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
                    url: $('#IndexData').data('menu-get-url'),    //"/Customer/GetCustomers",
                    type: "GET",
                    async: true,
                    datatype: "json"
                },
                columns: [
                    { "data": "nameOption", "className": "boldColumn", "autoWidth": false },
                    { "data": "controller", "autoWidth": false },
                    { "data": "action", "autoWidth": false },
                    { "data": "imageClass", "autoWidth": false },
                    {
                        "data": "status",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_yes.png" />';
                                } else {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_no.png" />';
                                }
                            }
                            return data;
                        }
                    },
                    {
                        "data": "isParent",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_yes.png" />';
                                } else {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_no.png" />';
                                }
                            }
                            return data;
                        }
                    },
                    { "data": "parentId", "autoWidth": false },
                    { "data": "parentName", "className": "boldColumn", "autoWidth": false },
                    { "data": "area", "autoWidth": false },
                    { "data": "menuseq", "autoWidth": false },
                    {
                        "data": "Is_Active",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                if (data) {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_yes.png" />';
                                } else {
                                    return '<img src="' + $('#IndexData').data('image-url') + '/mswitch/isavtive_no.png" />';
                                }
                            }
                            return data;
                        }
                    },
                    {
                        "autoWidth": true,
                        "render": function (data, type, menu, meta) {
                            return '<a id="viewMenu" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="Menu/Details/' + menu.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="editMenu" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="Menu/Edit/' + menu.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="delMenu" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="Menu/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                //rowCallback: function (row, data) {
                //    // Set the checked state of the checkbox in the table
                //    //$('input.chkStatus', row).prop('checked', data.status == 1);
                //    //$('input.chkIsParent', row).prop('checked', data.isParent == 1);
                //    //$('input.chkIs_Active', row).prop('checked', data.Is_Active == 1);
                //},
                columnDefs: [
                    { "width": "11%", "targets": 0 },
                    { "width": "15%", "targets": 1 },
                    { "width": "8%", "targets": 2 },
                    { "width": "15%", "targets": 3 },
                    { "className": "dt-center", "width": "7%", "targets": 4 },
                    { "className": "dt-center", "width": "7%", "targets": 5 },
                    { "width": "0%", "targets": 6, "visible": false },
                    { "width": "10%", "targets": 7 },
                    { "width": "7%", "targets": 8 },
                    { "className": "dt-center", "width": "5%", "targets": 9 },
                    { "className": "dt-center", "width": "5%", "targets": 10, "orderable": false },
                    { "width": "10%", "targets": 11, "orderable": false }
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
            dtMenu.ajax.reload();
        }
    }

    // initialize the datatables
    menuVM.init();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtMenu.page.info().page != 0) {
                dtMenu.page('first').draw('page');
            }
        }, 200);
    }

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add Machine
    $("#btnCreateMenu").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newMenuContainer').html(data);
                    $('#newMenuModal').modal('show');
                } else {
                    global.authenExpire();
                }

            }, 
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        //$.get(api, function (data) {
        //    $('#newMenuContainer').html(data);

        //    $('#newMenuModal').modal('show');
        //});

    });

    $("#newMenuModal").on("shown.bs.modal", function () {
        //$('input[type=text]:visible:first').focus();
        $('#nameOption').focus();
    });
    //clear html data for create new;
    $("#newMenuModal").on("hidden.bs.modal", function () {
        $('#newMenuContainer').html("");
    });

    //view Machine
    $('#tblMenu').on("click", "#viewMenu", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewMenuContainer').html(data);
                    $('#viewMenuModal').modal('show');
                } else {
                    global.authenExpire();
                }
            }, 
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        //$.get(url, function (data) {

        //    $('#viewMenuContainer').html(data);
        //    $('#viewMenuModal').modal('show');
        //});

    });

    //clear html data for View;
    $("#viewMenuModal").on("hidden.bs.modal", function () {
        $('#viewMenuContainer').html("");
    });

    //Edit Machine
    $('#tblMenu').on("click", "#editMenu", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editMenuContainer').html(data);
                    $('#editMenuModal').modal('show');
                } else {
                    global.authenExpire();
                }

            }, 
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        //$.get(api, function (data) {

        //    $('#editMenuContainer').html(data);
        //    $('#editMenuModal').modal('show');
        //});

    });

    //clear html data for edit;
    $("#editMenuModal").on("hidden.bs.modal", function () {
        $('#editMenuContainer').html("");
    });

    //Delete
    $('#tblMenu').on('click', '#delMenu', function (event) {

        event.preventDefault();
        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var menuData = (dtMenu.row(rowSelect).data());
        var menuId = menuData["Id"];
        var menuName = menuData["nameOption"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + menuName + '\"',
            buttons: {
                confirm: {
                    text: 'Confirm',
                    btnClass: 'btn-confirm',
                    keys: ['shift', 'enter'],
                    action: function () {

                        $.ajax({
                            async: true,
                            type: 'POST',
                            url: api,
                            data: addRequestVerificationToken({ id: menuId }),
                            success: function (response) {

                                if (response.success) {

                                    //menuVM.refresh();
                                    dtMenu.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Menu', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);
                                
                                toastr.error('Error: ' + reponseErr.message, 'Delete Menu', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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