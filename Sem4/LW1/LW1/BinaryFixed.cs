using System.Collections;

namespace LW1;
public class BinaryFixed
{
    private BitArray _bits = new(sizeof(int) * 8);
    private int _precision = 0;
    public int Precision { get => _precision; }
    public int BitCount { get => _bits.Count; }

    public BinaryFixed() { }
    public BinaryFixed(int decimalNumber, int precision = 0)
    {
        _precision = precision;

        int bitCount = _bits.Length;
        // Set sign bit
        if (decimalNumber < 0)
        {
            _bits[0] = decimalNumber < 0;
            decimalNumber = -decimalNumber;
        }

        for (int i = 0; i < bitCount - 1; i++)
        {
            _bits[bitCount - i - 1] = decimalNumber % 2 == 1;

            decimalNumber /= 2;
        }

        ShiftLeft(_precision);
    }
    public BinaryFixed(BitArray digits, int precision = 0)
    {
        _precision = precision;
        _bits = digits;
    }
    public BinaryFixed(BinaryFixed other)
    {
        _precision = other._precision;
        _bits = other._bits;
    }

    public bool Positive()
    {
        return !_bits[0];
    }

    public double ToDecimal()
    {
        double result = 0;

        for (int i = _bits.Length - 1; i >= 1; i--)
        {
            result += _bits[i] ? Math.Pow(2, BitCount - i - 1 - Precision) : 0;
        }

        if (!Positive())
        {
            result *= -1;
        }

        return result;
    }
    public BitArray DirectCode()
    {
        return (BitArray)_bits.Clone();
    }
    public BitArray AdditionalCode()
    {
        if (!Positive())
        {
            var result = InvertCode();

            var bitCount = _bits.Length;

            // Add one to the number
            for (int i = bitCount - 1; i > 1; i--)
            {
                // Adding a bit means changing inverting value
                result[i] = !result[i];

                // If changed value was 0, no need update bits on left side from it
                if (result[i])
                {
                    break;
                }
            }

            return result;
        }
        else
        {
            return DirectCode();
        }
    }
    public BitArray InvertCode()
    {
        var result = (BitArray)_bits.Clone();

        if (!Positive())
        {
            result = result.Not();

            // Preserve sign bit
            result[0] = !result[0];
        }

        return result;
    }
    public static BinaryFixed operator /(BinaryFixed first, BinaryFixed second)
    {
        if(second.ToDecimal() == 0)
        {
            throw new DivideByZeroException();
        }

        BinaryFixed quotient = new(0, 17);
        var dividend = new BinaryFixed(first);
        var divisor = new BinaryFixed(second);

        // Preserve sign
        quotient._bits[0] = dividend._bits[0] ^ divisor._bits[0];

        dividend._bits[0] = false;
        divisor._bits[0] = false;

        BinaryFixed one = new(1);

        while (dividend >= divisor)
        {
            quotient += one;
            dividend -= divisor;
        }

        for (int i = 0; i < quotient.Precision; i++)
        {
            dividend.ShiftLeft();
            quotient.ShiftLeft();

            while (dividend >= divisor)
            {
                quotient += one;
                dividend -= divisor;
            }
        }

        return quotient;
    }
    public void ShiftLeft(int count = 1)
    {
        for (int k = 0; k < count; k++)
        {
            bool carry = false;
            for (int i = _bits.Length - 1; i >= 1; i--)
            {
                bool temp = _bits[i];
                _bits[i] = carry;
                carry = temp;
            }
        }
    }
    public static bool operator >(BinaryFixed first, BinaryFixed second)
    {
        if (first._bits[0] && !second._bits[0])
            return false;
        if (!first._bits[0] && second._bits[0])
            if (first._bits[31] != second._bits[31])
                return true;

        for (int i = 1; i < first._bits.Length; i++)
        {
            if (first._bits[i] && !second._bits[i])
                return !first._bits[0];
            if (!first._bits[i] && second._bits[i])
                return first._bits[0];
        }
        return false;
    }
    public static bool operator <(BinaryFixed first, BinaryFixed second)
    {
        if (!first._bits[0] && second._bits[0])
            return false;
        if (first._bits[0] && !second._bits[0])
            if (first._bits[31] != second._bits[31])
                return true;

        for (int i = 1; i < first._bits.Length; i++)
        {
            if (!first._bits[i] && second._bits[i])
                return !first._bits[0];
            if (first._bits[i] && !second._bits[i])
                return first._bits[0];
        }
        return false;
    }

    public static bool operator >=(BinaryFixed first, BinaryFixed second)
    {
        return first == second || first > second;
    }
    public static bool operator <=(BinaryFixed first, BinaryFixed second)
    {
        return first == second || first < second;
    }
    public static bool operator ==(BinaryFixed first, BinaryFixed second)
    {
        for (int i = 0; i < first._bits.Length; i++)
        {
            if (first._bits[i] != second._bits[i])
                return false;
        }
        return true;
    }
    public static bool operator !=(BinaryFixed first, BinaryFixed second)
    {
        return !(first == second);
    }

    public static BinaryFixed operator +(BinaryFixed first, BinaryFixed second)
    {
        first._bits = first.AdditionalCode();
        second._bits = second.AdditionalCode();

        BinaryFixed answer = new(0, Math.Max(first.Precision, second.Precision));

        bool carry = false;

        for (int i = first._bits.Length - 1; i >= 0; i--)
        {
            answer._bits[i] = first._bits[i] ^ second._bits[i] ^ carry;

            carry = (
                (first._bits[i] && second._bits[i]) ||
                (first._bits[i] ^ second._bits[i] && carry)
                );
        }

        if (!answer.Positive())
        {
            answer._bits = answer.AdditionalCode();
        }

        return answer;
    }

    public static BinaryFixed operator -(BinaryFixed first, BinaryFixed second)
    {
        return first + second * new BinaryFixed(-1);
    }

    public static BinaryFixed operator *(BinaryFixed first, BinaryFixed second)
    {
        BinaryFixed answer = new();

        var signBit = first._bits[0];
        var arrayToShift = first.DirectCode();
        int shiftingAmount = 0;

        for (int i = 1; i < second._bits.Length; i++)
        {
            if (second._bits[second._bits.Length - i])
            {
                var shifted = arrayToShift.RightShift(shiftingAmount);
                shifted[0] = signBit;
                shiftingAmount = 1;
                answer += new BinaryFixed(shifted);
            }
            else
            {
                shiftingAmount++;
            }
        }

        answer._bits[0] = first.Positive() ^ second.Positive();

        return answer;
    }
}