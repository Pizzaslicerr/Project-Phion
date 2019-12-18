using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    public Camera orthoCam;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        float buttonDown = Input.GetAxis("Vertical");

        if (buttonDown == 1)
        {
            orthoCam.orthographicSize -= 0.1f;
        }
        if (buttonDown == -1)
        {
            orthoCam.orthographicSize += 0.1f;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
