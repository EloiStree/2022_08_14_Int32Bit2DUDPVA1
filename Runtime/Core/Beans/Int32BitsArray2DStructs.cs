using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public interface IContextID {
        public void GetContextID(out uint contextID);
        public uint GetContextID();
    }
    public interface IContextIDHolder{
        public void GetContextID(out IContextID contextID);
    }
    [System.Serializable]
    public struct ContextID : IContextID{
        public uint m_contextId;

        public void GetContextID(out uint contextID)=>
            contextID = m_contextId;

        public uint GetContextID(){
            return m_contextId; }
    }


    [System.Serializable]
    public struct TextureSourceToInt32BitsArray2D
    {
        public ContextID m_contextId;
        public Texture m_textureReference;
    }
    
    [System.Serializable]
    public struct Int32BitsArray2D
    {
        public ContextID m_contextId;
        public ushort m_width;
        public ushort m_height;
        public int [] m_arrayOfBitUnderInt;
    }
    
    [System.Serializable]
    public struct Int32BitsArray2DSoloPackagePreBytes
    {
        public ContextID m_contextId;
        public ushort m_width;
        public ushort m_height;
        public byte[] m_arrayOfBitUnderIntAsBytesGroup;
    }
    
    [System.Serializable]
    public struct Int32BitsArray2DSoloPackageFullBytes
    {
        public byte[] m_compressedInOneBlockOfBytesToStore;
    }
    
    [System.Serializable]
    public struct Int32BitsArray2DMultiPackagePreBytes
    {
        public ContextID m_contextId;
        public ushort m_width;
        public ushort m_height;
        public int m_startByteIndex1DAsByte;
        public byte[] m_arrayOfBitUnderIntAsBytesGroup;
    }
   
    [System.Serializable]
    public struct Int32BitsArray2DMultiPackageFullBytes
    {
        public byte[] m_compressedInOneBlockOfBytesToStore;
    }

    [System.Serializable]
    public struct BytesToUncompress {
        public byte[] m_rawByteThatCanBeUncompress;
    }
}
