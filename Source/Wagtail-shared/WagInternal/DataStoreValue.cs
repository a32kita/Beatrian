using System;
using System.Collections.Generic;
using System.Text;

namespace Wagtail.WagInternal
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
