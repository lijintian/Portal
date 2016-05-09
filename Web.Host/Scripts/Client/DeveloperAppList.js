//初始化页面
$(function () {
    $("#UserSource").chosen();
    $("#divFilter input").BindEnter(SearchInfo);
});

/*========================分页事件==============================*/
function ApplicationPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}
/*========================查询==============================*/
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    window.SearchConfig.State = window.SearchConfig.StateSource;
    //window.SearchConfig.UserId = window.SearchConfig.UserSource;
    ApplicationPager(1);
}

//查看明细
function ViewDailog(id) {
    var url = window.ServiceAppPath + "Admin/DeveloperApp/View?Id=" + id;
    $(this).showViewDailog("查看应用", url, null, 700, 320);
}

//编辑事件
function EditDailog(id) {
    var url = window.ServiceAppPath + "Admin/DeveloperApp/Detail?Id=" + id;
    $(this).showDailog("编辑应用", url, null, 500, 320);
}

//审核
function AuditDailog(id) {
    var url = window.ServiceAppPath + "Admin/DeveloperApp/Audit?Id=" + id;
    var footerhtml = '<input type="button" alt="取消" class="BtnCancel" Id="btnClose" value="取 消" />';
    footerhtml += '<input type="button" alt="驳回" class="BtnSend" onclick="SaveReject(this);" value="驳 回" />';
    footerhtml += '<input type="button" alt="审批通过" class="BtnSend2" onclick="SaveApprove(this);" value="审批通过" />';
    $(this).showOtherDailog("审批应用", url, null, 700, 320, footerhtml);
}

//Token
function OpenToken(clientId) {
    var url = window.ServiceAppPath + "Admin/Token/Index?ClientId=" + clientId;
    $.FormSubmit("createForm", url, null, "_self");
}

//授权码
function OpenAuthCode(clientId) {
    var url = window.ServiceAppPath + "Admin/AuthorizationCode/Index?ClientId=" + clientId;
    $.FormSubmit("createForm", url, null, "_self");
}

/*========================加载列表==============================*/
function InitData(isRefresh) {
    $.ajaxBfeData({
        url: 'Admin/DeveloperApp/Index', data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}

//保存
function SaveData(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { return false; }
    var postObject = $("#detailForm").BfeSerialize();
    postObject.AppType = postObject.cbxType;
    $(this).ajaxBfeData({
        url: 'Admin/DeveloperApp/Save', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                $(obj).Close();
                InitData();
            }
        }
    });
}

//审核通过
function SaveApprove(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { return false; }
    $(this).ajaxBfeData({
        url: 'Admin/DeveloperApp/Approve', data: GetPostConfig(), type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                $(obj).Close();
                InitData();
            }
        }
    });
}

//驳回审请
function SaveReject(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { return false; }
    $(this).ajaxBfeData({
        url: 'Admin/DeveloperApp/Reject', data: GetPostConfig(), type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                $(obj).Close();
                InitData();
            }
        }
    });
}

//获取post提交参数
function GetPostConfig() {
    var postConfig = {
        Id: $("#hddatakey").val(),
        Remark: $("#Remark").val().Trim()
    };
    return postConfig;
}