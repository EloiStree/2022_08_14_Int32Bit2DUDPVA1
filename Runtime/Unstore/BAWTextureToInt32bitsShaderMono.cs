using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class BAWTextureToInt32bitsShaderMono : MonoBehaviour
{
    public int m_width;
    public int m_height;
    public float m_whitePercent = 0.8f;
    public bool m_useUpdate;
    public Texture m_renderTextureIn;
    public ComputeShader m_convertShaderTextToWhite;
    public ComputeShader m_convertShaderWhiteToInt32_2;
    public ComputeShader m_convertShaderWhiteToInt32_16;
    public ComputeShader m_convertShaderWhiteToInt32_64;
    public ComputeBuffer m_recoverColor;
    public ComputeBuffer m_recoverIntBool;
    public int m_lenght;
    public double m_imagePerSecondsEstimation = 30;

    public int m_numberOfIntBAW;
    public int m_numberOfBytesBAW;
    public int m_numberOfUdpPackageBAW;
    public double m_numberOfUdpPackageBAWPerSeconds;
    public double m_numberOfUdpMBPerSeconds;
     int[] m_color;
     int[] m_colorIntBool;
    public long m_gpuShader;
    public long m_cpuCompute;
    public long m_totalCompute;
    public Int32bitsEvent m_bawInt32BitsChanged;
     bool[] m_bitsWhite;

    [System.Serializable]
    public class Int32bitsEvent : UnityEvent<int[]> { }


    void Update()
    {
        if (m_useUpdate)
            Push( m_renderTextureIn );
    }
    private int m_previousLenght;

    public void Convert(in Texture textureUsed, out int[] arrayOfBitUnderInt)
    {
        Stopwatch watchTotal = new Stopwatch();
        watchTotal.Start();
        m_renderTextureIn = textureUsed;
        arrayOfBitUnderInt = new int[0];
        if (m_renderTextureIn == null)
            return;

        if (m_renderTextureIn is RenderTexture)
        {
            RenderTexture rt = (RenderTexture)m_renderTextureIn;
            m_height = rt.height;
            m_width = rt.width;
            rt.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, rt);
        }
        else if (m_renderTextureIn is Texture2D)
        {
            Texture2D rt = (Texture2D)m_renderTextureIn;
            m_height = rt.height;
            m_width = rt.width;
        }

        Stopwatch watchGpu = new Stopwatch();
        watchGpu.Start();

        m_lenght = m_width * m_height;
        if (m_previousLenght != m_lenght)
        {
            m_previousLenght = m_lenght;
            m_numberOfIntBAW = (int)(m_lenght / 32f);
            m_numberOfBytesBAW = (int)(m_lenght / 4f);
            m_numberOfUdpPackageBAW = m_numberOfBytesBAW / 65000;
            m_colorIntBool = new int[m_lenght / 32];
            m_color = new int[m_lenght];
            m_numberOfUdpPackageBAWPerSeconds = (m_numberOfBytesBAW * m_imagePerSecondsEstimation / 65000.0);
            m_numberOfUdpMBPerSeconds = (m_numberOfBytesBAW * m_imagePerSecondsEstimation * 0.000001);
            m_bitsWhite = new bool[m_lenght * 32];
            if (m_recoverIntBool != null)
                m_recoverIntBool.Dispose();
            m_recoverIntBool = new ComputeBuffer(m_lenght / 32, sizeof(int));
            m_recoverIntBool.SetData(m_colorIntBool);

            if (m_recoverColor != null)
                m_recoverColor.Dispose();
            m_recoverColor = new ComputeBuffer(m_lenght, sizeof(int));
            m_recoverColor.SetData(m_color);
        }

        int kernelWhite = m_convertShaderTextToWhite.FindKernel("CSMain");
        m_convertShaderTextToWhite.SetInt("Width", m_width);
        m_convertShaderTextToWhite.SetFloat("WhitePercent", m_whitePercent);
        m_convertShaderTextToWhite.SetTexture(kernelWhite, "Given", m_renderTextureIn);
        m_convertShaderTextToWhite.SetBuffer(kernelWhite, "IsWhiteInt", m_recoverColor);
        m_convertShaderTextToWhite.Dispatch(kernelWhite, m_width / 32, m_height / 32, 1);
        //m_recoverColor.GetData(m_color);

        watchGpu.Stop();
        m_gpuShader = watchGpu.ElapsedMilliseconds;

        Stopwatch watchCpu = new Stopwatch();
        watchCpu.Start();


        ComputeShader c = m_convertShaderWhiteToInt32_2;
        int lGroup =2;
        if (m_recoverIntBool.count >= 16) { 
            c = m_convertShaderWhiteToInt32_16; lGroup = 16;
        }
        if (m_recoverIntBool.count >= 64) { 
            c = m_convertShaderWhiteToInt32_64; lGroup = 64;
        }
        int kernelBool = c.FindKernel("CSMain");
        c.SetBuffer(kernelBool, "IsWhiteInt", m_recoverColor);
        c.SetBuffer(kernelBool, "TextureAsIntBool", m_recoverIntBool);
        c.Dispatch(kernelBool, (m_colorIntBool.Length / lGroup), 1, 1);
        m_recoverIntBool.GetData(m_colorIntBool);


        //if (false) { 
        //    for (int i = 0; i < m_lenght; i++)
        //    {
        //        m_test.SetBit( in i, m_color[i] > 0);
        //    }
        //}
        //if (true) {
        //    //5MS
        //    for (int i = 0; i < m_lenght; i++)
        //    {
        //        m_bitsWhite[i] = m_color[i] > 0; ;
        //    }
        //    BitArray array = new BitArray(32);
        //    int[] iArray = new int[1];
        //    array.CopyTo(iArray, 0);
        //    for (int i = 0; i < m_lenght/32; i++)
        //    {
        //        for (int j = 0; j < 32; j++)
        //        {
        //            array.Set(j, m_bitsWhite[i+j]);
        //        }
        //        array.CopyTo(iArray, 0);
        //        m_test.m_storageInt[i] = iArray[0];
        //    }
        //}
        watchCpu.Stop();
        m_cpuCompute = watchCpu.ElapsedMilliseconds;

        watchTotal.Stop();
        m_totalCompute = watchTotal.ElapsedMilliseconds;
        arrayOfBitUnderInt = m_colorIntBool;
        m_bawInt32BitsChanged.Invoke(m_colorIntBool);
    }

    public void Push(Texture textureUsed)
    {
        Convert(in textureUsed, out m_colorIntBool);
    }
}


