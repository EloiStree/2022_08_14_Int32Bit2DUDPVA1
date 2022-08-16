using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class UDPUtility
{
    public const int MaxBytePayloadSize = 65527;
    public const int MaxBytePayloadMod32Size = 65472;
    public const int Max65527 = 65527;
    public const int MaxMod32_65472 = 65472;
    
}
public class Exp_Int32BitsToBytesPackageSender : MonoBehaviour
{
    public byte m_messageType;
    public int m_channelId;
    public ArrayAs32BitsIntWrapper  m_bitsArrayWrapper= new ArrayAs32BitsIntWrapper ();
    public IArrayAs32BitsIntWrapperGet m_boolRegisterGet;
    public IArrayAs32BitsIntWrapperSet m_boolRegisterSet;
    public int m_lenght;

    ArrayAs32BitsInt m_toSend;
    bool[] m_sent32First;

    public long m_timeUsedToCopy;
    public int m_packageCount;

     ArrayAs32BitsIntOneBlock m_sentSolo;
     ArrayAs32BitsIntBlockSegment m_sentMulti;

    public BytesToSend m_sent;
    [System.Serializable]
    public class BytesToSend : UnityEvent<byte[]> { }

    public byte m_soloPackageId = 1;
    public byte m_multiPackageId = 2;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        //m_bitsArrayWrapper.SetArrayWithRef(ref m_bitsArray.m_storageInt);
        m_boolRegisterGet = m_bitsArrayWrapper;
        m_boolRegisterSet = m_bitsArrayWrapper;
        m_boolRegisterSet.ResetForNBits(m_lenght);
        m_bitsArrayWrapper.GetRefToArray(out m_toSend.m_storageInt);
        m_boolRegisterSet.SetBit(0, true);
        m_boolRegisterSet.SetBit(1, true);
        m_boolRegisterSet.SetBit(2, true);
        m_boolRegisterSet.SetBit(3, true);
        m_boolRegisterSet.SetBit(33, true);
    }


     int[] m_humm;
    public int packageCount;


    public bool[] m_firstByte;
    public bool[] m_lastByte;
    public void SetWithInt32BitsArray(int[] _32bitsArray)
    {
        m_bitsArrayWrapper.SetArrayWithRef(ref _32bitsArray);
    }
    public void SetWithInt32BitsArrayAndSend(int[] _32bitsArray)
    {
        m_bitsArrayWrapper.SetArrayWithRef(ref _32bitsArray); 
        Refresh();
    }
    [ContextMenu("Send")]
    public void Refresh()
    {
        //if (!Application.isPlaying)
            Init();

        m_boolRegisterGet.GetTotalBitLenght(out int lenght);
        for (int i = 8; i < lenght; i++)
        {
            m_boolRegisterSet.SetBit(in i, UnityEngine.Random.value>0.5);
        }

        /// 65507 bytes max  == 524,056 bit
        /// %32 = 3  => -32 give 65,472 
        /// That let's us 35 bytes to tag info;

        m_boolRegisterGet.GetRefToArray(out int[] values);
        m_boolRegisterGet.GetTotalByteLenght(out int totaleByte);
        byte[] valuesAsBytes = new byte[totaleByte];
        ConvertPrimitiveArray2Byte.ConvertIntsToBytes(
            in values, ref valuesAsBytes, out m_timeUsedToCopy);

        //m_humm = new int[values.Length];
        //ConvertPrimitiveArray2Byte.ConvertBytesToInt(
        //    in valuesAsBytes , ref m_humm,  out m_timeUsedToCopy);
        packageCount =  (totaleByte / UDPUtility.MaxMod32_65472) +1;
        if (packageCount == 1)
        {

            byte[] packageToSent = new byte[totaleByte + 12];
            Buffer.BlockCopy(valuesAsBytes, 0, packageToSent, 12, valuesAsBytes.Length);
            packageToSent[0] = m_soloPackageId;
            packageToSent[1] = m_soloPackageId;
            packageToSent[2] = (byte)m_sentSolo.m_channalId;
            packageToSent[3] = (byte)(m_sentSolo.m_channalId >> 8);
            Eloi.E_PrimitiveBoolUtility.LongToEightBytes(
                 DateTime.Now.Ticks
                , out packageToSent[4]
                , out packageToSent[5]
                , out packageToSent[6]
                , out packageToSent[7]
                , out packageToSent[8]
                , out packageToSent[9]
                , out packageToSent[10]
                , out packageToSent[11]);
            m_sent.Invoke(packageToSent);

        }
        else if (packageCount > 1)
        {
            int byteLeft = totaleByte;
            int index = 0;
            int antiLoop = 50;
            while(byteLeft>0 && antiLoop>0)
            {
                if (byteLeft > UDPUtility.MaxMod32_65472)
                {
                    byte[] packageToSent = new byte[UDPUtility.MaxMod32_65472 + 16];
                    Buffer.BlockCopy(valuesAsBytes, index, packageToSent, 16, UDPUtility.MaxMod32_65472);
                    packageToSent[0] = m_multiPackageId;
                    packageToSent[1] = m_multiPackageId;
                    packageToSent[2] = (byte)m_sentSolo.m_channalId;
                    packageToSent[3] = (byte)(m_sentSolo.m_channalId >> 8);
                    Eloi.E_PrimitiveBoolUtility.LongToEightBytes(
                         DateTime.Now.Ticks
                        , out packageToSent[4]
                        , out packageToSent[5]
                        , out packageToSent[6]
                        , out packageToSent[7]
                        , out packageToSent[8]
                        , out packageToSent[9]
                        , out packageToSent[10]
                        , out packageToSent[11]);
                    Eloi.E_PrimitiveBoolUtility.IntToFourBytes(
                        in index
                        , out packageToSent[12]
                        , out packageToSent[13]
                        , out packageToSent[14]
                        , out packageToSent[15]);
                    m_sent.Invoke(packageToSent);
                    index += 65472;
                    byteLeft -= 65472;
                }
                else {
                    byte[] packageToSent = new byte[byteLeft + 16];
                    Buffer.BlockCopy(valuesAsBytes, index, packageToSent, 16, byteLeft);
                    packageToSent[0] = m_multiPackageId;
                    packageToSent[1] = m_multiPackageId;
                    packageToSent[2] = (byte)m_sentSolo.m_channalId;
                    packageToSent[3] = (byte)(m_sentSolo.m_channalId >> 8);
                    Eloi.E_PrimitiveBoolUtility.LongToEightBytes(
                        DateTime.Now.Ticks
                        , out packageToSent[4]
                        , out packageToSent[5]
                        , out packageToSent[6]
                        , out packageToSent[7]
                        , out packageToSent[8]
                        , out packageToSent[9]
                        , out packageToSent[10]
                        , out packageToSent[11]);
                    Eloi.E_PrimitiveBoolUtility.IntToFourBytes(
                        in index
                        , out packageToSent[12]
                        , out packageToSent[13]
                        , out packageToSent[14]
                        , out packageToSent[15]);
                    m_sent.Invoke(packageToSent);
                    index += byteLeft;
                    byteLeft =0;
                }
                antiLoop--;
            }
        }




        //Eloi.E_PrimitiveBoolUtility.IntTo32BitsRef(in
        //   m_toSend.m_storageInt[0], ref m_sent32First);
        //Eloi.E_PrimitiveBoolUtility.IntTo32BitsRef(in
        //   m_received.m_storageInt[0], ref m_received32First);
    }

}
