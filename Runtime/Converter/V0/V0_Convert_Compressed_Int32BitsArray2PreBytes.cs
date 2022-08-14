using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Compressed_Int32BitsArray2PreBytes : AConvert_Compressed_Int32BitsArray2PreBytesMono
    {
        public Int32BitsArray2BytesSolo m_converter = new Int32BitsArray2BytesSolo();
        public Int32BitsArray2BytesMultiple m_conertInBlock = new Int32BitsArray2BytesMultiple();
        public override void Convert(in Int32BitsArray2DWrapper source, ref Int32BitsArray2DSoloPackagePreBytesWrapper result)
        {
            result.m_data.m_height = source.m_data.m_height;
            result.m_data.m_width = source.m_data.m_width;
            result.m_data.m_contextId = source.m_data.m_contextId;
            m_converter.Convert(
                in source.m_data.m_arrayOfBitUnderInt,
                ref result.m_data.m_arrayOfBitUnderIntAsBytesGroup);

        }

        public override void Convert(in Int32BitsArray2DWrapper source, ref List<Int32BitsArray2DMultiPackagePreBytesWrapper> result, in int maxBitsAllowedSize = 65472)
        {

            m_conertInBlock.Convert(in source.m_data.m_arrayOfBitUnderInt,
                in maxBitsAllowedSize,
                ref result);
            for (int i = 0; i < result.Count; i++)
            {
                result[i].m_data.m_height = source.m_data.m_height;
                result[i].m_data.m_width = source.m_data.m_width;
                result[i].m_data.m_contextId = source.m_data.m_contextId;
            }
        }

        public override bool IsSmallerThatNBits(in int maxBitsAllowedSize, in Int32BitsArray2DWrapper whatToConvert, bool orEqual)
        {
            if(orEqual)
                return maxBitsAllowedSize <= whatToConvert.m_data.m_arrayOfBitUnderInt.Length * 32;
            else
                return maxBitsAllowedSize < whatToConvert.m_data.m_arrayOfBitUnderInt.Length * 32;
        }
    }


    [System.Serializable]
    public class Int32BitsArray2BytesSolo
    {
        public long m_lastConvetedWatchTime;
        public void Convert(in int[] array, ref byte[] output)
        {

            int byteCountNeeded = array.Length * 4;
            if (output == null || output.Length != byteCountNeeded)
                output = new byte[byteCountNeeded];
            ConvertPrimitiveArray2Byte.ConvertIntsToBytes(
                in array, ref output, out m_lastConvetedWatchTime);
        }
    }
    [System.Serializable]
    public class Int32BitsArray2BytesMultiple
    {
        public int m_maxSizeinByte=65472;
        public long m_lastConvetedWatchTime;
        //public void Convert(in int[] array, in int maxSizeMod32InByte, ref List<byte[]> result)
        //{
        //    m_maxSizeinByte = maxSizeMod32InByte;

        //    if (result == null)
        //        result = new List<byte[]>();

        //    int blockCount = Mathf.CeilToInt((array.Length * 4) / m_maxSizeinByte);
        //    int blockRest = (array.Length * 4) % m_maxSizeinByte;

        //    for (int i = 0; i < blockCount; i++)
        //    {
        //        if (i == blockCount - 1)
        //        {
        //            if (result[i] == null || result[i].Length != blockRest)
        //                result.Add(new byte[blockRest]);
        //        }
        //        else
        //        {
        //            if (result[i] == null || result[i].Length != m_maxSizeinByte)
        //                result.Add(new byte[m_maxSizeinByte]);
        //        }
        //    }

        //    for (int i = 0; i < blockCount; i++)
        //    {
        //        if (i == blockCount - 1)
        //        {
        //            Buffer.BlockCopy(array, 0, result[i], i * maxSizeMod32InByte, blockRest);
        //        }
        //        else
        //        {
        //            Buffer.BlockCopy(array, 0, result[i], i * maxSizeMod32InByte, maxSizeMod32InByte);
        //        }
        //    }
        //}

        public float m_blockCountAsFloat;
        public int m_lastBlockCount;
        public int m_lastBlockRest;
        public List<Int32BitsArray2DMultiPackagePreBytesWrapper> m_log;
        public void Convert(in int[] array, in int maxSizeMod32InByte, ref List<Int32BitsArray2DMultiPackagePreBytesWrapper> result)
        {
            m_maxSizeinByte = maxSizeMod32InByte;

           

            m_blockCountAsFloat = (array.Length * 4f) / m_maxSizeinByte;

            int blockCount = Mathf.CeilToInt(m_blockCountAsFloat);
            m_lastBlockCount = blockCount;
            int blockRest = (array.Length * 4) % m_maxSizeinByte;
            m_lastBlockRest = blockRest;
            bool hasRest= blockRest > 0;
            if (!hasRest)
                blockCount--;

            if (result == null) { 
                result = new List<Int32BitsArray2DMultiPackagePreBytesWrapper>(blockCount);
            }
            m_log = result;

            while (result.Count < blockCount)
            {
                result.Add(new Int32BitsArray2DMultiPackagePreBytesWrapper());
            }
            while (result.Count > blockCount)
            {
                result.RemoveAt(result.Count - 1);
            }


            for (int i = 0; i < blockCount; i++)
            {

                if (hasRest && i == blockCount - 1)
                {
                    if ( result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup==null
                        || result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup.Length != blockRest ) {
                        result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup = new byte[blockRest];
                        result.Add(result[i]);
                    }
                }
                else
                {
                    if (result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup == null 
                        || result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup.Length != m_maxSizeinByte )
                    { 
                        result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup = new byte[m_maxSizeinByte];
                        result.Add(result[i]);
                    }
                }
            }

            for (int i = 0; i < blockCount; i++)
            {
                result[i].m_data.m_startByteIndex1D = i * maxSizeMod32InByte;
                if (hasRest && i == blockCount - 1)
                {
                    Buffer.BlockCopy(array, i * maxSizeMod32InByte, result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup, 0, blockRest);
                }
                else
                {
                    Buffer.BlockCopy(array, i * maxSizeMod32InByte, result[i].m_data.m_arrayOfBitUnderIntAsBytesGroup,0, maxSizeMod32InByte);
                }
            }
        }
    }

}


