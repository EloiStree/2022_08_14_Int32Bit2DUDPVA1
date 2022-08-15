using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi
{
    public class V0_Convert_Uncompressed_PreBytes2Int32BitsArray : AConvert_Uncompressed_PreBytes2Int32BitsArrayMono
    {
        public ByteToInt32BitArray2DRegister m_register = new ByteToInt32BitArray2DRegister();
        public override void ConvertAs(in Int32BitsArray2DSoloPackagePreBytesWrapper source,
            ref Int32BitsArray2DWrapper result)
        {
            throw new System.NotImplementedException();
        }

        public override void ConvertIn(in Int32BitsArray2DMultiPackagePreBytesWrapper source,
            ref Int32BitsArray2DWrapper result)
        {

            ByteToInt32BitArray2DItem target = m_register.Get(source.m_data.m_contextId.m_contextId);
            target.Append(in source.m_data.m_arrayOfBitUnderIntAsBytesGroup ,
                in source.m_data.m_width,
                in source.m_data.m_height,
                in source.m_data.m_startByteIndex1DAsByte);
            result = target.m_array;
            result.m_data.m_width = source.m_data.m_width;
            result.m_data.m_height = source.m_data.m_height;
            result.m_data.m_contextId = source.m_data.m_contextId;
        }

        public override void ConvertIn(in IEnumerable<Int32BitsArray2DMultiPackagePreBytesWrapper> sources, ref Int32BitsArray2DWrapper result)
        {
            foreach (var item in sources)
            {
                ConvertIn(in item, ref result);
            }
        }
    }
}

public class ByteToInt32BitArray2DRegister
{
    public Dictionary<uint, ByteToInt32BitArray2DItem> contextArrays = new Dictionary<uint, ByteToInt32BitArray2DItem>();

    public ByteToInt32BitArray2DItem Get(uint contextId)
    {
        if (!contextArrays.ContainsKey(contextId))
            contextArrays.Add(contextId, new ByteToInt32BitArray2DItem());
        return contextArrays[contextId];
    }
}
public class ByteToInt32BitArray2DItem
{
    public Int32BitsArray2DWrapper m_array= new Int32BitsArray2DWrapper();
    public void Append(in byte[] arrayOfBitUnderInt, in ushort width, in ushort height, in int startIndex1DAsByte)
    {
        //(64*128) = 8192 bit
        int bitNeeded = (width * height);
        //1024 bytes
        int byteNeeded = bitNeeded / 8;
        //256 int
        int intNeeded = byteNeeded / 4;

        if (m_array.m_data.m_arrayOfBitUnderInt ==null || 
            m_array.m_data.m_arrayOfBitUnderInt.Length != intNeeded)
            m_array.m_data.m_arrayOfBitUnderInt = new int[intNeeded];

        Buffer.BlockCopy(arrayOfBitUnderInt, 0, m_array.m_data.m_arrayOfBitUnderInt, startIndex1DAsByte/4, arrayOfBitUnderInt.Length);
    }
    
}
