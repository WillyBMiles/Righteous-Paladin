using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerMovementGridded movement;
    public Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        for (; i < movement.Health; i++)
        {
            images[i].enabled = true;
        }
        for (; i < images.Count(); i++)
        {
            images[i].enabled = false;
        }
    }
}
