using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;

    private float upOrDown;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        upOrDown = Input.GetAxis("Vertical");

        if (upOrDown == 1)
        {
            Debug.Log("Pressed up");

            if (camera1.activeInHierarchy == true)
            {
                camera1.SetActive(false);
                camera2.SetActive(true);
            }
        }

        if (upOrDown == -1)
        {
            Debug.Log("Pressed down");
            if (camera2.activeInHierarchy == true)
            {
                camera1.SetActive(true);
                camera2.SetActive(false);
            }
        }
    }
}
