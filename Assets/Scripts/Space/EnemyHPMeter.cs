using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPMeter : MonoBehaviour
{
    [SerializeField]
    private List<Image> healthIcons;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private Sprite fullShield;
    [SerializeField]
    private Sprite emptyShield;

    private float HP = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < healthIcons.Count; i++)
        {
            Sprite full = i > 4 ? fullShield : fullHeart;
            Sprite empty = i > 4 ? emptyShield : emptyHeart;

            if (i < HP)
            {
                healthIcons[i].sprite = full;
            }
            else
            {
                healthIcons[i].sprite = empty;
            }
        }
    }

    public void SetHealth(float hp)
    {
        HP = hp;
    }
}
