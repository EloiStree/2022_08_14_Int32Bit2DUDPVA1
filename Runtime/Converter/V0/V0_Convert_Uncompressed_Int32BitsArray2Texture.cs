using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Uncompressed_Int32BitsArray2Texture : AConvert_Uncompressed_Int32BitsArray2TextureMono
    {
        public BAWInt32bitsToTextureMono m_intToTexture;
        public List<TextureByContextID> m_managedTexture= new List<TextureByContextID>();
        public override void Convert(in Int32BitsArray2DWrapper source,
            ref TextureSourceToInt32BitsArray2DWrapper result)
        {
            TextureByContextID t = null;
            for (int i = 0; i < m_managedTexture.Count; i++)
            {
                if (m_managedTexture[i].m_contextId == source.m_data.m_contextId.m_contextId) {
                    t = m_managedTexture[i];
                    continue;
                }
            }
            if (t == null)
                t = new TextureByContextID();
            if (t.m_texture == null
                || source.m_data.m_width != t.m_texture.width
                || source.m_data.m_height != t.m_texture.height) {

                RenderTexture rt = new RenderTexture(source.m_data.m_width, source.m_data.m_height, 0);
                rt.enableRandomWrite = true;
                Graphics.SetRandomWriteTarget(0, rt);
                t.m_texture = rt;
                //  t.m_texture = new Texture2D(source.m_data.m_width, source.m_data.m_height);
                //Color32[] c=  t.m_texture.GetPixels32();
                //  for (int i = 0; i < c.Length; i++)
                //  {
                //      c[i].g = 255;
                //      c[i].r = 0;
                //      c[i].b = 0;
                //      c[i].a = 255; 
                //  }
                //  t.m_texture.SetPixels32(c);
                //  t.m_texture.Apply();
                m_managedTexture.Add(t);
            }
            t.m_contextId = source.m_data.m_contextId.m_contextId;
            Texture td = t.m_texture;
            m_intToTexture.Push( in source.m_data.m_arrayOfBitUnderInt
                , source.m_data.m_width
                , source.m_data.m_height
                , ref td);
            if (result == null)
                result = new TextureSourceToInt32BitsArray2DWrapper();
            result.m_data.m_textureReference = t.m_texture; 
            result.m_data.m_contextId.m_contextId = t.m_contextId;
        }
    }


    [System.Serializable]
    public class TextureByContextID {
        public uint m_contextId;
        public RenderTexture m_texture;
    }
}
