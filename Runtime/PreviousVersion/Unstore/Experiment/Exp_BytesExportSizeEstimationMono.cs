
    using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;





public class Exp_BytesExportSizeEstimationMono : MonoBehaviour
{

    [Header("Set")]
    public RenderTexture m_renderTextureIn;
    public int m_width;
    public int m_height;
    public double m_imagePerSecondsToSend = 30;

    [Header("Estimation: Texture Export as 4 bytes as one ByteInt")]
    public int m_arrayLenght;
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





    private void OnValidate()
    {
        Refresh();
    }

    private void Refresh()
    {

        if (m_renderTextureIn != null)

        {
            m_width = m_renderTextureIn.width;
            m_height = m_renderTextureIn.height;

        }


        if (m_colorInt == null || m_colorInt.Length == 0)
        {
            m_arrayLenght = m_width * m_height;
            m_numberOfInt255 = m_arrayLenght * 4;
            m_numberOfBytes255 = m_numberOfInt255 * 4;
            m_numberOfUdpPackage255 = m_numberOfBytes255 / 65000;
            m_numberOfIntBAW = (int)(m_arrayLenght / 32f);
            m_numberOfBytesBAW = m_numberOfIntBAW * 4;
            m_numberOfUdpPackageBAW = m_numberOfBytesBAW / 65000;
            m_numberOfIntGray = (int)(m_arrayLenght / 4);
            m_numberOfBytesGray = m_numberOfIntGray * 4;
            m_numberOfUdpPackageGray = m_numberOfBytesGray / 65000;

            m_colorInt = new int[m_numberOfInt255];
            m_colorIntGray = new int[m_arrayLenght];
            m_colorIntBool = new int[m_arrayLenght / 32];
            m_colorAsByte = new byte[m_numberOfBytes255];

            m_numberOfUdpPackage255PerSeconds = (m_numberOfBytes255 * m_imagePerSecondsToSend / 65000.0);
            m_numberOfUdpPackageBAWPerSeconds = (m_numberOfBytesBAW * m_imagePerSecondsToSend / 65000.0);
            m_numberOfUdpPackageGrayPerSeconds = (m_numberOfBytesGray * m_imagePerSecondsToSend / 65000.0);
            m_numberOfUdpPackage255PerSecondsMb = (m_numberOfBytes255 * m_imagePerSecondsToSend * 0.000001);
            m_numberOfUdpPackageBAWPerSecondsMb = (m_numberOfBytesBAW * m_imagePerSecondsToSend * 0.000001);
            m_numberOfUdpPackageGrayPerSecondsMb = (m_numberOfBytesGray * m_imagePerSecondsToSend * 0.000001);
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

