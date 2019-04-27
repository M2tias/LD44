using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private Backdrop next;
    [SerializeField]
    private Backdrop previous;
    [SerializeField]
    private PlayerShipRuntime shipRuntime;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float maxX = transform.position.x + (boxCollider.size.x / 2);
        float minX = transform.position.x - (boxCollider.size.x / 2);
        if (shipRuntime.Position.x > maxX - 10f)
        {
            Vector3 v = next.transform.position;
            next.transform.position = new Vector3(boxCollider.size.x + transform.position.x, v.y, v.z);
        }
        else if (shipRuntime.Position.x < minX + 10f)
        {
            Vector3 v = previous.transform.position;
            next.transform.position = new Vector3(transform.position.x - boxCollider.size.x, v.y, v.z);
        }
    }
}
