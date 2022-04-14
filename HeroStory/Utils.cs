using UnityEngine;
using UnityEditor;

public class Utils
{
    public static float ClosestDistance(Collider a, Collider b)
    {
        return Vector3.Distance(a.ClosestPointOnBounds(b.transform.position),
                                b.ClosestPointOnBounds(a.transform.position));
    }

    public static string PrettyTime(float seconds)
    {
        var t = System.TimeSpan.FromSeconds(seconds);
        string res = "";
        if (t.Days > 0) res += t.Days + "일";
        if (t.Hours > 0) res += " " + t.Hours + "시간";
        if (t.Minutes > 0) res += " " + t.Minutes + "분";
        if (t.Milliseconds > 0) res += " " + t.Seconds + "." + (t.Milliseconds / 10) + "초";
        else if (t.Seconds > 0) res += " " + t.Seconds + "초";
        return res != "" ? res : "0초";
    }

}