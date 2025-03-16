using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDirection : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    Sprite leftSprite;
    [SerializeField]
    Sprite upSprite;

    [SerializeField]
    Sprite rightSprite;

    [SerializeField]
    Sprite downSprite;



    public void SetDirection(float direction)
    {
        int roundedDirection = mod(Mathf.RoundToInt( direction / 90f ),4);
        if (roundedDirection == 0)
        {
            sprite.sprite = rightSprite;
        }
        if (roundedDirection == 1)
        {
            sprite.sprite = upSprite;
        }
        if (roundedDirection == 2)
        {
            sprite.sprite = leftSprite;
        }
        if (roundedDirection == 3)
        {
            sprite.sprite = downSprite;
        }

    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
