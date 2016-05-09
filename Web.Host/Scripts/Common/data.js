/**
* 去除字符串的前后空格
* @return {String} 去除前后空格后的字符串
* @example  " dd dd ".Trim()  // 返回: "dd dd"
*/
String.prototype.Trim = function (direct) {
    return this.replace(new RegExp("(^[\\s　]+)|([\\s　]+$)", "g"), "");
};
/**
* 判断是否以子串开头
* @param  {String} sub 被判断的子串
* @return {Boolean} 以子串开头则返回 true，否则返回false
*/
String.prototype.startWith = function (sub) {
    if (this == sub || sub == '') return true;
    return this.length >= sub.length && this.slice(0, sub.length) == sub;
};
/**
* 判断字符串是否相等
* @example  "aa".equalWith("AA");
*/
String.prototype.equalWith = function(sub) {
    return this.toLowerCase() == sub.toLowerCase();
};
/**
* 判断是否以子串结尾
* @param  {String} sub 被判断的子串
* @return {String} 以子串结尾则返回 true，否则返回false
*/
String.prototype.endWith = function (sub) {
    if (this == sub || sub == '') return true;
    return this.length >= sub.length && this.slice(0 - sub.length) == sub;
};
/*
* 替换所有s1的字符串为s2
*/
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
};

/*调用方法 
var a = "I Love {0}, and You Love {1},Where are {0}!";
c$.alert('提示',a.Format("You","Me"););
*/
String.prototype.Format = function (str) {
    if (arguments.length == 0) return this;
    try {
        var arg = arguments;
        return this.replace(/\{(\d+)\}/g, function (a, b) { return arg[b] });
    }
    catch (ex) {
        for (var i = 0, s = this, n = arguments.length; i < n; i++) {
            s = s.split("{" + i + "}").join(arguments[i]);
        }
    }
    return s;
}

/**
* 获取字符长度
* @param  {Number} chsLength 一个非拉丁文(如中文)占多少个字符，默认为2个 (改数据库时需改这里)
* @return {Number} 字符长度
* @example "aa哈哈".chsLeng() // 返回: 6
*/
String.prototype.chsLeng = function (chsLength) {
    chsLength = (parseInt(chsLength) >= 0) ? parseInt(chsLength) : 2;
    //去除非拉丁文(如中文)的长度
    var noChsLength = this.replace(new RegExp('[^\x00-\xff]', 'gm'), "").length;
    //中文长度
    var chineseLength = (this.length - noChsLength) * chsLength;
    return noChsLength + chineseLength;
};

/**
* description : 按字节长度截取字符串,并添加后缀.
* @param len 需要截取的长度,字符串长度不足返回本身;
* @param alt 添加后缀(非必要),默认为空字符串;
* @return 返回截取后的字符串;
* @requires getLength;
*/
String.prototype.GetSubstring = function (len, alt) {
    var tempStr = this;
    if (this.chsLeng() > len) {
        if (!alt) {
            alt = "";
        }
        var i = 0;
        for (var z = 0; z < len; z++) {
            if (tempStr.charCodeAt(z) > 255) {
                i = i + 2;
            } else {
                i = i + 1;
            }
            if (i >= len) {
                tempStr = tempStr.slice(0, (z + 1)) + alt;
                break;
            }
        }
        return tempStr;
    } else {
        return this + "";
    }
}

