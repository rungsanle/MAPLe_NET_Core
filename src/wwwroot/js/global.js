var global = {};

global.applyToastrOption = function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-bottom-right",
        "newestOnTop": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
}

global.applyIcheckStyle = function () {

    //$('input[type="checkbox"], input[type="radio"]').iCheck({
    //    checkboxClass: "icheckbox_minimal-blue",
    //    radioClass: "iradio_minimal-blue"
    //});
}

global.applyDatepicker = function (id, showtoday) {

    $("input[id$='" + id + "']").inputmask('dd-mm-yyyy', { placeholder: 'dd-mm-yyyy' });
    $("input[id$='" + id + "']").datepicker({
        format: 'dd-mm-yyyy',
        autoclose: true,
        todayHighlight: true,
        todayBtn: "linked",
        language: "fr-FR"
    });

    if (showtoday) {
        $("input[id$='" + id + "']").datepicker('setDate', 'today');
    }
}

global.applyDatepickerS = function (id, showtoday, inpFormat = 'dd-mm-yyyy', picFormat = 'dd-mm-yyyy') {

    $("input[id$='" + id + "']").inputmask(inpFormat, { placeholder: inpFormat });
    $("input[id$='" + id + "']").datepicker({
        format: picFormat,
        autoclose: true,
        todayHighlight: true,
        todayBtn: "linked",
        language: "fr-FR"
    });

    if (showtoday) {
        $("input[id$='" + id + "']").datepicker('setDate', 'today');
    }
}


global.showDatepicker = function (id) {
    //$("input[id$='" + id + "']").datepicker("destroy");
    $("input[id$='" + id + "']").datepicker('show');
}

global.ConvertLocalDate = function (vDate, optFormat = 'DD-MM-YYYY') {

    if (vDate == null) return null;
    if (vDate instanceof Date) return vDate;

    var resultDate = moment(vDate, optFormat);

    if (!resultDate.isValid()) {
        resultDate = moment(vDate);
    }

    return resultDate;

}

global.convertJsonDate = function (vDate, optFormat = 'DD-MM-YYYY') {
    //if (vDate == null) return null;
    if (typeof vDate != 'undefined' && vDate) {

        if (vDate instanceof Date) return vDate;

        var resultDate = new Date(moment(vDate, optFormat));

        return ('/Date(' + resultDate.getTime() + ')/')

    } else {
        return '';
    }

}

global.localDate = function (locDate, optFormat = 'DD-MM-YYYY') {

    //if (locDate == null) return null;
    if (typeof locDate != 'undefined' && locDate) {

        if (locDate instanceof Date) {
            return locDate
        };

        var resultDate = moment(locDate);

        //.locale('en')

        if (!resultDate.isValid()) {
            resultDate = moment(locDate, optFormat);   //moment(locDate, optFormat).locale('en');
        }

        return resultDate.format(optFormat);    //resultDate.locale('en').format(optFormat); 

    } else {
        return '';
    }


}

global.convertNETDateTime = function (sNetDate) {

    if (sNetDate == null) return null;
    if (sNetDate instanceof Date) return sNetDate;

    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(sNetDate);

    if (results) {
        if (results.length == 2) {
            var dt = new Date(parseFloat(results[1]));
            return (("0" + dt.getDate()).slice(-2) + "-" + ("0" + (dt.getMonth() + 1)).slice(-2) + "-" + dt.getFullYear());
        }
        else {
            return sNetDate;
        }
    } else {
        return sNetDate;
    }


    //var r = /\/Date\(([0-9]+)\)\//i
    //var matches = sNetDate.match(r);
    //if (matches.length == 2) {
    //    return new Date(parseInt(matches[1]));
    //}
    //else {
    //    return sNetDate;
    //}
}

global.applyInputPicker = function (id, api, fColumns, wSize = '350px', isatOpen = true, selMode = 'restore', isShowHdr = true, fValue, fText) {
    $("input[id$='" + id + "']").inputpicker({
        url: api,
        fields: fColumns,
        width: wSize,
        autoOpen: isatOpen,
        selectMode: selMode,
        headShow: isShowHdr,
        fieldValue: fValue,
        fieldText: fText,
    });
}


global.applyBSwitchStyle = function (id, state, readOnly, swSize, swTextOn, swTextOff) {

    $("input[id$='" + id + "']").bootstrapSwitch();

    $("input[id$='" + id + "']").bootstrapSwitch('size', swSize);
    $("input[id$='" + id + "']").bootstrapSwitch('onText', swTextOn);
    $("input[id$='" + id + "']").bootstrapSwitch('offText', swTextOff);

    setTimeout(function () {

        $("input[id$='" + id + "']").bootstrapSwitch('state', state);
        $("input[id$='" + id + "']").bootstrapSwitch('readonly', readOnly);

    }, 180);
}

global.applyIsActiveSwitch = function (swState, swIsReadOnly) {

    $("input[id$='Is_Active']").bootstrapSwitch('size', 'small');
    $("input[id$='Is_Active']").bootstrapSwitch('onText', 'Yes');
    $("input[id$='Is_Active']").bootstrapSwitch('offText', 'No');

    setTimeout(function () {

        $("input[id$='Is_Active']").bootstrapSwitch('state', swState);
        $("input[id$='Is_Active']").bootstrapSwitch('readonly', swIsReadOnly);

    }, 180);
}

