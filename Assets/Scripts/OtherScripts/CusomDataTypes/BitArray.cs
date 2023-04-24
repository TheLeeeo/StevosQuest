using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct BitArray
{
    public BitArray(int value)
    {
        bitArray = value;
    }

    private int bitArray;

    public void SetBit(int bitIndex)
    {
        bitArray |= 1 << bitIndex;
    }

    public void UnsetBit(int bitIndex)
    {
        bitArray &= ~(1 << bitIndex);
    }

    public void SetMask(int mask)
    {
        bitArray |= mask;
    }

    public void UnsetMask(int mask)
    {
        bitArray &= ~mask;
    }

    public bool AnyBitIsSet(int mask)
    {
        return (bitArray & mask) > 0;
    }

    public bool AllBitsAreSet(int mask)
    {
        return (bitArray & mask) == mask;
    }

    public int CompareMask(int mask)
    {
        return bitArray & mask;
    }

    public bool BitIsSet(int bitIndex)
    {
        return (bitArray & 1 << bitIndex) > 0;
    }

    #region Operators
    public static implicit operator BitArray(int value)
    {
        return new BitArray(value);
    }

    public static implicit operator int(BitArray value)
    {
        return value.bitArray;
    }

    public static BitArray operator &(BitArray valueA, int valueB)
    {
        return new BitArray(valueA.bitArray & valueB);
    }

    public static BitArray operator |(BitArray valueA, int valueB)
    {
        return new BitArray(valueA.bitArray | valueB);
    }

    public static BitArray operator ^(BitArray valueA, int valueB)
    {
        return new BitArray(valueA.bitArray ^ valueB);
    }

    public static BitArray operator >>(BitArray valueA, int valueB)
    {
        return new BitArray(valueA.bitArray >> valueB);
    }

    public static BitArray operator <<(BitArray valueA, int valueB)
    {
        return new BitArray(valueA.bitArray << valueB);
    }

    public static BitArray operator ~(BitArray value)
    {
        return new BitArray(value.bitArray);
    }
    #endregion
}