/**
* 字符串格式化输出
* @param  {Object} value 格式化的对象内容(说明: 1. 属性名称区分大小写; 2. 没有匹配到到属性输出原始字符串。)
* @return {String} 格式化后的字符串
* @example  "#1 Name:#Name, Age:#Age".format({Name:"zhangsan", Age:23 }); // 返回："#1 Name:zhangsan, Age:23"
*/
String.prototype.format = function (value) {
    return this.replace(new RegExp('#\\w+', 'gi'), function (match) {
        var name = match.substring(1);
        return value.hasOwnProperty(name) ? value[name] : match;
    });
};
/**
* 检查字符串是否为整数
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.IsInt = function () {
    return (/^[1-9]\d*$/g.test(this.Trim()));
};
/**
* 检查字符串是否为email地址
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isLetter = function () {
    return /^[0-9a-zA-Z_]*$/g.test(this.Trim());
};
/**
* 检查字符串是否为email地址
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isEmail = function () {
    return new RegExp("^[a-z0-9][a-z0-9\\-_.]*[a-z0-9]+@(([a-z0-9]([a-z0-9]*[-_]?[a-z0-9]+)+\\.[a-z0-9]+(\\.[a-z0-9]+)?)|(([1-9]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))\\.(([\\d]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))\\.){2}([1-9]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))))$", 'gi').test(this.Trim());
};
/**
* 检查字符串是否为手机号码
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.IsMobile = function () {
    return /^(13[0-9]|15[0-9]|18[0-9])\d{8}$/.test(this.Trim());
};
//String.prototype.isMobile = function () {
//    return (/^(?:13\d|15\d|18\d)-?\d{5}(\d{3}|\*{3})$/.test(this.Trim()));
//}
/**
* 检查字符串是否为电话号码
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.IsTel = function () {
    //"兼容格式: 国家代码(2到3位)-区号(2到3位)-电话号码(7到8位)-分机号(3位)"
    return (/^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/.test(this.Trim()));
};
/**
* 检查字符串是否为日期格式(正确格式如: 2011-03-28 或者 11/3/28, 2011年03月28日, 20111028)
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isDate = function () {
    // 匹配检查
    var r = this.has("chinese") ?
this.match(/^(\d{1,4})(年)((0?[1-9])|(1[0-2]))月((0?[1-9])|([12]\d)|(3[01]))日?$/) : // 中文处理
this.match(/^(\d{1,4})(-|\/|\.)?((0?[1-9])|(1[0-2]))\2((0?[1-9])|([12]\d)|(3[01]))$/);
    if (r == null) return false;
    // 日期是否存在检查
    var d = new Date(r[1], r[3] - 1, r[6]);
    return ((d.getFullYear() == r[1] || d.getYear() == r[1]) && (d.getMonth() + 1) == r[3] && d.getDate() == r[6]);
};

/**
* 检查字符串是否为时间格式(正确格式如: 13:04:06 或者 21时5分10秒, 210521)
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isTime = function () {
    // 匹配检查
    var a = this.has("chinese") ?
this.match(/^(\d{1,2})([时時])(\d{1,2})分(\d{1,2})秒(\d+([毫微纳納诺諾皮可飞飛阿托]秒)?)?$/) : // 中文处理
this.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})([.]?\d+)?$/);
    if (a == null) return false;
    // 时间检查
    if (a[1] >= 24 || a[3] >= 60 || a[4] >= 60) return false;
    // 如果有“:”来分隔时间,则秒后面的数也要求有“.”来分隔
    if (a[2] == ':' && a[5] && a[5].indexOf('.') == -1) return false;
    // 验证成功
    return true;
};

/**
* 检查字符串是否为日期和时间格式 (正确格式如: 2003/12/05 13:04:06 或者 2001年10月20日10时5分30秒, 20110208230406)
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isDateTime = function () {
    var dateTimes = this.split(' ');
    // 中文时,可以不用空格隔开日期和时间
    if (dateTimes.length != 2 && this.indexOf('日') != -1) {
        dateTimes = this.split('日');
        dateTimes[0] += '日';
    }
    // 无符号时,可以不用空格隔开日期和时间
    if (dateTimes.length != 2 && this.indexOf(':') == -1
&& this.indexOf('-') == -1 && this.indexOf('/') == -1 && this.indexOf('.') == -1) {
        // 完整日期和时间
        if (this.length >= 14) {
            dateTimes[0] = this.substr(0, 8);
            dateTimes[1] = this.substr(8);
        }
            // 短日期和时间,认为日期部分为6位
        else {
            dateTimes[0] = this.substr(0, 6);
            dateTimes[1] = this.substr(6);
        }
    }
    // 英文时，必须空格隔开日期和时间
    if (dateTimes.length != 2) return false;
    return (dateTimes[0].isDate() && dateTimes[1].isTime());
};

/**
* 检查字符串是否为不带秒的日期和时间格式 (正确格式如: 2003/12/05 13:04 或者 2001年10月20日10时5分, 20110208230406)
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isNotSecondDateTime = function () {
    if (this.isDate()) {
        return true;
    }
    var obj = this.Trim() + ":00";
    return obj.isDateTime();
};
/**
* 检查字符串是否为URL地址
* @return {Boolean} 符合返回true,否则返回false (注:空字符串返回 false)
*/
String.prototype.isUrl = function () {
    var rep = "^((https|http|ftp|rtsp|mms)?://)+(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?"
        + "(([0-9]{1,3}\\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\\.[a-z]{2,6})"
        + "(:[0-9]{1,4})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    return new RegExp(rep).test(this.toLowerCase());
};

//获取字符长度 
function getByteLen(val) {
    val = val.Trim();
    return val.replace(/[^\x00-\xff]/g, "xx").length;
}
function getByteLen2(val) {
    return val.replace(/[^\x00-\xff]/g, "xx").length;
}

//---------------------------------------------------
// 日期格式化
// 格式 YYYY/yyyy/YY/yy 表示年份
// MM/M 月份
// W/w 星期
// dd/DD/d/D 日期
// hh/HH/h/H 时间
// mm/m 分钟
// ss/SS/s/S 秒
//---------------------------------------------------
Date.prototype.Format = function (formatStr, isadd) {
    var str = formatStr;
    var Week = ['日', '一', '二', '三', '四', '五', '六'];

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

    str = str.replace(/MM/, isadd == true ? c$.Appendzero(this.getMonth() + 1) : c$.Appendzero(this.getMonth()));
    str = str.replace(/M/g, isadd == true ? this.getMonth() + 1 : this.getMonth());

    str = str.replace(/w|W/g, Week[this.getDay()]);

    str = str.replace(/dd|DD/, c$.Appendzero(this.getDate()));
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, c$.Appendzero(this.getHours()));
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, c$.Appendzero(this.getMinutes()));
    str = str.replace(/m/g, this.getMinutes());

    str = str.replace(/ss|SS/, c$.Appendzero(this.getSeconds()));
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
};
//+---------------------------------------------------
//| 日期计算
//+---------------------------------------------------
Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's':
            return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n':
            return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h':
            return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd':
            return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w':
            return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q':
            return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm':
            return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y':
            return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
};

