using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Beatrian.DiffDetection;
using Beatrian.Handling;
using Beatrian.Internal;
using Beatrian.Internal.WebRequestorElements;

namespace Beatrian
{
    class BeatrianClient : IDisposable
    {
        // ����J�t�B�[���h
        private BeatrianInitializeConfig _config;
        private DataStore _dataStore;
        private HttpClientPool _httpClientPool;

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

            // DataStore �̏�����
            this._initializeDataStore();

            // HttpClient �v�[���̏�����
            this._httpClientPool = new HttpClientPool(new HttpClientPoolInitializeParameter()
            {
                MaxClients = this._config.HttpClientPoolSize,
            });
        }


        // ����J���\�b�h

        private void _initializeDataStore()
        {
            this._dataStore = new DataStore(this._config.CacheDataStream, false);

            switch (this._config.CacheMode)
            {
                case BeatrianCacheMode.CreateNew:
                    // ��̃f�[�^����������ł���
                    this._dataStore.SetValue(null);
                    break;
                case BeatrianCacheMode.Load:
                    // �ǂ߂�̂��m�F�������Ă���
                    try
                    {
                        this._dataStore.GetValueWithoutContent();
                    }
                    catch (Exception ex)
                    {
                        throw new BeatrianConfigException($"{nameof(BeatrianInitializeConfig.CacheMode)} �� {nameof(BeatrianCacheMode.Load)} ���w�肳��܂������A�w�肳�ꂽ�X�g���[���ɂ͗L���ȃL���b�V�������݂��Ă���܂���B", nameof(BeatrianInitializeConfig.CacheMode), ex);
                    }
                    break;
                case BeatrianCacheMode.Auto:
                    // �ǂ߂Ȃ���΋�̃f�[�^����������
                    try
                    {
                        this._dataStore.GetValueWithoutContent();
                    }
                    catch
                    {
                        this._dataStore.SetValue(null);
                    }
                    break;
                default:
                    throw new NotImplementedException($"��������Ă��Ȃ� {nameof(BeatrianCacheMode)} ���w�肳��܂����B");
            }
        }

        
        // ���J���\�b�h

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            var currentPageStream = new MemoryStream();
            await WebRequestor.ExecuteRequestAsync(this._config, currentPageStream, this._httpClientPool);

            var storedData = this._dataStore.GetValue();
            if (storedData == null)
            {
                // 1 ��ڂ̎擾���ǂ��Ή�����̂��@�� �C�x���g���o or ����
                this._pageUpdated?.Invoke(this, new BeatrianUpdatedEventArgs());
                return;
            }

            var oldPageStream = new MemoryStream(storedData.Content);
            var oldCapture = new DiffDetectionCapture() { CapturedDataStream = oldPageStream };
            var currentCapture = new DiffDetectionCapture() { CapturedDataStream = currentPageStream };

            var diffDetector = new DiffDetector(oldCapture, currentCapture);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this._dataStore.Dispose();
            this._httpClientPool.Dispose();
        }
    }
}
