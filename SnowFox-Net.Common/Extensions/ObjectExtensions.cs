namespace SnowFox_Net.Common.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Object扩展类
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 返回深拷贝的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            if (obj == null)
                return default;
            return (T)DeepCloneInternal(obj);
        }

        /// <summary>
        /// 进行深拷贝
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object DeepCloneInternal(object obj)
        {
            if (obj == null)
                return null;

            Type objType = obj.GetType();

            // 如果是值类型或者字符串，直接返回
            if (objType.IsValueType || obj is string)
                return obj;

            // 对象是引用类型，需要进行深拷贝
            var clone = Activator.CreateInstance(objType);

            // 获取对象的所有字段和属性
            foreach (var field in objType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // 获取字段值并递归深拷贝
                var fieldValue = field.GetValue(obj);
                field.SetValue(clone, DeepCloneInternal(fieldValue));
            }

            return clone;
        }
    }
}
