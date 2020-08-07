$(function () {

    //To solve Synchronous XMLHttpRequest warning
    global.AjaxPrefilter();
        
    //Get appSetting.json
    var appSetting = global.getAppSettings('AppSettings');

    //$("#message-alert").hide
    //--for check close print popup page.--
    var wpopup_print;
    $(window).focus(function () {
        if (wpopup_print) {
            wpopup_print.close();
        }
    });
    //-------------------------------------

    //Grid Table Config
    var page_p = 0;

    selMcVM = {
        dtSelMc: null,
        init: function () {
            dtSelMc = $('#tblSelMachine').DataTable({
                dom: "<'row'<'col-sm-2'B><'col-sm-5'l><'col-sm-5'f>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row'<'col-sm-6'i><'col-sm-6'p>>",
                buttons: [
                    {
                        text: '<i class="fa fa-refresh">&nbsp;<p class="setfont">Refresh</p></i>',
                        titleAttr: 'Refresh',
                        action: function (e, dt, node, config) {
                            dt.ajax.reload(null, false);
                        }
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
                    url: $('#PrintModalData').data('mc-get-url'),     
                    type: "GET",
                    async: true,
                    datatype: "json",
                    data: null,
                    error: function (xhr, txtStatus, errThrown) {

                        var reponseErr = JSON.parse(xhr.responseText);

                        toastr.error('Error: ' + reponseErr.message, 'Get Machine Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
                    }
                },
                columns: [
                    { data: null, className: "text-center", autoWidth: false },
                    { data: "MachineCode", className: "boldColumn", autoWidth: false },
                    { data: "MachineName", autoWidth: false },
                    { data: "MachineProdType", autoWidth: false },
                    { data: "MachineProdTypeName", autoWidth: false },
                    { data: "MachineSize", autoWidth: false },
                    { data: "MachineRemark", autoWidth: false },
                    { data: "CompanyCode", className: "boldColumn", "autoWidth": false },
                    {
                        "data": "Is_Active",
                        "autoWidth": false,
                        render: function (data, type, row) {
                            if (type === 'display') {
                                //return '<input type="checkbox" disabled="disabled" class="chkIs_Active">';
                                if (data) {
                                    return '<img src="' + $('#PrintModalData').data('image-url') + '/mswitch/isavtive_yes.png" />';
                                } else {
                                    return '<img src="' + $('#PrintModalData').data('image-url') + '/mswitch/isavtive_no.png" />';
                                }
                            }
                            return data;
                        }
                    }
                ],
                columnDefs: [
                    {
                        searchable: false,
                        orderable: false,
                        width: "5%",
                        className: "dt-body-center",
                        data: "DT_RowId",
                        render: function (data, type, full, meta) {
                            return '<input type="checkbox" >';
                        },
                        targets: 0
                    },
                    { width: "15%", targets: 1 },
                    { width: "26%", targets: 2 },
                    { width: "0%", targets: 3, visible: false },
                    { width: "15%", targets: 4 },
                    { width: "14%", targets: 5 },
                    { width: "0%", targets: 6, visible: false },
                    { width: "15%", targets: 7 },
                    { width: "10%", targets: 8, className: "dt-center", orderable: false },
                   
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
                order: [],
                lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                iDisplayLength: appSetting.tableDisplayLength,
                stateSave: true,
                stateDuration: -1, //force the use of Session Storage
                rowCallback: function (row, data, dataIndex) {
                    // Get row ID
                    var rowId = data["Id"];

                    // If row ID is in the list of selected row IDs
                    if ($.inArray(rowId, rows_selected) !== -1) {

                        $(row).find('input[type="checkbox"]').prop('checked', true);
                        $(row).addClass('selected');
                    } 
                }
            });

            //dt.on('draw', function () {
            //    global.applyIcheckStyle();
            //});

            //keep the current page after sorting
            dtSelMc.on('order', function () {
                if (dtSelMc.page() !== page_p) {
                    dtSelMc.page(page_p).draw('page');
                }
            });

            dtSelMc.on('page', function () {
                page_p = dtSelMc.page();
            });

            $('div.dataTables_filter input').addClass('form-control');
            $('div.dataTables_length select').addClass('form-control');

            //set width of input search.
            $('div.dataTables_filter#tblSelMachine_filter input[type = "search"]').css({ 'width': '200px' });
        },

        refresh: function () {
            dtSelMc.ajax.reload();
        },

        removeSorting: function () {  //remove order/sorting
            dtSelMc.order([]).draw(false);
        }
    }

    // initialize the datatables
    selMcVM.init();

    selMcVM.removeSorting();

    if (appSetting.defaultFirstPage == 1) {
        setTimeout(function () {
            if (dtSelMc.page.info().page != 0) {
                dtSelMc.page('first').draw('page');
            }
        }, 200);
    }

    //
    // Updates "Select all" control in a data table
    //
    function updateDataTableSelectAllCtrl(table) {

        var $table = table.table().node();
        var $chkbox_all = $('tbody input[type="checkbox"]', $table);
        var $chkbox_checked = $('tbody input[type="checkbox"]:checked', $table);
        var chkbox_select_all = $('thead input[name="select_all"]', $table).get(0);

        // If none of the checkboxes are checked
        if ($chkbox_checked.length === 0) {
            chkbox_select_all.checked = false;
            if ('indeterminate' in chkbox_select_all) {
                chkbox_select_all.indeterminate = false;
            }
            // If all of the checkboxes are checked
        } else if ($chkbox_checked.length === $chkbox_all.length) {
            chkbox_select_all.checked = true;
            if ('indeterminate' in chkbox_select_all) {
                chkbox_select_all.indeterminate = false;
            }
            // If some of the checkboxes are checked
        } else {
            chkbox_select_all.checked = true;
            if ('indeterminate' in chkbox_select_all) {
                chkbox_select_all.indeterminate = true;
            }
        }
    }

    // Array holding selected row IDs
    var rows_selected = new Array();

    // Handle click on checkbox
    $('#tblSelMachine tbody').on('click', 'input[type="checkbox"]', function (e) {
        var $row = $(this).closest('tr');

        // Get row data
        var data = dtSelMc.row($row).data();

        // Get row ID
        var rowData = data;

        // Determine whether row ID is in the list of selected row IDs 
        var index = $.inArray(rowData, rows_selected);

        // If checkbox is checked and row ID is not in list of selected row IDs
        if (this.checked && index === -1) {
            rows_selected.push(rowData);
            // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
        } else if (!this.checked && index !== -1) {
            rows_selected.splice(index, 1);
        }

        if (this.checked) {
            $row.addClass('selected');
        } else {
            $row.removeClass('selected');
        }

        // Update state of "Select all" control
        updateDataTableSelectAllCtrl(dtSelMc);

        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

    // Handle click on table cells with checkboxes
    $('#tblSelMachine').on('click', 'tbody td, thead th:first-child', function (e) {
        $(this).parent().find('input[type="checkbox"]').trigger('click');
    });

    // Handle click on "Select all" control
    $('thead input[name="select_all"]', dtSelMc.table().container()).on('click', function (e) {

        if (this.checked) {
            $('#tblSelMachine tbody input[type="checkbox"]:not(:checked)').trigger('click');
        } else {
            $('#tblSelMachine tbody input[type="checkbox"]:checked').trigger('click');
        }

        // Prevent click event from propagating to parent
        e.stopPropagation();
    });

    // Handle table draw event
    dtSelMc.on('draw', function () {
        // Update state of "Select all" control
        updateDataTableSelectAllCtrl(dtSelMc);
    });

    

    //// Handle click on "Select all" control
    //$('#tblSelMachine-select-all').on('click', function () {
    //    // Get all rows with search applied
    //    var rows = dtSelMc.rows({ 'search': 'applied' }).nodes();
    //    // Check/uncheck checkboxes for all rows in the table
    //    $('input[type="checkbox"]', rows).prop('checked', this.checked);
    //});

    //$('#tblSelMachine tbody').on('change', 'input[type="checkbox"]', function () {
    //    // If checkbox is not checked
    //    if (!this.checked) {
    //        var el = $('#tblSelMachine-select-all').get(0);
    //        // If "Select all" control is checked and has 'indeterminate' property
    //        if (el && el.checked && ('indeterminate' in el)) {
    //            // Set visual state of "Select all" control
    //            // as 'indeterminate'
    //            el.indeterminate = true;
    //        }
    //    }
    //});

    //// Handle click on table cells with checkboxes
    //$('#tblSelMachine-select-all').on('click', 'tbody td, thead th:first-child', function (e) {
    //    $(this).parent().find('input[type="checkbox"]').trigger('click');
    //});

    $("#btnPrintSelectMc").on("click", PrintMachineLabel);

    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function PrintMachineLabel(event) {

        event.preventDefault();

        // Iterate over all selected checkboxes
        //$.each(rows_selected, function (index, rowData) {
        //    // Create a hidden element 
        //    console.log(rowData);
        //});

        //var rows_selected = dtSelMc.column(0).checkboxes.selected();
        //$.each(rows_selected, function (index, rowId) {
        //    alert(rowId);
        //});

        //var printMachines = new Array();
        //var jsonData = JSON.parse(JSON.stringify($('#tblSelMachine').dataTable().fnGetData()));


        //for (var obj in jsonData) {
        //    if (jsonData.hasOwnProperty(obj)) {

        //        var printMachine = {};
        //        printMachine.Id = jsonData[obj]['Id'];
        //        printMachine.MachineCode = jsonData[obj]['MachineCode'];
        //        printMachine.MachineName = jsonData[obj]['MachineName'];
        //        printMachine.MachineProdType = jsonData[obj]['MachineProdType'];
        //        printMachine.MachineProdTypeName = jsonData[obj]['MachineProdTypeName'];
        //        printMachine.MachineSize = jsonData[obj]['MachineSize'];
        //        printMachine.MachineRemark = jsonData[obj]['MachineRemark'];
        //        printMachine.CompanyCode = jsonData[obj]['CompanyCode'];
        //        printMachine.Is_Active = jsonData[obj]['Is_Active'];;

        //        printMachines.push(printMachine);
        //    }
        //}
        

        var api = $('#PrintModalData').data('mc-print-url'); // + '?lstSelMc=' + JSON.stringify(addRequestVerificationToken({ lstSelMc: printMachines }));

        global.setCursor('wait', 'wait');

        $.ajax({
            async: true,
            cache: false,
            type: 'POST',
            url: api,
            data: addRequestVerificationToken({ lstSelMc: rows_selected }),
            //xhrFields is what did the trick to read the blob to pdf
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response, status, xhr) {

                var blob = new Blob([response], { type: 'application/pdf' });

                //console.log('Result Blob: ' + blob);

                var fileURL = URL.createObjectURL(blob);

                //console.log('Result FileUrl: ' + fileURL);

                //window.open(fileURL, 'PopupWindow', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=0,width=850,height=700');
                wpopup_print = global.popupBottomR(fileURL, "Print Machine Label", '_blank', 875, 660);

                global.setCursor('default', 'pointer');
                //w.onload = function () {
                //    //this.document.title = "Print Machine Label";
                //    setTimeout(function () {
                //        w.document.title = "Print Machine Label";
                //    }, 1800);
                //}

                $('#printSelectMachineModal').modal('hide');
                $('#printSelectMachineContainer').html("");
            },
            error: function (xhr, txtStatus, errThrown) {

                global.setCursor('default', 'pointer');
               // alert(errThrown.message);
               // var reponseErr = //JSON.parse(xhr.responseText);

                toastr.error('Error: ' + txtStatus, 'Print Label Error', { timeOut: appSetting.toastrErrorTimeout, extendedTimeOut: appSetting.toastrExtenTimeout });
            }
        });
        
    }

    
    
});