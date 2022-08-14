using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class BAWInt32bitsToTextureMono : MonoBehaviour
{
    public int[] m_colorIntBool;
    public int m_width;
    public int m_height;
    public RenderTexture m_renderTextureCreated;
    public bool m_useUpdate;
    public ComputeShader m_convertShader;
    public ComputeBuffer m_recovertIntBool;
    public int m_lenght;
    public double m_imagePerSecondsEstimation = 30;
    public int m_numberOfIntBAW;
    public int m_numberOfBytesBAW;
    public int m_numberOfUdpPackageBAW;
    public double m_numberOfUdpPackageBAWPerSeconds;
    public double m_numberOfUdpMBPerSeconds;
    public int[] m_debugInt = new int[50];
    public long m_convertedTimeInMilliseconds;

    public Eloi.ClassicUnityEvent_Texture m_onTextureChanged;


    void Update()
    {
        if (m_useUpdate) {
            if (m_colorIntBool.Length == 0)
                m_colorIntBool = new int[(m_width * m_height)/32];
            Push(m_colorIntBool);
        }
    }

    public void SetSize(int width, int height)
    {
        m_height = width;
        m_width = height;
        if (m_renderTextureCreated == null || 
            (m_renderTextureCreated != null && (m_renderTextureCreated.width != width
        || m_renderTextureCreated.height != height))) { 
        
        RenderTexture rt = new RenderTexture(m_width, m_height, 0);
        rt.enableRandomWrite = true;
        Graphics.SetRandomWriteTarget(0, rt);
            m_renderTextureCreated = rt;
        } 

    }
    public void SetSizeFrom(Texture2D texture)
    {
        SetSize(texture.width, texture.height);
    }
    public void SetTextureToUse(RenderTexture texture)
    {
        SetSize(texture.width, texture.height);
    }
    public void SetTextureToUse(Texture texture)
    {

        if (m_renderTextureCreated == null) {
            RenderTexture rt =(RenderTexture)texture;
            SetSize(rt.width, rt.height);
        }

        if (m_renderTextureCreated is RenderTexture)
        {
            RenderTexture rt = (RenderTexture)m_renderTextureCreated;
            SetSize(rt.width, rt.height);
        }
    }
  

    public  void Push(int[] int32bitsArray)
    {
        m_colorIntBool = int32bitsArray;
        if (m_colorIntBool.Length <= 0)
            return;
        SetSize(m_width, m_height);
        Stopwatch watch = new Stopwatch();
        watch.Start();

        m_lenght = m_width * m_height;
        m_numberOfIntBAW = (int)(m_lenght / 32f);
        m_numberOfBytesBAW = (int)(m_lenght / 4f);
        m_numberOfUdpPackageBAW = m_numberOfBytesBAW / 65000;
        m_numberOfUdpPackageBAWPerSeconds = (m_numberOfBytesBAW * m_imagePerSecondsEstimation / 65000.0);
        m_numberOfUdpMBPerSeconds = (m_numberOfBytesBAW * m_imagePerSecondsEstimation * 0.000001);

        if (m_recovertIntBool == null || int32bitsArray.Length != m_recovertIntBool.count)
        {
            //TODO: Should Change if not same lenght
            m_recovertIntBool = new ComputeBuffer(m_lenght / 32, sizeof(int));
        }
        m_recovertIntBool.SetData(m_colorIntBool);
        int kernel = m_convertShader.FindKernel("CSMain");
        m_convertShader.SetTexture(kernel, "Given", m_renderTextureCreated);
        m_convertShader.SetBuffer(kernel, "TextureAsIntBool", m_recovertIntBool);
        m_convertShader.SetInt("Width", m_width);
        m_convertShader.Dispatch(kernel, m_width / 16, m_height / 16, 1);
        watch.Stop();
        m_convertedTimeInMilliseconds = watch.ElapsedMilliseconds;
        m_onTextureChanged.Invoke(m_renderTextureCreated);
    }
}