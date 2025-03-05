namespace SnowFox_Net.Shared.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// 分页输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageOutput<T>
    {
        /// <summary>
        /// Gets or sets the Total
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the Summary
        /// 摘要
        /// </summary>
        public object Summary { get; set; }

        /// <summary>
        /// Gets or sets the List
        /// 列表
        /// </summary>
        public List<T> List { get; set; }
    }
}
