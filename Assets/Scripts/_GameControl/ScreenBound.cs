using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// calculating the bounds of the screen
public class ScreenBound : MonoBehaviour
{

    public static Vector2 bounds;
    float objWidth;
    float objHeight;


    void Start()
    {
        //Calculating screen size
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            Camera.main.transform.position.z));

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, bounds.x * -1 + objWidth, bounds.x - objWidth );
        viewPos.y = Mathf.Clamp(viewPos.y, bounds.y * -1 + objHeight, bounds.y - objHeight);
        transform.position = viewPos;
    }
}
