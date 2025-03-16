using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance;

    public int points;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddPoints(int amount)
    {
        instance.points += amount;
        instance.text.text = instance.points.ToString();
    }

    public static float SpeedMult()
    {
        float mult = 1f;
        int points = PointsManager.instance.points;
        while (points > 1000)
        {
            mult *= .92f;
            points -= 1000;
        }
        return mult;
    }
}
