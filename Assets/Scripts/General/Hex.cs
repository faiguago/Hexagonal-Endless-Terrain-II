using System;
using System.Collections.Generic;
using UnityEngine;

public struct Hex
{
    public static float cos60 = Mathf.Cos(Mathf.Deg2Rad * 60);
    public static float sin60 = Mathf.Sin(Mathf.Deg2Rad * 60);

    public static float f0 = 3f / 2f;
    public static float f1 = 0f;
    public static float f2 = Mathf.Sqrt(3f) / 2f;
    public static float f3 = Mathf.Sqrt(3f);

    public static float b0 = 2f / 3f;
    public static float b1 = 0f;
    public static float b2 = -1f / 3f;
    public static float b3 = Mathf.Sqrt(3f) / 3f;

    public readonly int q, r, s;

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
        s = -q - r;
    }

    public Hex(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }

    public enum Directions
    {
        SE,
        NE,
        N,
        NW,
        SW,
        S
    }

    private static Hex[] directions =
        {
            new Hex(1, -1),
            new Hex(1, 0),
            new Hex(0, 1),
            new Hex(-1, 1),
            new Hex(-1, 0),
            new Hex(0, -1)
        };

    public static Hex Direction(Directions direction)
    {
        return directions[(int)direction];
    }

    public static Hex Neighbor(Hex hex, Directions direction)
    {
        return hex + Direction(direction);
    }

    public static int Length(Hex a)
    {
        return ((Math.Abs(a.q) + Math.Abs(a.r) + Math.Abs(a.s)) / 2);
    }

    public static int Distance(Hex a, Hex b)
    {
        return Length(a - b);
    }

    public static Hex[] Ring(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();
        Hex cube = center + Direction((Directions)4) * radius;

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                results.Add(cube);
                cube = Neighbor(cube, (Directions)i);
            }
        }

        return results.ToArray();
    }

    public static Hex[] Spiral(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();
        for (int i = 1; i <= radius; i++)
        {
            Hex[] temp = Ring(center, i);
            for (int j = 0; j < temp.Length; j++) //>
                results.Add(temp[j]);
        }

        return results.ToArray();
    }

    public static Vector2 HexToPixel(Hex h, int size)
    {
        float x = (f0 * h.q + f1 * h.r) * size;
        float y = (f2 * h.q + f3 * h.r) * size;

        return new Vector2(x, y);
    }

    public static Hex PixelToHex(Vector2 pos, int size)
    {
        Vector2 pt = new Vector2(pos.x / size,
            pos.y / size);
        float q = b0 * pt.x + b1 * pt.y;
        float r = b2 * pt.x + b3 * pt.y;

        return HexRound(new FractionalHex(q, r, -q - r));
    }

    private static Hex HexRound(FractionalHex h)
    {
        int q = (int)(Math.Round(h.q));
        int r = (int)(Math.Round(h.r));
        int s = (int)(Math.Round(h.s));

        float dq = Math.Abs(q - h.q);
        float dr = Math.Abs(r - h.r);
        float ds = Math.Abs(s - h.s);

        if (dq > dr && dq > ds)
            q = -r - s;
        else if (dr > ds)
            r = -q - s;
        else
            s = -q - r;

        return new Hex(q, r, s);
    }

    public static Hex operator +(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r);
    }

    public static Hex operator -(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r);
    }

    public static Hex operator *(Hex a, int k)
    {
        return new Hex(a.q * k, a.r * k);
    }

    public static bool operator ==(Hex a, Hex b)
    {
        return a.q == b.q && a.r == b.r;
    }

    public static bool operator !=(Hex a, Hex b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Hex && obj != null)
        {
            Hex hex = (Hex)obj;
            return hex == this;
        }
        else
            return false;
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}, {2})", q, r, s);
    }
}

public struct FractionalHex
{
    public readonly float q, r, s;

    public FractionalHex(float q, float r)
    {
        this.q = q;
        this.r = r;
        s = -q - r;
    }

    public FractionalHex(float q, float r, float s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
}