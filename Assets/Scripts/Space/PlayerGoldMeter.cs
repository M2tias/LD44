using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPMeter : MonoBehaviour
{
    [SerializeField]
    private PlayerShipRuntime runtime;
    [SerializeField]
    private List<Image> healths;
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
        for (int i = 0; i < healths.Count; i++)
        {

            if (i < runtime.HP)
            {
                healths[i].sprite = full;
            }
            else
            {
                healths[i].sprite = empty;
            }
        }
    }
}
