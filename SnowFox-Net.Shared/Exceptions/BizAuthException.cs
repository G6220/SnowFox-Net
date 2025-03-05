namespace SnowFox_Net.Shared.Exceptions
{
    using System;

    /// <summary>
    /// 鉴权异常
    /// </summary>
    public class BizAuthException : Exception
    {
        public BizAuthException(string message) : base(message)
        {
        }
    }
}
