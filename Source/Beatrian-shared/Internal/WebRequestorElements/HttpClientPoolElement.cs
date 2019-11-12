using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Beatrian.Internal.WebRequestorElements
{
    internal class HttpClientPoolElement
    {
        // 公開プロパティ

        /// <summary>
        /// <see cref="System.Net.Http.HttpClient"/> のインスタンスを取得します。
        /// </summary>
        public HttpClient HttpClient
        {
            get;
            private set;
        }

        /// <summary>
        /// 現在使用中かどうかを取得または設定します。
        /// </summary>
        public bool IsUsingNow
        {
            get;
            set;
        }


        // コンストラクタ

        /// <summary>
        /// 現在使用していない <see cref="System.Net.Http.HttpClient"/> インスタンスを使用して、 <see cref="HttpClientPoolElement"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="httpClient"></param>
        public HttpClientPoolElement(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this.IsUsingNow = false;
        }
    }
}
