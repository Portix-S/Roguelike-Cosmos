using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float h)
    {
        slider.maxValue = h;
        slider.value = h;
    }

    public void SetHealth(float h)
    {
        slider.value = h;
    }
    public void SetSlider()
    {
        slider = GetComponent<Slider>();
    }
}
