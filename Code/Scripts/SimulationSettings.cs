using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombinedScript : MonoBehaviour
{
    [SerializeField] private Slider spawnerSlider;
    [SerializeField] private TextMeshProUGUI spawnerSliderText;
    private int updatedNumAgents = 20;

    [SerializeField] private Slider diseaseSlider;
    [SerializeField] private TextMeshProUGUI diseaseSliderText;
    private float updatedDiseaseSeverity = 1.0f;

    [SerializeField] private Slider immunitySlider;
    [SerializeField] private TextMeshProUGUI immunitySliderText;
    private float updatedImmunity = 0.000f;

    void Start(){
        spawnerSlider.onValueChanged.AddListener((v) => {
            spawnerSliderText.text = v.ToString("0");
            updatedNumAgents = Mathf.RoundToInt(v);
            PlayerPrefs.SetInt("NumAgents", updatedNumAgents);
        });

        diseaseSlider.onValueChanged.AddListener((v) => {
            diseaseSliderText.text = v.ToString("0.0");
            updatedDiseaseSeverity = v;
            PlayerPrefs.SetFloat("DiseaseSev", updatedDiseaseSeverity);
        });

        immunitySlider.onValueChanged.AddListener((v) => {
            immunitySliderText.text = v.ToString("0.000");
            updatedImmunity = v;
            PlayerPrefs.SetFloat("BaseImmunity", updatedImmunity);
        });
    }
}
