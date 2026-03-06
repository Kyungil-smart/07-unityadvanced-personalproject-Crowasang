using System;
using UnityEngine;

public class GuidGroup : MonoBehaviour
{
    public float radius = 2f;
    [Range(0, 360)] public float startAngle = 90f;
    public Sprite[] fruitSprites;
    public Sprite arrowSprite;
    
    private void Start()
    {
        FruitAlign();
    }

    public void FruitAlign()
    {

        int fruitCount = fruitSprites.Length;
        int arrowCount = fruitCount - 1; 
        int totalCount = fruitCount + arrowCount;
        
        for (int i = 0; i < totalCount; i++)
        {
            GameObject go = new GameObject($"Guide_{i}");
            go.transform.SetParent(this.transform);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            
            float angleDeg = startAngle - (i * 360f / totalCount);
            float angleRad = angleDeg * Mathf.Deg2Rad;
            go.transform.localPosition = new Vector3(Mathf.Cos(angleRad) * radius, Mathf.Sin(angleRad) * radius, 0);
            
            if (i % 2 == 0)
            {
                sr.sprite = fruitSprites[i / 2];
                go.transform.localRotation = Quaternion.identity;
            }
            else 
            {
                sr.sprite = arrowSprite;
                sr.transform.localScale = 0.1f * Vector3.one;
                go.transform.localRotation = Quaternion.Euler(0, 0, angleDeg - 90f);
            }
        }
    }
}
