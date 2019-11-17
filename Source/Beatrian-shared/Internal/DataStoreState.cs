using System;
using System.Collections.Generic;
using System.Text;

namespace Beatrian.Internal
{
    internal enum DataStoreState : UInt32
    {
        /// <summary>
        /// 何もデータが格納されていないことを表します。
        /// </summary>
        Empty = 0,

        /// <summary>
        /// データが格納されていることを表します。
        /// </summary>
        Stored = 1,
    }
}
