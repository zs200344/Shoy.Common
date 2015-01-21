﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Shoy.Utility.Extend
{
    /// <summary>
    /// 对象扩展辅助
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 是否包含在列表中
        /// </summary>
        /// <param name="o"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool In(this object o, IEnumerable c)
        {
            return c.Cast<object>().Contains(o);
        }

        /// <summary>
        /// 是否包含在列表中
        /// </summary>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] c)
        {
            return c.Any(i => i.Equals(t));
        }

        /// <summary>
        /// 遍历当前对象，并且调用方法进行处理
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="instance">实例</param>
        /// <param name="action">方法</param>
        /// <returns>当前集合</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            foreach (T item in instance)
            {
                action(item);
            }
            return instance;
        }

        /// <summary>
        /// 遍历N次
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int Each(this int instance, Action<int> action)
        {
            for (int i = 0; i < instance; i++)
            {
                action(i);
            }
            return instance;
        }

        /// <summary>
        /// 以“,”拼接字符串
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable items)
        {
            return items.Join(",", "{0}");
        }

        /// <summary>
        /// 使用分隔符拼接字符串
        /// </summary>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable items, string separator)
        {
            return items.Join(separator, "{0}");
        }

        /// <summary>
        /// 使用分隔符、以及模板字符串拼接字符串
        /// </summary>
        /// <param name="items">待拼接集合</param>
        /// <param name="separator">分隔符</param>
        /// <param name="template">字符串格式化模板</param>
        /// <returns></returns>
        public static string Join(this IEnumerable items, string separator, string template)
        {
            var sb = new StringBuilder();
            foreach (object item in items)
            {
                if (item == null) continue;
                sb.Append(separator);
                var type = item.GetType();
                if (type.IsValueType)
                    sb.Append(string.Format(template, item));
                else
                {
                    var temp = template;
                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    temp = props.Aggregate(temp,
                        (current, prop) =>
                            new Regex("\\{" + prop.Name + "\\}").Replace(current, prop.GetValue(item, null).ToString()));
                    sb.Append(temp);
                }
            }
            return sb.ToString().Substring(separator.Length);
        }

        /// <summary>
        /// 对象转换为泛型
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ObjectToT<T>(this object obj)
        {
            return obj.ObjectToT(default(T));
        }

        /// <summary>
        /// 对象转换为泛型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ObjectToT<T>(this object obj, T def)
        {
            var value = obj.ConvertTo(typeof(T));
            if (value == null)
                return def;
            return (T)value;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertTo(this object obj, Type type)
        {
            try
            {
                if (type.Name == "Nullable`1")
                    type = type.GetGenericArguments()[0];
                if (type.IsValueType)
                    return Activator.CreateInstance(type);
                return Convert.ChangeType(obj, type, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 写异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        public static void WriteTo(this Exception ex, string path)
        {
            Utils.WriteException(path, ex);
        }

        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ObjectToXml(this object obj, string path)
        {
            return XmlHelper.XmlSerialize(path, obj);
        }

        /// <summary>
        /// Xml反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static T XmlToObject<T>(this string path)
        {
            return XmlHelper.XmlDeserialize<T>(path);
        }
    }
}