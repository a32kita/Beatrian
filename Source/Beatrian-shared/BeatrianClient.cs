using System;
using System.Collections.Generic;
using System.Text;

using Beatrian.Internal;

namespace Beatrian
{
    class BeatrianClient
    {
        // ����J�t�B�[���h
        private BeatrianInitializeConfig _config;


        // �R���X�g���N�^

        public BeatrianClient(BeatrianInitializeConfig config)
        {
            this._config = config;
            this._config.VerifyConfigurations();
        }

        
        // ����J���\�b�h
    }
}
