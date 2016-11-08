using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.ComponentModel;
namespace Qos.xin.Common
{
    public static class CalendarExtensions
    {
        private static string defaultFormat = "yyyy-MM-dd";

        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name)
        {
            return Calendar(helper, name, defaultFormat);
        }
        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime date)
        {
            return Calendar(helper, name, date, defaultFormat);
        }
        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime date, object htmlAttributes)
        {
            return Calendar(helper, name, date, defaultFormat, htmlAttributes);
        }
        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, object htmlAttributes)
        {
            return GenerateHtml(name, null, null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, string format)
        {
            return GenerateHtml(name, null, format, null);
        }
       
        /// <summary>
        /// 使用特定的名称生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, string format, object htmlAttributes)
        {
            return Calendar(helper,name,null,format, htmlAttributes);
        }
        
        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime date, string format)
        {
            return Calendar(helper,name, date, format, null);
        }
        /// <summary>
        /// 使用特定的名称和初始值生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="name">控件名称</param>
        /// <param name="date">要显示的日期时间</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString Calendar(this HtmlHelper helper, string name, DateTime? date, string format, object htmlAttributes)
        {
            return GenerateHtml(name, date, format, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return CalendarFor(helper, expression, defaultFormat);
        }

        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;

            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return GenerateHtml(name, value, format, null);
            }
            return GenerateHtml(name, null, format, null);
        }
        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="htmlAttributes">html属性对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return GenerateHtml(name, value, defaultFormat, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            return GenerateHtml(name, null, defaultFormat, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 通过lambda表达式生成控件
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="format">显示格式</param>
        /// <param name="htmlAttributes">html属性对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;

            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return GenerateHtml(name, value, format, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            return GenerateHtml(name, null, format, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 通过lambda表达式获取要显示的日期时间
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <param name="format">显示格式</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            DateTime value;

            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null && DateTime.TryParse(data.ToString(), out value))
            {
                return GenerateHtml(name, value, format, null);
            }
            else
            {
                return GenerateHtml(name, null, format, null);
            }
        }

        /// <summary>
        /// 通过lambda表达式获取要显示的日期时间
        /// </summary>
        /// <param name="helper">HtmlHelper对象</param>
        /// <param name="expression">lambda表达式，指定要显示的属性及其所属对象</param>
        /// <returns>Html文本</returns>
        public static MvcHtmlString CalendarDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return CalendarDisplayFor(helper, expression, defaultFormat);
        }

        /// <summary>
        /// 生成输入框的Html
        /// </summary>
        /// <param name="name">calendar的名称</param>
        /// <param name="date">calendar的值</param>
        /// <returns>html文本</returns>
        private static MvcHtmlString GenerateHtml(string name, DateTime? date, string format, IDictionary<string, object> attributes)
        {
            StringBuilder html = new StringBuilder("<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" onfocus=\"WdatePicker({dateFmt:'" + format + "'})\"");
            if (attributes != null)
            {
                if (attributes.ContainsKey("class"))
                {
                    attributes["class"] = "Wdate " + attributes["class"];
                }
                else
                {
                    attributes.Add("class", "Wdate " + attributes["class"]);
                }

                foreach (var item in attributes)
                {
                    html.Append(item.Key + "=\"" + item.Value + "\" ");
                }
                if (date != null)
                {
                    html.Append("value=\"" + date.Value.ToString(format) + "\"");
                }
                else
                {
                    html.Append("value=\"\"");
                }
            }
            html.Append("/>");

            return MvcHtmlString.Create(html.ToString());
        }
    }
}
