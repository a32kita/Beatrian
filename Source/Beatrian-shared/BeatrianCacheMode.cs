using System;
using System.Collections.Generic;
using System.Text;

namespace Beatrian
{
    public enum BeatrianCacheMode
    {
        /// <summary>
        /// 有効なデータが格納されたストリームの場合、自動的に利用します。
        /// 無効なデータが格納されたストリームの場合、初期化します。
        /// </summary>
        Auto = 0,

        /// <summary>
        /// ストリームの内容に関わらず新しいキャッシュとして初期化します。
        /// </summary>
        CreateNew = 1,

        /// <summary>
        /// 有効なデータが格納されたストリームとして利用します。
        /// </summary>
        Load = 2,
    }
}
