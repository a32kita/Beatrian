using System;
using System.Collections.Generic;
using System.Text;

using Beatrian.Internal;
using Beatrian.Handling;
using Beatrian.Internal.WebRequestorElements;

namespace Beatrian
{
    class BeatrianClient
    {
        // 非公開フィールド
        private BeatrianInitializeConfig _config;

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
        }


        // 非公開メソッド



        
        // 公開メソッド

        
    }
}
