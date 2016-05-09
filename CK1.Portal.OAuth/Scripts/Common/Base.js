
/// 名称: 判断对象是否为空
function isUndefinedOrNull(o) {
    return typeof (o) == "undefined" || o == null;
}

/// 名称: 判断字符串是否为空
function isNullOrEmpty(s) {
    return s == undefined || s == null || s.Trim().length == 0;
}

/// 名称: 转换空字符串
function nullToEmpty(s) {
    return isNullOrEmpty(s) ? emptyString : s.Trim();
}

(function ($) {
    /**
    * 把对象或者对象下的子元素序列化成数组
    * 序列化的结果形式如: [{name:'元素的name的值', value:'元素的value的值'}, {name:'name', value:'value'}]
    * 需要对值encodeURI的元素,需加上 encodeurl='true' 属性
    * @return {Array} 子元素序列化后的数组
    *
    * @example
    *    <form id='myform' action=''>
    *        用户名：<input type='text' name='userName' value='myname'/><br/>
    *        密码：<input type='text' name='password' value='mypass'/><br/>
    *    </form>
    *    <script>
    *        $('#myform').BaseBfeSerialize();   //返回数组: [{ 'name':'userName', 'value':'myname'}, { 'name':'password', 'value':'mypass'}]
    *    </script>
    */
    $.fn.BaseBfeSerialize = function () {
        return this.map(function () {
            // 查找出此元素,或者此元素的子元素(此元素为form时)
            return this.elements ? jQuery.makeArray(this.elements) : this;
        })
		.filter(function () { // 过滤出需提交的元素,排除掉button等
		    return this.name && (this.checked || /select|textarea/i.test(this.nodeName) ||
					/text|hidden|password|search/i.test(this.type));
		})
		.map(function (i, elem) {
		    var val = jQuery(this).val();
		    var encodeurl = typeof ($(elem).attr("encodeurl")) == 'undefined' ? "false" : "" + $(elem).attr("encodeurl");
		    return val == null ? null :
				jQuery.isArray(val) ?
					jQuery.map(val, function (val, i) {
					    return { name: elem.name, value: val };
					}) : encodeurl.toLowerCase() == 'true' ?
					{ name: elem.name, value: encodeURI(val) } : { name: elem.name, value: val };
		}).get();
    };
    //$("#OrderDetail tr").BfeSerializeArray();
    $.fn.BfeSerializeArray = function () {
        var result = [];
        $(this).each(function (trindex, tritem) {
            if (trindex > 0) {
                var json = $(this).BfeSerialize();
                result.push(json);
            }
        });
        return $.toJSON(result);
    };
    /**
    * 获取容器内(包括容器自身)数据,返回值为 {name:value, ...}
    * @return {Object} 自身及子元素的 name 和 value 组成的 json对象
    *
    * @example
    *    <form id='myform' action=''>
    *        用户名：<input type='text' name='userName' value='myname'/><br/>
    *        密码：<input type='text' name='password' value='mypass'/><br/>
    *    </form>
    *    <script>
    *        $('#myform').BfeSerialize();   //返回对象: { 'userName':'myname', 'password':'mypass'}
    *    </script>
    */
    $.fn.BfeSerialize = function () {
        var data = {};
        // 选择容器下的所有可提交子元素,及容器自身
        var elements = this.find("input,textarea,select").add(this).filter("input,textarea,select");
        var elements1 = elements.not("input[type=checkbox],select").BaseBfeSerialize();;
        $.each(elements1, function (i, o) {
            // 如果有多个同名的元素,用逗号隔开各值
            data[o.name] = data.hasOwnProperty(o.name) ? (data[o.name] + "," + $.trim(o.value)) : $.trim(o.value);
        });
        // 对checkbox,select的处理
        var elements2 = elements.filter("input[type=checkbox],select").BaseBfeSerialize();
        $.each(elements2, function (i, o) {
            var value = o.value.Trim();
            if (value == "on" || value == "true" || value == 'yes') {
                data[o.name] = true;
            }
            else {
                data[o.name] = data.hasOwnProperty(o.name) ? (data[o.name] + "," + value) : value;
            }
        });
        return data;
    };
    /**
    * 刷新表单
    isClearhidden: 是否连隐藏域的也清空
    * @example
    *   
    */
    $.fn.clearInputs = function (isClearhidden) {
        $("input[type=text],input[type=password],textarea", this).val("");
        $("input[type=checkbox],input[type=radio]", this).each(function () {
            this.checked = false;
        });
        $("select", this).each(function () {
            this.selectedIndex = 0;
        });
        if (isClearhidden) $("input[type=hidden]", this).val("");
    };
    /**
    * CheckBox全选
    * @example
    *   $(this).CheckBoxSelect2('cbxgroup');
    */
    $.fn.CheckBoxSelect2 = function (name) {
        name = typeof name == "undefined" ? $(this).val() : name;
        if ($(this).get(0).checked) {
            $("input[type=checkbox][name=" + name + "]").each(function () { this.checked = true; });
        }
        else {
            $("input[type=checkbox][name=" + name + "]").each(function () { this.checked = false; });
        }
    };

    /**
 *CheckBox全选 根据attribute选择器全选全不选
 * @example
 *   $(this).CheckBoxSelect3(attr1=abc);
 */
    $.fn.CheckBoxSelect3 = function (attrSelector) {
        if ($(this).get(0).checked) {
            $("input[type=checkbox][" + attrSelector + "]").each(function () {
                if ($(this).attr("disabled") != "disabled")
                {//非禁用状态才执行
                    this.checked = true;
                    $(this).change();
                }
            });
        }
        else {
            $("input[type=checkbox][" + attrSelector + "]").each(function () {
                if ($(this).attr("disabled") != "disabled")
                {//非禁用状态才执行
                    this.checked = false;
                    $(this).change();
                }
            });
        }


    };


    $.fn.ChangePermission = function (currentPermissionSelector, permissionsSelector) {
        var permissions = $(permissionsSelector).val();
        var permission = "," + $(currentPermissionSelector).val() + ",";

        if (permission != ",,") {
            if ($(currentPermissionSelector).get(0).checked) {
                if (permissions.indexOf(permission) < 0) {//添加新权限
                    $(permissionsSelector).val(permissions + $(currentPermissionSelector).val() + ",");
                }
            }
            else {//移除原有权限
                $(permissionsSelector).val(permissions.replaceAll(permission, ","));
            }
        }
    };


    /**
*CheckBox 判断group 成员 是否有选中来选中来选中groupparent
* @example
*   $(this).CheckBoxSelect3(attr1=abc,parent=efg);
*/
    $.fn.CheckBoxSelect4 = function (attrSelector, groupParent, permissionSelector) {
        if ($(this).get(0).checked) {
            //当前成员选中，选择中groupparent
            $("input[type=checkbox][" + groupParent + "]").each(function () {
                this.checked = true;
                $.fn.ChangePermission(this, permissionSelector);
            });

        }
        else {
            var needToSelectParent = false;
            $("input[type=checkbox][" + attrSelector + "]").each(function () {
                if (this.checked == true) {
                    needToSelectParent = true;
                }
            });

            if (needToSelectParent) {
                //如果子集有选中则选中父级
                $("input[type=checkbox][" + groupParent + "]").each(function () {
                    this.checked = true;
                    $.fn.ChangePermission(this, permissionSelector);
                });
            }
        }
    };



    /**
    * CheckBox全选，查找某一范围内的name=X的复选框，并设置是否选中
    * @example
    *    <input type="checkbox"  onclick="$('#accordion2').CheckBoxSelect('cbxgroup_1',this.checked);" />全选
    */
    $.fn.CheckBoxSelect = function (name, checked) {
        var checkBoxs = $("input[type=checkbox][name=" + name + "]", $(this)).not(':disabled'); // 选取 checkbox元素
        if (typeof (checked) == 'undefined') {
            checked = !checkBoxs.get(0).checked; // 没有checked参数时，以自己的checked为准
        }
        checkBoxs.each(function () {
            this.checked = checked;
        });
    };
    /**
    * 获取CheckBox的值,注意返回的是值的数组
    * @example
    *    $("input[name=cbxgroup]").getCheckBoxValue();
    */
    $.fn.getCheckBoxValue = function () {
        var c = [];
        this.each(function () {
            if (this.checked) c.push(this.value);
        });
        return c;
    };
    /**
    * 获取Listbox的值,注意返回的是值的数组
    * @example
    *    获取所有项： $("input[name=ReceivedCount]").getTextSum();
    */
    $.fn.getTextSum = function () {
        var sum = 0;
        this.each(function () {
            sum = sum + (isNullOrEmpty(this.value) ? 0 : parseInt(this.value));
        });
        return sum;
    };
    /**
    * 获取Listbox的值,注意返回的是值的数组
    * @example
    *    获取所有项：$("#lstUserBox option").getListBoxValue();
    *    获取选中项：$("#lstUserBox option:selected").getListBoxValue();
    */
    $.fn.getListBoxValue = function () {
        var c = [];
        this.each(function () {
            c.push(this.value);
        });
        return c;
    };
    /**
    * 获取Listbox的值,注意返回的是text的数组
    * @example
    *    获取所有项：$("#lstUserBox option").getListBoxText();
    *    获取选中项：$("#lstUserBox option:selected").getListBoxText();
    */
    $.fn.getListBoxText = function () {
        var c = [];
        this.each(function () {
            c.push(this.text);
        });
        return c;
    };
    /**
    * 是否选中,可应用于checkbox及radio
    * all: 为true时,需要有所有都选中时才返回true；为false时只要有任意一个选中即返回true, 默认为false
    * @example
    *
    */
    $.fn.IsChecked = function (all) {
        if (this.length <= 0) return false; // 没有元素时,返回false
        var isbool = !!all; //all为true时,isbool为true； all为false或者为空时,isbool为false
        this.each(function () {
            if (all) {
                isbool = isbool && this.checked;
            }
            else {
                isbool = isbool || this.checked;
            }
        });
        return isbool;
    };
    /**
   * 绑定自定义页码回车
   * @example  $("#divmain input").BindEnter(SearchInfo);
   *
   */
    $.fn.BindEnter = function (func) {
        // 回车键模仿点击提交查询操作.
        $(this).keydown(function (event) {
            event = event || window.event;
            var charCode = event.charCode || event.keyCode;
            if (13 === charCode) {
                if (typeof func != 'undefined' && func instanceof Function) {
                    func && func();
                }
            }
        });
    };
    /**
   * 按钮禁用
   * @example  $("#button1").Disable();
   */
    $.fn.Disable = function () {
        $(this).attr("disabled", "disabled");
    };
    /**
   * 按钮启用
   * @example  $("#button1").Enable();
   */
    $.fn.Enable = function () {
        $(this).removeAttr("disabled");
    };
    /**
    * 判断是否存在
    * @example
    *   $("#div").Exists();
    */
    $.fn.Exists = function () {
        return (this.length > 0);
    };

    /**
    * 初始化表格
    * @example
    *    获取所有项：$(".extTable").InitTable();
    */
    $.fn.InitTable = function () {
        if ($(this).length > 0) {
            var height = document.documentElement.clientHeight - 120;
            $(this).wrap("<div class='flexigrid'><div class='bDiv' style='height: " + height + "px;'></div></div>");
        }
    };

    /**
    * 发送ajax请求
    * 参数与下面的 $.ajaxBfeData 一样
    *
    * @example
    *    $('#formid').ajaxBfe({
    *        url: 'submit.htm',
    *        data: {na:1, id:33, key:'广州'},
    *        dataType: 'json',
    *        success: function(data, textStatus){ c$.alert('提示','执行成功啦!'); }
    *    });
    */
    $.fn.ajaxBfe = function (option) {
        option = option || {};
        option.data = option.data || $(this).BfeSerialize();
        option.btn = option.btn || $(this).find(".submit");
        option.isloading = isUndefinedOrNull(option.isloading) ? true : option.isloading;
        option.msg = isUndefinedOrNull(option.msg) ? "正在处理，请稍等..." : option.msg;
        return $.ajaxBfeData(option);
    };

    /**
    * 发送ajax请求
    * 参数与下面的 $.ajaxBfeData 一样
    *
    * @example
    *    // 与 $.ajaxBfeData 唯一的不同是,可以用容器选择那个提交按钮,提交时让那按钮不可重复点
    *    $('#submitBtn').ajaxBfeData({
    *        url: 'submit.htm',
    *        data: {na:1, id:33, key:'广州'},
    *        dataType: 'json',
    *        success: function(data, textStatus){ c$.alert('提示','执行成功啦!'); }
    *    });
    */
    $.fn.ajaxBfeData = function (option) {
        option = option || {};
        option.btn = option.btn || this;
        option.isloading = typeof (option.isloading) == 'undefined' ? true : option.isloading;
        option.msg = isUndefinedOrNull(option.msg) ? "正在处理，请稍等..." : option.msg;
        return $.ajaxBfeData(option);
    };

    /*
    处理ajax response.redirect login的问题
    */
    $.fn.CheckServerRedirect = function(data) {
        if (typeof data === "string" && data.indexOf('<meta name="CK1-Portal-LoginPage">') > -1) {
            window.top.location.href = window.ServiceAppPath + "Account/Login";
            return true;
        }
        return false;
    };


    /**
    * 发送ajax请求
    * @param option:请求的参数(参考 jQuery 的),里面包含 {
    *   btn: {HTMLElement | String} 提交的按钮(发送请求时将会不可用,请求返回后恢复)
    *   isloading: {Boolean} 请求时显示"正在处理..."信息,默认为 true
    *   url: {String} 请求的地址(默认: 当前页地址)
    *   data: {Object | String} 请求的数据,将自动转换为请求字符串格式,必须为 Key/Value 格式。
    *   type: {String} 请求方式 (默认: "GET", 可选值为"POST" 或 "GET")， 
    *   dataType: {String} 预期服务器返回的数据类型。可选值:"xml","html","script","json","jsonp","text"(默认是智能判断的)
    *   success: {Function} 请求成功后的回调函数。
    *   其它: 传递给 jQuery.ajax 的其它参数,参考 jQuery.ajax 函数
    * }
    * 注意：全局变量 window.ServiceAppPath 指定网站的绝对路径
    *
    * @example
    *    $.ajaxBfeData({
    *        btn: '#submitBtn',
    *        url: 'submit.htm',
    *        data: {na:1, id:33, key:'广州'},
    *        dataType: 'json',
    *        success: function(data, textStatus){ c$.alert('提示','执行成功啦!'); },
    *        error: function(XMLHttpRequest, textStatus, errorThrown){ c$.alert('提示','后台出错啦，请联系开发者!'); },
    *        complete: function(XMLHttpRequest, textStatus){ c$.alert('提示','执行完成啦!'); }
    *    });
    */
    $.ajaxBfeData = function (option) {
        //$.ajaxBfeData = function(url, data, type, dataType, btn, isloading, success) {
        option = option || {};
        // url 如果不是 window.ServiceAppPath 开头,在加上 window.ServiceAppPath
        if (window.ServiceAppPath && option.url && option.url.charCodeAt(0) !== 47) {
            option.url = window.ServiceAppPath + option.url;
        }

        // 取出所需参数,并释放
        var btn = option.btn;
        var isloading = typeof option.isloading == 'undefined' ? true : option.isloading;
        var complete = option.complete; // 请求后的事件
        var success = option.success;
        delete option.btn;
        delete option.isloading;
        delete option.complete; // 需要覆盖此事件,故删除
        delete option.success;

        // 发送请求前的动作(按钮不可点击,加上"正在处理..."的显示信息)
        if (btn) {
            btn = $(btn);
            btn.attr("disabled", true);
        }
        //  $("#loading").css("display", "block");
        if (isloading) {
            try {
                $.blockUI({
                    fadeIn: 0,
                    fadeOut: 0,
                    showOverlay: false,
                    message: option.msg,
                    centerY: 0,
                    css: {
                        border: '#6198dd 1px solid',
                        padding: '30px 0px 0px 0px',
                        width: '240px',
                        height: '72px',
                        position: 'fixed',
                        color: '#6a6868',
                        top: '50%', left: '50%',
                        margin: '-31px 0 0 -120px',
                        background: "url(" + window.ServiceAppPath + "Content/images/loading.gif) no-repeat 50% 28% #f4f9fe"
                    }
                });
            }
            catch (e) { }
        }

        // 要执行的参数
        var sendOption = {
            dataType: "html",
            cache: false,
            // 请求返回后,恢复ui
            complete: function (XHR, TS) {
                var data = XHR.responseText;
                if (!$.fn.CheckServerRedirect(data)) {
                    if (btn) btn.attr("disabled", false);
                    if (isloading) {
                        try { $.unblockUI(); } catch (e) { }
                    }
                    if (complete) complete(XHR, TS);
                }
            },
            success: function (data) {
                if (!$.fn.CheckServerRedirect(data)) {
                    if (success) {
                        success(data);
                    }
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                var data = xmlHttpRequest.responseText;
                if (isNullOrEmpty(data)) {
                    return;
                }
                if (!$.fn.CheckServerRedirect(data)) {
                    var html = $(data);
                    var error = "系统维护中，请稍后再试!";
                    if (html.find("#errorMsg").length > 0) {
                        error = html.find("#errorMsg").eq(0).html();
                    }
                    $(this).showMsgDailog("异常提示", error, 500, 50);
                }
            }
        };
        // 添加要执行的参数
        for (var property in option) {
            sendOption[property] = option[property];
        }
        // 发送请求
        $.ajax(sendOption);
        return false;
    };

    /**
    * 给from表单所有控件根据导出参数outputConfig赋值
    * @example
    *    $.FormSubmit("form1",window.ServiceAppPath + 'Home/DownLoad',SearchConfig);
    */
    $.FormSubmit = function (formname, url, outputConfig, target) {
        if (isNullOrEmpty(target)) {
            target = "_blank";
        }
        var html = "";
        var download = $("#" + formname);
        if (download.length < 1) {
            $("body").append("<form id='" + formname + "' action='' method='post' target='" + target + "' style='display:none;'></form>");
            download = $("#" + formname);
        } else {
            download.empty();
        }
        if (!isUndefinedOrNull(outputConfig)) {
            $.each(outputConfig, function (key, value) {
                html += "<input type=\"hidden\" id=\"" + key + "\" name=\"" + key + "\" value=\"" + value + "\" />";
            });
            download.html(html);
        }
        var form = download[0];
        form.action = url;
        form.submit();
    };
})(jQuery);
