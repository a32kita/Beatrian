using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

using Beatrian.Internal.WebRequestorElements;

namespace Beatrian.Internal
{
    internal static class WebRequestor
    {
        // 非公開静的フィールド
        private static HttpClientPool _httpClient;


        // 公開メソッド

        /// <summary>
        /// <see cref="BeatrianInitializeConfig"/> に基づいてコンテンツの取得を実施いたします。
        /// </summary>
        /// <param name="config"></param>
        /// <param name="outputStream"></param>
        public static void ExecuteRequest(BeatrianInitializeConfig config, Stream outputStream)
        {
        }



    }
}
