//初始化页面
$(function () {
    $("#divFilter input").BindEnter(SearchInfo);
});

/*========================分页事件==============================*/
function SysUserPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}
/*========================查询==============================*/
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    SysUserPager(1);
}
function TypeChange() {
    var value = $("#SearchTypeSource").val();
    var obj = $("#txtKeyword");
    switch (value) {
        case "2":
            obj.attr("name", "EmployeeNo");
            break;
        case "3":
            obj.attr("name", "DisplayName");
            break;
        case "4":
            obj.attr("name", "Email");
            break;
        case "5":
            obj.attr("name", "PermissionCode");
            break;
        default:
            obj.attr("name", "LoginName");
            break;
    }
    obj.attr("placeholder", "请输入" + $("#SearchTypeSource").find("option:selected").text());
    $("#txtKeyword").val("");
    window.SearchConfig.LoginName = "";
    window.SearchConfig.EmployeeNo = "";
    window.SearchConfig.DisplayName = "";
    window.SearchConfig.Email = "";
    window.SearchConfig.PermissionCode = "";
    SearchInfo();
}
/*========================添加事件==============================*/
function CreateDailog() {
    var url = window.ServiceAppPath + "Admin/User/Detail";
    ShowUserDailog(url);
}
/*========================编辑事件==============================*/
function EditDailog(id, isApproved) {
    var url = window.ServiceAppPath + "Admin/User/Detail?UserId=" + id;
    if (isApproved == '1') {
        $(this).showDailog("API帐号信息管理", url, null, 460, 320);
    } else {
        ShowUserDailog(url);
    }
}

//导出
function Export() {
    $.FormSubmit("exportForm", window.ServiceAppPath + 'Admin/User/Export', window.SearchConfig);
}
//用户角色弹出框
function RoleDailog(id, name) {
    var typeName = window.SearchConfig.UserType == 'Customer' ? "客户帐号" : window.SearchConfig.UserType == 'Api' ? "API帐号" : "用户";
    var url = window.ServiceAppPath + "Admin/PermissionSetting/AsynUserRoleSetting?loginName=" + encodeURIComponent(name);
    $(this).showDailog("设置" + typeName + "【" + name + "】的角色", url, null, 700, 350, 'Role');
}
//用户权限弹出框
function PermissionDailog(id, name) {
    var typeName = window.SearchConfig.UserType == 'Customer' ? "客户帐号" : window.SearchConfig.UserType == 'Api' ? "API帐号" : "用户";

    var url = window.ServiceAppPath + "Admin/PermissionSetting/AsynUserPermissionSetting?loginName=" + encodeURIComponent(name);
    if (window.SearchConfig.UserType == 'Customer') {
        window.ServiceAppPath + "Admin/PermissionSetting/AsynCustomerPermissionSetting?loginName=" + encodeURIComponent(name);
    }
    if (window.SearchConfig.UserType == 'Api') {
        url = window.ServiceAppPath + "Admin/PermissionSetting/AsynApiUserPermissionSetting?loginName=" + encodeURIComponent(name);
    }
    $(this).showDailog("设置" + typeName + "【" + name + "】的权限", url, null, 700, 350, 'Permission');
}

function ShowUserDailog(url) {
    var footerhtml = '<input type="button" alt="取消" class="BtnCancel" Id="btnClose" value="取 消" />';
    footerhtml += '<input type="button" alt="保存关启用" class="BtnSend2" onclick="SaveData2(this);" value="保存并启用" />';
    footerhtml += '<input type="button" alt="保存" class="BtnSend" onclick="SaveData(this);" value="保 存" />';
    $(this).showOtherDailog("API帐号信息管理", url, null, 460, 320, footerhtml);
};

//克隆用户权限弹出框
function ClonePermissionDailog(userType, loginName) {
    var url = window.ServiceAppPath + "Admin/PermissionSetting/ClonePermissionSetting?loginName=" + loginName + "&userType=" + userType;
    $(this).showDailog("克隆用户权限设置", url, null, 400, 150, 'ClonePermission');
}


