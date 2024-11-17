using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    Vector3 movement;

    [SerializeField] private Animator anime;

    private void Start()
    {
        
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        transform.position += movement * speed * Time.deltaTime;

        if (movement.x != 0)
        {
            anime.SetBool("Jalan", true);
            anime.SetBool("JalanZ", false);
            anime.SetBool("JalanY", false);

            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (movement.y > 0)
        {
            anime.SetBool("JalanZ", true);
            anime.SetBool("JalanY", false);
            anime.SetBool("Jalan", false);
        }
        else if (movement.y < 0)
        {
            if (movement.x == 0)
            {
                anime.SetBool("JalanY", true);
                anime.SetBool("JalanZ", false);
                anime.SetBool("Jalan", false);
            }

            else
            {
                anime.SetBool("Jalan", true);
                anime.SetBool("JalanY", false);
                anime.SetBool("JalanZ", false);
            }
        }
        else
        {
            anime.SetBool("Jalan", false);
            anime.SetBool("JalanY", false);
            anime.SetBool("JalanZ", false);
        }

    }
}
