using System.Collections;
using UnityEngine;

public class BalleDeNeigeAttaque : MonoBehaviour
{

    void Start()
    {
        Invoke("Disparition", 5);
    }

    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ennemie") Disparition();

    }

    private void Disparition()
    {
        GameObject.Destroy(gameObject);
    }
}
