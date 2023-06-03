public class BitwiseFlag
{
    #region Constants and Statics

    const int CAPACITY = 31;

    static void Message(string message)
    {
        throw new System.Exception(message);
    }

    #endregion

    bool[] flags = new bool[CAPACITY];

    public bool this[int index]
    {
        get
        {
            if (index >= CAPACITY || index < 0)
                Message(index + "is out of range");

            return flags[index];
        }
        set
        {
            if (index >= CAPACITY || index < 0)
                Message(index + "is out of range");

            flags[index] = value;
        }
    }

    public int ToInt()
    {
        int result = 0;
        for (int i = 0; i < CAPACITY; i++)
        {
            if (flags[i])
                result += PowTwo(i);
        }

        return result;
    }

    public void FromInt(int value)
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            flags[i] = (value % 2 == 1);
            value = value >> 1;
        }
    }

    int PowTwo(int p)
    {
        int r = 1;
        for (int j = 0; j < p; j++)
            r *= 2;

        return r;
    }

    public override string ToString()
    {
        System.Text.StringBuilder b = new System.Text.StringBuilder();
        for (int i = CAPACITY - 1; i >= 0; i--)
        {
            if (flags[i])
                b.Append("1");
            else
                b.Append("0");
        }

        b.Append("=");
        b.Append(ToInt());

        return b.ToString();
    }

    public void ResetAll()
    {
        for (int i = 0; i < CAPACITY; i++)
            this[i] = false;
    }
}