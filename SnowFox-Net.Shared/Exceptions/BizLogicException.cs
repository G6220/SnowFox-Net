namespace SnowFox_Net.Shared.Exceptions
{
    using System;

    /// <summary>
    /// 权限验证异常
    /// </summary>
    public class BizLogicException : Exception
    {
        public BizLogicException(string message) : base(message)
        {
        }
    }
}
