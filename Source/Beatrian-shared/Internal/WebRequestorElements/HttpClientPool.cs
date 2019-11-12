using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Beatrian.Internal.WebRequestorElements
{
    internal class HttpClientPool
    {
        // 非公開フィールド
        private List<HttpClientPoolElement> _clients;


        // 公開プロパティ

        public int MaxClient
        {
            get;
            set;
        }


        // コンストラクタ

        
    }
}
