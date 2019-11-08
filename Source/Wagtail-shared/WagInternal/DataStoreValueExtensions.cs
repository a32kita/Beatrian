using System;
using System.Collections.Generic;
using System.Text;

namespace Wagtail.WagInternal
{
    internal static class DataStoreValueExtensions
    {
        // 非公開静的フィールド
        private static Encoding _enc;


        // コンストラクタ

        static DataStoreValueExtensions()
        {
            _enc = Encoding.GetEncoding("utf-16");
        }


        // 公開静的メソッド

        public static void SetHtmlSource(this DataStoreValue dataStoreValue, string source)
        {
            var contentBuffer = _enc.GetBytes(source);
            dataStoreValue.Content = contentBuffer;
            dataStoreValue.ContentLength = (uint)contentBuffer.Length;
        }

        public static string GetHtmlSource(this DataStoreValue dataStoreValue)
        {
            return _enc.GetString(dataStoreValue.Content);
        }
    }
}
