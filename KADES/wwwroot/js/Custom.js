var idleTime = 0;

function timerIncrement() {
    var timeOut = 3; //$("#sessionTimeOut").val();
    var warning = 2;//$("#sessionWarning").val();
    idleTime += 1;
    console.log("Idletime : " + idleTime + " warning: " + warning + " timeout: " + timeOut);

    //if (idleTime % 2 == 0) {
    //    var url = "Home/CheckSession";
    //    var proses = GetDataAjax(url, false, null, "POST", false);
    //    if (proses == null)
    //        window.location.reload();
    //    else if (proses.check == 0) {
    //        window.location.reload();
    //        parent.window.location.reload();
    //        return false;
    //    }
    //}
    if (idleTime < warning) {
        $("#modalTimeoutWarning").modal('hide');
    } else if (idleTime == warning) {
        $("#modalTimeoutWarning").modal('show');
        //WarningMessage("The system will automatically close your session after a few minutes of inactivity.");
    }
    if (idleTime == timeOut) {
        //CheckSession();
        $("#modalTimeoutWarning").modal('hide');
        LogOutSystem();
    }
}

window.onkeyup = function (e) {
    ResetTimers();
}
window.onkeydown = function (e) {
    ResetTimers();
}

document.onmousemove = function () {
    ResetTimers();
}

function ResetTimers() {
    idleTime = 0;
}

$(function () {
    //$("#sidebar_blog_1").on('click', 'li', function (e) {
    //    $('#sidebar_blog_1 li').removeClass('active');
    //    $(this).addClass("active");
    //    // return false;
    //});

    //Increment the idle time counter every minute.
    var idleInterval = setInterval(timerIncrement, 60000); // 1 minute
    //var idleInterval = setInterval(set_post, 60000 ); // 1 minute
    //console.log("idleInterval : "+idleInterval);
    //Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        idleTime = 0;
    });
    $(this).keypress(function (e) {
        idleTime = 0;
    });
});

function CheckSession() {
    var url = "Home/CheckSession";
    var proses = GetDataAjax(url, false, null, "POST", true);
    if (proses === null)
        window.location.reload();
    else if (proses.check === 0) {
        WarningMessage("Session Habis, Silahkan Login Ulang");
        LogOutSystem();
        //window.location.reload();
        //parent.window.location.reload();
        return false;
    }
}

function LogOutSystem() {
    var data = CallAjaxRequest("Account/Login", null, false, "POST", true);
    if (data != null)
        window.location = '..Account/Login';
    else
        window.location.reload();
}

function GetDataAjax(url, asynch, dataParam, type, showLoading) {
    var data = CallAjaxRequest(url, dataParam, asynch, type, showLoading);
    return data;
}

function CallAjaxRequest(url, dataParam, asynch, type, showLoading) {
    url = GetUrlSite(url);
    var data = null;
    $.ajax({
        type: type,
        async: asynch,
        url: url,
        data: JSON.stringify(dataParam),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            if (showLoading == true) {
                if ($("#divLoadAjax").css("display") == "none")
                    $("#divLoadAjax").show();
            }
        },
        success: function (msg) {
            if (msg.error == "Session") {
                WarningMessage("Session Login Habis!");
                window.location.reload();
                return false;
            }
            else if (msg.error != null) {
                ErrorMessage(msg.error);
                return false;
            }
            else {
                data = msg;
            }
        }, error: function (xhr, msg) {
            ErrorMessage(msg);
            return false;
        }, complete: function () {
            if (showLoading == true) {
                $("#divLoadAjax").hide();
            }

        }
    });
    return data;
}

function GetUrlSite(urlPage) {
    var urlSite = window.location.href;
    if (urlSite.toLowerCase().indexOf('main') > 0) {
        urlSite = urlSite.substring(0, urlSite.toLowerCase().indexOf('main'));
    }
    var last = urlSite.charAt(urlSite.length - 1);
    if (last === '/') {
        urlPage = urlSite + '' + urlPage;
    }
    else {
        urlPage = urlSite + '/' + urlPage;
    }
    urlPage = urlPage.split(' ').join('%20');
    return urlPage;
}

function WarningMessage(val) {
    Swal.fire({
        icon: 'warning',
        type: 'Pesan Peringatan!',
        title: val,
        allowOutsideClick: false,
        footer: ''
    });
}

function ClearDataInDIV(idDiv) {
    $('#' + idDiv + ' input[type=text],input[type=number],input[type=file],input[type=password], textarea,input[type=date]').val("");
    $('#' + idDiv + ' input[type=checkbox],input[type=radio]').prop("checked", false);
    $('#' + idDiv + ' select').find('option[value="0"]').prop("selected", true);
    $('#' + idDiv + ' select').find('option[value="0"]').prop("selected", true).trigger("change");
    $('#' + idDiv + ' select').find('option[value=""]').prop("selected", true);
    $('#' + idDiv + ' select').find('option[value=""]').prop("selected", true).trigger("change");
}