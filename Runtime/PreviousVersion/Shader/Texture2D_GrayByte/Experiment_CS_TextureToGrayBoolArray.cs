using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Experiment_CS_TextureToGrayBoolArray : MonoBehaviour
{

    public int m_width;
    public int m_height;
    public float m_whitePercent = 0.8f;
    public bool m_useUpdate;
    public RenderTexture m_renderTextureIn;
    public RenderTexture m_renderTextureDebugReturnGray;
    public RenderTexture m_renderTextureDebugReturnBlackWhite;
    public ComputeShader m_convertShader;
    public ComputeBuffer m_recovertInt;
    public ComputeBuffer m_recovertIntGray;
    public ComputeBuffer m_recovertIntBool;
    public int m_lenght;
    public double m_imagePerSeconds = 30;
    public int m_numberOfInt255;
    public int m_numberOfBytes255;
    public int m_numberOfUdpPackage255;
    public double m_numberOfUdpPackage255PerSeconds;
    public double m_numberOfUdpPackage255PerSecondsMb;
    public int m_numberOfIntBAW;
    public int m_numberOfBytesBAW;
    public int m_numberOfUdpPackageBAW;
    public double m_numberOfUdpPackageBAWPerSeconds;
    public double m_numberOfUdpPackageBAWPerSecondsMb;
    public int m_numberOfIntGray;
    public int m_numberOfBytesGray;
    public int m_numberOfUdpPackageGray;
    public double m_numberOfUdpPackageGrayPerSeconds;
    public double m_numberOfUdpPackageGrayPerSecondsMb;
    int[] m_colorInt;
    int[] m_colorIntGray;
    int[] m_colorIntBool;
    byte[] m_colorAsByte;
    public int[] m_debugInt = new int[50];
    public int[] m_debugByte = new int[50];

    // int[] m_index1D;

    public long m_textreToFloatInMilliseconds;
    public long m_floatToByte;
    //public long m_byteToFloat;

    public EloiDependency.ClassicUnityEvent_RenderTexture m_renderTextureChanged;
    public IntEvent m_appliedFloat;
    public BytesEvent m_appliedBytes;
    [Header("If you need source to debug")]
    public Camera m_cameraInForDebug;
    public bool m_useWebcam;

    [System.Serializable]
    public class IntEvent : UnityEvent<int[]> { }
    [System.Serializable]
    public class BytesEvent : UnityEvent<byte[]> { }


    void Update()
    {
        if (m_useUpdate)
            Push();
        
    }

    public void SetRenderTextureWith(RenderTexture renderTexture) {
        m_renderTextureIn = renderTexture;
        m_renderTextureIn.enableRandomWrite = true;
        Graphics.SetRandomWriteTarget(0, m_renderTextureIn);
        m_renderTextureChanged.Invoke(m_renderTextureIn);

    }

    private void Push()
    {

        Stopwatch watch = new Stopwatch();
        watch.Start();
        if (m_renderTextureIn == null)
        {
            m_renderTextureIn = new RenderTexture(m_width, m_height, 0);
            m_renderTextureIn.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureIn);

            if (m_cameraInForDebug)
                m_cameraInForDebug.targetTexture = m_renderTextureIn;
            m_renderTextureChanged.Invoke(m_renderTextureIn);
        }
        else {
            m_width = m_renderTextureIn.width;
            m_height=m_renderTextureIn.height;
            m_renderTextureIn.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureIn);

            if (m_cameraInForDebug)
                m_cameraInForDebug.targetTexture = m_renderTextureIn;
            m_renderTextureChanged.Invoke(m_renderTextureIn);
        }
        if (m_renderTextureDebugReturnGray == null)
        {
            m_renderTextureDebugReturnGray = new RenderTexture(m_width, m_height, 0);
            m_renderTextureDebugReturnGray.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureDebugReturnGray);
        }
        if (m_renderTextureDebugReturnBlackWhite == null)
        {
            m_renderTextureDebugReturnBlackWhite = new RenderTexture(m_width, m_height, 0);
            m_renderTextureDebugReturnBlackWhite.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, m_renderTextureDebugReturnBlackWhite);
        }


        if (m_colorInt == null || m_colorInt.Length == 0)
        { 
            m_lenght = m_width * m_height;
            m_numberOfInt255 = m_lenght * 4;
            m_numberOfBytes255 = m_numberOfInt255 * 4;
            m_numberOfUdpPackage255 = m_numberOfBytes255 / 65000;
            m_numberOfIntBAW = (int)(m_lenght / 32f);
            m_numberOfBytesBAW = m_numberOfIntBAW * 4;
            m_numberOfUdpPackageBAW = m_numberOfBytesBAW / 65000;
            m_numberOfIntGray           = (int)(m_lenght / 4);
            m_numberOfBytesGray         = m_numberOfIntGray * 4;
            m_numberOfUdpPackageGray = m_numberOfBytesGray / 65000;

            m_colorInt = new int[m_numberOfInt255];
            m_colorIntGray = new int[m_lenght];
            m_colorIntBool = new int[m_lenght / 32];
            m_colorAsByte = new byte[m_numberOfBytes255];

            m_numberOfUdpPackage255PerSeconds = (m_numberOfBytes255 * m_imagePerSeconds / 65000.0);
            m_numberOfUdpPackageBAWPerSeconds =(m_numberOfBytesBAW * m_imagePerSeconds / 65000.0);
            m_numberOfUdpPackageGrayPerSeconds = (m_numberOfBytesGray * m_imagePerSeconds / 65000.0);
            m_numberOfUdpPackage255PerSecondsMb = (m_numberOfBytes255 * m_imagePerSeconds * 0.000001 );
            m_numberOfUdpPackageBAWPerSecondsMb = (m_numberOfBytesBAW * m_imagePerSeconds * 0.000001);
            m_numberOfUdpPackageGrayPerSecondsMb = (m_numberOfBytesGray * m_imagePerSeconds * 0.000001);


        }

        if (m_recovertInt == null)
        {
            m_recovertInt = new ComputeBuffer(m_lenght * 4, sizeof(int));
            m_recovertInt.SetData(m_colorInt);
        }
        if (m_recovertIntGray == null)
        {
            m_recovertIntGray = new ComputeBuffer(m_lenght, sizeof(int));
            m_recovertIntGray.SetData(m_colorIntGray);
        }
        if (m_recovertIntBool == null)
        {
            m_recovertIntBool = new ComputeBuffer(m_lenght/32, sizeof(int));
            m_recovertIntBool.SetData(m_colorIntBool);
        }
        int kernel = m_convertShader.FindKernel("CSMain");
        m_convertShader.SetTexture(kernel, "Given", m_renderTextureIn);
        m_convertShader.SetTexture(kernel, "ResultGray", m_renderTextureDebugReturnGray);
        m_convertShader.SetTexture(kernel, "ResultBlackWhite", m_renderTextureDebugReturnBlackWhite);
        m_convertShader.SetBuffer(kernel, "TextureAs255", m_recovertInt);
        m_convertShader.SetBuffer(kernel, "TextureAs255Gray", m_recovertIntGray);
        m_convertShader.SetBuffer(kernel, "TextureAsIntBool", m_recovertIntBool);
        m_convertShader.SetInt("Width", m_width);
        m_convertShader.SetFloat("WhitePercent", m_whitePercent);
        m_convertShader.Dispatch(kernel,m_width/16, m_height/16, 1);
        m_recovertInt.GetData(m_colorInt);
        m_recovertIntGray.GetData(m_colorIntGray);
        m_recovertIntBool.GetData(m_colorIntBool);
        watch.Stop();
        m_textreToFloatInMilliseconds = watch.ElapsedMilliseconds;

        m_appliedFloat.Invoke(m_colorInt);

        ConvertPrimitiveArray2Byte.ConvertIntsToBytes(in m_colorInt, ref m_colorAsByte, out m_floatToByte);
       
        m_appliedBytes.Invoke(m_colorAsByte);

       // ConvertFloat2Byte.ConvertBytesToFloat(in m_colorAsByte, ref m_colorFloat, out m_byteToFloat);
      

        for (int i = 0; i < m_debugInt.Length; i++)
        {
            m_debugInt[i] = m_colorInt[i];
        }
        for (int i = 0; i < m_debugByte.Length; i++)
        {
            m_debugByte[i] = m_colorAsByte[i];
        }

    }

}


/*
 //https://stackoverflow.com/questions/40759199/sending-a-byte-to-the-gpu
 //encoding on the cpu

int myInt = 0;
myInt += (int)myByte1;
myInt += (int)(myByte2 << 8);
myInt += (int)(myByte3 << 16);
myInt += (int)(myByte4 << 24);

//decoding on the gpu

myByte1 = myInt & 0xFF;
myByte2 = (myInt >> 8) & 0xFF;
myByte3 = (myInt >> 16) & 0xFF;
myByte4 = (myInt >> 24) & 0xFF;
 
 */
