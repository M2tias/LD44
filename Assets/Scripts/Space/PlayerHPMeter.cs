using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGoldMeter : MonoBehaviour
{
    [SerializeField]
    private PlayerShipRuntime runtime;
    [SerializeField]
    private List<Sprite> digits;
    [SerializeField]
    private List<Image> numbers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int ones = runtime.Gold % 10;
        int tens = (int)(runtime.Gold % 100 / 10);
        int hundreds = (int)(runtime.Gold % 1000 / 100);
        int thousands = (int)(runtime.Gold % 10000 / 1000);
        int tens_of_thousands = (int)(runtime.Gold % 100000 / 10000);
        int hundreds_of_thousands = (int)(runtime.Gold % 1000000 / 100000);

        numbers[0].sprite = digits[ones];
        numbers[1].sprite = digits[tens];
        numbers[2].sprite = digits[hundreds];
        numbers[3].sprite = digits[thousands];
        numbers[4].sprite = digits[tens_of_thousands];
        numbers[5].sprite = digits[hundreds_of_thousands];
    }
}
