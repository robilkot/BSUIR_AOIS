namespace LW1;
using System;
using System.Collections;

public class BinaryFloat
{
    private BitArray bits;

    public BinaryFloat(float value)
    {
        bits = new(32);

        if (value == 0)
        {
            return;
        }

        bits[0] = value < 0;

        int exponent = 0;
        float mantissa = Math.Abs(value);

        // Normalize mantissa
        while (mantissa >= 2)
        {
            mantissa /= 2;
            exponent++;
        }
        while (mantissa < 1)
        {
            mantissa *= 2;
            exponent--;
        }
        mantissa -= 1; // Remove leading bit

        // Shift exponent
        exponent += 127;

        for(int i = 1; i < 9; i++)
        {
            bits[i] = (exponent >> (8 - i) & 1) == 1;
        }

        for (int i = 0; i < 23; i++)
        {
            mantissa *= 2;

            if (mantissa >= 1)
            {
                bits[i + 9] = true;
                mantissa--;
            }
        }
    }

    public float ToFloat()
    {
        int exponent = 0;

        for (int i = 1; i <= 8; i++)
        {
            exponent += (bits[i] == true ? 1 : 0) << (8 - i);
        }

        exponent -= 127;

        // Account for leading bit
        float mantissa = 1;

        for (int i = 9; i < 32; i++)
        {
            mantissa += (bits[i] == true) ? (float)Math.Pow(2, 8 - i) : 0;
        }

        if(mantissa == 1 && exponent == -127)
        {
            return 0;
        }

        return (float)((bits[0] ? -1 : 1) * mantissa * Math.Pow(2, exponent));
    }

    public static BinaryFloat operator +(BinaryFloat a, BinaryFloat b)
    {
        bool sequenceEquals = true;
        
        for(int i = 1; i < 32; i++)
        {
            if (a.bits[i] != b.bits[i])
            {
                sequenceEquals = false;
                break;
            }
        }

        if (sequenceEquals)
        {
            return new BinaryFloat(0);
        }

        var resultBits = new BitArray(32);

        int exponent1 = 0;
        int exponent2 = 0;
        int mantissa1 = 0;
        int mantissa2 = 0;

        for (int i = 1; i <= 8; i++)
        {
            exponent1 |= (a.bits[i] ? 1 : 0) << (8 - i);
            exponent2 |= (b.bits[i] ? 1 : 0) << (8 - i);
        }
        for (int i = 9; i <= 31; i++)
        {
            mantissa1 |= (a.bits[i] ? 1 : 0) << (31 - i);
            mantissa2 |= (b.bits[i] ? 1 : 0) << (31 - i);
        }

        mantissa1 |= 1 << 23;
        mantissa2 |= 1 << 23;

        int resultExponent = Math.Max(exponent1, exponent2);

        // Shift mantissas if needed
        mantissa1 >>= resultExponent - exponent1;
        mantissa2 >>= resultExponent - exponent2;

        int resultMantissa = (a.bits[0] ? -mantissa1 : mantissa1) + (b.bits[0] ? -mantissa2 : mantissa2);

        if (resultMantissa < 0)
        {
            resultBits[0] = true;
            resultMantissa = -resultMantissa;
        }

        // Normalize the mantissa
        while (resultMantissa >= (1 << 23))
        {
            resultMantissa >>= 1;
            resultExponent++;
        }

        if (resultMantissa != 0)
        {
            while (resultMantissa < (1 << 23))
            {
                resultMantissa <<= 1;
                resultExponent--;
            }
        }

        for (int i = 1; i <= 8; i++)
        {
            resultBits[i] = ((resultExponent >> (8 - i)) & 1) == 1;
        }
        for (int i = 9; i <= 31; i++)
        {
            resultBits[i] = ((resultMantissa >> (31 - i)) & 1) == 1;
        }

        return new BinaryFloat(0) { bits = resultBits };
    }

    public BitArray ToBitArray()
    {
        return new BitArray(bits);
    }
}