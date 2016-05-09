//初始化页面
$(function () {
    $("#UserSource").chosen();
    $("#divFilter input").BindEnter(SearchInfo);
});

/*========================分页事件==============================*/
function TokenPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}

/*========================查询==============================*/
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    TokenPager(1);
}

/*========================查看明细==============================*/
function ViewDailog(id) {
    $.FormSubmit("createForm", window.ServiceAppPath + "Admin/Token/View?Id=" + id);
}

/*========================加载列表==============================*/
function InitData(isRefresh) {
    $.ajaxBfeData({
        url: 'Admin/Token/Index', data: window.SearchConfig, type: 'post', dataType: 'html',
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


/*========================删除一项==============================*/
function DeleInfo(id, enabled) {
    var str = enabled == 1 ? "启用" : "禁用";
    if (confirm('是否' + str + '此信息?')) {
        $(this).ajaxBfeData({
            url: 'Admin/Token/Delete', data: { Id: id, State: enabled }, type: 'post', dataType: 'Json',
            success: function (data) {
                if (data.Status) {
                    InitData();
                }
                else {
                    c$.alert(data.ErrorMessage);
                }
            }
        });
        return false;
    }
}