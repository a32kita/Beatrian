using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;

namespace Beatrian.Internal.WebRequestorElements
{
    internal class HttpClientPool : IDisposable
    {
        // 非公開フィールド
        private List<HttpClientPoolElement> _clients;
        private int _maxCliets;


        // 公開プロパティ

        /// <summary>
        /// このプールが保持する最大の <see cref="HttpClient"/> の数を取得または設定します。
        /// この値を減らすとプール内のクライアントの破棄を試みますが、使用中だった場合などにトラブルの発生の原因となります。
        /// </summary>
        public int MaxClients
        {
            get => this._maxCliets;
            set
            {
                this._maxCliets = value;
                this._fillPool();
            }
        }

        /// <summary>
        /// 現在使用中の <see cref="HttpClient"/> の数を取得します。
        /// </summary>
        public int UsingClients
        {
            get => this._clients.Where(item => item.IsUsingNow).Count();
        }


        // コンストラクタ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initializeParameter"></param>
        public HttpClientPool(HttpClientPoolInitializeParameter initializeParameter)
        {
            // MaxClients の値の適用と同時に
            // プールが満たされる
            this.MaxClients = initializeParameter.MaxClients;
        }


        // 非公開メソッド

        /// <summary>
        /// プールを満たします。
        /// </summary>
        private void _fillPool()
        {
            if (this._clients == null)
                this._clients = new List<HttpClientPoolElement>();

            var creationCount = this.MaxClients - this._clients.Count;
            if (creationCount > 0)
            {
                // 追加
                for (var i = 0; i < creationCount; i++)
                    this._clients.Add(new HttpClientPoolElement(new HttpClient()));
            }
            else if (creationCount < 0)
            {
                // 削除
                var unusingElements = this._clients.Where(item => !item.IsUsingNow).ToArray();
                var removeCount = Math.Min(unusingElements.Length, creationCount);

                for (var i = 0; i < removeCount; i++)
                    this._clients.Remove(unusingElements[i]);
            }
        }


        // 公開メソッド

        /// <summary>
        /// 使用可能なクライアントをプールから払い出します。
        /// </summary>
        /// <returns></returns>
        public HttpClient GetClient()
        {
            var unusingElements = this._clients.Where(item => !item.IsUsingNow);
            if (unusingElements.Count() == 0)
                throw new HttpClientPoolOutOfStockException();

            var targetElement = unusingElements.First();
            targetElement.IsUsingNow = true;
            return targetElement.HttpClient;
        }

        /// <summary>
        /// 使用し終わったクライアントを片付けます。
        /// </summary>
        /// <param name="httpClient"></param>
        public void PutAwayClient(HttpClient httpClient)
        {
            var targetElements = this._clients.Where(item => item.HttpClient == httpClient);
            if (targetElements.Count() == 0)
                throw new ArgumentException($"指定された {nameof(HttpClient)} は、この {nameof(HttpClientPool)} での管理対象ではありません。", nameof(httpClient));

            targetElements.Single().IsUsingNow = false;

            // MaxCount よりも実際のクライアントの数が上回ってる場合に
            // 今片付けられたクライアントを破棄する
            this._fillPool();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.MaxClients = 0;
        }
    }
}
