using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_ConvertTexture2DToBytes : MonoBehaviour
{
    public Texture2D m_texture;


     Color[] m_pixels;
    public float m_pixelsSize;
    public float m_pixelsCount;
     float[] m_colorAsFloat;
    public float m_colorAsFloatSize;
    public float m_colorAsFloatCount;
     int[] m_colorAsInt;
    public int m_colorAsIntSize;
    public int m_colorAsIntCount;
     byte[] m_colorAsBytes;
    public int m_colorAsBytesSize;
    public int m_colorAsBytesCount;
     byte[] m_grayScale255;
    public int m_grayScale255Count;
     byte[] m_isWhite;
    public int m_isWhiteCount;
    byte[] m_isWhiteOn8BitsByte;
    public int m_isWhiteOn8BitsByteCount;
    int[] m_isWhiteOn32BitsInt;
    public int m_isWhiteOn16BitsIntCount;

    public Texture2D m_texture2DGray;
    public Texture2D m_texture2DBits;
    public Texture2D m_texture2DBits32;
    [ContextMenu("Compute")]
    public void Compute() {

        m_pixels= m_texture.GetPixels();
        m_pixelsCount = m_pixels.Length;
        m_pixelsSize = m_pixelsCount * 4 * sizeof(float);
        m_colorAsFloat = new float[m_pixels.Length*4];
        m_colorAsInt = new int[m_pixels.Length * 4];
        m_colorAsBytes = new byte[m_pixels.Length * 4];
        m_grayScale255 = new byte[m_pixels.Length];
        m_isWhite = new byte[m_pixels.Length];
        m_isWhiteOn8BitsByte = new byte[1 + (m_pixels.Length / 8)];
        m_isWhiteOn32BitsInt = new int[1 + (m_pixels.Length / 32)];

        m_colorAsFloatSize = m_pixels.Length * 4 * sizeof(float);
        m_colorAsIntSize = m_pixels.Length * 4 * sizeof(int);
        m_colorAsBytesSize = m_pixels.Length * 4 * sizeof(byte);

        m_colorAsFloatCount = m_pixels.Length;
        m_colorAsIntCount = m_pixels.Length;
        m_colorAsBytesCount = m_pixels.Length;
        m_grayScale255Count = m_pixels.Length;
        m_isWhiteCount = m_pixels.Length;
        m_isWhiteOn8BitsByteCount =  m_pixels.Length / 8;
        m_isWhiteOn16BitsIntCount =  m_pixels.Length / 32;


        for (int i = 0; i < m_pixels.Length; i++)
        {
            m_colorAsFloat[i] = m_pixels[i].r;
            m_colorAsFloat[i + 1] = m_pixels[i].g;
            m_colorAsFloat[i + 2] = m_pixels[i].b;
            m_colorAsFloat[i + 3] = m_pixels[i].a;

            m_colorAsInt[i] = (int)(m_colorAsFloat[i] * 255f);
            m_colorAsInt[i + 1] = (int)(m_colorAsFloat[i + 1] * 255f);
            m_colorAsInt[i + 2] = (int)(m_colorAsFloat[i + 2] * 255f);
            m_colorAsInt[i + 3] = (int)(m_colorAsFloat[i + 3] * 255f);

            m_colorAsBytes[i] = (byte)(m_colorAsFloat[i] * 255f);
            m_colorAsBytes[i + 1] = (byte)(m_colorAsFloat[i + 1] * 255f);
            m_colorAsBytes[i + 2] = (byte)(m_colorAsFloat[i + 2] * 255f);
            m_colorAsBytes[i + 3] = (byte)(m_colorAsFloat[i + 3] * 255f);
            m_isWhite[i] = IsWhite(i);
            m_grayScale255[i] = GetColorGray(i);
        }

        BitArray bitArray = new BitArray(new bool[8]);
        for (int i = 0; i < m_isWhite.Length; i++)
        {
            int byteIndex = (int)(i / 8f);
            int bitIndex = i % 8;
            bitArray.Set(bitIndex, m_isWhite[i] == 1);

            if (bitIndex == 7)
                m_isWhiteOn8BitsByte[byteIndex] = ConvertToByte(bitArray);
        }

        //BYTE ARRAY
        Color[] pixelWhite = new Color[m_isWhiteOn8BitsByte.Length * 8];
        for (int i = 0; i < m_isWhiteOn8BitsByte.Length; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                pixelWhite[(i * 8) + j] = GetByteValueAsColor(in m_isWhiteOn8BitsByte[i], in j);
            }
        }
        m_texture2DBits = new Texture2D(m_texture.width, m_texture.height);
        m_texture2DBits.SetPixels(pixelWhite);
        m_texture2DBits.Apply();


        BitArray bitIntArray = new BitArray(new bool[32]);
        for (int i = 0; i < m_isWhite.Length; i++)
        {
            int byteIndex = (int)(i / 32f);
            int bitIndex = i % 32;
            bitIntArray.Set(bitIndex, m_isWhite[i] == 1);

            if (bitIndex == 31)
                m_isWhiteOn32BitsInt[byteIndex] = ConvertToInt(bitIntArray);
        }

        // INT ARRAY
        Color[] pixelWhite32 = new Color[m_isWhiteOn32BitsInt.Length * 32];
        for (int i = 0; i < m_isWhiteOn32BitsInt.Length; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                pixelWhite32[(i * 32) + j] = GetIntValueAsColor(in m_isWhiteOn32BitsInt[i], in j);
            }
        }
        m_texture2DBits32 = new Texture2D(m_texture.width, m_texture.height);
        m_texture2DBits32.SetPixels(pixelWhite32);
        m_texture2DBits32.Apply();

      

        Color[] pixelGray = new Color[m_grayScale255.Length ];
        for (int i = 0; i < m_grayScale255.Length; i++)
        {
            float v = m_grayScale255[i]/255f;
            pixelGray[i] = new Color(v, v, v);
        }
        m_texture2DGray = new Texture2D(m_texture.width, m_texture.height);
        m_texture2DGray.SetPixels(pixelGray);
        m_texture2DGray.Apply();

    }

    public bool GetByteValue(in byte value, in int index)
    {
        //int mask = 1 << (index - 1); // if "bitToReturn" is 1-based
        int mask = 1 << index; // if "bitToReturn" is 0-based
        return (value & mask) != 0;
    }
    public Color GetByteValueAsColor(in byte value, in int index)
    {
        return GetByteValue(in value, in index) ? Color.white : Color.black;
    }
    public bool GetIntValue(in int value, in int index)
    {
        //int mask = 1 << (index - 1); // if "bitToReturn" is 1-based
        int mask = 1 << index; // if "bitToReturn" is 0-based
        return (value & mask) != 0;
    }
    public Color GetIntValueAsColor(in int value, in int index)
    {
        return GetIntValue(in value, in index) ? Color.white : Color.black;
    }

    private byte IsWhite(int i)
    {
        return (byte)((m_pixels[i].r > 0.95f && m_pixels[i].g > 0.95f && m_pixels[i].b > 0.95f) ? 1 : 0);
    }
    private byte GetColorGray(int i)
    {
        return (byte)((m_pixels[i].r *0.3f + m_pixels[i].g * 0.3f+ m_pixels[i].b * 0.3f)*255f);
    }

    

    public byte GetByte(bool[] bits) {
        BitArray a = new BitArray(bits);
        return ConvertToByte(in a);
    }
    public byte ConvertToByte(in BitArray bits)
    {
        if (bits.Count != 8)
        {
            throw new ArgumentException("bits");
        }
        byte[] bytes = new byte[1];
        bits.CopyTo(bytes, 0);
        return bytes[0];
    }
    public int ConvertToInt(in BitArray bits)
    {
        if (bits.Count != 32)
        {
            throw new ArgumentException("bits");
        }
        int[] bytes = new int[1];
        bits.CopyTo(bytes, 0);
        return bytes[0];
    }
}
