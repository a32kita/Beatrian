using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Beatrian
{
    public class BeatrianInitializeConfig
    {
        // 公開プロパティ
        
        /// <summary>
        /// キャッシュとして利用する <see cref="Stream"/> を取得または設定します。
        /// 読み書き、シーク、長さ設定が可能な任意の <see cref="Stream"/> を必ず指定する必要があります。
        /// </summary>
        public Stream CacheDataStream
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="BeatrianInitializeConfig.CacheDataStream"/> で指定された <see cref="Stream"/> の扱いについて、取得または設定します。
        /// </summary>
        public BeatrianCacheMode CacheMode
        {
            get;
            set;
        }

        /// <summary>
        /// ターゲットとなる URL を取得または設定します。
        /// 有効な <see cref="Uri"/> を必ず指定する必要があります。
        /// </summary>
        public Uri TargetUri
        {
            get;
            set;
        }

        /// <summary>
        /// Web ページの内容をロードするときの最大サイズを取得または設定します。
        /// 0 を指定すると最大サイズの制限は無効になります。
        /// </summary>
        public uint MaxContentSize
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP / GET または HEAD メソッドでリクエストを送信する際、 If-Modified-Since をヘッダに加えるかどうかを取得または設定します。
        /// 静的なページである場合、 304 Not Modified がレスポンスされることが期待できます。
        /// </summary>
        public bool SendIfModifiedSince
        {
            get;
            set;
        }

        /// <summary>
        /// HEAD リクエストを送信しコンテンツのサイズが変化していない場合は変更がないものとみなすかどうかを取得または設定します。
        /// </summary>
        public bool EnableHeadRequestToCheckSize
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="BeatrianInitializeConfig.EnableHeadRequestToCheckSize"/> が true のときにキャッシュを有効とみなす最大期間を取得または設定します。
        /// このパラメータは、コンテンツのサイズが変化しない更新があった場合の検出漏れを防ぎます。
        /// </summary>
        public TimeSpan HeadRequestToCheckSizeMaxHoldTime
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP リクエストで使用するクライアントのプール サイズを取得または設定します。
        /// </summary>
        public int HttpClientPoolSize
        {
            get;
            set;
        }


        // コンストラクタ

        /// <summary>
        /// デフォルトのパラメータを格納した <see cref="BeatrianInitializeConfig"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public BeatrianInitializeConfig()
        {
            this.CacheDataStream = null;
            this.CacheMode = BeatrianCacheMode.Auto;
            this.TargetUri = null;
            this.MaxContentSize = 0u;
            this.SendIfModifiedSince = true;
            this.EnableHeadRequestToCheckSize = true;
            this.HeadRequestToCheckSizeMaxHoldTime = new TimeSpan(1, 0, 0);
            this.HttpClientPoolSize = 10;
        }


        // 公開メソッド

        /// <summary>
        /// 設定値が問題ないか検証します。
        /// </summary>
        public void VerifyConfigurations()
        {
            if (this.CacheDataStream == null)
                throw new BeatrianConfigException("任意のストリームを指定する必要があります。", nameof(this.CacheDataStream));

            if (this.TargetUri == null)
                throw new BeatrianConfigException("対象の URL を指定する必要があります。", nameof(this.TargetUri));

            if (this.HeadRequestToCheckSizeMaxHoldTime < new TimeSpan(0, 0, 1))
                throw new BeatrianConfigException("1 秒よりも長い期間を指定する必要があります。", nameof(this.HeadRequestToCheckSizeMaxHoldTime));

            if (this.HttpClientPoolSize < 1)
                throw new BeatrianConfigException("コネクション プールのサイズは 1 以上を指定する必要があります。", nameof(this.HttpClientPoolSize));
        }
    }
}
