using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Exp_ImageToBytes : MonoBehaviour
{
    public int m_width = 256;
    public int m_height = 256;
    public int m_maxPackageSize = 65507;
    public int m_valueCount;
    public int m_packagePossible;

    public Color m_color;
    public int m_colorByteSize;
    public ColorByte m_colorAsByte;
    public int m_colorAsByteSize;

    public SItuPollResponse bytesId;
    public int bytesIdCount;
    public SItuPollResponseColor colorGroup;
    public int colorGroupIdCount;

    private void OnValidate()
    {
        m_valueCount = m_width * m_height;
        m_packagePossible = 1 + (m_valueCount / m_maxPackageSize);
        m_colorByteSize = Marshal.SizeOf(typeof(Color));
        m_colorAsByteSize = Marshal.SizeOf(typeof(ColorByte));

        bytesIdCount = bytesId.ToBytes().Length;
        colorGroupIdCount = colorGroup.ToBytes().Length;

    }


}
[System.Serializable]
public struct ColorByte {
    public byte r;
    public byte g;
    public byte b;
    public byte a;

}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[System.Serializable]
public struct SItuPollResponse
{
    public byte m_textureId;
    public int m_index;
    public ushort m_count;
    public byte[] m_bytes;

    public byte[] ToBytes()
    {
        Byte[] bytes = new Byte[Marshal.SizeOf(typeof(SItuPollResponse))];
        GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
        try
        {
            Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
            return bytes;
        }
        finally
        {
            pinStructure.Free();
        }
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[System.Serializable]
public struct SItuPollResponseColor
{
    public Color[] m_colors;

    public byte[] ToBytes()
    {
        Byte[] bytes = new Byte[Marshal.SizeOf(typeof(SItuPollResponseColor))];
        GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
        try
        {
            Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
            return bytes;
        }
        finally
        {
            pinStructure.Free();
        }
    }
}