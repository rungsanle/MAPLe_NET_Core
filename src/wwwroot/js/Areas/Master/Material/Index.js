$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();

    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    $("#message-alert").hide();
    //Grid Table Config
    productVM = {
        dtMat: null,
        init: function () {
            dtMat = $('#tblMaterial').DataTable({
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
                                title: 'Material Master',
                                titleAttr: 'Excel'
                            },
                            {
                                extend: 'csvHtml5',
                                text: '<i class="fa fa-file-text-o">&nbsp;<p class="setfont">Export CSV</p></i>',
                                title: 'Material Master',
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
                    url: $('#IndexData').data('mat-get-url'),
                    type: "GET",
                    async: true,
                    datatype: "json"
                },
                columns: [
                    { "data": "MaterialCode", "className": "boldColumn", "autoWidth": false },
                    { "data": "MaterialName", "autoWidth": false },
                    { "data": "MaterialDesc1", "autoWidth": false },
                    { "data": "MaterialDesc2", "autoWidth": false },
                    { "data": "RawMatTypeId", "autoWidth": false },
                    { "data": "RawMatType", "autoWidth": false },
                    { "data": "PackageStdQty", "autoWidth": false, render: $.fn.dataTable.render.number(',', '.', 2, '')},
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
                        "render": function (data, type, mat, meta) {
                            return '<a id="viewMaterial" class="btn btn-view btn-sm" data-toggle="tooltip" title="View" href="Material/Details/' + mat.Id + '"><span class="glyphicon glyphicon-search" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="editMaterial" class="btn btn-edit btn-sm" data-toggle="tooltip" title="Edit" href="Material/Edit/' + mat.Id + '"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></a>&nbsp;' +
                                '<a id="delMaterial" class="btn btn-delete btn-sm" data-toggle="tooltip" title="Remove" href="Material/Delete/"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        }
                    }
                ],
                columnDefs: [
                    { "width": "12%", "targets": 0 },
                    { "width": "18%", "targets": 1 },
                    { "width": "15%", "targets": 2 },
                    { "width": "0%", "targets": 3, "visible": false },
                    { "width": "0%", "targets": 4, "visible": false },
                    { "width": "12%", "targets": 5 },
                    { "className": "dt-right", "width": "10%", "targets": 6 },
                    { "width": "0%", "targets": 7, "visible": false },
                    { "width": "8%", "targets": 8 },
                    { "width": "10%", "targets": 9 },
                    { "className": "dt-center", "width": "5%", "targets": 10, "orderable": false },
                    { "width": "10%", "targets": 11, "orderable": false }
                ],
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                scroller: true,
                stateSave: true,
                stateDuration: -1
            });

            //dt.on('draw', function () {
            //    global.applyIcheckStyle();
            //});

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');


        },

        refresh: function () {
            dtMat.ajax.reload();
        }
    }

    // initialize the datatables
    productVM.init();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtMat.page.info().page != 0) {
                dtMat.page('first').draw('page');
            }
        }, 300);
    }

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    //Add
    $("#btnCreateMaterial").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#newMatContainer').html(data);
                    $('#newMatModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Create Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    $("#newMatModal").on("shown.bs.modal", function () {
        //$('input[type=text]:visible:first').focus();
        $('#MaterialCode').focus();
    });
    //clear html data for create new;
    $("#newMatModal").on("hidden.bs.modal", function () {
        $('#newMatContainer').html("");
    });

    //Upload Customer
    $("#btnUpload").on("click", function (event) {

        event.preventDefault();

        var api = $(this).data("url");

        $.get(api, function (data) {
            if (data) {
                $('#uploadMatContainer').html(data);
                $('#uploadMatModal').modal('show');
            } else {
                global.authenExpire();
            }
        });

    });

    $("#uploadMatModal").on("shown.bs.modal", function () {

    });
    //clear html data for create new;
    $("#uploadMatModal").on("hidden.bs.modal", function () {
        $('#uploadCustContainer').html("");
    });

    //View
    $('#tblMaterial').on("click", "#viewMaterial", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#viewMatContainer').html(data);
                    $('#viewMatModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'View Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    //clear html data for View;
    $("#viewMatModal").on("hidden.bs.modal", function () {
        $('#viewMatContainer').html("");
    });

    //Edit
    $('#tblMaterial').on("click", "#editMaterial", function (event) {

        event.preventDefault();

        var api = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: api,
            async: true,
            success: function (data) {
                if (data) {
                    $('#editMatContainer').html(data);
                    $('#editMatModal').modal('show');
                } else {
                    global.authenExpire();
                }
            },
            error: function (xhr, txtStatus, errThrown) {

                var reponseErr = JSON.parse(xhr.responseText);
                
                toastr.error('Error: ' + reponseErr.message, 'Edit Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });

    });

    $("#editMatModal").on("shown.bs.modal", function () {
        $('#MaterialName').focus();   //.select()
    });

    //clear html data for edit;
    $("#editMatModal").on("hidden.bs.modal", function () {
        $('#editMatContainer').html("");
    });

    //Delete
    $('#tblMaterial').on('click', '#delMaterial', function (event) {

        event.preventDefault();

        var api = $(this).attr("href");
        var rowSelect = $(this).parents('tr')[0];
        var materialData = (dtMat.row(rowSelect).data());
        var materialId = materialData["Id"];
        var materialName = materialData["MaterialName"];

        $.confirm({
            title: 'Please Confirm!',
            content: 'Are you sure you want to delete this \"' + materialName + '\"',
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
                            data: addRequestVerificationToken({ id: materialId }),
                            success: function (response) {

                                if (response.success) {

                                    //productVM.refresh();
                                    dtMat.row(rowSelect).remove().draw(false);

                                    toastr.success(response.message, 'Delete Material', { timeOut: appSetting.toastrSuccessTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                                else {
                                    toastr.error(response.message, 'Delete Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                                }
                            },
                            error: function (xhr, txtStatus, errThrown) {

                                var reponseErr = JSON.parse(xhr.responseText);
                                
                                toastr.error('Error: ' + reponseErr.message, 'Delete Material', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
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