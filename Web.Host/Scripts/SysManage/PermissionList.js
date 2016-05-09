//初始化页面
$(function () {
    $("#divFilter input").BindEnter(SearchInfo);
});

/*========================分页事件==============================*/
function PermissionPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}
/*========================查询==============================*/
function SearchInfo() {
    window.SearchConfig.ApplicationId = $("#ApplicationList2").val();
    window.SearchConfig.Name = $("#content1").val().Trim();
    window.SearchConfig.Code = $("#code1").val().Trim();
    if ($("#OpenedSource").length > 0) {
        window.SearchConfig.IsOpened = $("#OpenedSource").val();
    }
    PermissionPager(1);
}
/*========================添加事件==============================*/
function CreateDailog() {
    var url = window.ServiceAppPath + "Admin/Permission/Detail?applicationId=" + window.SearchConfig.ApplicationId;
    $(this).showDailog("系统页面权限管理", url, null, 710, 480);
}
function CreateApiDailog() {
    var url = window.ServiceAppPath + "Admin/Permission/ApiDetail?applicationId=" + window.SearchConfig.ApplicationId;
    $(this).showDailog("API权限管理", url, null, 460, 320);
}
//添加明细
function AddDetail() {
    var row1 = $("#trLast");
    var count = $("#tableDetail tr").length - 1 + "";
    var tr = row1.clone();
    tr.removeAttr("Id");
    tr.removeAttr("style");
    tr.find("td").eq(0).html(count);
    tr.find("input[name='Order']").eq(0).val(count);
    row1.before(tr);
}

/*========================编辑事件==============================*/
function EditDailog(code) {
    var url = window.ServiceAppPath + "Admin/Permission/Detail?code=" + encodeURIComponent(code);
    $(this).showDailog("系统页面权限管理", url, null, 710, 480);
}
function EditApiDailog(code) {
    var url = window.ServiceAppPath + "Admin/Permission/ApiDetail?code=" + encodeURIComponent(code);
    $(this).showDailog("API权限管理", url, null, 460, 320);
}
/*========================删除一项==============================*/
function DeleInfo(id) {
    if (confirm('是否删除此信息?')) {
        $(this).ajaxBfeData({
            url: 'Admin/Permission/Delete', data: { Id: id }, type: 'post', dataType: 'Json',
            success: function (data) {
                var messageBox = ["删除失败,请稍后再试！", "删除成功!"];
                if (data.Status) {
                    InitData();
                } else {
                    c$.alert(messageBox[data.Status ? 1 : 0]);
                }
            }
        });
        return false;
    }
}
function DeleInfo2(id, obj) {
    if (id != "") {
        if (confirm('是否删除此信息?')) {
            $(this).ajaxBfeData({
                url: 'Admin/Permission/Delete', data: { Id: id }, type: 'post', dataType: 'Json',
                success: function (data) {
                    var messageBox = ["删除失败,请稍后再试！", "删除成功!"];
                    if (data.Status) {
                        RemoveRow(obj);
                    } else {
                        c$.alert(messageBox[data.Status ? 1 : 0]);
                    }
                }
            });
            return false;
        }
    } else {
        RemoveRow(obj);
    }
}
function RemoveRow(obj) {
    $(obj).parent().parent().remove();
    $("#tableDetail tr").each(function (trindex, tritem) {
        $(tritem).find("td").eq(0).html(trindex + "");
    });
}

//
function ChangeCustomerGranted(obj) {
    if ($(obj).IsChecked()) {
        $("#cbxIsOpened").attr("disabled", "disabled");
        $("#cbxIsOpened").get(0).checked = true;
    } else {
        $("#cbxIsOpened").removeAttr("disabled");
    }
}

/*========================加载列表==============================*/
function InitData(isRefresh) {
    var url = 'Admin/Permission' + window.SearchConfig.PermissionType == "Api" ? "/Api" : "";
    $.ajaxBfeData({
        url: url, data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}
/*========================保存==============================*/
function SaveData(obj) {
    var check1 = c$.checkForm(this.form);
    var check2 = true;
    var detailStr = "";
    if ($("#tableDetail").length > 0) {
        $("#error7").empty();
        $("#error7").hide();
        var result = [];
        var errorMsg = [];
        var length = $("#tableDetail tr").length - 1;
        $("#tableDetail tr").each(function (trindex, tritem) {
            if (trindex > 0 && trindex < length) {
                var json = $(this).BfeSerialize();
                result.push(json);
                var message = [];
                if (isNullOrEmpty(json.Name)) {
                    message.push("名称");
                }
                if (isNullOrEmpty(json.Code)) {
                    message.push("权限码");
                }
                if (isNullOrEmpty(json.Order)) {
                    json.Order = 0;
                }
                var isLetter = json.Code.isLetter();
                var msg1 = message.length > 0 ? message.join("、") + "不能为空" : "";
                if (!isLetter && message.length > 0) {
                    errorMsg.push("第" + trindex + "行", msg1 + "，权限码输入不正确！</font><br/>");
                }
                else if (!isLetter) {
                    errorMsg.push("第" + trindex + "行", "权限码输入不正确！</font><br/>");
                }
                else if (message.length > 0) {
                    errorMsg.push("第" + trindex + "行", msg1 + "！</font><br/>");
                }
            }
        });
        if (errorMsg.length > 0) {
            check2 = false;
            $("#error7").html("<font color='red'>系统页面权限明细验证不通过：</font><br/><font color='red'>" + errorMsg.join("<font color='red'>"));
            $("#error7").show();
        } else {
            $("#error7").hide();
        }
        detailStr = $.toJSON(result);
    }
    if (!check1 || !check2) { $("#dtips").show(); return false; }
    var postObject = $("#formMain").BfeSerialize();
    postObject.FunctionPermissionSummary = detailStr;
    var url1 = 'Admin/Permission/' + (window.SearchConfig.PermissionType == "Api" ? "ApiSave" : "Save");
    $(this).ajaxBfeData({
        url: url1, data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                c$.alert("保存成功！");
                InitData();
                //关闭弹出框
                $(obj).Close();
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}