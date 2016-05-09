//初始化页面
$(function () {
    $("#divFilter input").BindEnter(SearchInfo);
});


/*========================查询==============================*/
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    window.SearchConfig.PageIndex = 1;
    InitData();
}

/*========================添加事件==============================*/
function CreateDailog() {
    var url = window.ServiceAppPath + "Admin/ApiGroup/Create";
    $(this).showDailog("创建API权限分组", url, null, 450, 160);
}

/*========================编辑事件==============================*/
function EditDailog(code) {
    var url = window.ServiceAppPath + "Admin/ApiGroup/Edit?Code=" + code;
    $(this).showDailog("编辑API权限分组", url, null, 450, 160);
}

/*========================编辑API权限分组权限==============================*/
function EditPermissionDailog(name, code) {
    window.SearchConfig.PermissionState = "edit";
    var url = window.ServiceAppPath + "Admin/ApiGroup/Permission?Code=" + code;
    $(this).showDailog("设置<font color='blue'>" + name + "</font>API权限分组权限", url, null, 700, 350, "Permission");
}
function ViewPermissionDailog(name, code) {
    window.SearchConfig.PermissionState = "view";
    var url = window.ServiceAppPath + "Admin/ApiGroup/Permission?Code=" + code;
    $(this).showViewDailog("查看<font color='blue'>" + name + "</font>API权限分组权限", url, null, 700, 350);
}
/*========================加载列表==============================*/
function InitData() {
    $.ajaxBfeData({
        url: 'Admin/ApiGroup', data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
        }
    });
}
//初始化设置权限页面
function InitPermissionPage() {
    GetApplicationApiPermission = function (obj, appId) {
        $.ajaxBfeData({
            isloading: false,
            url: window.ServiceAppPath + "Admin/PermissionSetting/AsynApiPermission",
            data: { "applicationId": appId, IsOpened: true },
            type: 'post',
            dataType: 'html',
            success: function (data) {
                $(obj).parents('.level1').find('.level2_1').html(data);
                $(this).attr("hasload", "true");

                var currenUserPermissions = $("#txtCurrentUserPermission").val();
                var currentUserRolePermissions = $("#txtCurrentUserRolePermission").val();
                if (window.SearchConfig.PermissionState == "view") {
                    $(obj).parents('.level1').find('.level2').find("input[type=checkbox]").each(function () {
                        $(this).attr("disabled", "disabled");
                    });
                } 
                if (currenUserPermissions != "") { //选中原有权限
                    $(obj).parents('.level1').find('.level2').find("input[type=checkbox]").each(function () {
                        if (currenUserPermissions.indexOf("," + $(this).val() + ",") > -1) {
                            this.checked = true;
                            if (currentUserRolePermissions.indexOf("," + $(this).val() + ",") > -1) {
                                //如果该权限当前用户的角色权限，灰掉
                                $(this).attr("disabled", "disabled");
                            }
                        }
                    });
                }
            }
        });
    };
    $("[applicationid]").each(function () {
        $(this).click(function () {
            if (!$(this).attr("hasload")) {
                GetApplicationApiPermission(this, $(this).attr("applicationId"));
            }
            ShowOrHide(this);
        });
    });
    //绑定checkbox change引起的权限变化
    BindPermissionChange("#permissionForm", "#txtCurrentUserPermission");
}

/*========================保存==============================*/
function SaveData(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { $("#dtips").show(); return false; }
    var postObject = $("#detailForm").BfeSerialize();
    $(this).ajaxBfeData({
        url: 'admin/ApiGroup/Save', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status) {
                InitData();
                //关闭弹出框
                $(obj).Close();
            }
        }
    });
}

function SaveDataPermission(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { $("#dtips").show(); return false; }
    var postObject = $("#permissionForm").BfeSerialize();
    if (postObject.PermissionCode == "") {
        c$.alert("请选择权限！");
        return;
    }
    $(this).ajaxBfeData({
        url: 'admin/ApiGroup/SavePermission', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            c$.alert(data.ErrorMessage);
            if (data.Status == true) {
                InitData();
                //关闭弹出框
                $(obj).Close();
            }
        }
    });
}