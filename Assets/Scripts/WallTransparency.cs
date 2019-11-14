using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Wall")
            {
                renderer = hit.transform.GetComponent<Renderer>();
                renderer.enabled = false;
            }
            else
            {
                if(renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }
    }
}
