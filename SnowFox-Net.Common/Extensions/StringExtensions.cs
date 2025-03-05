namespace SnowFox_Net.Common.Extensions
{
    using System;

    /// <summary>
    /// String扩展类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Null、空串或者只包含空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 不是Null、空串或者只包含空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNull(this string str)
        {
            return !String.IsNullOrWhiteSpace(str);
        }
    }
}
