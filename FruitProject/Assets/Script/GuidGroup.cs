using System;
using UnityEngine;

public class GuidGroup : MonoBehaviour
{
    public float radius = 2f;
    [Range(0, 360)] public float startAngle = 90f;

    private void Start()
    {
        FruitAlign();
    }

    public void FruitAlign()
    {
        int childCount = transform.childCount;
        
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            RectTransform rect = child.GetComponent<RectTransform>();

            if (rect != null)
            {
                float angle = (startAngle + (i * 360f / childCount)) * Mathf.Deg2Rad;
                
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                
                rect.anchoredPosition = new Vector2(x, y);
                if (i % 2 == 0)
                {
                    rect.localRotation = Quaternion.identity;
                }
                else
                {
                    float angleDegrees = angle * Mathf.Rad2Deg;
                    rect.localRotation = Quaternion.Euler(0, 0, angleDegrees - 90f); 
                }
            }
        }
    }
}
