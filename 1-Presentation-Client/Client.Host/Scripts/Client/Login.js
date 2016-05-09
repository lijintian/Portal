//初始化页面
$(function () {
    $('div.validation-summary-errors').each(function () {
        $(this).find('ul').each(function () {
            if ($(this).find('li').length > 0 && $(this).find('li').text().length > 0) {
                $('div.validation-summary-errors').addClass("alert alert-block");
            }
        });
    });
});



function Keydown(event) {
    event = event || window.event;
    if (!event) return;
    var currentKey = event.charCode || event.keyCode;
    // 挡住页面上的 Enter 键，并执行指定程式
    if (13 == currentKey) {
        // 执行指定程式
        CheckLogin();
    }
}
/*========================登录==============================*/
function CheckLogin() {
    var loginParam = {
        UserName: $.trim($("#loginUserName").val()),
        PassWord: $.trim($("#loginPassWord").val()),
        Code: $.trim($("#Code").val())
    };
    var validObj = $(".validation-summary-errors");
    if (validObj.length > 0) {
        validObj.hide();
    }
    var obj = $("#warning");
    obj.empty();
    if (loginParam.UserName == "" && loginParam.PassWord == "") {
        obj.html("<ul><li>请输入用户名和密码</li></ul>");
        obj.show();
        return false;
    }
    else if (loginParam.UserName == "") {
        obj.html("<ul><li>请输入用户名</li></ul>");
        obj.show();
        return false;
    }
    else if (loginParam.PassWord == "") {
        obj.html("<ul><li>请输入密码</li></ul>");
        obj.show();
        return false;
    }
    if (loginParam.Code == "") {
        obj.html("<ul><li>请输入验证码</li></ul>");
        obj.show();
        return false;
    }
    obj.hide();
    var form = $("#LoginForm");
    form.submit();
    return true;
}