global.applyCompanyCodeDropdown = function () {

    $("input[id$='CompanyCode']").inputpicker({
        url: ROOT + 'Master/Company/GetCompany',
        fields: [
            { name: 'CompanyCode', text: 'CODE', width: '30%' },
            { name: 'CompanyName', text: 'NAME', width: '70%' }
        ],
        width: '350px',
        autoOpen: true,
        selectMode: 'restore',
        headShow: true,
        fieldText: 'CompanyCode',
        fieldValue: 'CompanyCode'
    });
}

//global.applyInputPickerStyle = function (id, api, fColumns = [
//    { name: 'CompanyCode', text: 'CODE', width: '30%' },
//    { name: 'CompanyName', text: 'NAME', width: '70%' }
//], wSize = '350px', isatOpen = true, selMode = 'restore', isShowHdr = true, fValue, fText) {
//    $("input[id$='" + id + "']").inputpicker({
//        url: api,
//        fields: fColumns,
//        width: wSize,
//        autoOpen: isatOpen,
//        selectMode: selMode,
//        headShow: isShowHdr,
//        fieldValue: fValue,
//        fieldText: fText,
//    });
//}

global.applyDraggable = function () {

    $(".modal.fade").draggable({
        handle: ".modal-header"
    });
}

global.MessageAlert = function (id, classType, message) {

    $(id + ' strong').text('Error! ');


    $(id + ' span').text(message + ' ');
    $(id).removeClass();
    $(id).addClass(classType);

    $(id).fadeTo(5000, 500).slideUp(500, function () {
        $(id).slideUp(500);
    });
}

global.successAlert = function (message, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('Success! ');
    $(divId + ' span').text(message);
    $(divId).removeClass();
    $(divId).addClass('alert alert-success');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.infoAlert = function (message, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('Info! ');
    $(divId + ' span').text(message);
    $(divId).removeClass();
    $(divId).addClass('alert alert-info');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.warningAlert = function (message, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('Warning! ');
    $(divId + ' span').text(message);
    $(divId).removeClass();
    $(divId).addClass('alert alert-warning');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.dangerAlert = function (message, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('Danger! ');
    $(divId + ' span').text(message);
    $(divId).removeClass();
    $(divId).addClass('alert alert-danger');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.errorsAlert = function (errors, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('Danger! ');
    $.each(errors, function (idx, errorMessage) {
        $(divId + ' span').append('<li>' + errorMessage + '</li>');
    });

    $(divId).removeClass();
    $(divId).addClass('alert alert-danger');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.containsArray = function (colsCheck, colsMaster) {
    for (var i = 0, len = colsCheck.length; i < len; i++) {
        if ($.inArray(colsCheck[i], colsMaster) == -1) return false;
    }
    return true;
}

global.timeoutAlert = function (message, iDuration = 2000, divId = '#message-alert') {

    $(divId + ' strong').text('System Timeout ');
    $(divId + ' span').text(message);
    $(divId).removeClass();
    $(divId).addClass('alert alert-danger');

    $(divId).fadeTo(iDuration, 500).slideUp(500, function () {
        $(divId).slideUp(500);
    });
}

global.authenExpire = function () {

    $('#message-alert strong').text('Authentication Expire, ');
    $('#message-alert span').text('please login again.');
    $('#message-alert').removeClass();
    $('#message-alert').addClass('alert alert-danger');

    $('#message-alert').fadeTo(4000, 500).slideUp(500, function () {
        $('#message-alert').slideUp(500);
    });

    setTimeout(function () {
        window.location.href = ROOT + 'Account/Login';
    }, 4500);
}

global.numberWithCommas = function (number) {
    var parts = number.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

global.displayValidationErrors = function (errors) {

    $.each(errors, function (idx, errorMessage) {
        var res = errorMessage.split("|");
        $("span[data-valmsg-for='" + res[0] + "']").append('<li>' + res[1] + '</li>');
    });
}

global.removeValidationErrors = function (id) {
    $("span[data-valmsg-for='" + id + "'] li").remove();
}

global.resetValidationErrors = function() {

    var listItems = document.querySelectorAll('.text-danger li');
    for (let i = 0; i < listItems.length; i++) {
        if (listItems[i].textContent != null)
            listItems[i].remove();
    };
}

//Solve Synchronous XMLHttpRequest warning
global.AjaxPrefilter = function () {

    $.ajaxPrefilter(function (options, original_Options, jqXHR) {
        options.async = true;
    });
}

global.setAppSettings = function (strKey, objJson) {
    localStorage.setItem(strKey, objJson);
};

global.getAppSettings = function (strKey) {
    var appSettings = JSON.parse(localStorage.getItem(strKey));
    return appSettings;
};

global.popupCenter = function (url, target, w, h) {
    var y = window.outerHeight / 2 + window.screenY - (h / 2);
    var x = window.outerWidth / 2 + window.screenX - (w / 2);
    return window.open(url, target, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);
};

global.popupBottomR = function (url, title, target, w, h) {
    var y = (window.outerHeight - h) - 75;
    var x = (window.outerWidth - w) - 15;
    var w = window.open(url, target, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);

    w.onload = function () {
        setTimeout(function () {
            w.document.title = title;
        }, 1800);
    }    //w.on('load', function (e) {
    //    e.preventDefault();
    //    setTimeout(function (ex) {
    //        w.document.title = title;
    //    }, 1800);

    //})

    

    //w.addEventListener('onload', function () {
    //    setTimeout(function () {
    //        w.document.title = title;
    //    }, 1800);
    //}, false);

    return w;
};

global.setCursor = function (sCur, bCur) {
    document.body.style.cursor = sCur;
    $(".btn").css("cursor", bCur);
}


