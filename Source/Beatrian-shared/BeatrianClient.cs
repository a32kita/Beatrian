using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Beatrian.DiffDetection;
using Beatrian.Handling;
using Beatrian.Internal;
using Beatrian.Internal.WebRequestorElements;

namespace Beatrian
{
    class BeatrianClient : IDisposable
    {
        // 非公開フィールド
        private BeatrianInitializeConfig _config;
        private DataStore _dataStore;
        private HttpClientPool _httpClientPool;

        private BeatrianUpdatedEventHandler _pageUpdated;


        // 公開プロパティ


        // 公開イベント

        /// <summary>
        /// 対象のページの更新を検出したときに発生します。
        /// </summary>
        public event BeatrianUpdatedEventHandler PageUpdated
        {
            add => this._pageUpdated += value;
            remove => this._pageUpdated -= value;
        }


        // コンストラクタ

        /// <summary>
        /// 初期構成の <see cref="BeatrianInitializeConfig"/> を指定して、 <see cref="BeatrianClient"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="config"></param>
        public BeatrianClient(BeatrianInitializeConfig config)
        {
            this._config = config;
            this._config.VerifyConfigurations();

            // DataStore の初期化
            this._initializeDataStore();

            // HttpClient プールの初期化
            this._httpClientPool = new HttpClientPool(new HttpClientPoolInitializeParameter()
            {
                MaxClients = this._config.HttpClientPoolSize,
            });
        }


        // 非公開メソッド

        private void _initializeDataStore()
        {
            this._dataStore = new DataStore(this._config.CacheDataStream, false);

            switch (this._config.CacheMode)
            {
                case BeatrianCacheMode.CreateNew:
                    // 空のデータを書き込んでおく
                    this._dataStore.SetValue(null);
                    break;
                case BeatrianCacheMode.Load:
                    // 読めるのか確認だけしておく
                    try
                    {
                        this._dataStore.GetValueWithoutContent();
                    }
                    catch (Exception ex)
                    {
                        throw new BeatrianConfigException($"{nameof(BeatrianInitializeConfig.CacheMode)} で {nameof(BeatrianCacheMode.Load)} が指定されましたが、指定されたストリームには有効なキャッシュが存在しておりません。", nameof(BeatrianInitializeConfig.CacheMode), ex);
                    }
                    break;
                case BeatrianCacheMode.Auto:
                    // 読めなければ空のデータを書き込む
                    try
                    {
                        this._dataStore.GetValueWithoutContent();
                    }
                    catch
                    {
                        this._dataStore.SetValue(null);
                    }
                    break;
                default:
                    throw new NotImplementedException($"実装されていない {nameof(BeatrianCacheMode)} が指定されました。");
            }
        }

        
        // 公開メソッド

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            var currentPageStream = new MemoryStream();
            await WebRequestor.ExecuteRequestAsync(this._config, currentPageStream, this._httpClientPool);

            var storedData = this._dataStore.GetValue();
            if (storedData == null)
            {
                // 1 回目の取得をどう対応するのか　→ イベント発出 or 無視
                this._pageUpdated?.Invoke(this, new BeatrianUpdatedEventArgs());
                return;
            }

            var oldPageStream = new MemoryStream(storedData.Content);
            var oldCapture = new DiffDetectionCapture() { CapturedDataStream = oldPageStream };
            var currentCapture = new DiffDetectionCapture() { CapturedDataStream = currentPageStream };

            var diffDetector = new DiffDetector(oldCapture, currentCapture);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this._dataStore.Dispose();
            this._httpClientPool.Dispose();
        }
    }
}
