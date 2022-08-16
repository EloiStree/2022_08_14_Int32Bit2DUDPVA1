using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exp_Int32BitsToBytesPackageReceiver : MonoBehaviour
{
    public  ArrayAs32BitsIntOneBlock m_lastReceivedSolo;
     public ArrayAs32BitsIntBlockSegment m_lastReceivedBlock;

    public byte m_soloPackageId = 1;
    public byte m_multiPackageId = 2;

    public UE_ColorAs32BitsIntFullArray m_smallPackageReceived;
    public UE_ColorAs32BitsIntBlockArray m_blockReceived;

    [System.Serializable]
    public class UE_ColorAs32BitsIntFullArray : UnityEvent<ArrayAs32BitsIntOneBlock> { };
    [System.Serializable]
    public class UE_ColorAs32BitsIntBlockArray : UnityEvent<ArrayAs32BitsIntBlockSegment> { };

    public void PushAsReceived( byte [] received) {

        if (received.Length > 2)
        {
            if (received[0] == m_soloPackageId && received[1] == m_soloPackageId)
            {
                PushAsReceived_SoloPackage(received);
            }
            if (received[0] == m_multiPackageId && received[1] == m_multiPackageId)
            {

                PushAsReceived_MultiPackage(received);
            }

        }

    }


    public long m_sent;
    public long m_received;
    public long m_elapsedTime;
    void PushAsReceived_SoloPackage( byte[] received)
    {

        ArrayAs32BitsIntOneBlock package = new ArrayAs32BitsIntOneBlock();
        E_PrimitiveBoolUtility.TwoBytesToUshort(
        in received[2], in received[3], out package.m_channalId);
        byte[] receivedByInt = new byte[received.Length - 12];
        Buffer.BlockCopy(received, 12, receivedByInt, 0, receivedByInt.Length);
        package.m_booleanAsint32bits.m_storageInt = new int[receivedByInt.Length / 4];


        Eloi.E_PrimitiveBoolUtility.EightBytesToLong(
                 in receivedByInt[4]
               , in receivedByInt[5]
               , in receivedByInt[6]
               , in receivedByInt[7]
               , in receivedByInt[8]
               , in receivedByInt[9]
               , in receivedByInt[10]
               , in receivedByInt[11]
               , out m_sent);
        m_received = DateTime.Now.Ticks;
        m_elapsedTime = (m_received - m_sent);

        ConvertPrimitiveArray2Byte.ConvertBytesToInt(
            in receivedByInt, ref package.m_booleanAsint32bits.m_storageInt, out long t);
        m_lastReceivedSolo = package;
        m_smallPackageReceived.Invoke(package);
    }
    void PushAsReceived_MultiPackage(  byte[] received)
    {
        ArrayAs32BitsIntBlockSegment blockPackage = new ArrayAs32BitsIntBlockSegment();
        E_PrimitiveBoolUtility.TwoBytesToUshort(
                in received[2], in received[3],
                out blockPackage.m_channalId);

        Eloi.E_PrimitiveBoolUtility.EightBytesToLong(
                 in received[4]
               , in received[5]
               , in received[6]
               , in received[7]
               , in received[8]
               , in received[9]
               , in received[10]
               , in received[11]
               , out m_sent);
        m_received = DateTime.Now.Ticks;
        m_elapsedTime = (m_received - m_sent);
        E_PrimitiveBoolUtility.FourBytesToInt(
            in received[12], 
            in received[13],
            in received[14],
            in received[15],
            out blockPackage.m_blockStartIndex);

        byte[] receivedByInt = new byte[received.Length - 16];
        Buffer.BlockCopy(received, 16, receivedByInt, 0, receivedByInt.Length);
        blockPackage.m_booleanAsint32bits.m_storageInt = new int[receivedByInt.Length / 4];

        ConvertPrimitiveArray2Byte.ConvertBytesToInt(
            in receivedByInt, ref blockPackage.m_booleanAsint32bits.m_storageInt, out long t);
        m_lastReceivedBlock = blockPackage;
        m_blockReceived.Invoke(blockPackage);
    }

}
