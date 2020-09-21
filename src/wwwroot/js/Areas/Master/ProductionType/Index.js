$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    var page = 0;

    prodTypeVM = {
        dtProdType: null,
        init: function () {
            dtProdType = $('#tblProdType').DataTable({
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
                                title: 'Production Type Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Production Type Master',
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
                    url: $('#IndexData').data('prodtype-get-url'),    //"/Customer/GetCustomers",
                    type: "GET",
                    async: true,
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Production Type Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { "data": "ProdTypeCode", "className": "boldColumn", "autoWidth": false },
                    { "data": "ProdTypeName", "autoWidth": false },
                    { "data": "ProdTypeDesc", "autoWidth": false },
                    { "data": "ProdTypeSeq", "autoWidth": false },
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
                        "render": function (data, type, prodType, meta) {
                            return '<a id="viewProdType" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="ProductionType/Details/' + prodType.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>' +
                                '<a id="editProdType" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="ProductionType/Edit/' + prodType.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>' +
                                '<a id="delProdType" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="ProductionType/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "14%", "targets": 0 },
                    { "width": "22%", "targets": 1 },
                    { "width": "27%", "targets": 2 },
                    { "className": "dt-center", "width": "8%", "targets": 3 },
                    { "width": "12%", "targets": 4 },
                    { "className": "dt-center", "width": "7%", "targets": 5, "orderable": false },
                    { "width": "10%", "targets": 6, "orderable": false }
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
            dtProdType.on('order', function () {
                if (dtProdType.page() !== page) {
                    dtProdType.page(page).draw('page');
                }
            });

            dtProdType.on('page', function () {
                page = dtProdType.page();
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        refresh: function () {
            dtProdType.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtProdType.order([]).draw(false);
        }
    }

    // initialize the datatables
    prodTypeVM.init();

    prodTypeVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtProdType.page.info().page != 0) {
                dtProdType.page('first').draw('page');
            }
        }, 200);
    }

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add Machine
    $("#btnCreateProdType").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newProdTypeContainer').html(data);
                    $('#newProdTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }

            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    $("#newProdTypeModal").on("shown.bs.modal", function () {
        //$('input[type=text]:visible:first').focus();
        $('#ProdTypeCode').focus();
    });
    //clear html data for create new;
    $("#newProdTypeModal").on("hidden.bs.modal", function () {
        $('#newProdTypeContainer').html("");
    });

    //view Machine
    $('#tblProdType').on("click", "#viewProdType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewProdTypeContainer').html(data);
                    $('#viewProdTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

        //$.get(url, function (data) {

        //    $('#viewMenuContainer').html(data);
        //    $('#viewMenuModal').modal('show');
        //});

    });

    //clear html data for View;
    $("#viewProdTypeModal").on("hidden.bs.modal", function () {
        $('#viewProdTypeContainer').html("");
    });

    //Edit Machine
    $('#tblProdType').on("click", "#editProdType", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editProdTypeContainer').html(data);
                    $('#editProdTypeModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
    });

    //clear html data for edit;
    $("#editProdTypeModal").on("hidden.bs.modal", function () {
        $('#editProdTypeContainer').html("");
    });

    //Delete
    $('#tblProdType').on('click', '#delProdType', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var prodTypeData = (dtProdType.row(rowSelect).data());
        var prodTypeId = prodTypeData["Id"];
        var prodTypeName = prodTypeData["ProdTypeName"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + prodTypeName + '\"',
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
                            data: addRequestVerificationToken({ id: prodTypeId }),
                            success: function (response) {

                                if (response.success) {

                                    //prodTypeVM.refresh();
                                    dtProdType.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Production Type', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);
                                
                                toastr.error('Error: ' + reponseErr.message, 'Delete Production Type', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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