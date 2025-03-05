namespace SnowFox_Net.Common.Encrypt
{
    using Konscious.Security.Cryptography;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Argon2加密工具
    /// </summary>
    public class Argon2Tool
    {
        /// <summary>
        /// Defines the _saltLength
        /// </summary>
        public readonly int _saltLength;// 盐值长度

        /// <summary>
        /// Defines the _iterations
        /// </summary>
        public readonly int _iterations;// 迭代次数

        /// <summary>
        /// Defines the _memorySize
        /// </summary>
        public readonly int _memorySize;// 内存大小

        /// <summary>
        /// Defines the _parallelism
        /// </summary>
        public readonly int _parallelism;// 并行度

        /// <summary>
        /// Initializes a new instance of the <see cref="Argon2Tool"/> class.
        /// </summary>
        /// <param name="saltLength">The saltLength<see cref="int"/></param>
        /// <param name="iterations">The iterations<see cref="int"/></param>
        /// <param name="memorySize">The memorySize<see cref="int"/></param>
        /// <param name="parallelism">The parallelism<see cref="int"/></param>
        public Argon2Tool(int saltLength = 16, int iterations = 32, int memorySize = 65536, int parallelism = 8)
        {
            _iterations = iterations;
            _memorySize = memorySize;
            _parallelism = parallelism;
            _saltLength = saltLength;
        }

        // 密码哈希加密

        /// <summary>
        /// The HashPassword
        /// </summary>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string HashPassword(string password)
        {
            // 生成盐值
            byte[] salt = GenerateSalt(_saltLength);

            // 使用 Argon2id 算法对密码进行哈希处理
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.Iterations = _iterations;
                argon2.MemorySize = _memorySize;
                argon2.DegreeOfParallelism = _parallelism;

                // 计算哈希值
                byte[] hash = argon2.GetBytes(32); // 获取32字节的哈希结果

                // 合并盐值和哈希值进行存储
                byte[] saltedHash = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, saltedHash, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, saltedHash, salt.Length, hash.Length);

                // 返回包含盐值和哈希值的 base64 字符串
                return Convert.ToBase64String(saltedHash);
            }
        }

        // 密码验证

        /// <summary>
        /// The VerifyPassword
        /// </summary>
        /// <param name="password">The password<see cref="string"/></param>
        /// <param name="storedHash">The storedHash<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool VerifyPassword(string password, string storedHash)
        {
            // 解码存储的哈希和盐
            byte[] saltedHash = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16]; // 假设盐的长度为16字节
            byte[] storedHashBytes = new byte[saltedHash.Length - salt.Length];

            Buffer.BlockCopy(saltedHash, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(saltedHash, salt.Length, storedHashBytes, 0, storedHashBytes.Length);

            // 使用 Argon2id 对输入的密码进行哈希验证
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.Iterations = _iterations;  // 可以根据需求调整
                argon2.MemorySize = _memorySize;
                argon2.DegreeOfParallelism = _parallelism;

                byte[] computedHash = argon2.GetBytes(32); // 获取32字节的哈希值

                // 比较计算的哈希与存储的哈希值
                return AreHashesEqual(computedHash, storedHashBytes);
            }
        }

        // 比较两个哈希值是否相等

        /// <summary>
        /// The AreHashesEqual
        /// </summary>
        /// <param name="hash1">The hash1<see cref="byte[]"/></param>
        /// <param name="hash2">The hash2<see cref="byte[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool AreHashesEqual(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }

        // 生成盐值

        /// <summary>
        /// The GenerateSalt
        /// </summary>
        /// <param name="length">The length<see cref="int"/></param>
        /// <returns>The <see cref="byte[]"/></returns>
        private static byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
    }
}
