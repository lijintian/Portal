// 用户树对象
var treeObj = null;
/**
* 加载用户树对象
* @param zNodes {Array} 用户树的数据
* 用法：
@{
var model = ViewBag.TreeSource as FrameWork.Model.TreeModel2;
string nodeTree = model.TreeSource;
}
<script type="text/javascript">
(function(){
var zNodes = [
@Html.Raw(nodeTree)
];
loadTree(zNodes);
})();
</script>
*/
function loadTree(zNodes) {
    var treetype = TreeConfig.TreeType;
    var asyncEnable = false;
    var setType = {};
    if (treetype == 1) {
        //一般的树形结构
        asyncEnable = true;
    }
    else if (treetype == 2) {
        //checkbox 选择框
        setType.enable = true;
        setType.chkStyle = "checkbox";
        setType.chkboxType = { "Y": "s", "N": "s" };
    }
    else {
        //radiobutton单选框
        setType.enable = true;
        setType.chkStyle = "radio";
        setType.radioType = "all";
    }
    var setting = {
        check: setType,
        data: {
            simpleData: {
                // 简单模式,一个列表显示出父子关系,数据不用写多级
                enable: true
            }
        },
        callback: {
            onClick: onClick
        },
        // 异步加载
        async: {
            enable: asyncEnable,
            url: window.ServiceAppPath + 'Tree/LoadChildTree',
            // 异步加载时需要自动提交父节点属性的参数
            autoParam: ["id"],
            otherParam: TreeConfig//["jsname", TreeConfig.JSName]
        }
    };
    // 加载树
    $.fn.zTree.init($("#treeList"), setting, zNodes);
    // 树对象
    treeObj = $.fn.zTree.getZTreeObj("treeList");
}

/**
* 返回被选中的组和用户
* 半选中状态的父组，以及已经被父组全选的所有子节点，都会被过滤掉
* @return {Object} 内容为: {users:[用户id列表], groups:[组id列表]}
*/
function getChecks() {
    // 返回值
    var ret = { users: [], userinfo: [], groups: [], groupsinfo: [] };
    var groupAll = []; // 保存全部子节点被选中的组id, 以免与他下面的子节点重复
    checkCount = treeObj.getCheckedNodes(true);
    if (!checkCount || checkCount.length <= 0) {
        return ret;
    }
    for (var i = 0; i < checkCount.length; i++) {
        var obj = checkCount[i];
        // 组
        if (obj.isParent) {
            // 组需要判断，根据 check_Child_State 属性,含义为： -1:没有子节点； 0：没有子节点被选中； 1：部分子节点被选中； 2：全部子节点被选中
            //            var state = obj.check_Child_State;
            //            if (state == 1) continue; // 部分子节点被选中的，去掉。保存它下面被选中的子节点们就够了。
            //            if (state == 2) {
            //                groupAll.push(obj.id); // 保存全部子节点被选中的组id,以免重复
            //            }
            if (groupAll.contains(obj.id)) continue; // 父组已经保存的，不用重复
            ret.groups.push(obj.id);
            ret.groupsinfo.push(obj.name);
        }
        // 用户
        else {
            if (groupAll.contains(obj.id)) continue; // 父组已经保存的，不用重复
            ret.users.push(obj.id);
            ret.userinfo.push(obj.name);
        }
    }
    return ret;
}
//点击展开节点
function onClick(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("treeList");
    zTree.expandNode(treeNode);
}
/*========================点击树节点引发的JS==============================*/
function ShowMsg(id) {
    TreeConfig.SelectID = id;
}
/*========================选择用户组树弹出对话框保存事件==============================*/
function TreeData(obj) {
    var type = TreeConfig.TreeType;
    var result = false;
    if (type == 1) {
        result = ClickData();
    }
    else if (type == 2) {
        result = CheckBoxData();
    }
    if (result) {
        $(obj).Close();
    }
}
/*========================获取CheckBox选中项==============================*/
function CheckBoxData() {
    var trees = getChecks();
    var ids = trees.users;
    var groupsid = trees.groups;
    if (trees.users.length == 0 && trees.groups.length == 0) {
        alert("请选择" + TreeConfig.TreeName);
        return false;
    }
    else {
        parent.InitText(trees.groups.toString(), trees.groupsinfo.toString());
    }
    return true;
}
/*========================获取单击事件选中项==============================*/
function ClickData() {
    var id = TreeConfig.SelectID;
    if (isNullOrEmpty(id)) {
        alert("请选择" + TreeConfig.TreeName);
        return false;
    }
    else {
        var node = treeObj.getNodeByParam("id", id, null);
        parent.InitText(id, node.name);
    }
    return true;
}
