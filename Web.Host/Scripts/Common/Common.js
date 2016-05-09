/*
* 前台一些要用到的公有函数
*/
var Tab$ = {};
$(function () {
    Tab$.FormatToLocalTime();
    Tab$.SetMenuHeight();
});

$(window).resize(function () {
    var iframe = $("#mainFrame");
    if (iframe.length > 0) {
        Tab$.SetHeight(document.frames('mainFrame'));
    }
    Tab$.SetMenuHeight();
});

Tab$.FormatToLocalTime = function () {
    $("[utc]").each(function () {
        Tab$.GetLocalTime(this);
    });
};
//注册菜单点击事件
Tab$.BindMenu = function () {
    $("#mainMenu li a[name=navMenu]").bind("click", function () {
        var aObj = $(this);
        var url = aObj.attr("href");
        if (isNullOrEmpty(url)) {
            return false;
        }
        Tab$.SetTitle(aObj);
        Tab$.SetHeight(document.frames('mainFrame'));
        return true;
    });
    var src = $("#mainFrame").attr("src");
    if (src != "" && !src.equalWith("/Home/Index2")) {
        var find = Tab$.SetHeadNavigation(src, "?");
        if (!find) {
            Tab$.SetHeadNavigation(src, "&");
        }
    }
};

Tab$.SetTitle = function (aObj) {
    if (typeof (aObj.attr("parentid")) != "undefined") {
        var firstId = aObj.attr("firstid");
        var firstObj = $("#mainMenu li.open");
        if (firstObj.length > 0) {
            firstObj.removeClass("open");
        }
        firstObj = $("#mainMenu li.active");
        if (firstObj.length > 0) {
            firstObj.removeClass("active");
        }
        firstObj = $("#mainMenu li[Id=Li" + firstId + "]");
        firstObj.addClass("active");
        firstObj.addClass("open");
    } else {
        $("#mainMenu li.active").removeClass("active");
    }
    aObj.parent().addClass("active");

    var html = '';
    var parentId = aObj.attr("parentid");
    //var title = "";
    var hasParentId = typeof (parentId) != "undefined";
    var parentNode = [];
    parentNode.push(aObj.attr("title"));
    while (hasParentId) {
        var parentObj = $("#mainMenu [Id=" + parentId + "]");
        parentId = parentObj.attr("parentid");
        //title = parentObj.attr("title");
        hasParentId = typeof (parentId) != "undefined";
        if (hasParentId) {
            parentObj.addClass("open");
        }
        var ulObj = $(".submenu", parentObj);
        if (ulObj.length > 0) {
            ulObj.addClass("nav-show");
            ulObj.show();
        }
        parentNode.push(parentObj.attr("title"));
    }
    var count = parentNode.length - 1;
    for (var i = 0; i <= count; i++) {
        html = '<li class="active">' + parentNode[i] + '</li>' + html;
    }
    html = '<li><i class="ace-icon fa fa-home home-icon"></i><a href="' + window.ServiceAppPath + 'Home/Index">首页</a><span class="divider"><i class="ace-icon glyphicon glyphicon-angle-right"></i></span></li>' + html;
    $("#divTitle").html(html);
};

//设置iframe自适应高度
Tab$.SetHeight = function (iframe) {
    iframe = $(iframe);
    var top = iframe.offset().top;
    var bottom = iframe.parent().css("padding-bottom").replace("px", "");
    var iframeHeight = $(window).height() - top - bottom - 10;
    iframe.height(iframeHeight);
};
Tab$.SetMenuHeight = function () {
    var obj = $("#sidebar");
    var height = $(window).height() - 50;
    obj.height(height);
};
Tab$.SetHeadNavigation = function (url, str) {
    var temp = url + str + "portalFrame=1";
    var result = false;
    $("#mainMenu li a[name=navMenu]").each(function () {
        var aObj = $(this);
        var rel = aObj.attr("rel");
        var exists = false;
        if (rel != "") {
            if (temp.equalWith(rel)) {
                exists = true;
            }
            var temp2 = temp.replace(":80", "");
            if (temp2.equalWith(rel)) {
                exists = true;
            }
            if (exists) {
                Tab$.SetTitle(aObj);
                result = true;
                return result;
            }
        }
    });
    return result;
};

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
//绑定自定义页码回车
Tab$.BindPageEnter = function () {
    // 回车键模仿点击提交查询操作.
    $("#pageTurn input[name='txtPage']").keydown(function (event) {
        event = event || window.event;
        var charCode = event.charCode || event.keyCode;
        if (13 === charCode) {
            var btnObj = $(this).parent().parent().parent().find("a[name='gopager']");
            if (btnObj.length > 0) {
                btnObj.click();
            }
        }
    });
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

//去顶部
Tab$.GoToTop = function (isRefresh) {
    isRefresh = isRefresh || false;
    if (isRefresh) {
        var parentObj = window.parent;
        if (parentObj) {
            parentObj.scrollTo(0, 0);
        }
        parentObj = window.parent.parent;
        if (parentObj) {
            parentObj.scrollTo(0, 0);
        }
    }
};

/*========================添加事件==============================*/
Tab$.EditPwdDailog = function () {
    var url = window.ServiceAppPath + "Admin/Personal/Index";
    $(this).showDailog("修改密码", url, null, 420, 300, "Pwd");
};
/*========================保存==============================*/
function SaveDataPwd(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { $("#dtips").show(); return false; }
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
Tab$.GetLocalTime = function (element) {
    var utcTime = $(element).attr("utc");
    var localTime = "";
    if (moment(utcTime).isValid()) {
        var formatUtcTime = moment(utcTime).format("YYYY-MM-DDTHH:mm:ss") + "Z";
        var format = $(element).attr("utcformat");
        if (isNullOrEmpty(format)) {
            format = "YYYY-MM-DD HH:mm:ss";
        }
        localTime = moment(formatUtcTime).format(format);

        //如果时间等于默认时间则不显示
        var defaultUtcTime = "1900-01-01T00:00:00.000Z"
        var defalultLocalTime = moment(defaultUtcTime).format(format);
        if (moment(localTime).isSame(defalultLocalTime)) {
            return;
        }
    }

    $(element).val(localTime);
    $(element).text(localTime);
};
//批量导入弹出对话框
Tab$.ImportDailog = function (type, title) {
    var url = window.ServiceAppPath + "File/BatchImport?type=" + type;
    $(this).showDailog(title, url, null, 600, 130, 'Import');
}