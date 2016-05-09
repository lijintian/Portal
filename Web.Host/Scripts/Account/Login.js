$('.weixin').popover({
    content: "<img src='/Content/Images/0_c5c571.jpg' style='width:129px;height:129px;max-width:129px;' /><br><span style='font-size:10px;'>关注我们，获取最新优惠</span>",
    trigger: "hover",
    html: true
});
window.onAmazonLoginReady = function () {
    amazon.Login.setClientId('@Html.GetAppSetting(AppSettingKey.AMAZON_LOGIN_CLIENTID)');
};
(function (d) {
    var a = d.createElement('script'); a.type = 'text/javascript';
    a.async = true; a.id = 'amazon-login-sdk';
    a.src = 'https://api-cdn.amazon.com/sdk/login1.js';
    d.getElementById('amazon-root').appendChild(a);
})(document);
document.getElementById('LoginWithAmazon').onclick = function () {
    options = { scope: 'profile' };
    amazon.Login.authorize(options, '@Html.GetAppSetting(AppSettingKey.AMAZON_LOGIN_AUTHORIZE_URL)');
    return false;
};
$(function () {
    var timing = setInterval(autoRoll, 3000);
    function autoRoll() {
        $(".roll-notice").find("ul:first").animate({ marginTop: "-40px" }, 800, function () {
            $(this).css({ marginTop: "0px" }).find("li:first").appendTo(this);
        });
    }
    $(".roll-notice").hover(function () {
        clearInterval(timing);
    }, function () {
        timing = setInterval(autoRoll, 3000);
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
        UserName: $.trim($("#UserName").val()),
        PassWord: $.trim($("#PassWord").val())
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
    if ($("#hidCheckCode").length > 0) {
        var code = $.trim($("#Code").val());
        if (code == "") {
            obj.html("<ul><li>请输入验证码</li></ul>");
            obj.show();
            return false;
        }
    }
    obj.hide();
    $.ajax({
        type: 'POST', url: window.ServiceAppPath + 'Account/RefreshToken', dataType: 'Json',
        success: function (data) {
            if (!data.Status) {
                $("#LoginForm input[name='__RequestVerificationToken']").val(data.Data);
            }
            $("#LoginForm").submit();
        }
    });
    return true;
}