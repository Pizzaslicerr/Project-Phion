using UnityEngine;

public class SpritePosition : MonoBehaviour
{
    public GameObject sprite;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - sprite.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = sprite.transform.position + offset;
    }
}
