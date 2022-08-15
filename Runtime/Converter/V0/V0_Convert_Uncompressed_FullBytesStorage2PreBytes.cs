using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Uncompressed_FullBytesStorage2PreBytes : AConvert_Uncompressed_FullBytesStorage2PreBytesMono
    {

        public byte m_soloPackageTypeId = 4;
        public byte m_multiPackageTypeId = 5;


        public override void Convert(in Int32BitsArray2DSoloPackageFullBytesWrapper source, 
            ref Int32BitsArray2DSoloPackagePreBytesWrapper result)
        {
            throw new System.NotImplementedException();
        }

        public override void Convert(in Int32BitsArray2DMultiPackageFullBytesWrapper source, 
            ref Int32BitsArray2DMultiPackagePreBytesWrapperWithDate result)
        {
            if (result == null)
                result = new Int32BitsArray2DMultiPackagePreBytesWrapperWithDate();
            if (m_multiPackageTypeId == source.m_data.m_compressedInOneBlockOfBytesToStore[0]
                && m_multiPackageTypeId == source.m_data.m_compressedInOneBlockOfBytesToStore[1] )
            { 
                Convert(in source.m_data.m_compressedInOneBlockOfBytesToStore , ref result);
            }

        }
        public  void Convert(in byte[] finalArray,
           ref Int32BitsArray2DMultiPackagePreBytesWrapperWithDate result)
        {
            int sizeInResult = finalArray.Length - 22;
            Eloi.E_PrimitiveBoolUtility.EightBytesToLong(
                       in finalArray[2]
                      , in finalArray[3]
                      , in finalArray[4]
                      , in finalArray[5]
                      , in finalArray[6]
                      , in finalArray[7]
                      , in finalArray[8]
                      , in finalArray[9]
                      , out long date
                      );
            result.m_sentTime = new DateTime(date);
            result.m_receivedTime = DateTime.Now;
            result.m_tickElapsed = (result.m_receivedTime.Ticks - result.m_sentTime.Ticks);
            result.m_millisecondsElapsed = result.m_tickElapsed/ 10000.0;
            result.m_secondsElapsed = result.m_tickElapsed / 10000000.0;
            Eloi.E_PrimitiveBoolUtility.FourBytesToUInt(
                         in finalArray[10]
                        , in finalArray[11]
                        , in finalArray[12]
                        , in finalArray[13]
                        , out result.m_data.m_contextId.m_contextId
                        );
            Eloi.E_PrimitiveBoolUtility.TwoBytesToUshort(
                         in finalArray[14]
                        , in finalArray[15]
                        , out result.m_data.m_width
                        ); 
            Eloi.E_PrimitiveBoolUtility.TwoBytesToUshort(
                         in finalArray[16]
                         , in finalArray[17]
                         , out result.m_data.m_height
                         );
            Eloi.E_PrimitiveBoolUtility.FourBytesToInt(
                         in finalArray[18]
                        , in finalArray[19]
                        , in finalArray[20]
                        , in finalArray[21]
                        , out result.m_data.m_startByteIndex1DAsByte
                        );

            if (result.m_data.m_arrayOfBitUnderIntAsBytesGroup == null ||
                result.m_data.m_arrayOfBitUnderIntAsBytesGroup.Length != sizeInResult)
                result.m_data.m_arrayOfBitUnderIntAsBytesGroup = new byte[sizeInResult];

            Buffer.BlockCopy(finalArray, finalArray.Length- sizeInResult,
                result.m_data.m_arrayOfBitUnderIntAsBytesGroup,
                0, sizeInResult);

            
        }
    }
}
