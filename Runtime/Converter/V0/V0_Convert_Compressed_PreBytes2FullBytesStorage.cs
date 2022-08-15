using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Compressed_PreBytes2FullBytesStorage : AConvert_Compressed_PreBytes2FullBytesStorageMono
    {

        public byte m_soloPackageTypeId=4;
        public byte m_multiPackageTypeId=5;

        public override void Convert(in Int32BitsArray2DSoloPackagePreBytesWrapper source,
            ref Int32BitsArray2DSoloPackageFullBytesWrapper result)
        {
            throw new System.NotImplementedException();
        }

        public override void Convert(in Int32BitsArray2DMultiPackagePreBytesWrapper source,
            ref Int32BitsArray2DMultiPackageFullBytesWrapper result)
        {
            int freeSpaceNeeded = 22;
            int sourceSize = source.m_data.m_arrayOfBitUnderIntAsBytesGroup.Length;
            int wantedSize = freeSpaceNeeded + sourceSize;
            if (result == null)
                result = new Int32BitsArray2DMultiPackageFullBytesWrapper();
            if (result.m_data.m_compressedInOneBlockOfBytesToStore == null)
                result.m_data.m_compressedInOneBlockOfBytesToStore = new byte[wantedSize];

            byte[] finalArray = result.m_data.m_compressedInOneBlockOfBytesToStore;

            if (finalArray == null ||
                finalArray.Length != wantedSize)
                finalArray = new byte[wantedSize];
            finalArray[0] = m_multiPackageTypeId;
            finalArray[1] = m_multiPackageTypeId;
            Eloi.E_PrimitiveBoolUtility.LongToEightBytes(
                        DateTime.Now.Ticks
                       , out finalArray[2]
                       , out finalArray[3]
                       , out finalArray[4]
                       , out finalArray[5]
                       , out finalArray[6]
                       , out finalArray[7]
                       , out finalArray[8]
                       , out finalArray[9]);
            Eloi.E_PrimitiveBoolUtility.IntToFourBytes(
                        in source.m_data.m_contextId.m_contextId
                        , out finalArray[10]
                        , out finalArray[11]
                        , out finalArray[12]
                        , out finalArray[13]);
            Eloi.E_PrimitiveBoolUtility.UshortToTwoBytes(
                        in source.m_data.m_width
                        , out finalArray[14]
                        , out finalArray[15]);
            Eloi.E_PrimitiveBoolUtility.UshortToTwoBytes(
                        in source.m_data.m_height
                        , out finalArray[16]
                        , out finalArray[17]);
            Eloi.E_PrimitiveBoolUtility.IntToFourBytes(
                        in source.m_data.m_startByteIndex1DAsByte
                        , out finalArray[18]
                        , out finalArray[19]
                        , out finalArray[20]
                        , out finalArray[21]);

            Buffer.BlockCopy(source.m_data.m_arrayOfBitUnderIntAsBytesGroup, 0, finalArray, freeSpaceNeeded, sourceSize);
            result.m_data.m_compressedInOneBlockOfBytesToStore = finalArray;
        }
    }
}
