using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Beatrian.Internal
{
    internal class DataStore : IDisposable
    {
        // 非公開フィールド
        private Stream _dataStream;
        private BinaryReader _binaryReader;
        private BinaryWriter _binaryWriter;
        private bool _streamLeaveOpen;

        private static readonly string _MAGICNUMBER;
        private static readonly uint _VERSION;


        // コンストラクタ

        /// <summary>
        /// 読み書き可能なストリームを指定して、 <see cref="DataStore"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dataStream"></param>
        /// <param name="leaveOpen"></param>
        public DataStore(Stream dataStream, bool leaveOpen)
        {
            this._dataStream = dataStream;
            this._streamLeaveOpen = leaveOpen;

            try
            {
                this._intializeStreamAccess();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ストリームへのアクセスの初期化に失敗しました。" + ex.Message, ex);
            }
        }

        /// <summary>
        /// <see cref="DataStore"/> クラスを初期化します。
        /// </summary>
        static DataStore()
        {
            _MAGICNUMBER = "BEATRIAN DATASTORE ";
            _VERSION = 1;
        }


        // 非公開メソッド

        /// <summary>
        /// <see cref="DataStore._dataStream"/> が有効であるか確認し、アクセスを初期化します。
        /// </summary>
        private void _intializeStreamAccess()
        {
            if (!this._dataStream.CanRead)
                throw new InvalidOperationException("読み込みが有効でないストリームが指定されました。");

            if (!this._dataStream.CanWrite)
                throw new InvalidOperationException("書き込みが有効でないストリームが指定されました。");

            if (!this._dataStream.CanSeek)
                throw new InvalidOperationException("シークが有効でないストリームが指定されました。");

            this._binaryReader = new BinaryReader(this._dataStream, Encoding.UTF8, true);
            this._binaryWriter = new BinaryWriter(this._dataStream, Encoding.UTF8, true);
        }

        private string _readASCIIString(int len)
        {
            var buf = this._binaryReader.ReadBytes(len);
            return Encoding.ASCII.GetString(buf);
        }

        private void _writeASCIIString(string value)
        {
            var buf = Encoding.ASCII.GetBytes(value);
            this._binaryWriter.Write(buf);
        }

        private DateTime _readDateTime()
        {
            var y = this._binaryReader.ReadUInt16();
            var md = this._binaryReader.ReadUInt16();
            var m = md / 100;
            var d = md % 100;
            var hms = this._binaryReader.ReadUInt32();
            var hour = hms / 10000;
            var min = hms / 100 % 100;
            var sec = hms % 100;
            var ms = this._binaryReader.ReadUInt16();

            return new DateTime((int)y, (int)m, (int)d, (int)hour, (int)min, (int)sec, (int)ms);
        }

        private void _writeDateTime(DateTime value)
        {
            this._binaryWriter.Write((ushort)value.Year);
            this._binaryWriter.Write((ushort)(value.Month * 100 + value.Day));
            this._binaryWriter.Write((uint)(value.Hour * 10000 + value.Minute * 100 + value.Second));
            this._binaryWriter.Write((ushort)value.Millisecond);
            this._binaryWriter.Flush();
        }

        private uint _readUInt32()
        {
            return this._binaryReader.ReadUInt32();
        }

        private void _writeUInt32(uint value)
        {
            this._binaryWriter.Write(value);
            this._binaryWriter.Flush();
        }

        private DataStoreState _readDataStoreState()
        {
            return (DataStoreState)_readUInt32();
        }

        private void _writeDataStoreState(DataStoreState value)
        {
            this._writeUInt32((uint)value);
        }

        private byte[] _readBuffer()
        {
            var len = (long)this._binaryReader.ReadUInt32();
            return this._binaryReader.ReadBytes((int)len);
        }

        private void _writeBuffer(byte[] buffer)
        {
            this._binaryWriter.Write((uint)buffer.LongLength);
            this._binaryWriter.Write(buffer);
            this._binaryWriter.Flush();
        }

        private void _verifyMagicNumber()
        {
            if (this._readASCIIString(_MAGICNUMBER.Length) != _MAGICNUMBER)
                throw new NotSupportedException("データ形式の検証に失敗しました。");

            if (this._readUInt32() != _VERSION)
                throw new NotSupportedException("バージョン互換性の確認に失敗しました。");
        }


        // 公開メソッド

        /// <summary>
        /// <see cref="DataStoreValue"/> を格納します。
        /// </summary>
        /// <param name="storeValue">格納する <see cref="DataStoreValue"/> を指定します。 null を指定すると、格納状態でないことが書き込まれます。</param>
        public void SetValue(DataStoreValue storeValue)
        {
            this._dataStream.SetLength(0);
            this._dataStream.Seek(0, SeekOrigin.Begin);

            this._writeASCIIString(_MAGICNUMBER);
            this._writeUInt32(_VERSION);
            
            if (storeValue == null)
            {
                // 空の DataStore
                this._writeDataStoreState(DataStoreState.Empty);
                return;
            }

            this._writeDataStoreState(DataStoreState.Stored);
            this._writeDateTime(storeValue.UpdatedTime);
            this._writeUInt32(storeValue.ContentLength);
            this._writeBuffer(storeValue.Content);
        }

        /// <summary>
        /// <see cref="DataStoreValue"/> を読み取ります。格納状態でない場合、 null が返ります。
        /// </summary>
        /// <returns></returns>
        public DataStoreValue GetValue()
        {
            this._dataStream.Seek(0, SeekOrigin.Begin);
            this._verifyMagicNumber();

            if (this._readDataStoreState() == DataStoreState.Empty)
                // 空の DataStore
                return null;

            return new DataStoreValue()
            {
                UpdatedTime = this._readDateTime(),
                ContentLength = this._readUInt32(),
                Content = this._readBuffer(),
            };
        }

        public DataStoreValue GetValueWithoutContent()
        {
            this._dataStream.Seek(0, SeekOrigin.Begin);
            this._verifyMagicNumber();

            if (this._readDataStoreState() == DataStoreState.Empty)
                // 空の DataStore
                return null;

            return new DataStoreValue()
            {
                UpdatedTime = this._readDateTime(),
                ContentLength = this._readUInt32(),
            };
        }

        public void Dispose()
        {
            this._binaryReader.Dispose();
            this._binaryWriter.Dispose();

            if (!this._streamLeaveOpen)
                this._dataStream.Dispose();
        }
    }
}
