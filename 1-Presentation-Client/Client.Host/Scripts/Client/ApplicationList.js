//初始化页面
$(function () {
    $("#divFilter input").BindEnter(SearchInfo);
});

//分页事件
function ApplicationPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}

//查询
function SearchInfo() {
    window.SearchConfig.PageIndex = 1;
    window.SearchConfig.Name = $("#content1").val().Trim();
    window.SearchConfig.State = $("#StateSource").val();
    window.SearchConfig.Type = $("#TypeSource").val();
    InitData();
}

//添加事件
function CreateDailog() {
    $.FormSubmit("createForm", window.ServiceAppPath + 'Application/Detail', null, "_self");
}

//编辑事件
function OpenEdit(id, pageId) {
    $.FormSubmit("createForm", window.ServiceAppPath + "Application/Detail", { Id: id, BackPageId: pageId }, "_self");
}

//查看明细
function ViewDailog(id) {
    $.FormSubmit("viewForm", window.ServiceAppPath + "Application/ViewList?Id=" + id, null, "_self");
}

//取消
function Cancel() {
    var obj = $("#hddatakey");
    var backPageId = obj.attr("backpageid");
    var url = backPageId == "ViewList" ? "Application/ViewList?Id=" + obj.val() : "Application";
    $.FormSubmit("createForm", window.ServiceAppPath + url, null, "_self");
}
/*========================删除一项==============================*/
function DeleInfo(id) {
    if (confirm('是否删除此信息?')) {
        $(this).ajaxBfeData({
            url: 'Application/Delete', data: { Id: id }, type: 'post', dataType: 'Json',
            success: function (data) {
                c$.alert(data.ErrorMessage);
                if (data.Status) {
                    InitData();
                }
            }
        });
        return false;
    }
}
/*========================加载列表==============================*/
function InitData(isRefresh) {
    $.ajaxBfeData({
        url: 'Application/Index', data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}
//初始化审核数据
function InitAuditData() {
    var id = $("#hddatakey").val();
    $.ajaxBfeData({
        url: 'Application/Audit', data: { Id: id }, type: 'post', dataType: 'html',
        success: function (data) {
            $("#innerAudit").html(data);
        }
    });
}
//初始化查看页面
function InitFlowPage(step, data) {
    var json = eval('(' + data + ')');
    $("#orderFlow").loadStep({
        size: "large",
        color: "green",
        steps: json
    });
    $("#orderFlow").setStep(step);
}
//初始化查看页面
function InitViewPage(step, data) {
    var json = eval('(' + data + ')');
    $("#orderFlow").loadStep({
        size: "large",
        color: "green",
        steps: json
    });
    $("#orderFlow").setStep(step);
}
//全选权限
function CheckPermission(obj) {
    $('#AppPermission').CheckBoxSelect('Code', $(obj).IsChecked());
}
//保存
function SaveData() {
    var checkresult = c$.checkForm(this.form);
    var postObject = $("#detailForm").BfeSerialize();
    var check2 = CheckData(postObject);
    if (!checkresult || !check2) { return false; }
    postObject.AppType = postObject.cbxType;
    $(this).ajaxBfeData({
        url: 'Application/Save', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                $.FormSubmit("createForm", window.ServiceAppPath + 'Application/AuditView?Id=' + data.Data, { BackPageId: $("#hddatakey").attr("backpageid") }, "_self");
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}
//保存权限
function SubmitPermission() {
    var postObject = {};
    var code = GetCheckPermission();
    if (code.length <= 0) {
        c$.alert("请选择产品API！");
        return;
    }
    postObject.Id = $("#hddatakey").val();
    postObject.Codes = code.toString();
    $(this).ajaxBfeData({
        url: 'Application/SubmitPermission', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                InitAuditData();
            }
        }
    });
}
//获取选择的权限
function GetCheckPermission() {
    var c = [];
    $("#AppPermission input[name=Code]").not(':disabled').each(function () {
        if (this.checked) c.push(this.value);
    });
    return c;
};
//提交审核
function SubmitAudit(obj) {
    var postObject = {};
    postObject.Id = $("#hddatakey").val();
    $(this).ajaxBfeData({
        url: 'Application/SubmitAudit', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                $(obj).remove();
                $("#btnSubmit").hide();
                var perDiv = $("#AppPermission");
                $(".btn-primary", perDiv).remove();
                CheckBoxDisabled();
            }
        }
    });
}

function CheckData(postObject) {
    var result = true;
    var check1 = true;
    check1 = !isNullOrEmpty(postObject.cbxType);
    c$.InitMsg(check1, $("#divType"), "请选择应用类型");
    if (!check1) {
        result = false;
    }
    if ($("#ckAgree").length > 0) {
        check1 = $("#ckAgree").IsChecked();
        c$.InitMsg(check1, $("#divAgree"), "请确认是否阅读并同意 《ABC隐私申明》！！！");
        if (!check1) {
            result = false;
        }
    }
    return result;
}

function CheckBoxDisabled() {
    var perDiv = $("#AppPermission");
    perDiv.find("input[type=checkbox]").each(function () {
        $(this).attr("disabled", "disabled");
    });
}