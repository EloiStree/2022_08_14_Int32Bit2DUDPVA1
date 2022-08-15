using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    [System.Serializable]
    public class TextureSourceToInt32BitsArray2DWrapper
    {
        public TextureSourceToInt32BitsArray2D m_data;
    }

    public abstract class AConvert_Compressed_Texture2Int32BitsArrayMono : MonoBehaviour, IConvert_Compressed_Texture2Int32BitsArray
    {
        public abstract void Convert(in TextureSourceToInt32BitsArray2DWrapper source, ref Int32BitsArray2DWrapper result);
    }
    public abstract class AConvert_Compressed_Texture2Int32BitsArray : IConvert_Compressed_Texture2Int32BitsArray
    {
        public abstract void Convert(in TextureSourceToInt32BitsArray2DWrapper source, ref Int32BitsArray2DWrapper result);
    }
    public interface IConvert_Compressed_Texture2Int32BitsArray
    {
        public void Convert(in TextureSourceToInt32BitsArray2DWrapper source, ref Int32BitsArray2DWrapper result);
    }
    public abstract class AConvert_Uncompressed_Int32BitsArray2TextureMono : MonoBehaviour, IConvert_Uncompressed_Int32BitsArray2Texture
    {
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref TextureSourceToInt32BitsArray2DWrapper result);
    }
    public abstract class AConvert_Uncompressed_Int32BitsArray2Texture : IConvert_Uncompressed_Int32BitsArray2Texture
    {
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref TextureSourceToInt32BitsArray2DWrapper result);
    }
    public interface IConvert_Uncompressed_Int32BitsArray2Texture
    {
        public void Convert(in Int32BitsArray2DWrapper source, ref TextureSourceToInt32BitsArray2DWrapper result);
    }

    [System.Serializable]
    public class Int32BitsArray2DWrapper
    {
        public Int32BitsArray2D m_data;
    }
    public abstract class AConvert_Compressed_Int32BitsArray2PreBytesMono : MonoBehaviour, IConvert_Compressed_Int32BitsArray2PreBytes
    {
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref List<Int32BitsArray2DMultiPackagePreBytesWrapper> result, in int maxBitsAllowedSize = 65472);
        public abstract bool IsSmallerThatNBits(in int maxBitsAllowedSize, in Int32BitsArray2DWrapper whatToConvert, bool orEqual=false);
    }
    public abstract class AConvert_Compressed_Int32BitsArray2PreBytes : IConvert_Compressed_Int32BitsArray2PreBytes
    {
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DWrapper source, ref List<Int32BitsArray2DMultiPackagePreBytesWrapper> result, in int maxBitsAllowedSize = 65472);
        public abstract bool IsSmallerThatNBits(in int maxBitsAllowedSize, in Int32BitsArray2DWrapper whatToConvert, bool orEqual = false);
    }
    public interface IConvert_Compressed_Int32BitsArray2PreBytes
    {
        // 65507 Max udp Payload | 65504 Max with mod 32  | 65472 max with mod 32 less on int32bit for space
        public  bool IsSmallerThatNBits(in int maxBitsAllowedSize, in Int32BitsArray2DWrapper whatToConvert, bool orEqual = false);
        public void Convert(in Int32BitsArray2DWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result);
        public void Convert(in Int32BitsArray2DWrapper source, ref List<Int32BitsArray2DMultiPackagePreBytesWrapper> result, in int maxBitsAllowedSize = 65472);
    }
    public abstract class AConvert_Uncompressed_PreBytes2Int32BitsArrayMono : MonoBehaviour, IConvert_Uncompressed_PreBytes2Int32BitsArray
    {
        public abstract void ConvertAs(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public abstract void ConvertIn(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public abstract void ConvertIn(in IEnumerable<Int32BitsArray2DMultiPackagePreBytesWrapper> sources, ref Int32BitsArray2DWrapper result);
    }
    public abstract class AConvert_Uncompressed_PreBytes2Int32BitsArray : IConvert_Uncompressed_PreBytes2Int32BitsArray
    {
        public abstract void ConvertAs(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public abstract void ConvertIn(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public abstract void ConvertIn(in IEnumerable<Int32BitsArray2DMultiPackagePreBytesWrapper> sources, ref Int32BitsArray2DWrapper result);
    }
    public interface IConvert_Uncompressed_PreBytes2Int32BitsArray
    {
        public void ConvertAs(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public void ConvertIn(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DWrapper result);
        public void ConvertIn(in IEnumerable<Int32BitsArray2DMultiPackagePreBytesWrapper> sources, ref Int32BitsArray2DWrapper result);
    }


    [System.Serializable]
    public class Int32BitsArray2DSoloPackagePreBytesWrapper
    {
        public Int32BitsArray2DSoloPackagePreBytes m_data;
    }
    [System.Serializable]
    public class Int32BitsArray2DMultiPackagePreBytesWrapper
    {
        public Int32BitsArray2DMultiPackagePreBytes m_data;
    }
    [System.Serializable]
    public class Int32BitsArray2DMultiPackagePreBytesWrapperWithDate: Int32BitsArray2DMultiPackagePreBytesWrapper
    {
        public DateTime m_sentTime;
        public DateTime m_receivedTime;
        public long m_tickElapsed;
        public double m_millisecondsElapsed ;
        public double m_secondsElapsed ;
    }
    public abstract class AConvert_Compressed_PreBytes2FullBytesStorageMono : MonoBehaviour, IConvert_Compressed_PreBytes2FullBytesStorage
    {
        public abstract void Convert(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DSoloPackageFullBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DMultiPackageFullBytesWrapper result);
    }
    public abstract class AConvert_Compressed_PreBytes2FullBytesStorage : IConvert_Compressed_PreBytes2FullBytesStorage
    {
        public abstract void Convert(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DSoloPackageFullBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DMultiPackageFullBytesWrapper result);
    }
    public interface IConvert_Compressed_PreBytes2FullBytesStorage
    {
        public void Convert(in Int32BitsArray2DSoloPackagePreBytesWrapper source, ref Int32BitsArray2DSoloPackageFullBytesWrapper result);
        public void Convert(in Int32BitsArray2DMultiPackagePreBytesWrapper source, ref Int32BitsArray2DMultiPackageFullBytesWrapper result);
    }
    public abstract class AConvert_Uncompressed_FullBytesStorage2PreBytesMono : MonoBehaviour, IConvert_Uncompressed_FullBytesStorage2PreBytes
    {
        public abstract void Convert(in Int32BitsArray2DSoloPackageFullBytesWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DMultiPackageFullBytesWrapper source, ref Int32BitsArray2DMultiPackagePreBytesWrapperWithDate result);
    }
    public abstract class AConvert_Uncompressed_FullBytesStorage2PreBytes : IConvert_Uncompressed_FullBytesStorage2PreBytes
    {
        public abstract void Convert(in Int32BitsArray2DSoloPackageFullBytesWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result);
        public abstract void Convert(in Int32BitsArray2DMultiPackageFullBytesWrapper source, ref Int32BitsArray2DMultiPackagePreBytesWrapperWithDate result);
    }
    public interface IConvert_Uncompressed_FullBytesStorage2PreBytes
    {
        public void Convert(in Int32BitsArray2DSoloPackageFullBytesWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper  result);
        public void Convert(in Int32BitsArray2DMultiPackageFullBytesWrapper source, ref Int32BitsArray2DMultiPackagePreBytesWrapperWithDate result);
    }

    [System.Serializable]
    public class Int32BitsArray2DSoloPackageFullBytesWrapper
    {
        public Int32BitsArray2DSoloPackageFullBytes m_data;
    }
    [System.Serializable]
    public class Int32BitsArray2DMultiPackageFullBytesWrapper
    {
        public Int32BitsArray2DMultiPackageFullBytes m_data;
    }
    [System.Serializable]
    public class BytesToUncompressWrapper
    {
        public BytesToUncompress m_data;
        public BytesToUncompressWrapper(ref BytesToUncompress data)
            => m_data = data;
        public BytesToUncompressWrapper(ref byte[] data)
            => m_data.m_rawByteThatCanBeUncompress = data;
        public BytesToUncompressWrapper()
            => m_data.m_rawByteThatCanBeUncompress = new byte[0];
    }
   
}
