namespace SnowFox_Net.Shared.Responses
{
    /// <summary>
    /// 响应类
    /// </summary>
    public static class ResponseOutput
    {
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static IResponseOutput OK()
        {
            return new IResponseOutput
            {
                Success = true,
                Code = 0,
            };
        }

        /// <summary>
        /// 响应成功（带状态码）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(int code)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = code,
            };
        }

        /// <summary>
        /// 响应成功（带信息）
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(string message)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = 0,
                Message = message
            };
        }

        /// <summary>
        /// 响应成功（带数据）
        /// </summary>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(object data)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = 0,
                Data = data
            };
        }

        /// <summary>
        /// 响应成功（带状态码和信息）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(int code, string message)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = code,
                Message = message
            };
        }

        /// <summary>
        /// 响应成功（带状态码和数据）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(int code, object data)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = code,
                Data = data
            };
        }

        /// <summary>
        /// 响应成功（带信息和数据）
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(string message, object data)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = 0,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 响应成功（全带）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput OK(int code, string message, object data)
        {
            return new IResponseOutput
            {
                Success = true,
                Code = code,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public static IResponseOutput NotOK()
        {
            return new IResponseOutput
            {
                Success = false,
                Code = 1,
            };
        }

        /// <summary>
        /// 响应失败（带状态码）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(int code)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = code,
            };
        }

        /// <summary>
        /// 响应失败（带信息）
        /// </summary>
        /// <param name="error">The error<see cref="string"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(string error)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = 1,
                Error = error
            };
        }

        /// <summary>
        /// 响应失败（带数据）
        /// </summary>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(object data)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = 1,
                Data = data
            };
        }

        /// <summary>
        /// 响应失败（带状态码和信息）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="error">The error<see cref="string"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(int code, string error)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = code,
                Error = error
            };
        }

        /// <summary>
        /// 响应失败（带状态码和数据）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(int code, object data)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = code,
                Data = data
            };
        }

        /// <summary>
        /// 响应失败（带信息和数据）
        /// </summary>
        /// <param name="error">The error<see cref="string"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(string error, object data)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = 1,
                Error = error,
                Data = data
            };
        }

        /// <summary>
        /// 响应失败（全带）
        /// </summary>
        /// <param name="code">The code<see cref="int"/></param>
        /// <param name="error">The error<see cref="string"/></param>
        /// <param name="data">The data<see cref="object"/></param>
        /// <returns>The <see cref="IIResponseOutput"/></returns>
        public static IResponseOutput NotOK(int code, string error, object data)
        {
            return new IResponseOutput
            {
                Success = false,
                Code = code,
                Error = error,
                Data = data
            };
        }
    }
}
