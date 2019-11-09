using System;
using System.Collections.Generic;
using System.Text;

using Beatrian.Internal;

namespace Beatrian
{
    class BeatrianClient
    {
        // 非公開フィールド
        private BeatrianInitializeConfig _config;


        // コンストラクタ

        public BeatrianClient(BeatrianInitializeConfig config)
        {
            this._config = config;
            this._config.VerifyConfigurations();
        }

        
        // 非公開メソッド
    }
}
