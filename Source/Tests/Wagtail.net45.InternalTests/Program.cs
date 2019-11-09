using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wagtail.WagInternal;

namespace Wagtail.net45.InternalTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Test001();
            Test002();
            Test003();
            Test901();
        }

        /// <summary>
        /// Test001: DataStore への任意のデータの書き込み
        /// </summary>
        static void Test001()
        {
            var ms = new MemoryStream();
            var dataStore = new DataStore(ms, true);

            var updateTime = new DateTime(9999, 12, 30, 23, 58, 59, 999);
            var content = Encoding.GetEncoding("utf-16").GetBytes("こんにちは！世界");
            dataStore.SetValue(new DataStoreValue()
            {
                UpdatedTime = updateTime,
                ContentLength = (uint)content.Length,
                Content = content
            });


            // 読み取り
            var value = dataStore.GetValue();

            // 日付の確認
            if (value.UpdatedTime != updateTime)
                throw new Exception("UpdateTime が書き込み時と異なる値で展開されました。");
        }

        /// <summary>
        /// Test002: DataStoreExtensions による DataStoreValue の自動設定
        /// </summary>
        static void Test002()
        {
            var source = "<test>hello, world!!</test>";
            var dataStoreValue = new DataStoreValue();

            dataStoreValue.SetHtmlSource(source);

            // 内容の確認
            if (dataStoreValue.GetHtmlSource() != source)
                throw new Exception("格納したデータと異なる内容が展開されました。");

            // ContentLength の確認 (utf-16 のため、半角文字列の場合、文字数の 2 倍のサイズとなる)
            if (dataStoreValue.ContentLength != (uint)(source.Length * 2))
                throw new Exception("ContentLength の値が誤りです。");
        }

        /// <summary>
        /// Test003: 内容の変更
        /// </summary>
        static void Test003()
        {
            var dataStoreValue = new DataStoreValue();

            var source = "<test>hello, world!!</test>";
            dataStoreValue.SetHtmlSource(source);

            source = "<test2>hello, world!!</test2>";
            dataStoreValue.SetHtmlSource(source);

            // 内容の確認
            if (dataStoreValue.GetHtmlSource() != source)
                throw new Exception("格納したデータと異なる内容が展開されました。");

            // ContentLength の確認 (utf-16 のため、半角文字列の場合、文字数の 2 倍のサイズとなる)
            if (dataStoreValue.ContentLength != (uint)(source.Length * 2))
                throw new Exception("ContentLength の値が誤りです。");
        }

        /// <summary>
        /// Test901: ファイルへの出力 (生データ確認)
        /// </summary>
        static void Test901()
        {
            var dataStoreValue = new DataStoreValue();

            var source = "hello, world!!";
            dataStoreValue.SetHtmlSource(source);

            using (var fs = File.Open("Test901.txt", FileMode.Create, FileAccess.ReadWrite))
            using (var dataStore = new DataStore(fs, true))
            {
                // 内容の書き込み
                dataStore.SetValue(dataStoreValue);

                // 内容の確認
                if (dataStoreValue.GetHtmlSource() != source)
                    throw new Exception("格納したデータと異なる内容が展開されました。");

                // ContentLength の確認 (utf-16 のため、半角文字列の場合、文字数の 2 倍のサイズとなる)
                if (dataStoreValue.ContentLength != (uint)(source.Length * 2))
                    throw new Exception("ContentLength の値が誤りです。");
            }
        }
    }
}
