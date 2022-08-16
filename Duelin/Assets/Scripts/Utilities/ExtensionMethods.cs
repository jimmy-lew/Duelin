using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void ForEach<T>(this IEnumerable<T> enumator, Action<T> callback)
    {
        foreach (T element in enumator) { callback(element); }
    }

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
        if (attributes != null && attributes.Any()) return attributes.First().Description;
        return value.ToString();
    }

    public static void RenderSprite(Transform parent, string spriteName, string componentName, string resourceName = "UI_Atlas") {
        Sprite sprite = Resources.LoadAll<Sprite>(resourceName).Single(sprite => sprite.name.Equals(spriteName));
        SpriteRenderer renderer = parent.Find(componentName).gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}

 public static class TransformHelper
 {
     public static List<Transform> GetAllChildren(this Transform aTransform, List<Transform> aList = null)
     {
         if (aList == null)
             aList = new List<Transform>();
         int start = aList.Count;
         for (int n = 0; n < aTransform.childCount; n++)
             aList.Add(aTransform.GetChild(n));
         for (int i = start; i < aList.Count; i++)
         {
             var t = aList[i];
             for (int n = 0; n < t.childCount; n++)
                 aList.Add(t.GetChild(n));
         }
         return aList;
     }
 }