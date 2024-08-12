using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class maker : MonoBehaviour
{
    public GameObject prefab;

    float x, y;
    public float initRadius = 0.2f;
    public float lineDensity = 0.75f;
    public float angle = 0;
    public float angleGrowthRate = 5f;
    private GameObject MakeVertex()
    {
        float radius = initRadius + lineDensity * angle;
        angle += angleGrowthRate / radius;
        x = angle * Mathf.Sin(angle);
        y = angle * Mathf.Cos(angle);

        x = radius * Mathf.Sin(angle);
        y = radius * Mathf.Cos(angle);

        GameObject gameObject = Instantiate(prefab);

        gameObject.transform.position = new Vector2(x, y);

        return gameObject;
    }

    void Start()
    {
        x = 0;
        y = 0;
        for (int i = 0; i < 10000; i++)
        {
            MakeVertex();
            //Thread.Sleep(1);
            
        }
    }
}
