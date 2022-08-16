using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Int32BitsBawReceiverMono : MonoBehaviour
{

    public Int32BitsBawReceiver m_receiver = new Int32BitsBawReceiver();

    public TextureSourceToInt32BitsArray2DWrapperEvent m_onTextureWrapperReceived;
    public Int32BitsAsTextureEvent m_onTextureReceived;
    public Queue<byte[]> m_inWaitingQueue = new Queue<byte[]>();

    //public void Convertion(in byte[] source, out bool succed,
    //    out TextureSourceToInt32BitsArray2DWrapper representationAffected)
    //{
    //    m_receiver.Convertion(in source, out succed, out representationAffected);
    //    if (succed) { 
    //        m_onTextureWrapperReceived.Invoke(representationAffected);
    //        m_onTextureReceived.Invoke(representationAffected.m_data.m_textureReference);
    //    }
    //}
    private void Awake()
    {
        m_receiver.m_notificationFinishConverting += ProcessNextInQueue;
    }
    private void OnDestroy()
    {

        m_receiver.m_notificationFinishConverting -= ProcessNextInQueue;
    }

    private void ProcessNextInQueue()
    {
        if (m_inWaitingQueue.Count > 0) {
            TryConvertionAndPush(m_inWaitingQueue.Dequeue());
        }
    }

    public void Update()
    {
        if (m_inWaitingQueue.Count > 0 && !m_receiver.m_isConverting) {
            ProcessNextInQueue();
        }
    }
    public void AddToQueueTryConvertionAndPush(byte[] source) {
        m_inWaitingQueue.Enqueue(source);
    }
    public void TryConvertionAndPush( byte[] source)
    {
        if (m_receiver.m_isConverting)
        {
            AddToQueueTryConvertionAndPush(source);
        }
        else { 
            m_receiver.Convertion(in source, out bool succed, out TextureSourceToInt32BitsArray2DWrapper representationAffected);
            if (succed) {
                m_onTextureWrapperReceived.Invoke(representationAffected);
                m_onTextureReceived.Invoke(representationAffected.m_data.m_textureReference);
            }
        }
    }

    [System.Serializable]
    public class TextureSourceToInt32BitsArray2DWrapperEvent : UnityEvent<TextureSourceToInt32BitsArray2DWrapper> { }
    [System.Serializable]
    public class Int32BitsAsTextureEvent : UnityEvent<Texture> { }
}



[System.Serializable]
public class Int32BitsBawReceiver
{
    [Header("Dev Note")]
    public string m_note = "This code is not multi-thread safe";

    [Header("Uncompress")]
    public AConvert_Uncompressed_FullBytesStorage2PreBytesMono i_fullToPreByte;
    public AConvert_Uncompressed_PreBytes2Int32BitsArrayMono i_preByteToInt32Bits;
    public AConvert_Uncompressed_Int32BitsArray2TextureMono i_int32BitsToTexture;
    public IConvert_Uncompressed_FullBytesStorage2PreBytes i3;
    public IConvert_Uncompressed_PreBytes2Int32BitsArray i4;
    public IConvert_Uncompressed_Int32BitsArray2Texture i5;

    [Header("Data")]
    public Int32BitsArray2DMultiPackageFullBytesWrapper m_received_Full = new Int32BitsArray2DMultiPackageFullBytesWrapper();
    public Int32BitsArray2DMultiPackagePreBytesWrapperWithDate m_received_PreByte = new Int32BitsArray2DMultiPackagePreBytesWrapperWithDate();
    public Int32BitsArray2DWrapper m_receivedAsInt32bits = new Int32BitsArray2DWrapper();
    public TextureSourceToInt32BitsArray2DWrapper m_resultAsTexture = new TextureSourceToInt32BitsArray2DWrapper();

    public bool m_isConverting;
    public double m_lastTimeTakenMS;
    public long m_lastTimeTakenTick;
    public Action m_notificationFinishConverting;

    [ContextMenu("Attempt")]
    public void Convertion(in byte[] source, out bool succed,
        out TextureSourceToInt32BitsArray2DWrapper representationAffected)
    {
        Stopwatch w = new Stopwatch();
        w.Start();
        m_isConverting = true;
           succed = false;
        representationAffected = null;
        try
        {
            i3 = i_fullToPreByte;
            i4 = i_preByteToInt32Bits;
            i5 = i_int32BitsToTexture;
            m_received_Full.m_data.m_compressedInOneBlockOfBytesToStore = source;
            i3.Convert(in this.m_received_Full, ref m_received_PreByte);
            i4.ConvertIn(this.m_received_PreByte, ref this.m_receivedAsInt32bits);
            i5.Convert(in this.m_receivedAsInt32bits, ref this.m_resultAsTexture);
            representationAffected = this.m_resultAsTexture;
            succed = true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning("Exception:" + e.StackTrace);
            succed = false;
            m_notificationFinishConverting.Invoke();
        }
        m_isConverting = false;
        w.Stop();
        m_lastTimeTakenMS = w.ElapsedMilliseconds;
        m_lastTimeTakenTick = w.ElapsedTicks;
        m_notificationFinishConverting.Invoke();
    }
}