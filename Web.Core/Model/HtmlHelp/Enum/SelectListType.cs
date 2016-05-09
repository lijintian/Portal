/*
 EasyDDD
系统名称：  Model
模块名称：  下拉数据源枚举
创建日期：  2015-07-30

内容摘要： 
*/

using System.ComponentModel;

namespace Portal.Web.Core.Model
{
    /// <summary>
    /// </summary>
    public enum SelectListType
    {
        /// <summary>
        /// 下拉框
        /// 返回：<option value='1'  selected  Other='xs' disabled=''>张三</option>
        /// </summary>
        [Description("<option value='{0}' {1} {3}>{2}</option>")]
        Select = 1,

        /// <summary>
        /// 单选框
        /// 返回：<input type='radio' value='1'  name='rbtName'  checked="checked"  Other='xs'/><label>张三</label>
        /// </summary>
        [Description("<input type='radio' value='{{0}}'  name='{1}' {{1}} {{3}} /><label>{{2}}</label>")]
        Radio = 2,

        /// <summary>
        /// 复选框
        /// 返回：<input type='checkbox' value='1'  name='cbxName' checked="checked"   Other='xs'/><label>张三</label>
        /// </summary>
        [Description("<input type='checkbox' value='{{0}}'  name='{1}' {{1}} {{3}} /><label>{{2}}</label>")]
        Checkbox = 3,

        /// <summary>
        /// 常用的，如：<a href="javascript:void(0)"  rel="1"  class="active" >张三</a>
        /// </summary>
        [Description("<a href=\"javascript:void(0)\"  rel=\"{0}\"  {1} {3} >{2}</a>")]
        Href = 4,

        /// <summary>
        /// 单选框
        /// 返回：<input type="radio" value="1" name="rbSexType" checked="checked" /><span class="lbl">男</span>
        /// </summary>
        [Description("<input type='radio' value='{{0}}'  name='{1}' {{1}} {{3}} /><span class='lbl'>{{2}}</span>")]
        RadioSpan = 6,

        /// <summary>
        /// 复选框
        /// 返回： <input type="checkbox" value="1" name="cbxSexType" checked="checked" /><span class="lbl">男</span>
        /// </summary>
        [Description("<input type='checkbox' value='{{0}}'  name='{1}' {{1}} {{3}} /><span class='lbl'>{{2}}</span>")]
        CheckboxSpan = 7,

        /// <summary>
        /// 用于li的下拉框数据源，如：<li><a href="javascript:void(0)"  rel="{0}"  class="active" >{1}</a></li>
        /// </summary>
        [Description("<li><a href=\"javascript:void(0)\"  rel=\"{0}\"  {1} {3} >{2}</a></li>")]
        HrefLi = 8,

        /// <summary>
        /// 单选框
        /// 返回：<span><input type='radio' value='1'  name='rbSexType'  checked="checked"　/>女</span>
        /// </summary>
        [Description("<label><input type='radio' value='{{0}}'  name='{1}' {{1}} {{3}} /><span class=\"lbl\">{{2}}</span></label>")]
        SpanRadio = 9,

        /// <summary>
        /// 复选框
        /// 返回： <span><input type='checkbox' value='1'  name='cbxSexType'  checked="checked"　/>女</span>
        /// </summary>
        [Description("<label><input type='checkbox' value='{{0}}'  name='{1}' {{1}} {{3}} /><span class=\"lbl\">{{2}}</span></label>")]
        SpanCheckbox = 10,
    }
}
