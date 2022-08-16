using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyScript_ReceivedTexture : MonoBehaviour
{
    public int m_soloWidth;
    public int m_soloHeight;
    public Texture2D m_soloTexture;

    public int m_multiWidth;
    public int m_multiHeight;
    public Texture2D m_multiTexture;

    public ArrayAs32BitsIntBlockSegment m_last;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        m_soloTexture = new Texture2D(m_soloWidth, m_soloHeight);
        m_multiTexture = new Texture2D(m_multiWidth, m_multiHeight);
    }

    public void SetWith(ArrayAs32BitsIntOneBlock received)
    {
        if (m_soloTexture==null )
            m_soloTexture = new Texture2D(m_soloWidth, m_soloHeight);

        ArrayAs32BitsIntWrapper  w = new ArrayAs32BitsIntWrapper ();
        w.SetArrayWithRef(ref received.m_booleanAsint32bits.m_storageInt);        ;
        w.GetTotalBitLenght(out int bits);
        Color [] c =  m_soloTexture.GetPixels();
        bool isTrue;
        for (int i = 0; i < bits; i++)
        {
            w.GetBit(in i, out isTrue);
            if(i<c.Length)
                c[i] = isTrue ? Color.white : Color.black;
        }
        m_soloTexture.SetPixels(c );
        m_soloTexture.Apply();
    }

    public void SetWith(ArrayAs32BitsIntBlockSegment received) {

        if (m_multiTexture == null) { 
            m_multiTexture = new Texture2D(m_multiWidth, m_multiHeight);
            Color[] cd = m_multiTexture.GetPixels();
            for (int i = 0; i < cd.Length; i++)
            {
            cd[i] = Color.green;
            }
            m_multiTexture.SetPixels(cd);
            m_multiTexture.Apply();
        }
        ArrayAs32BitsIntWrapper  w = new ArrayAs32BitsIntWrapper ();
        w.SetArrayWithRef(ref received.m_booleanAsint32bits.m_storageInt);
        w.GetTotalBitLenght(out int bits);
        Color[] c = m_multiTexture.GetPixels();
        bool isTrue;
//        Debug.Log("C" + received.m_blockStartIndex);
        
        int d = received.m_blockStartIndex / 65472;
        int start = received.m_blockStartIndex *8;
        for (int i = 0; i < bits; i++)
        {
            w.GetBit(in i, out isTrue);

            if (d == 0)
                c[start + i] = isTrue ? Color.blue : Color.black;
            else if (d == 1)
                c[start + i] = isTrue ? Color.red : Color.black;
            else if (d == 2)
                c[start + i] = isTrue ? Color.clear : Color.black;
            else 
                c[start + i] = isTrue ? Color.white : Color.black;
        }
        m_multiTexture.SetPixels(c);
        m_multiTexture.Apply();
        m_last = received;

    }
}
