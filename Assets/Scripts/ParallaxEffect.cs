using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Vector2 startPos; // Posisi awal layer
    private float lengthX, lengthY; // Panjang layer pada sumbu X dan Y
    public GameObject cam; // Kamera utama
    public Vector2 parallaxFactor; // Faktor parallax untuk X dan Y (contoh: (0.5f, 0.2f))

    void Start()
    {
        startPos = transform.position;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Hitung pergeseran kamera relatif terhadap layer
        Vector2 temp = new Vector2(cam.transform.position.x * (1 - parallaxFactor.x),
                                   cam.transform.position.y * (1 - parallaxFactor.y));
        Vector2 dist = new Vector2(cam.transform.position.x * parallaxFactor.x,
                                   cam.transform.position.y * parallaxFactor.y);

        // Update posisi layer berdasarkan parallax
        transform.position = new Vector3(startPos.x + dist.x, startPos.y + dist.y, transform.position.z);

        // Loop layer jika kamera bergerak jauh
        if (temp.x > startPos.x + lengthX) startPos.x += lengthX;
        else if (temp.x < startPos.x - lengthX) startPos.x -= lengthX;

        if (temp.y > startPos.y + lengthY) startPos.y += lengthY;
        else if (temp.y < startPos.y - lengthY) startPos.y -= lengthY;
    }
}