/*========================删除一项==============================*/
function DeleInfo(loginName, enabled) {
    var str = enabled == 1 ? "启用" : "禁用";
    if (confirm('是否' + str + '此信息?')) {
        $(this).ajaxBfeData({
            url: 'Admin/User/Delete', data: { loginName: loginName, State: enabled }, type: 'post', dataType: 'Json',
            success: function (data) {
                var messageBox = [str + "失败,请稍后再试！", str + "成功!", str + "失败！", str + "保存,该公司不存在！", str + "失败,超过上限不能启用用户！"];
                if (data.Status) {
                    InitData();
                }
                else if (!isNullOrEmpty(data.ErrorMessage)) {
                    c$.alert(data.ErrorMessage);
                }
                else {
                    c$.alert(messageBox[data.Status ? 1 : 0]);
                }
            }
        });
        return false;
    }
}
//重置密码
function Reset(loginName) {
    if (confirm('是否确定重置密码?')) {
        $(this).ajaxBfeData({
            url: 'Admin/User/ResetPassword', data: { loginName: loginName }, type: 'post', dataType: 'Json',
            success: function (data) {
                var messageBox = ["密码重置失败,请稍后再试！", "密码重置成功!"];
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

/*========================加载列表==============================*/
function InitData(isRefresh) {
    var url = 'Admin/User' + (isNullOrEmpty(window.SearchConfig.UserType) ? "" : '/' + window.SearchConfig.UserType);
    $.ajaxBfeData({
        url: url, data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}

//检查用户名称是否重复
function Check(obj) {
    var loginName = $(obj).val().Trim();
    if (loginName == "") {
        return;
    }
    $(this).ajaxBfeData({
        url: 'Admin/User/Check', data: { loginName: loginName }, type: 'post', dataType: 'Json',
        success: function (data) {
            if (!data.Status) {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}
//保存并启用
function SaveData2(obj) {
    $("#hdIsApproved").val("1");
    BaseSaveData(obj);
}
//保存
function SaveData(obj) {
    $("#hdIsApproved").val("0");
    BaseSaveData(obj);
}
//保存
function BaseSaveData(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { $("#dtips").show(); return false; }
    var postObject = $("#detailForm").BfeSerialize();
    postObject.IsApproved = $("#hdIsApproved").val() == "1";
    //var isUpdate = $("#hdIsUpdatePwd").val() == "1";
    //if (!isUpdate) {
    //    postObject.OldPassword = "";
    //    postObject.Password = "";
    //}
    $(this).ajaxBfeData({
        url: 'Admin/User/Save', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                InitData();
                //关闭弹出框
                $(obj).Close();
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}
//保存用户角色信息
function SaveDataRole(obj) {
    var postObject = $("#permissionForm").BfeSerialize();
    $(this).ajaxBfeData({
        url: 'Admin/PermissionSetting/SaveUserRole', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                //关闭弹出框
                $(obj).Close();
                InitData();
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}
//保存用户角色信息
function SaveDataPermission(obj) {
    var postObject = $("#permissionForm").BfeSerialize();
    $(this).ajaxBfeData({
        url: 'Admin/PermissionSetting/SaveUserPermission', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                if (window.SearchConfig.UserType == 'Api') {
                    var message = isNullOrEmpty(postObject.PermissionCode) ? "保存用户权限成功！" : "保存用户权限成功，前往【管理访问Token】页面可查看到为您生成的访问Token！";
                    c$.alert(message);
                }
                //关闭弹出框
                $(obj).Close();
                InitData();
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}

function SaveDataClonePermission(obj) {
    SaveClonePermission(obj);
}

/*
//修改密码
function UpdatePwd() {
    var isUpdate = $("#hdIsUpdatePwd").val();//0表示目前是不修改密码，1表示目前是修改密码
    if (isUpdate == "0") {
        $("#detailForm tr[name=trUpdatePwd]").show();
        $("#hdIsUpdatePwd").val("1");
    } else {
        $("#detailForm tr[name=trUpdatePwd]").hide();
        $("#hdIsUpdatePwd").val("0");
        var checkresult = c$.checkForm(this.form);
        if (!checkresult) { $("#dtips").show(); return false; }
    }
}

//检查原始密码
function CheckOldPassword() {
    var isUpdate = $("#hdIsUpdatePwd").val() == "1";
    var oldPassword = $("#txtOldPassword").val().Trim();
    if (isUpdate && oldPassword.length < 6) {
        return "请输入最少6位的原始密码";
    }
    return "";
}
//检查登陆密码
function CheckPassword() {
    var isUpdate = $("#hdIsUpdatePwd").val() == "1";
    var id = $("#hddatakey").val();
    var password = $("#txtPassword").val().Trim();
    if (isUpdate && password.length < 6) {
        return isNullOrEmpty(id) ? "请输入最少6位的密码" : "请输入最少6位的新密码";
    }
    return "";
}

//检查确认密码
function CheckAgainPassword() {
    var isUpdate = $("#hdIsUpdatePwd").val() == "1";
    var password = $("#txtPassword").val().Trim();
    var againPassword = $("#txtAgainPassword").val().Trim();
    if (isUpdate) {
        if (againPassword.length < 6) {
            return "请输入最少6位的确认密码";
        }
        if (password != againPassword) {
            return "两次密码输入不一致";
        }
    }
    return "";
}
*/