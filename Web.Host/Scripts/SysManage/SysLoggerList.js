//初始化页面
$(function () {
    $("#ApplicationSelect").chosen();
    $("#divFilter input").BindEnter(SearchInfo);
});
//分页事件
function SysLoggerPager(page) {
    window.SearchConfig.PageIndex = page;
    InitData(true);
}
//查询
function SearchInfo() {
    Tab$.FilterCondition(window.SearchConfig, "divFilter");
    //window.SearchConfig.ApplicationId = $("#ApplicationSelect").val();
    window.SearchConfig.Type = $("#TypeSelect").val();
    window.SearchConfig.Level = $("#LevelSelect").val();
    SysLoggerPager(1);
}
//查看明细
function ViewDailog(id) {
    var url = window.ServiceAppPath + "SysLogger/View?Id=" + id;
    $(this).showViewDailog("查看系统日志", url, null, 720, 330);
}
//加载列表
function InitData(isRefresh) {
    $.ajaxBfeData({
        url: 'SysLogger/Index', data: window.SearchConfig, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
            Tab$.GoToTop(isRefresh);
        }
    });
}
//保存
function SaveData(obj) {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { return checkresult; }
    var postObject = $("#detailForm").BfeSerialize();
    $(this).ajaxBfeData({
        url: 'SysLogger/Save', data: postObject, type: 'post', dataType: 'Json',
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