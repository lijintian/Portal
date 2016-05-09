/*
* 前台一些要用到的公有函数
*/
var Tab$ = {};

//用户退出
Tab$.LogOff = function () {
    var url = window.ServiceAppPath + "Account/Logout";
    $.FormSubmit("offForm", url, null, "_self");
};

/**
* 初始化查询条件
* @example
*   Tab$.FilterCondition(window.SearchConfig,"divFilter");
*/
Tab$.FilterCondition = function (obj, filterDiv) {
    var filterObject = $("#" + filterDiv).BfeSerialize();
    $.extend(obj, filterObject);//合并对象，修改第一个对象
};

//获取跳转的页码
Tab$.GetPageIndex = function (obj) {
    var pageindex = $(obj).parent().parent().find("input[name='txtPage']").val();
    if (window.SearchConfig.PageIndex == "") {
        c$.alert("请输入跳转页码！");
        return 0;
    }
    return pageindex;
};

Tab$.CheckResultAll = function (obj, name) {
    name = isNullOrEmpty(name) ? "InfoResultID" : name;
    $(obj).CheckBoxSelect2(name);
};

Tab$.CheckResultAllByAttribute = function (obj, attrSelector) {
    $(obj).CheckBoxSelect3(attrSelector);
};

Tab$.CheckParent = function (obj, chilSelector, parentSelector, permissionSelector) {
    $(obj).CheckBoxSelect4(chilSelector, parentSelector, permissionSelector);
};


//列表全选事件
Tab$.CheckboxSelect = function () {
    $('table th input:checkbox').on('click', function () {
        var that = this;
        $(this).closest('table').find('tr > td:first-child input:checkbox')
            .each(function () {
                this.checked = that.checked;
                $(this).closest('tr').toggleClass('selected');
            });
    });
};

//返回顶部事件
Tab$.BackToTop = function () {
    var btnObj = $("#back-to-top");
    btnObj.hide();
    $(window).scroll(function () {
        if ($(window).scrollTop() > 100) {
            btnObj.fadeIn(1500);
        } else {
            btnObj.fadeOut(1500);
        }
    });
    btnObj.bind("click", function () {
        $('body,html').animate({ scrollTop: 0 }, 1000);
        return false;
    });
};

/*========================添加事件==============================*/
Tab$.EditPwdDailog = function () {
    var url = window.ServiceAppPath + "Admin/Personal/Index";
    $(this).showDailog("修改密码", url, null, 420, 300, "Pwd");
};
/*========================保存==============================*/
function SaveDataPwd(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { return false; }
    var postObject = $("#detailForm").BfeSerialize();
    $(this).ajaxBfeData({
        url: 'Admin/Personal/SavePwd', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Data == 1) {
                c$.alert("修改成功");
                $(obj).Close();  //关闭弹出框

            } else if (data.ErrorMessage) {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}