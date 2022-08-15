using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDD_Int32BitsFirstAttemptMulti : MonoBehaviour
{

    //public TextureSourceToInt32BitsArray2D a0;
    //public Int32BitsArray2D a1;
    //public Int32BitsArray2DMultiPackagePreBytes a2;
    //public Int32BitsArray2DMultiPackageFullBytes a3;
    //public BytesToUncompress m_storedBytes;
    //public Int32BitsArray2DMultiPackageFullBytes a5;
    //public Int32BitsArray2DMultiPackagePreBytes a6;
    //public Int32BitsArray2D a7;
    //public TextureSourceToInt32BitsArray2D a9;

    public int m_maxPackageByteSize=64;

    public TextureSourceToInt32BitsArray2DWrapper aw0;
    public Int32BitsArray2DWrapper aw1;
    public List<Int32BitsArray2DMultiPackagePreBytesWrapper> aw2;
    public Int32BitsArray2DMultiPackageFullBytesWrapper [] aw3;
    public BytesToUncompressWrapper []  maw_storedBytes;
    public Int32BitsArray2DMultiPackageFullBytesWrapper [] aw5;
    public Int32BitsArray2DMultiPackagePreBytesWrapperWithDate [] aw6;
    public Int32BitsArray2DWrapper aw7;
    public TextureSourceToInt32BitsArray2DWrapper aw9;



    public AConvert_Compressed_Texture2Int32BitsArrayMono ia0;
    public AConvert_Compressed_Int32BitsArray2PreBytesMono ia1;
    public AConvert_Compressed_PreBytes2FullBytesStorageMono ia2;
    public AConvert_Uncompressed_FullBytesStorage2PreBytesMono ia3;
    public AConvert_Uncompressed_PreBytes2Int32BitsArrayMono ia4;
    public AConvert_Uncompressed_Int32BitsArray2TextureMono ia5;


    public IConvert_Compressed_Texture2Int32BitsArray i0;
    public IConvert_Compressed_Int32BitsArray2PreBytes i1;
    public IConvert_Compressed_PreBytes2FullBytesStorage i2;
    public IConvert_Uncompressed_FullBytesStorage2PreBytes i3;
    public IConvert_Uncompressed_PreBytes2Int32BitsArray i4;
    public IConvert_Uncompressed_Int32BitsArray2Texture i5;

    public int m_width;
    public int m_height;
    public Camera m_cameraTest;
    public void Awake()
    {
        if (m_cameraTest) { 
            RenderTexture rt = new RenderTexture(m_width, m_height,0);
            rt.enableRandomWrite = true;
            Graphics.SetRandomWriteTarget(0, rt);
            m_cameraTest.targetTexture = rt;
            SetTextureToUse(rt);
        }
    }
    public void SetTextureToUse(Texture texture)
    {
        aw0.m_data.m_textureReference = texture;

    }


    [ContextMenu("Se tMax Size As Udp Mod 32 Less One")]
    public void SetMaxSizeAsUdpMod32LessOne() {
        m_maxPackageByteSize = 65407;
    }

    [ContextMenu("Attempt")]
    public void Attempt() {

        i0 = ia0;
        i1 = ia1;
        i2 = ia2;
        i3 = ia3;
        i4 = ia4;
        i5 = ia5;

        if (i0 == null) return;
        i0.Convert(in this.aw0, ref this.aw1);

        if (i1 == null) return;
        i1.Convert(in this.aw1, ref this.aw2, in m_maxPackageByteSize);
        aw3 = new Int32BitsArray2DMultiPackageFullBytesWrapper[aw2.Count];
        maw_storedBytes = new BytesToUncompressWrapper[aw2.Count];
        aw5 = new Int32BitsArray2DMultiPackageFullBytesWrapper[aw2.Count];
        aw6 = new Int32BitsArray2DMultiPackagePreBytesWrapperWithDate[aw2.Count];
        if (i2 == null) return;
        for (int i = 0; i < aw2.Count; i++)
        {
           i2.Convert( aw2[i], ref this.aw3[i]);
           maw_storedBytes[i] = new BytesToUncompressWrapper();
           maw_storedBytes[i].m_data.m_rawByteThatCanBeUncompress =
                this.aw3[i].m_data.m_compressedInOneBlockOfBytesToStore;
        }
        if (i3 == null) return;
        for (int i = 0; i < maw_storedBytes.Length; i++)
        {
            aw5[i] = new Int32BitsArray2DMultiPackageFullBytesWrapper();
            aw5[i].m_data.m_compressedInOneBlockOfBytesToStore =
                 this.maw_storedBytes[i].m_data.m_rawByteThatCanBeUncompress;

            aw6[i] = new Int32BitsArray2DMultiPackagePreBytesWrapperWithDate();
            i3.Convert(in this.aw5[i], ref aw6[i]);
        }
        if (i4 == null) return;
        i4.ConvertIn( this.aw6, ref this.aw7);
        if (i5 == null) return;
        i5.Convert(in this.aw7, ref this.aw9);
    }
}