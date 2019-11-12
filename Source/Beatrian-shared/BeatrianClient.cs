using System;
using System.Collections.Generic;
using System.Text;

using Beatrian.Internal;
using Beatrian.Handling;
using Beatrian.Internal.WebRequestorElements;

namespace Beatrian
{
    class BeatrianClient
    {
        // ����J�t�B�[���h
        private BeatrianInitializeConfig _config;

        private BeatrianUpdatedEventHandler _pageUpdated;


        // ���J�v���p�e�B


        // ���J�C�x���g

        /// <summary>
        /// �Ώۂ̃y�[�W�̍X�V�����o�����Ƃ��ɔ������܂��B
        /// </summary>
        public event BeatrianUpdatedEventHandler PageUpdated
        {
            add => this._pageUpdated += value;
            remove => this._pageUpdated -= value;
        }


        // �R���X�g���N�^

        /// <summary>
        /// �����\���� <see cref="BeatrianInitializeConfig"/> ���w�肵�āA <see cref="BeatrianClient"/> �N���X�̐V�����C���X�^���X�����������܂��B
        /// </summary>
        /// <param name="config"></param>
        public BeatrianClient(BeatrianInitializeConfig config)
        {
            this._config = config;
            this._config.VerifyConfigurations();
        }


        // ����J���\�b�h



        
        // ���J���\�b�h

        
    }
}
