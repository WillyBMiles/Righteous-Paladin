using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlowlyFadeIn : MonoBehaviour
{
    
    TextMeshProUGUI TextMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro = GetComponent<TextMeshProUGUI>();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);
        sequence.Append(TextMeshPro.DOFade(1f, 10f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("JakeGrid");
        }
    }
}