public class FastBoolToInt {


    public static void BoolsToInt(ref int target, params bool[] boolArr) {
        target = 0;
        foreach (var bit in boolArr)
        {
            target <<= 1; target |= bit ? 1 : 0;
        }
    }

    public static void BoolsToInt(
        ref int target,
        in bool b0,
        in bool b1,
        in bool b2,
        in bool b3,
        in bool b4,
        in bool b5,
        in bool b6,
        in bool b7,
        in bool b8,
        in bool b9,
        in bool b10,
        in bool b11,
        in bool b12,
        in bool b13,
        in bool b14,
        in bool b15,
        in bool b16,
        in bool b17,
        in bool b18,
        in bool b19,
        in bool b20,
        in bool b21,
        in bool b22,
        in bool b23,
        in bool b24,
        in bool b25,
        in bool b26,
        in bool b27,
        in bool b28,
        in bool b29,
        in bool b30,
        in bool b31,
        in bool b32)
    {
        target = 0;
        target <<= 1; target |= b0  ? 1 : 0;
        target <<= 1; target |= b1  ? 1 : 0;
        target <<= 1; target |= b2  ? 1 : 0;
        target <<= 1; target |= b3  ? 1 : 0;
        target <<= 1; target |= b4  ? 1 : 0;
        target <<= 1; target |= b5  ? 1 : 0;
        target <<= 1; target |= b6  ? 1 : 0;
        target <<= 1; target |= b7  ? 1 : 0;
        target <<= 1; target |= b8  ? 1 : 0;
        target <<= 1; target |= b9  ? 1 : 0;
        target <<= 1; target |= b10 ? 1 : 0;
        target <<= 1; target |= b11 ? 1 : 0;
        target <<= 1; target |= b12 ? 1 : 0;
        target <<= 1; target |= b13 ? 1 : 0;
        target <<= 1; target |= b14 ? 1 : 0;
        target <<= 1; target |= b15 ? 1 : 0;
        target <<= 1; target |= b16 ? 1 : 0;
        target <<= 1; target |= b17 ? 1 : 0;
        target <<= 1; target |= b18 ? 1 : 0;
        target <<= 1; target |= b19 ? 1 : 0;
        target <<= 1; target |= b20 ? 1 : 0;
        target <<= 1; target |= b21 ? 1 : 0;
        target <<= 1; target |= b22 ? 1 : 0;
        target <<= 1; target |= b23 ? 1 : 0;
        target <<= 1; target |= b24 ? 1 : 0;
        target <<= 1; target |= b25 ? 1 : 0;
        target <<= 1; target |= b26 ? 1 : 0;
        target <<= 1; target |= b27 ? 1 : 0;
        target <<= 1; target |= b28 ? 1 : 0;
        target <<= 1; target |= b29 ? 1 : 0;
        target <<= 1; target |= b30 ? 1 : 0;
        target <<= 1; target |= b31 ? 1 : 0;
        target <<= 1; target |= b32 ? 1 : 0;
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
