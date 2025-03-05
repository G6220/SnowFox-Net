namespace SnowFox_Net.Shared.Requests
{
    /// <summary>
    /// 请求接口
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// Gets the PageSize
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the CurrentPage
        /// 当前分页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsAsc
        /// 是否顺序
        /// </summary>
        public bool IsAsc { get; set; }

        /// <summary>
        /// Gets the OrderBy
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }
    }
}
