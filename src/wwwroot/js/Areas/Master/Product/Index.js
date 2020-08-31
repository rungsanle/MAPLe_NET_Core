$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');
    
    $("#message-alert").hide();
    //Grid Table Config
    var page = 0;

    productVM = {
        dtProd: null,
        init: function () {
            dtProd = $('#tblProduct').DataTable({
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
                                title: 'Product Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Product Master',
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
                    url: $('#IndexData').data('prod-get-url'),    //"/Customer/GetCustomers",
                    type: "GET",
                    async: true,
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Arrival Type Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { "data": "ProductCode", "className": "boldColumn", "autoWidth": false },
                    { "data": "ProductName", "autoWidth": false },
                    { "data": "MaterialTypeId", "autoWidth": false },
                    { "data": "MaterialType", "autoWidth": false },
                    { "data": "ProductionTypeId", "autoWidth": false },
                    { "data": "ProductionType", "autoWidth": false },
                    { "data": "MachineId", "autoWidth": false },
                    { "data": "Machine", "autoWidth": false },
                    { "data": "PackageStdQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 2, '') },
                    { "data": "UnitId", "autoWidth": false },
                    { "data": "Unit", "autoWidth": false },
                    { "data": "CompanyCode", "className": "boldColumn", "autoWidth": false },
                    {
                        "data": "Is_Active",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                //return '<input type="checkbox" disabled="disabled" class="chkIs_Active">';
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
                        "render": function (data, type, prod, meta) {
                            return '<a id="viewProduct" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="Product/Details/' + prod.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>' +
                                '<a id="editProduct" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="Product/Edit/' + prod.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>' +
                                '<a id="delProduct" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="Product/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "14%", "targets": 0 },
                    { "width": "22%", "targets": 1 },
                    { "width": "0%", "targets": 2, "visible": false },
                    { "width": "15%", "targets": 3 },
                    { "width": "0%", "targets": 4, "visible": false },
                    { "width": "6%", "targets": 5 },
                    { "width": "0%", "targets": 6, "visible": false },
                    { "width": "8%", "targets": 7 },
                    { "className": "dt-right", "width": "7%", "targets": 8 },
                    { "width": "0%", "targets": 9, "visible": false },
                    { "width": "5%", "targets": 10 },
                    { "width": "8%", "targets": 11 },
                    { "className": "dt-center", "width": "5 %", "targets": 12, "orderable": false },
                    { "width": "10%", "targets": 13, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                scroller: true,
                stateSave: true,
                stateDuration: -1 //force the use of Session Storage
            });

            //dt.on('draw', function () {
            //    global.applyIcheckStyle();
            //});

            //keep the current page after sorting
            dtProd.on('order', function () {
                if (dtProd.page() !== page) {
                    dtProd.page(page).draw('page');
                }
            });

            dtProd.on('page', function () {
                page = dtProd.page();
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        refresh: function () {
            dtProd.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtProd.order([]).draw(false);
        }
    }

    // initialize the datatables
    productVM.init();

    productVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtProd.page.info().page != 0) {
                dtProd.page('first').draw('page');
            }
        }, 300);
    }

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add
    $("#btnCreateProduct").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newProdContainer').html(data);
                    $('#newProdModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    $("#newProdModal").on("shown.bs.modal", function () {
        //$('input[type=text]:visible:first').focus();
        $('#ProductCode').focus();
    });
    //clear html data for create new;
    $("#newProdModal").on("hidden.bs.modal", function () {
        prodProcessVM.tbdestroy();
        $('#newProdContainer').html("");
    });

    //Upload Customer
    $("#btnUpload").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.get(api, function (data) {
            if (data) {
                $('#uploadProdContainer').html(data);
                $('#uploadProdModal').modal('show');
            } else {
                global.authenExpire();
            }
        });

    });

    $("#uploadProdModal").on("shown.bs.modal", function () {

    });
    //clear html data for create new;
    $("#uploadProdModal").on("hidden.bs.modal", function () {
        $('#uploadCustContainer').html("");
    });

    //View
    $('#tblProduct').on("click", "#viewProduct", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewProdContainer').html(data);
                    $('#viewProdModal').modal('show');
                } else {
                    global.authenExpire();
                }
            }, 
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for View;
    $("#viewProdModal").on("hidden.bs.modal", function () {
        $('#viewProdContainer').html("");
    });

    //Edit
    $('#tblProduct').on("click", "#editProduct", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editProdContainer').html(data);
                    $('#editProdModal').modal('show');
                } else {
                    global.authenExpire();
                }
            }, 
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    $("#editProdModal").on("shown.bs.modal", function () {
        $('#ProductName').focus();   //.select()
    });

    //clear html data for edit;
    $("#editProdModal").on("hidden.bs.modal", function () {
        prodProcessVM.tbdestroy();
        $('#editProdContainer').html("");
    });

    //Delete
    $('#tblProduct').on('click', '#delProduct', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var productData = (dtProd.row(rowSelect).data());
        var productId = productData["Id"];
        var productName = productData["ProductName"];


        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + productName + '\"',
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
                            data: addRequestVerificationToken({ id: productId }),
                            success: function (response) {

                                if (response.success) {

                                    //productVM.refresh();
                                    dtProd.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Product', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);
                                
                                toastr.error('Error: ' + reponseErr.message, 'Delete Product', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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