using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public List<Sprite> sprites;

    SpriteRenderer renderer;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        timer = time;
        renderer = GetComponent<SpriteRenderer>();
    }

    float timer;
    int index;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            index = (index + 1) % sprites.Count;
            timer = time;
        }
        renderer.sprite = sprites[index];
    }
}
