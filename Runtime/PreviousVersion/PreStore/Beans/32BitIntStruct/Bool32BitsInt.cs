using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Allow to store boolean in int to be used in compute shader or transported.
/// </summary>
[System.Serializable]
public struct Bool32BitsInt
{
    public int m_storageInt;
}
/// <summary>
/// Allow to store true or false value in an array int format for shader
/// </summary>
[System.Serializable]
public struct ArrayAs32BitsInt
{
    public int[] m_storageInt;
}
[System.Serializable]
public class ArrayAs32BitsIntWrapper : IArrayAs32BitsIntWrapperGet, IArrayAs32BitsIntWrapperSet
{
    public int[] m_storageInt;
    public void GetBit(in int index, out bool value)
    {
        int byteIndex = (int)(index / 32.0);
        int bitIndex = index % 32;
        value = E_PrimitiveBoolUtility.GetBit(in m_storageInt[byteIndex],in bitIndex);
    }

    public void GetIntCount(out int lenght)
    {
        lenght = m_storageInt.Length;
    }

    public void GetRefToArray(out int[] array)
    {
        array = m_storageInt;
    }

    public void GetTotalBitLenght(out int lenght)
    {
        lenght = m_storageInt.Length *  32;
    }

    public void GetTotalByteLenght(out int lenght)
    {
        lenght = m_storageInt.Length * 4 ;
    }

    public void ResetForNBits(in int numberOfBitToStore)
    {
        int c = numberOfBitToStore / ( 32);
        m_storageInt = new int[c];
    }

    public void SetArrayByCopyRef(IEnumerable<int> array)
    {
        int i = 0;
        foreach (var item in array)
        {
            if(i<m_storageInt.Length)
                m_storageInt[i]=item;
            i++;
        }
    }
   

    public void SetArrayWithRef(ref int[] array)
    {
        m_storageInt = array;
    }

    public void SetBit(in int index, in bool value)
    {
        int byteIndex = (int)(index / 32.0);
        int bitIndex = index % 32;
        Eloi.E_PrimitiveBoolUtility.
            SetBit(
            ref m_storageInt[byteIndex],
            in bitIndex, in value);
    }
}
public interface IArrayAs32BitsIntWrapperGet
{

    public void GetIntCount(out int lenght);
    public void GetTotalByteLenght(out int lenght);
    public void GetTotalBitLenght(out int lenght);
    public void GetRefToArray(out int[] array);
    public void GetBit(in int index, out bool value);
}
public interface IArrayAs32BitsIntWrapperSet
{
    public void SetBit(in int index, in bool value);
    public void SetArrayWithRef(ref int[] array);
    public void SetArrayByCopyRef( IEnumerable<int> array);
    public void ResetForNBits(in int numberOfBitToStore);
}


[System.Serializable]
public struct ArrayAs32BitsIntBlockSegment
{
    public ushort m_channalId; //2 1 2
    public int m_blockStartIndex;  //4 3 4
    public ArrayAs32BitsInt m_booleanAsint32bits;
}
[System.Serializable]
public struct ArrayAs32BitsIntOneBlock
{
    public ushort m_channalId; //2 1 2
    public ArrayAs32BitsInt m_booleanAsint32bits;
}

public class Bool32BitsIntUtility
{
    public static void GetBoolAt(in Bool32BitsInt target, in int index0to31, out bool value)
    {
        value = E_PrimitiveBoolUtility.GetBit(in target.m_storageInt, in index0to31);
    }
    public static void GetBoolAt(in Bool32BitsInt[] target, in int index, out bool value)
    {
        int byteIndex = (int)(index / 32.0);
        value = E_PrimitiveBoolUtility.GetBit(in target[byteIndex].m_storageInt, in index);
    }
    public static void SetBoolAt(ref Bool32BitsInt target, in int index0to31, in bool value)
    {
        E_PrimitiveBoolUtility.SetBit(ref target.m_storageInt, in index0to31, in value);
    }
    public static void SetBoolAt(ref Bool32BitsInt[] target, in int index, in bool value)
    {
        int byteIndex = (int)(index / 32.0);
        E_PrimitiveBoolUtility.SetBit(ref target[byteIndex].m_storageInt, in index, in value);
    }

}

public class ColorAs32BitsIntCompressor
{
    public static void GetArrayCount(in int width, in int height,
        out int array1DLenght, out int intToStoreCount, out int byteToStoreCount, out int bitToStoreCount)
    {
        array1DLenght = width * height;
        GetArrayCount(array1DLenght,
            out intToStoreCount,
            out byteToStoreCount,
            out bitToStoreCount);
    }
    public static void GetArrayCount(in int lenght,
        out int intToStoreCount, out int byteToStoreCount, out int bitToStoreCount)
    {
        if (lenght % 32 != 0) throw new System.Exception("Array bust be n%32 else it does not work.");
        intToStoreCount = lenght/32;
        byteToStoreCount = intToStoreCount * 4;
        bitToStoreCount = intToStoreCount * 4 * 32;

    }
    public static void CompressIntsToBytes(in int[] target, out byte [] asBytes)
    {
        GetArrayCount(target.Length,
        out int intToStoreCount, out int byteToStoreCount, out int bitToStoreCount);
        asBytes = new byte[byteToStoreCount];
        Array.Copy(target, asBytes, target.Length);
        Buffer.BlockCopy(target, 0, asBytes, 0, asBytes.Length);
    }
    

}
