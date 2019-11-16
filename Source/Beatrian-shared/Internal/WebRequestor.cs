using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Beatrian.Internal.WebRequestorElements;

namespace Beatrian.Internal
{
    internal static class WebRequestor
    {
        // 非公開静的フィールド
        private static HttpClientPool _httpClientPool;


        // コンストラクタ

        /// <summary>
        /// <see cref="WebRequestor"/> クラスを初期化します。
        /// </summary>
        static WebRequestor()
        {
            // HttpClient のプールを作成
            _httpClientPool = new HttpClientPool(new HttpClientPoolInitializeParameter()
            {
                MaxClients = 5,
            });
        }

        
        // 非公開メソッド

        private static async Task _transferStreamDataAsync(Stream input, Stream output, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            while (true)
            {
                var readLen = await input.ReadAsync(buffer, 0, buffer.Length);
                await output.WriteAsync(buffer, 0, readLen);

                if (readLen != bufferSize)
                    break;
            }
        }


        // 公開メソッド

        /// <summary>
        /// <see cref="BeatrianInitializeConfig"/> に基づいてコンテンツの取得を実施します。
        /// </summary>
        /// <param name="config"></param>
        /// <param name="outputStream">レスポンス内容の出力先を指定します。出力によりカーソルが移動しますが、クローズは行われません。</param>
        public static async Task ExecuteRequestAsync(BeatrianInitializeConfig config, Stream outputStream)
        {
            var targetUri = config.TargetUri;
            var httpMethod = HttpMethod.Get;
            var httpClient = _httpClientPool.GetClient();

            using (var httpRequest = new HttpRequestMessage(httpMethod, targetUri))
            using (var httpResponse = await httpClient.SendAsync(httpRequest))
            using (var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync())
            {
                // HTTP の応答データを outputStream へ転送
                await _transferStreamDataAsync(
                    httpResponseStream, outputStream, 1024);
            }

            _httpClientPool.PutAwayClient(httpClient);
        }

        /// <summary>
        /// <see cref="BeatrianInitializeConfig"/> に基づいてコンテンツの取得を実施します。
        /// </summary>
        /// <param name="config"></param>
        /// <param name="outputStream"></param>
        public static void ExecuteRequest(BeatrianInitializeConfig config, Stream outputStream)
        {
            Task.Run(async () => await ExecuteRequestAsync(config, outputStream)).Wait();
        }
    }
}
