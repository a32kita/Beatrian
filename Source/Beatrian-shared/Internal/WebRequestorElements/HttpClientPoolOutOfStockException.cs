using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Beatrian.Internal.WebRequestorElements
{
    /// <summary>
    /// <see cref="HttpClientPool"/> 内に払い出し可能なクライアントの在庫が無いことを表します。
    /// </summary>
    internal class HttpClientPoolOutOfStockException : Exception, IBeatrianException
    {
        public HttpClientPoolOutOfStockException()
        {
        }

        public HttpClientPoolOutOfStockException(string message) : base(message)
        {
        }

        public HttpClientPoolOutOfStockException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
