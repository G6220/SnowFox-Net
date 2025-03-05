namespace SnowFox_Net.Shared.Responses
{
    /// <summary>
    /// 响应体
    /// </summary>
    public class IResponseOutput
    {
        /// <summary>
        /// Gets a value indicating whether Success
        /// 响应状态
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets the Code
        /// 响应状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets the Error
        /// Gets or sets the Error
        /// 响应错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets the Message
        /// 响应信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the Data
        /// 响应数据
        /// </summary>
        public object Data { get; set; }
    }
}
