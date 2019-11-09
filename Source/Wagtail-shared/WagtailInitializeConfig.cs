using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wagtail
{
    public class WagtailInitializeConfig
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
        /// <see cref="WagtailInitializeConfig.CacheDataStream"/> で指定された <see cref="Stream"/> の扱いについて、取得または設定します。
        /// </summary>
        public WagtailCacheMode CacheMode
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
        /// <see cref="WagtailInitializeConfig.EnableHeadRequestToCheckSize"/> が true のときにキャッシュを有効とみなす最大期間を取得または設定します。
        /// このパラメータは、コンテンツのサイズが変化しない更新があった場合の検出漏れを防ぎます。
        /// </summary>
        public TimeSpan HeadRequestToCheckSizeMaxHoldTime
        {
            get;
            set;
        }


        // コンストラクタ

        /// <summary>
        /// デフォルトのパラメータを格納した <see cref="WagtailInitializeConfig"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public WagtailInitializeConfig()
        {
            this.CacheDataStream = null;
            this.CacheMode = WagtailCacheMode.Auto;
            this.MaxContentSize = 0u;
            this.SendIfModifiedSince = true;
            this.EnableHeadRequestToCheckSize = true;
            this.HeadRequestToCheckSizeMaxHoldTime = new TimeSpan(1, 0, 0);
        }
    }
}
