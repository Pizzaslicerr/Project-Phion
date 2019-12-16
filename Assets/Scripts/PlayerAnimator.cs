using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private bool walking;
    private bool attacking;
    private bool idle;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        anim.SetBool("isWalking", walking);
        anim.SetBool("isAttacking", attacking);
        anim.SetBool("isIdle", idle);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (Input.GetButton("Fire1") && hit.collider.tag == "Enemy")
            {
                attacking = true;
                walking = false;
                idle = false;
            }
            else if (Input.GetButton("Fire1"))
            {
                walking = true;
                attacking = false;
                idle = false;
            }
            else
            {
                walking = false;
                attacking = false;
                idle = true;
            }
        }
    }
}
