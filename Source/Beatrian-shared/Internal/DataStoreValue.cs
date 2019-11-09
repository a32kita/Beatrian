using System;
using System.Collections.Generic;
using System.Text;

namespace Beatrian.Internal
{
    internal class DataStoreValue
    {
        // 公開プロパティ

        public DateTime UpdatedTime
        {
            get;
            set;
        }

        public uint ContentLength
        {
            get;
            set;
        }

        public byte[] Content
        {
            get;
            set;
        }
    }
}
