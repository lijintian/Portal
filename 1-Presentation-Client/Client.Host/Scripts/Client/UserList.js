//初始化页面

//加载列表
function InitData(identity) {
    $.FormSubmit("viewForm", window.ServiceAppPath + 'User/View?identity=' + identity, null, "_self");
}

//修改
function Update() {
    $.FormSubmit("viewForm", window.ServiceAppPath + 'User/Detail', null, "_self");
}

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
        if (!checkresult) { return false; }
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

/*========================保存==============================*/
function SaveData(obj) {
    $(obj).Disable();
    var checkresult = c$.checkForm(this.form);
    var check2 = CheckData();
    if (!checkresult || !check2) { $(obj).Enable(); return false; }
    var postObject = $("#detailForm").BfeSerialize();
    var isUpdate = $("#hdIsUpdatePwd").val() == "1";
    if (!isUpdate) {
        postObject.OldPassword = "";
        postObject.Password = "";
    }
    var url = isNullOrEmpty(postObject.Id) ? "User/Registered" : 'User/Save';
    $(this).ajaxBfeData({
        url: url, data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            $(obj).Enable();
            if (data.Status) {
                InitData(postObject.LoginName);
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}

function CheckData() {
    var result = true;
    if ($("#ckAgree").length > 0) {
        result = $("#ckAgree").IsChecked();
        c$.InitMsg(result, $("#divAgree"), "请确认是否阅读并同意 《ABC隐私申明》！！！");
        if (!result) {
            result = false;
        }
    }
    return result;
}