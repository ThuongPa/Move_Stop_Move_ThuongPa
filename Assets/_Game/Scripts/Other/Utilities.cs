using System.Collections.Generic;
using System.Linq;

public static class Utilities
{
    //Random thứ tự một list
    public static List<T> SortOrder<T>(List<T> list, int amount)
    {
        return list.OrderBy(d => System.Guid.NewGuid()).Take(amount).ToList();
    }

    //Lấy kết quả theo tỷ lệ xác suất
    public static bool Chance(int rand, int max = 100)
    {
        return UnityEngine.Random.Range(0, max) < rand;
    }

    //Random giá trị enum trong một kiểu enum
    private static System.Random random = new System.Random();
    public static T RandomEnumValue<T>()
    {
        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(random.Next(v.Length));
    }

    //Random giá trị từ 1 list
    public static T RandomInMember<T>(params T[] ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Length)];
    }

    public static T RandomEnum<T>()
    {
        // Lấy danh sách các giá trị trong enum
        T[] enumValues = (T[])System.Enum.GetValues(typeof(T));

        // Chọn một giá trị ngẫu nhiên từ danh sách
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);
        return enumValues[randomIndex];
    }
}