/**
* 添加时间
* @param {Number | Object} option 要添加的时间,需key/value格式(key忽略大小写),或者int格式(表示多少毫秒)
*  其中对应的写法为: {
*     year : 1, // 缩写为 y, 加上多少年, 想减少1年则写 -1
*     month : 2, // 缩写为 m, 加上多少个月
*     date : 1,  // 缩写为 d, 加上多少天
*     hour : 1,  // 缩写为 h, 加上多少小时
*     minute : 1,  // 缩写为 n, 加上多少分钟
*     seconds : 1,  // 缩写为 s, 加上多少秒
*     time : 1  // 缩写为 t, 加上多少毫秒
*  }
* @return {Date} 返回添加后的时间(支持连缀),原时间也会被改变
*
* @example
*    var d = new Date();
*    d.add(15); // 加上15毫秒
*    d.add({year: 2}); // 加上2年
*    d.add({y:2, d:3}); // 加上2年零3天
*/
Date.prototype.add = function (option) {
    var o = this;
    // 数值类型的参数
    if (typeof option == 'number') {
        return o.setTime(o.getTime() + option);
    }
    // key/value格式的参数
    for (var key in option) {
        var value = c$.toInt(option[key]); // value 需取整
        if (value == 0) continue;
        switch (key.Trim().toLowerCase()) {
            case 'year':
            case 'y':
                o.setUTCFullYear(o.getUTCFullYear() + value); break;
            case 'month':
            case 'm':
                o.setUTCMonth(o.getUTCMonth() + value); break;
            case 'date':
            case 'd':
                o.setUTCDate(o.getUTCDate() + value); break;
            case 'hour':
            case 'h':
                o.setUTCHours(o.getUTCHours() + value); break;
            case 'minute':
            case 'n':
                o.setUTCMinutes(o.getUTCMinutes() + value); break;
            case 'seconds':
            case 's':
                o.setUTCSeconds(o.getUTCSeconds() + value); break;
            case 'time':
            case 't':
                o.setTime(o.getTime() + value); break;
            default:;
        }
    }
    return o;
};
(function (window, undefined) {
    var document = window.document,
        navigator = window.navigator,
        location = window.location;
    /**
    * 定义 c$ 类
    */
    var c$ = window.c$ = window.c$ || function () {
        return c$;
    };
    /**
    * 关闭窗口(IE上的关闭窗口时不提示)
    * @param  {HTMLElement} win 窗口对象
    * @return {Object} c$ 对象本身，以支持连缀
    */
    c$.closeWin = function (win) {
        win = win || window;
        win.opener = null; // 关闭IE6不提示
        win.open("", "_self"); // 关闭IE7不提示
        //关闭窗口
        win.close();
        return this;
    };
    /**
    * alert提示框
    * @return {Object} c$ 对象本身，以支持连缀
    * @example c$.alert('要提示的内容!');
    */
    c$.alert = function (title, content) {
        title = (title || '') + '';
        if (isUndefinedOrNull(content)) {
            $(this).showMsgDailog("提示信息", title, 500, 50);
        } else {
            $(this).showMsgDailog(title, content, 500, 50);
        }
    };
    /**
    * 获取URL的参数
    * @param  {String} name 要获取的参数名称
    * @param  {String} url 保存着参数的 URL
    * @return {String} 返回获取到的值,没有对应的键时返回 null
    */
    c$.getRequestValue = function (name, url) {
        url = url || location.href;
        var res = new RegExp("[\\?&]" + name + "=([^&#]*)").exec(url);
        return res ? decodeURIComponent(res[1]) : null;
    };
    /**
   * 这是出错调试代码
   * 当页面发生错误时，提示错误信息
   * @param msg   出错信息
   * @param url   出错文件的地址
   * @param sLine 发生错误的行
   * @return true 让出错时不显示出错图标
   */
    window.onerror = function (msg, url, sLine) {
        var hostUrl = window.location.href;
        // 判断网址,测试时可以提示出错信息;正式发布时不提示
        if (hostUrl.indexOf("http://localhost") === 0 ||
        hostUrl.indexOf("http://127.0.0.1") === 0 ||
            //hostUrl.indexOf("http://192.168.") === 0 ||
        hostUrl.indexOf("file://") === 0) {
            var errorMsg = "当前页面的脚本发生错误.\n\n";
            errorMsg += "错误: " + msg + "\n";
            errorMsg += "URL: " + url + "\n";
            errorMsg += "行: " + sLine + "\n\n";
            errorMsg += "点击“确定”以继续。\n\n";
            c$.alert('提示', errorMsg);
        }
        // 返回true,会消去 IE下那个恼人的“网页上有错误”的提示
        return true;
    };

    /**
    * 时间转换,把 Unix时间 转换成年月日格式
    * @param  {Number} date Unix时间(单位为秒)
    * @param  {String} format 显示的时间格式化的字符串(默认为：yyyy-MM-dd HH:mm:ss )
    * @return {String} 格式化时间后的字符串
    * @example c$.formatUnixDate(1); // 返回: 1970-01-01 08:00:01
    */
    c$.formatUnixDate = function (date, format) {
        date = c$.toInt(date); // 变整数
        var d = new Date(date * 1000); // js的时间是格林威治时间(1970-01-01 08:00:00)的毫秒数,故需要乘1000
        return d.format(format, false);
    };
    /**
    *产生随机数
    *使用方法: c$.GetRandom({ x: 150, y: 1 });
    **/
    c$.GetRandom = function (option) {
        var msgoption = option || {};
        msgoption.x = isUndefinedOrNull(option.x) ? 99 : option.x; //x上限
        msgoption.y = isUndefinedOrNull(option.y) ? 1 : option.y; //y下限 
        var rand = parseInt(Math.random() * (msgoption.x - msgoption.y + 1) + msgoption.y);
        return rand;
    };
    /**
    *根据时间和随机数计算方式产生随机数
    *使用方法: c$.GetTimeRandom();
    **/
    c$.GetTimeRandom = function () {
        return c$.GetDateFormat() + c$.GetRandom({ x: 1000, y: 1 }).toString();
    };
    /**
    *接收一个基数返回一个随机数
    *使用方法: c$.GetRandomByLength(36);
    **/
    c$.GetRandomByLength = function (len) {
        var rdmString = "";
        for (; rdmString.length < len; rdmString += Math.random().toString(36).substr(2));
        return rdmString.substr(0, len);
    };
    c$.GetDate = function () {
        var mydate = new Date();
        var time1 = mydate.getFullYear() + "-" + c$.Appendzero(mydate.getMonth() + 1) + "-" + c$.Appendzero(mydate.getDate());
        return time1;
    };
    c$.GetDateFormat = function () {
        var mydate = new Date();
        return mydate.Format("YYYYMMDDHHmmSS", true);
    };
    c$.Appendzero = function (obj) {
        if (obj < 10) return "0" + "" + obj;
        else return obj;

    };

    /**
    * 过滤数字(不符合的不让输入)
    * @param  {Boolean} isFloat 是否允许输入一个小数点
    * @param  {Event} event 兼容 IE 和 FireFox 的事件
    * @return {Boolean} 是否符合要求
    * @example <input type="text" name="stdcost" onkeydown="return c$.inputNumber(event,true);"/>
    */
    c$.inputNumber = function (event, isFloat) {
        event = event || window.event || c$.getEvent();
        isFloat = isFloat || false;
        // 获取事件源
        var source = event.target || event.srcElement;
        // 是否已按下 ctrl 键和 shift 键
        var otherKey = event.ctrlKey === true || event.shiftKey === true;
        var keyCode = event.charCode || event.keyCode;
        // 只允许输入数字、删除8、左(37)右(39)键、TAB键9、负号109; 小数时可输入一个小数点190
        if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39 || keyCode == 46 || (keyCode == 109 && source.value == "")) return true;
        //小数点
        if ((isFloat && (keyCode == 110 || keyCode == 190) && !otherKey && source.value.length > 0 && source.value.indexOf(".") == -1)) return true;
        //数字
        if (((keyCode >= 48 && keyCode <= 57 && !otherKey) || (keyCode >= 96 && keyCode <= 105))) return true;
        return false;
    };

    /**
    * 过滤字母数字(不符合的不让输入)
    * @param  {Event} event 兼容 IE 和 FireFox 的事件
    * @return {Boolean} 是否符合要求
    * @example <input type="text" name="stdcost" onkeydown="return c$.inputNumLetter(event);"/>
    */
    c$.inputNumLetter = function (event) {
        event = event || window.event || c$.getEvent();
        // 是否已按下 ctrl 键和 shift 键
        var otherKey = event.ctrlKey === true || event.shiftKey === true;
        var keyCode = event.charCode || event.keyCode;
        // 只允许输入字母、数字、删除8、左(37)右(39)键、TAB键9、负号109
        if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39 || keyCode == 46) return true;
        //数字
        if (((keyCode >= 48 && keyCode <= 57 && !otherKey) || (keyCode >= 96 && keyCode <= 105))) return true;
        //字母
        if ((keyCode >= 65 && keyCode <= 90)) return true;
        //下划线
        if (keyCode == 173 && otherKey) return true;
        return false;
    };


    /**
    * 过滤字母(不符合的不让输入)
    * @param  {Event} event 兼容 IE 和 FireFox 的事件
    * @return {Boolean} 是否符合要求
    * @example <input type="text" name="stdcost" onkeydown="return c$.inputLetter(event);"/>
    */
    c$.inputLetter = function (event) {
        event = event || window.event || c$.getEvent();
        // 是否已按下 ctrl 键和 shift 键
        var otherKey = event.ctrlKey === true || event.shiftKey === true;
        var keyCode = event.charCode || event.keyCode;
        // 只允许输入字母、删除、左右键、TAB键
        if (keyCode == 8 || keyCode == 9 || keyCode == 37 || keyCode == 39 || keyCode == 46) return true;
        //字母
        if ((keyCode >= 65 && keyCode <= 90)) return true;
        //下划线
        if (keyCode == 173 && otherKey) return true;
        return false;
    };
    /**
* 检查表单是否可提交
* @param  {HTMLElement} formField 待检查的form,给出的不是 form 元素而是 div,table 之类的元素也可以
* @param  {String} type 提示类型,有： alert(逐步提示,缩写A)、confirm(逐步判断,缩写C)、show(文字显示,缩写S,默认是show)
*                  注: 以 alert 或者 confirm 提示信息时,只要有其中一个验证不正确则不再验证下面,直接返回 false,避免不断地alert；而show则验证完整个form
*                  如果 type 是 show 类型,且需要指定页面显示位置,则可指定显示元素的id或者name,如: type={id:'显示元素的id', name:'显示元素的name'}
* @return {Boolean} 检查是否通过
*
* @example
*  <form action="#">
*  <table>
*     <tr>
*       <td>名称：<input name="txt1" type="text" checkType="R{请输入名称}S{id:'error1'}" onchange="c$.checkElement2(this)"/></td>
*        
*     </tr>
*     <tr>
*       <td>金额：<input name="txt2" type="text" checkType="S{id:'error2'}F{max:999.99, min:0, dec:2, msg:'请输入正确的金额'}" onchange="c$.checkElement2(this)"/></td>
*       
*     </tr>
*     <tr>
*       <td>人数：<input name="txt3" type="text" checkType="S{id:'error3'}R{请输入人数}I{max:999, min:0, msg:'请输入正确的人数'}" onchange="c$.checkElement2(this)"/></td>
*        
*     </tr>
*  </table>
*<div class="tips" id="dtips">
*       <p id="error1">
*       </p> 
*       <p id="error2">
*       </p>
*       <p id="error3">
*       </p>
*   </div>
*  <input type="submit" value="提交" onclick="return c$.checkForm(this.form,'','dtips');" />
*  </form>
*/
    c$.checkForm = function (formField, type, errordiv) {
        // 类型默认是 show,
        type = type || 'show';
        errordiv = errordiv || 'dtips';
        // 转成小写
        type = typeof type == 'string' ? type.toLowerCase() : type;

        var return_value = true;
        //元素列表
        var elements = c$.getElements(formField);

        //检查窗体的所有元素
        for (var index = 0, length = elements.length; index < length; index++) {
            if (false === c$.checkElement2(elements[index], type)) {
                return_value = false;
                // alert 和 confirm 验证需要中断,避免不断地提示
                if (type == 'alert' || type == 'a' || type == 'confirm' || type == 'c') return false;
            }
        }
        if (return_value) {
            $("#" + errordiv).hide();
        }
        else {
            $("#" + errordiv).show();
        }
        //检查通过
        return return_value;
    };

    ///c$.checkElement(this,'error','dtips')
    c$.checkElement = function (element, errdivname, errordiv) {
        var return_value = true;
        // 类型默认是 show,
        errdivname = errdivname || 'error';
        errordiv = errordiv || 'dtips';
        c$.checkElement2(element, 'show');
        $("p[name=" + errdivname + "]").each(function () {
            if ($(this).css("display") == "block") {
                return_value = false;
                return return_value;
            }
        });
        if (return_value) {
            $("#" + errordiv).hide();
        } else {
            $("#" + errordiv).show();
        }
        return return_value;
    };
    /**
    * 检查一个元素
    * @param  {HTMLElement} element 需检查的元素
    * @param  {String} type 提示类型,有： alert(缩写A)、confirm(缩写C)、show(文字显示,缩写S,默认是show)
    *                  如果 type 是 show 类型,且需要指定页面显示位置,则可指定显示元素的id或者name,如: type={id:'显示元素的id', name:'显示元素的name'}
    * @return {Boolean} 检查通过返回true,否则返回false
    *
    * @example
    *  <input id="txt1" name='txt1' type="text" checkType="R{请输入金额}F{max:999.99, min:0, dec:2, msg:'请输入正确的金额'}" />
    *  <input type="submit" value="提交" onclick="return c$.checkElement2(document.getElementById('txt1'));" />
    *  弹出框：c$.checkElement2(this,'confirm')
    * 说明: 检查类型的关键词需紧跟大括号,不跟大括号则提示“M”的讯息或者默认的讯息;
    * 使用“C”关键词则是选择提示框,点选“确定”可忽略此提示
    * 大括号里面的 msg 属性是对应的提示讯息,需用引号括起来; 大括号里面没有冒号则认为全是提示讯息; 注:不能嵌套大括号
    */
    c$.checkElement2 = function (element, type) {
        //防呆
        //if (!element || (typeof element.getAttribute != 'function')) return true;
        // 类型默认是 show,
        type = type || 'show';
        // 转成小写
        type = typeof type == 'string' ? type.toLowerCase() : type;

        //所有需检查讯息
        var message = ("" + element.getAttribute("checkType")).Trim();
        if (!element.getAttribute("checkType") || !message) return true; //防呆
        //获取需检查的类型,去除大括号里面的内容
        var checkType = message.replace(new RegExp("({[^}]*})*", 'g'), "").toUpperCase();
        var value = element.value;

        //验证类型
        //必须输入 (用法: checkType="R{请输入名称}" 或者 checkType="R{msg:'请输入名称'}" 或者 checkType="M{请输入名称}R")
        var rPos = (checkType.indexOf('R') > -1);

        //验证数据类型 (用法: checkType="B{msg:'英文名称只允许输入字母、数字、下划线'}")
        var bPos = (checkType.indexOf('B') > -1);

        //整型 (用法: checkType="I{max:100, min:-50, name:'人数'}" 或者 checkType="I{min:-50,max:100,msg:'请输入正确的人数'}")
        // 注:有 name 属性时, msg 属性将不会生效; name属性是提示讯息的名称; 属性 max, min 分别表示最大值最小值(含)
        var iPos = (checkType.indexOf('I') > -1);

        //浮点型 (用法: checkType="F{max:999.99, min:0, dec:2, name:'金额'}" 或者 checkType="F{dec:2,max:999.99,msg:'请输入正确的金额'}")
        // 注:有 name 属性时, msg 属性将不会生效; name属性是提示讯息的名称; dec 属性表示小数字数限制; 属性 max, min 分别表示最大值最小值(含)
        var fPos = (checkType.indexOf('F') > -1);

        //验证字母数字下划线 (用法: checkType="N{ID只能是字母,数字和下划线} 或者 checkType="N{msg:'ID只能是字母、数字和下划线',type:['chinese','letter','number','_']}")
        var nPos = (checkType.indexOf("N") > -1);

        //验证输入长度 (用法: checkType="L{len:100, msg:'名称长度不能超过100'}")
        var lPos = (checkType.indexOf("L") > -1);

        //验证 email (用法: checkType="@{请输入正确的邮箱地址}")
        var eMailPos = (checkType.indexOf("@") > -1);

        //验证 日期 (用法: checkType="D{日期输入不正确}" 或者 checkType="D{msg:'日期输入不正确'}")
        var dPos = (checkType.indexOf("D") > -1);

        //验证 时间 (用法: checkType="T{时间输入不正确}" 或者 checkType="T{msg:'时间输入不正确'}")
        var tPos = (checkType.indexOf("T") > -1);

        //验证 日期和时间 (用法: checkType="V{时间输入不正确}" 或者 checkType="V{msg:'时间输入不正确'}")
        var vPos = (checkType.indexOf("V") > -1);

        //验证 URL (用法: checkType="U{URL地址输入不正确}" 或者 checkType="U{msg:'URL地址输入不正确'}")
        var uPos = (checkType.indexOf("U") > -1);

        //验证 手机号码 (用法: checkType="P{手机号码输入不正确}" 或者 checkType="U{msg:'手机号码输入不正确'}")
        var pPos = (checkType.indexOf("P") > -1);

        //确认提示框,以提示框的形式提示出来,并可以选择是否忽略此提示
        var cPos = (checkType.indexOf('C') > -1) || type == 'confirm' || type == 'c';

        // 直接显示到页面,不使用提示框
        // 用法: 建议在函数的type参数上设值,也可以在元素上设定: checkType="R{请输入名称}S{id:'text1', name:'text1'}"  或者 checkType="SR{请输入名称}"
        // 可根据属性 id 或者 name 获取要显示的位置,不写参数则默认显示在节点的后面； 显示内容使用 .innerHTML 追加进去,红色字体显示
        var sPos = (checkType.indexOf('S') > -1) || type == 'show' || type == 's' || !!(type.id) || !!(type.name);

        //扩展方法型 (用法: checkType="Z{func:func}"
        var zPos = (checkType.indexOf('Z') > -1);

        var thisFun = arguments.callee;
        /*
        * 获取所需检查的类型的类别(匿名函数,仅供此函数内部使用; 缓存此函数以减少内存消耗)
        * @param  {String} key 对应的键
        * @param  {String} message 所有需检查讯息
        * @return {Object} 所需检查的类型的类别
        */
        var getCheckTypeObj = thisFun.getCheckTypeObj = thisFun.getCheckTypeObj || function (key, message) {
            var upMess = message.toUpperCase();
            // 如果此关键词没有跟大括号,返回空类别
            if (upMess.indexOf(key + "{") < 0) return {};
            // 取出对应的大括号的讯息
            var key_mess = message.substring(upMess.indexOf(key + "{") + 1);
            key_mess = key_mess.substring(0, key_mess.indexOf('}') + 1);
            try {
                // 将大括号的讯息转成的类别
                eval("var tem_obj = " + key_mess);
                return tem_obj;
            }
            catch (e) {
                // 如果大括号里面的讯息不能转成类别,则认为它全是提示讯息
                return { msg: key_mess.substring(1, key_mess.length - 1) };
            }
        };

        /*
        * 提示出对应的讯息(匿名函数,仅供此函数内部使用)
        * @param  {String} key 对应的键
        * @param  {HTMLElement} element 需检查的元素
        * @param  {String} message 所有需检查讯息
        * @param  {String} iniMess 默认的提示讯息
        * @return {Boolean} 一般都返回false,仅当显示选择框点选“是”时返回true
        */
        var showDialog = thisFun.showDialog = thisFun.showDialog || function (key, element, message, C_pos, S_pos, iniMess) {
            // 获取提示讯息
            var alert_message = key == "Z" ? (iniMess + "!") : ((getCheckTypeObj(key, message).msg || getCheckTypeObj("M", message).msg || iniMess) + "!");
            c$.setFocus(element);

            // "C"类型用 confirm 选择框
            if (C_pos) return confirm(alert_message);
            // "S"类型用文字显示到页面
            if (S_pos) {
                var tem_obj = getCheckTypeObj("S", message);
                // 创建一个对象来显示内容
                var font = document.createElement('font');
                font.color = 'red';
                // 以 element 作为提示元素的标志
                font.element = element;
                font.innerHTML = alert_message;

                var font2 = document.createElement('font');
                font2.color = 'red';
                // 以 element 作为提示元素的标志
                font2.element = element;
                font2.innerHTML = "*";

                var showElement = null;
                if (tem_obj.id) showElement = document.getElementById(tem_obj.id);
                else if (tem_obj.name) showElement = document.getElementsByName(tem_obj.name)[0];
                else if (type.id) showElement = document.getElementById(type.id);
                else if (type.name) showElement = document.getElementsByName(type.name)[0];


                document.body.appendChild(font2);
                // 插入到元素的后面
                var pos = element.nextSibling;
                pos.parentNode.insertBefore(font2, pos);

                // 没有获取到显示的元素,或者设置id或者name属性时,追加到节点后面
                if (!showElement) {
                    document.body.appendChild(font);
                    // 插入到元素的后面
                    var pos = element.nextSibling;
                    pos.parentNode.insertBefore(font, pos);
                }
                else {
                    $(showElement).show();
                    showElement.appendChild(font);
                    var br = document.createElement('br');
                    br.element = element;
                    showElement.appendChild(br);
                }
            }
            else {
                // 使用 alert 提示出来
                c$.alert(alert_message);
            }
            return false;
        };


        // 页面显示提示信息时,先清除页面的旧显示
        if (sPos) {
            var tem_obj = getCheckTypeObj("S", message);
            var showElement = null;
            if (tem_obj.id) showElement = document.getElementById(tem_obj.id);
            else if (tem_obj.name) showElement = document.getElementsByName(tem_obj.name)[0];
            else if (type.id) showElement = document.getElementById(type.id);
            else if (type.name) showElement = document.getElementsByName(type.name)[0];

            var nextElement = element.nextSibling;
            if (nextElement && nextElement.tagName && nextElement.element === element) {
                nextElement.parentNode.removeChild(nextElement);
            }

            // 没有获取到显示的元素,或者设置id或者name属性时,追加到节点后面
            if (!showElement) {
                var nextElement = element.nextSibling;
                if (nextElement && nextElement.tagName && nextElement.element === element) {
                    nextElement.parentNode.removeChild(nextElement);
                }
            }
            else {
                var childs = showElement.childNodes;
                $(showElement).hide();
                for (var i = 0, length = childs.length; i < length; i++) {
                    if (childs[i] && childs[i].tagName && childs[i].element === element) {
                        showElement.removeChild(childs[i]); // 删除 font 标签
                        showElement.removeChild(childs[i]); // 删除 br 标签
                        break;
                    }
                }
            }
        }
        //一定要输入
        if (rPos && "" === value.Trim() && false === showDialog("R", element, message, cPos, sPos, "请输入必要的值")) {
            return false;
        }
        //可以不输入时
        else if ("" === value && !zPos) {
            return true;
        }
        //字符串
        if (bPos && false === value.isLetter() && false === showDialog("B", element, message, cPos, sPos, "请输入正确的字符")) return false;
        //验证整形
        if (iPos) {
            var tem_obj = getCheckTypeObj("I", message);
            if (sPos) {
                var checkResult = c$.isInt(element, tem_obj.max, tem_obj.min, tem_obj.name, false, sPos);
                if (true !== checkResult && false === showDialog("I", element, message, cPos, sPos, checkResult.errMessage || "请输入正确的整数值")) return false;
            }
            else if (false === c$.isInt(element, tem_obj.max, tem_obj.min, tem_obj.name, false)) {
                //有name属性时按名称提示,这里不用重复提示; 否则取提示讯息
                if (tem_obj.name || false === showDialog("I", element, message, cPos, sPos, "请输入正确的整数值")) {
                    return false;
                }
            }
        }
        //验证浮点型
        if (fPos) {
            var tem_obj = getCheckTypeObj("F", message);
            if (sPos) {
                var checkResult = c$.isNumber(element, tem_obj.max, tem_obj.min, tem_obj.dec, tem_obj.name, false, sPos);
                if (true !== checkResult && false === showDialog("F", element, message, cPos, sPos, checkResult.errMessage || "请输入正确的数值")) return false;
            }
            else if (false === c$.isNumber(element, tem_obj.max, tem_obj.min, tem_obj.dec, tem_obj.name, false)) {
                //有name属性时按名称提示,这里不用重复提示; 否则取提示讯息
                if (tem_obj.name || false === showDialog("F", element, message, cPos, sPos, "请输入正确的数值")) {
                    return false;
                }
            }
        }
        //验证输入长度
        if (lPos) {
            var tem_obj = getCheckTypeObj("L", message);
            var strLen = getByteLen(value.Trim());
            if ((strLen < tem_obj.minLen) && false === showDialog("L", element, message, cPos, sPos, "输入的内容超过长度了，请缩短内容")) {
                return false;
            }
            if ((strLen > tem_obj.len) && false === showDialog("L", element, message, cPos, sPos, "输入的内容超过长度了，请缩短内容")) {
                return false;
            }
        }
        //验证字母数字下划线组合
        if (nPos) {
            var tem_obj = getCheckTypeObj("N", message);
            var t = tem_obj.type || ['number', 'letter', '_'];
            if (false === ''.is.apply(value, t) && false === showDialog("N", element, message, cPos, sPos, "此处只能输入字母、数字和下划线")) {
                return false;
            }
        }
        //方法验证
        if (zPos) {
            var tem_obj = getCheckTypeObj("Z", message);
            var func = tem_obj.func;
            if (typeof func != 'undefined' && func instanceof Function) {
                var zMessage = func && func();
                //var zMessage =  eval(func).call(this);
                if (zMessage != "" && false === showDialog("Z", element, message, cPos, sPos, zMessage)) {
                    return false;
                }
            }
        }
        //email验证
        if (eMailPos && false === value.isEmail() && false === showDialog("@", element, message, cPos, sPos, "请输入正确的邮箱地址")) return false;
        //日期验证
        if (dPos && false === value.isDate() && false === showDialog("D", element, message, cPos, sPos, "请输入正确的日期")) return false;
        //时间验证
        if (tPos && false === value.isTime() && false === showDialog("T", element, message, cPos, sPos, "请输入正确的时间")) return false;
        //日期和时间验证
        if (vPos && false === value.isDateTime() && false === showDialog("V", element, message, cPos, sPos, "请输入正确的时间")) return false;
        //URL验证
        if (uPos && false === value.isUrl() && false === showDialog("U", element, message, cPos, sPos, "请输入正确的URL地址")) return false;
        //手机号码验证
        if (pPos && false === value.IsMobile() && false === showDialog("P", element, message, cPos, sPos, "请输入正确的手机号码")) return false;
        //检查通过
        return true;
    };
    /**
    * 设定为焦点
    * @param  {HTMLElement} element 对象
    * @return {Object} c$ 对象本身，以支持连缀
    * @example c$.setFocus(docuemnt.getElementById('text1')); // 在id为text1的元素上设置焦点
    */
    c$.setFocus = function (element) {
        //element.focus();
        setTimeout(function () { try { element.focus(); } catch (e) { } }, 10);
        return this;
    };

    /**
    * 整数检查，判断对象是否为数字(包括负数、科学记数法、逗号分隔的数)
    * @param  {HTMLElement} element 需检查的对象,或者是字符串
    * @param  {Number} max 最大值(含), 默认为 2147483647
    * @param  {Number} min 最小值(含), 默认为 -2147483648
    * @param  {String} name 提示的讯息的名称
    * @param  {Boolean} isMust 是否可以为空,true表示不可以为空, 否则可为空
    * @param  {Boolean} isAlert 是否以alert提示出错信息,为true时返回出错信息但不提示,否则以alert提示。验证没错时,不管 isAlert 是什么值都将返回 true
    *                   注： isAlert 为false或者没有这参数时,直接 alert 提示出错信息,返回结果是 true 或 false。
    *                   但 isAlert 为true,且出错时,将返回结果对象：{result:true|false, errMessage:'出错信息'}
    * @return {Boolean} 验证符合则返回true,否则返回false
    * @example  if(c$.isInt(document.getElementById("amt"), 9999999, -9999999, "人数", flase))...
    */
    c$.isInt = function (element, max, min, name, isMust, isAlert) {
        // 没有最大值时,默认为整型的最大值
        if (typeof max != 'number') {
            max = 2147483647;
        }
        // 没有最大值时,默认为整型的最大值
        if (typeof min != 'number') {
            min = -2147483648;
        }
        return c$.isNumber(element, max, min, 0, name, isMust, isAlert);
    };

    /**
    * 数字检查，判断对象是否为数字(包括负数、科学记数法、逗号分隔的数)
    * @param  {HTMLElement} element 需检查的对象,或者是字符串
    * @param  {Number} max 最大值(含)
    * @param  {Number} min 最小值(含)
    * @param  {Number} decimal 小数多少位
    * @param  {String} name 提示的讯息的名称
    * @param  {Boolean} isMust 是否可以为空,true表示不可以为空, 否则可为空
    * @param  {Boolean} isAlert 是否以alert提示出错信息,为true时返回出错信息但不提示,否则以alert提示。验证没错时,不管 isAlert 是什么值都将返回 true
    *                   注： isAlert 为false或者没有这参数时,直接 alert 提示出错信息,返回结果是 true 或 false。
    *                   但 isAlert 为true,且出错时,将返回结果对象：{result:true|false, errMessage:'出错信息'}
    * @return {Boolean} 验证符合则返回true,否则返回false
    *
    * @example if(c$.isNumber(document.getElementById("amt"), 9999999.999, -9999999.999, 3, "金额", flase))...
    */
    c$.isNumber = function (element, max, min, decimal, name, isMust, isAlert) {
        var thisFun = arguments.callee;
        //出错时: 提示讯息,设定焦点,返回true
        var doError = thisFun.doError || (thisFun.doError = function (msg, name, element, isAlert) {
            var tem_name = name || '此项';
            msg = msg.format({ name: tem_name });
            // 有 isAlert 时不提示，但返回出错结果
            if (isAlert) return { result: false, errMessage: msg };
            // 有 name 属性时提示讯息,没有则不提示
            if (name) {
                c$.setFocus(element);
                c$.alert('提示', msg);
            }
            return false;
        });

        //获取字符串
        var str = element.value || element.innerHTML || "" + element;
        //去除空白; 去除逗号; 转成大写
        str = str.replace(/\s/gi, '').replace(/[,]/gi, '').toUpperCase();

        //不可以为空; 提示讯息: 请输入 name !
        if (true === isMust && "" === str) return doError("请输入 #name !", name, element, isAlert);
        //可以为空
        if (!isMust && "" === str) return true;

        //是否为数值; 提示讯息: name 不是正确的数字!
        if (!str.match(/^[+-]?\d+([.]?\d*)([eE][+-]\d+)?$/g)) return doError("#name 不是正确的数字!", name, element, isAlert);
        // 如果没有指定最大值,不可以超过数值的最大值
        if (0 !== max && !max) max = Number.MAX_VALUE;
        //判断最大值,提示讯息: name 不能大于 max
        if (parseFloat(str) > parseFloat(max)) return doError("#name 不能大于 " + max, name, element, isAlert);
        // 如果没有指定最小值,不可以小于数值的最小值
        if (0 !== min && !min) min = -1 * Number.MAX_VALUE;
        //判断最小值,提示讯息: name 不能小于 min
        if (parseFloat(str) < parseFloat(min)) return doError("#name 不能小于 " + min, name, element, isAlert);

        //小数判断
        decimal = parseInt(decimal);
        if (decimal >= 0 && (str.indexOf(".") > -1 || str.indexOf("E") > -1)) {
            //获取小数点后的数字
            var val = str.replace(/^[+-]?\d+[.]/g, '');
            val = val.replace(/0*([E][+-]\d+)?$/g, '');
            //小数长度
            var decimalLength = val.length;

            //如果是科学记算法
            if (str.indexOf("E") > -1) {
                decimalLength = val.length - parseInt(str.substring(str.indexOf("E") + 1, str.length));
            }
            // 要求整数时; 提示讯息: name 必须是整数
            if (decimal === 0 && (decimalLength > 0 || (str.indexOf(".") > -1 && str.indexOf("E") == -1))) {
                return doError("#name 必须是整数!", name, element, isAlert);
            }
            //小数位判断; 提示讯息: name 请保留 decimal 位小数
            if (decimalLength > decimal) {
                return doError("#name 请保留 " + decimal + " 位小数!", name, element, isAlert);
            }
        }
        //检验通过
        return true;
    };
    c$.HideError = function (obj) {
        $("#" + obj).slideUp("show");
    };
    /**
    * 判断对象是否页面元素
    * @param  {HTMLElement | 任意对象} obj 需判断的对象
    * @return {Boolean} 是页面元素则为 true, 否则为 false
    */
    c$.isElement = function (obj) {
        return !!(obj && obj.nodeType == 1);
    };
    /**
    * 获取 form 表单的所有元素(仅 input, checkbox 等可以表单提交的元素)
    * @param  {HTMLElement} formField 表单(没有参数则是整个页面的所有 form),给出的不是 form 元素而是 div,table 之类的元素也可以
    * @return {Array[HTMLElement]} 返回一个数组,里面保存着获取的元素
    */
    c$.getElements = function (formField) {
        //储存元素
        var elements = [];
        // 如果传入的是字符串,则根据此字符串获取对象
        if (typeof formField == 'string') formField = jQuery(formField).get(0);
        // 获取指定form的元素
        if (c$.isElement(formField)) {
            if (formField.tagName == 'FORM') {
                elements = formField.elements;
            }
                // 如果给出的不是form,而是div之类的,照样获取里面可提交的元素
            else {
                var inputs = formField.getElementsByTagName('INPUT');
                if (inputs && inputs.length > 0) {
                    for (var i = 0, length = inputs.length; i < length; i++) {
                        elements.push(inputs[i]);
                    }
                }
                var selects = formField.getElementsByTagName('SELECT');
                if (selects && selects.length > 0) {
                    for (var i = 0, length = selects.length; i < length; i++) {
                        elements.push(selects[i]);
                    }
                }
                var textareas = formField.getElementsByTagName('TEXTAREA');
                if (textareas && textareas.length > 0) {
                    for (var i = 0, length = textareas.length; i < length; i++) {
                        elements.push(textareas[i]);
                    }
                }
            }
        }
            // 获取form的所有元素
        else {
            var formArray = document.forms;
            for (var i = 0; i < formArray.length; i++) {
                formField = formArray[i];
                for (var j = 0, length = formField.length; j < length; j++) {
                    elements.push(formField[j]);
                }
            }
        }
        // 返回元素列表
        return elements;
    };
    // 整个工具类结束
})(window);