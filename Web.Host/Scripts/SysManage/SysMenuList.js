/*========================添加事件==============================*/
function CreateDailog() {
    EditDailog2("");
}
/*========================编辑事件==============================*/
function EditDailog(id) {
    var node = treeObj.getNodeByParam("id", id, null);
    if (isNullOrEmpty(node.pId)) {
        return;
    }
    EditDailog2(id);
}

function EditDailog2(id) {
    $.ajaxBfeData({
        url: 'Admin/Menu/Detail', data: { Id: id }, type: 'post', dataType: 'html',
        success: function (data) {
            $("#menuDetail").html(data);
        }
    });
}
/*========================删除一项==============================*/
function DeleInfo() {
    var id = $("#hddatakey").val();
    if (id == "") {
        c$.alert("没有可删除的信息！");
        return false;
    }
    if (confirm('是否删除此信息?')) {
        $(this).ajaxBfeData({
            url: 'Admin/Menu/Delete', data: { Id: id }, type: 'post', dataType: 'Json',
            success: function (data) {
                if (data.Status) {
                    $("#hddatakey").val("");
                    InitData();
                } else {
                    c$.alert(data.ErrorMessage);
                }
            }
        });
        return false;
    }
}
/*========================加载列表==============================*/
function InitData() {
    var id = $("#hddatakey").val();
    $.ajaxBfeData({
        url: 'Admin/Menu', data: { Id: id, IsPostBack: true }, type: 'post', dataType: 'html',
        success: function (data) {
            $("#divcontent").html(data);
        }
    });
}

//应用系统改变
function ApplicationIdChanged(obj) {
    var check = c$.checkElement(obj);
    if (check) {
        $.ajaxBfeData({
            url: 'Admin/Menu/ApplicationIdChange',
            data: { Id: $("#hddatakey").val(), ApplicationId: $("#ApplicationList").val(), ParentId: $("#ParentList").val(), PermissionCode: $("#PermissionList").val() },
            type: 'post', dataType: 'Json',
            success: function (data) {
                $("#ParentList").empty();
                $("#ParentList").html(data.ParentSource);
                $("#PermissionList").empty();
                $("#PermissionList").html(data.PermissionSource);
                $("#ParentList_chosen").remove();
                $("#PermissionList_chosen").remove();
                $("#ParentList").show();
                $("#PermissionList").show();
                $("#ParentList").chosen();
                $("#PermissionList").chosen();
            }
        });
    }
}
/*========================保存==============================*/
function SaveData() {
    var checkresult = c$.checkForm(this.form);
    if (!checkresult) { $("#dtips").show(); return false; }
    var postObject = $("#detailForm").BfeSerialize();
    postObject.Target = postObject.Target ? 1 : 0;
    $(this).ajaxBfeData({
        url: 'Admin/Menu/Save', data: postObject, type: 'post', dataType: 'Json',
        success: function (data) {
            if (data.Status) {
                var msg = "菜单【" + postObject.Name + "】" + (postObject.Id == "" ? "创建" : "修改") + "成功";
                c$.alert(msg);
                $("#hddatakey").val(data.Data);
                InitData();
            } else {
                c$.alert(data.ErrorMessage);
            }
        }
    });
}