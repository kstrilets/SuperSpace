using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scrolling background image using main texture offset
public class ScrollingBackground : MonoBehaviour
{

    [SerializeField] float speed;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();  
    }

    void Update()
    {
        float offSet = Time.time * speed;
        rend.material.mainTextureOffset = new Vector2(0, offSet);
    }
}
