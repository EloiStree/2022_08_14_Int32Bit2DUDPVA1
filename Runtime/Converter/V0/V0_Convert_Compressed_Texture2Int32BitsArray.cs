using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Compressed_Texture2Int32BitsArray : AConvert_Compressed_Texture2Int32BitsArrayMono
    {
        public BAWTextureToInt32bitsShaderMono m_int32bitsToArray;
        public override void Convert(in TextureSourceToInt32BitsArray2DWrapper source, ref Int32BitsArray2DWrapper result)
        {
            if (source == null) throw new System.Exception("Can't be null");
            if (result == null) throw new System.Exception("Can't be null");
            m_int32bitsToArray.Convert(in source.m_data.m_textureReference, out result.m_data.m_arrayOfBitUnderInt);
            result.m_data.m_contextId = source.m_data.m_contextId;
            GetWithAndHeightof(in source.m_data.m_textureReference, out result.m_data.m_width, out result.m_data.m_height);

        }
        private void GetWithAndHeightof(in Texture textureReference, out ushort width, out ushort height)
        {
            if (textureReference is Texture2D)
            {
                Texture2D t = (Texture2D)textureReference;
                width =(ushort) t.width;
                height = (ushort)t.height;
            }
            else if (textureReference is RenderTexture)
            {
                RenderTexture t = (RenderTexture)textureReference;
                width = (ushort)t.width;
                height = (ushort)t.height;
            }
            else {
                width = 0;
                height = 0;
            }
        }
    }
}
