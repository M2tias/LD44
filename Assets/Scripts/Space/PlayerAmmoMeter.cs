using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoMeter : MonoBehaviour
{
    [SerializeField]
    private PlayerShipRuntime runtime;
    [SerializeField]
    private List<Image> ammo;
    [SerializeField]
    private Sprite full;
    [SerializeField]
    private Sprite empty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ammo.Count; i++)
        {

            if (i < runtime.HP)
            {
                ammo[i].sprite = full;
            }
            else
            {
                ammo[i].sprite = empty;
            }
        }
    }
}
