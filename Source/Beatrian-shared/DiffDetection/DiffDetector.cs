using System;
using System.Collections.Generic;
using System.Text;

namespace Beatrian.DiffDetection
{
    public class DiffDetector
    {
        // 公開プロパティ
        
        /// <summary>
        /// 以前のキャプチャを取得します。
        /// </summary>
        public DiffDetectionCapture OldCapture
        {
            get;
            private set;
        }

        /// <summary>
        /// 現在のキャプチャを取得します。
        /// </summary>
        public DiffDetectionCapture CurrentCapture
        {
            get;
            private set;
        }


        // コンストラクタ

        /// <summary>
        /// 新旧それぞれの <see cref="DiffDetectionCapture"/> を指定して、 <see cref="DiffDetector"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="oldCapture"></param>
        /// <param name="currentCapture"></param>
        public DiffDetector(DiffDetectionCapture oldCapture, DiffDetectionCapture currentCapture)
        {
            this.OldCapture = oldCapture;
            this.CurrentCapture = currentCapture;
        }
    }
}
