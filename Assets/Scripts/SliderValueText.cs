using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField] private Piano piano;
    
    private Slider slider;
    private TextMeshProUGUI textComp;

    void Awake()
    {
        slider = GetComponentInParent<Slider>();
        textComp = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    public void UpdateText(float val)
    {
        textComp.text = (System.Math.Round((decimal)val, 2)).ToString();
        piano.ChangePitchCorrection(val);
    }
}