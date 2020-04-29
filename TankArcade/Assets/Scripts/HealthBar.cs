using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient grad;
    public Image fill;

    private Slider slider;


    void Start()
    {
        slider = GetComponent<Slider>();
    }


    public void SetHealth(float value)
    {
        slider.value = value;
        fill.color = grad.Evaluate(slider.normalizedValue);
    }
}
