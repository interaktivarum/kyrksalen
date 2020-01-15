using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LetterSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color c = GetComponentInChildren<TextMesh>().color;
        GetComponentInChildren<TextMesh>().color = new Color(c.r, c.g, c.b, c.a + 0.02f);

        if(transform.position.y < -1) {
            Destroy(gameObject);
        }
            
    }
}
