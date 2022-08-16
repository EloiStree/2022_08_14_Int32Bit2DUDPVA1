using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBytesReceivedMono : MonoBehaviour
{

    public int m_count;
    public bool[] m_firstByte= new bool[8];
    public byte[] m_received;


    public void PushBytes(byte[] value) {
        m_received = value;
        m_count = value.Length;
        if (value.Length <= 0) { 
            m_firstByte = new bool[8];
        }
        else { 
            if (m_firstByte.Length!=8)
                m_firstByte = new bool[8];
            Eloi.E_PrimitiveBoolUtility.ByteTo8Bits(
                in value[0],
                 out m_firstByte[0],
                  out m_firstByte[1],
                   out m_firstByte[2],
                    out m_firstByte[3],
                     out m_firstByte[4],
                      out m_firstByte[5],
                       out m_firstByte[6],
                       out m_firstByte[7]);
        }
    }
}
