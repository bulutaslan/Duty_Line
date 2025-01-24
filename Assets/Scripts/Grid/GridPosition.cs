using System;

public struct GridPosition : IEquatable<GridPosition>
{
    // Bu yap�, Grid sistemi �zerindeki bir h�crenin pozisyonunu tan�mlar

    public int x;   // X eksenindeki pozisyon
    public int z;   // Z eksenindeki pozisyon

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // Bu Grid pozisyonunun e�it olup olmad���n� kontrol eder
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;   // Di�er GridPosition ile e�itlik kontrol�
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);   // Pozisyonun hash de�erini d�nd�r
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";   // Grid pozisyonunun metinsel temsilini d�nd�r
    }

    // E�itlik operat�r�: iki GridPosition'�n e�it olup olmad���n� kontrol eder
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    // E�itsizlik operat�r�: iki GridPosition'�n e�it olup olmad���n� kontrol eder
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    // �ki GridPosition'�n toplama operat�r�
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    // �ki GridPosition'�n ��karma operat�r�
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}
