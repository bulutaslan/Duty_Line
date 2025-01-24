using System;

public struct GridPosition : IEquatable<GridPosition>
{
    // Bu yapý, Grid sistemi üzerindeki bir hücrenin pozisyonunu tanýmlar

    public int x;   // X eksenindeki pozisyon
    public int z;   // Z eksenindeki pozisyon

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // Bu Grid pozisyonunun eþit olup olmadýðýný kontrol eder
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;   // Diðer GridPosition ile eþitlik kontrolü
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);   // Pozisyonun hash deðerini döndür
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";   // Grid pozisyonunun metinsel temsilini döndür
    }

    // Eþitlik operatörü: iki GridPosition'ýn eþit olup olmadýðýný kontrol eder
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    // Eþitsizlik operatörü: iki GridPosition'ýn eþit olup olmadýðýný kontrol eder
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    // Ýki GridPosition'ýn toplama operatörü
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    // Ýki GridPosition'ýn çýkarma operatörü
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}
