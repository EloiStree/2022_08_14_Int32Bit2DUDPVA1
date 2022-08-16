using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ConvertPrimitiveArray2Byte
{
    public static void ConvertBytesToFloat(in byte[] byteArray, ref float[] floatArray, out long executeTimeMS)
    {
        if (byteArray.Length * 4 != floatArray.Length)
            throw new Exception("you array must be the four time the size of the float array");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);
        watch.Stop();
        executeTimeMS = watch.ElapsedMilliseconds;
    }

    public static void ConvertFloatsToBytes(in float[] floatArray, ref byte[] byteArray, out long executeTimeMS)
    {
        if (byteArray.Length /4 != floatArray.Length)
            throw new Exception("you array must be the four time the size less of the float array");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
        watch.Stop();
        executeTimeMS = watch.ElapsedMilliseconds;
    }
    public static void ConvertBytesToInt(in byte[] byteArray, ref int[] intArray, out long executeTimeMS)
    {
        //if (byteArray.Length * 4 != intArray.Length)
        //    throw new Exception("you array must be the four time the size of the int array");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        Buffer.BlockCopy(byteArray, 0, intArray, 0, byteArray.Length);
        watch.Stop();
        executeTimeMS = watch.ElapsedMilliseconds;
    }

    public static void ConvertIntsToBytes(in int[] intArray, ref byte[] byteArray, out long executeTimeMS)
    {
        //if (byteArray.Length / 4 != intArray.Length)
        //    throw new Exception("you array must be the four time the size less of the int array");
        Stopwatch watch = new Stopwatch();
        watch.Start();
        Buffer.BlockCopy(intArray, 0, byteArray, 0, byteArray.Length);
        watch.Stop();
        executeTimeMS = watch.ElapsedMilliseconds;
    }


    /*

    var floatArray1 = new float[] { 123.45f, 123f, 45f, 1.2f, 34.5f };

    // create a byte array and copy the floats into it...
    var byteArray = new byte[floatArray1.Length * 4];
    Buffer.BlockCopy(floatArray1, 0, byteArray, 0, byteArray.Length);

// create a second float array and copy the bytes into it...
var floatArray2 = new float[byteArray.Length / 4];
    Buffer.BlockCopy(byteArray, 0, floatArray2, 0, byteArray.Length);

     
    //*/
}
