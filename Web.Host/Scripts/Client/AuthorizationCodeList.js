//初始化页面
$(function () {
    $("#UserSource").chosen();
    $("#divFilter input").BindEnter(SearchInfo);
});

/*========================分页事件==============================*/
function AuthPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}

/*========================查询==============================*/
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    AuthPager(1);
}

/*========================加载列表==============================*/
function InitData(isRefresh) {
    $.ajaxBfeData({
        url: 'Admin/AuthorizationCode/Index', data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}

//返回外部应用管理
function Back() {
    $.FormSubmit("form1", window.ServiceAppPath + "Admin/DeveloperApp", null, "_self");
}
