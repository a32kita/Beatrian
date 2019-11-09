using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Beatrian
{
    /// <summary>
    /// 構成パラメータに問題があることを表します。
    /// </summary>
    public class BeatrianConfigException : Exception, IBeatrianException
    {
        // プロパティ

        /// <summary>
        /// 問題の対象となっている構成パラメータの名前を取得します。
        /// </summary>
        public string ConfigParameterName
        {
            get;
            private set;
        }


        // コンストラクタ
        
        /// <summary>
        /// メッセージと対象の構成パラメータの名前を指定して、 <see cref="BeatrianConfigException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="configParameterName"></param>
        public BeatrianConfigException(string message, string configParameterName) : base(message)
        {
            this.ConfigParameterName = configParameterName;
        }

        /// <summary>
        /// メッセージ、対象の構成パラメータの名前、内部例外を指定して、 <see cref="BeatrianConfigException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="configParameterName"></param>
        /// <param name="innerException"></param>
        public BeatrianConfigException(string message, string configParameterName, Exception innerException) : base(message, innerException)
        {
            this.ConfigParameterName = configParameterName;
        }
    }
}
