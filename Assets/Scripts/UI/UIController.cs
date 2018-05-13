using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private Transform light;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject weatherTexture;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<GameObject> settingPanels;
    [SerializeField] private Text fpsText;

    [SerializeField] private CloudScript clouds;
    [SerializeField] private WeatherScript weather;
    
    private ColorBlock selectedButtonColors;
    private float deltaTime = 0.0f;

    public void GenerateNewWeatherAction()
    {
        weather.GenerateAndChangeWeatherTexture();
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    private int previousSelection = -1;
    public void SelectPanel(int panelId)
    {
        settingPanel.SetActive(true);
        settingPanels.ForEach(p => p.SetActive(false));
        buttons.ForEach(b => b.colors = ColorBlock.defaultColorBlock);
        if (previousSelection != panelId)
        {
            settingPanels[panelId].SetActive(true);
            var colors = buttons[panelId].colors;
            colors.normalColor = Color.gray;
            colors.highlightedColor = Color.gray;
            buttons[panelId].colors = colors;
            previousSelection = panelId;
        }
        else
        {
            previousSelection = -1;
            settingPanel.SetActive(false);
        }
    }

    public void HideShowWeatherTexture(Text text)
    {
        weatherTexture.SetActive(!weatherTexture.activeSelf);
        text.text = weatherTexture.activeSelf ? "Hide" : "Show";
    }

    public void HideShowControls(Text text)
    {
        controlsPanel.SetActive(!controlsPanel.activeSelf);
        text.text = controlsPanel.activeSelf ? "Hide controls" : "Show controls";
    }

    public void ToggleLowFreq()
    {
        clouds.debugNoLowFreqNoise = !clouds.debugNoLowFreqNoise;
    }

    public void ToggleHighFreq()
    {
        clouds.debugNoHighFreqNoise = !clouds.debugNoHighFreqNoise;
    }

    public void ToggleCurl()
    {
        clouds.debugNoCurlNoise = !clouds.debugNoCurlNoise;
    }

    public void OnChangeStepCount(Slider slider)
    {
        clouds.steps = (int) slider.value;
    }

    public void OnChangeLightX(Slider slider)
    {
        light.localRotation = Quaternion.Euler((float)slider.value, light.transform.eulerAngles.y, 0);
    }

    public void OnChangeLightY(Slider slider)
    {
        light.localRotation = Quaternion.Euler(light.transform.eulerAngles.x, (float)slider.value, 0);
    }

    public void OnChangeDownsample(Slider slider)
    {
        clouds.downSample = (int) slider.value;
    }

    public void ToggleInCLouds()
    {
        clouds.allowFlyingInClouds = !clouds.allowFlyingInClouds;
    }

    public void ToggleTemporal()
    {
        clouds.temporalAntiAliasing = !clouds.temporalAntiAliasing;
        TemporalReprojection c = GameObject.Find("Main Camera").GetComponent<TemporalReprojection>();
        FrustumJitter f = GameObject.Find("Main Camera").GetComponent<FrustumJitter>();
        c.enabled = !c.enabled;
        f.enabled = !f.enabled;
    }

    public void TogglePreCalculatedWeatherTexture()
    {
        weather.useCustomTexture = !weather.useCustomTexture;
    }

    public void RandomOffsetDropdown(Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 0:
                clouds.randomJitterNoise = CloudScript.RandomJitter.Off;
                break;
            case 1:
                clouds.randomJitterNoise = CloudScript.RandomJitter.Random;
                break;
            case 2:
                clouds.randomJitterNoise = CloudScript.RandomJitter.BlueNoise;
                break;
        }
    }

    public void OnChangeShapeScale(Slider slider)
    {
        clouds.scale = (float)slider.value;
    }

    public void OnChangeDetailScale(Slider slider)
    {
        clouds.detailScale = (float)slider.value;
    }

    public void OnChangeCurlScale(Slider slider)
    {
        clouds.curlDistortScale = (float)slider.value;
    }

    public void OnChangeWeatherScale(Slider slider)
    {
        clouds.weatherScale = (float)slider.value;
    }

    public void OnChangeShapeMin(Slider slider)
    {
        clouds.lowFreqMin = (float)slider.value;
    }

    public void OnChangeShapeMax(Slider slider)
    {
        clouds.lowFreqMax = (float)slider.value;
    }

    public void OnChangeDetailModifier(Slider slider)
    {
        clouds.highFreqModifier = (float)slider.value;
    }

    public void OnChangeCoverage(Slider slider)
    {
        clouds.coverage = (float)slider.value;
    }

    public void OnChangeHighCloudsCoverage(Slider slider)
    {
        clouds.coverageHigh = (float)slider.value;
    }

    public void OnChangeAmbientFactor(Slider slider)
    {
        clouds.ambientLightFactor = (float)slider.value;
    }

    public void OnChangeDirectFactor(Slider slider)
    {
        clouds.sunLightFactor = (float)slider.value;
    }

    public void OnChangeHGForward(Slider slider)
    {
        clouds.henyeyGreensteinGForward = (float)slider.value;
    }

    public void OnChangeHGBackward(Slider slider)
    {
        clouds.henyeyGreensteinGBackward = (float)slider.value;
    }

    public void OnChangeOpticalDensity(Slider slider)
    {
        clouds.density = (float)slider.value;
    }

    public void OnChangeLightStepLength(Slider slider)
    {
        clouds.lightStepLength = (float)slider.value;
    }

    public void OnChangeConeRadius(Slider slider)
    {
        clouds.lightConeRadius = (float)slider.value;
    }

    public void ToggleRandomSphere()
    {
        clouds.randomUnitSphere = !clouds.randomUnitSphere;
    }

    public void ToggleExtraLightSamples()
    {
        clouds.aLotMoreLightSamples = !clouds.aLotMoreLightSamples;
    }

    public void OnChangeNoiseSpeed(Slider slider)
    {
        clouds.windSpeed = (float)slider.value;
    }

    public void OnChangeNoiseDirection(Slider slider)
    {
        clouds.windDirection = (float)slider.value;
    }

    public void OnChangeWeatherSpeed(Slider slider)
    {
        clouds.coverageWindSpeed = (float)slider.value;
    }

    public void OnChangeWeatherDirection(Slider slider)
    {
        clouds.coverageWindDirection = (float)slider.value;
    }

    public void OnChangeHighCloudsSpeed(Slider slider)
    {
        clouds.highCloudsWindSpeed = (float)slider.value;
    }

    public void OnChangeHighCloudsDirection(Slider slider)
    {
        clouds.highCloudsWindDirection = (float)slider.value;
    }

    public void OnChangeGlobalMultiplier(Slider slider)
    {
        clouds.globalMultiplier = (float)slider.value;
    }

    public void OnChangeWeatherChangeDuration(Slider slider)
    {
        weather.blendTime = (float)slider.value;
    }
    /*
    public void OnChange(Slider slider)
    {
        clouds. = (float) slider.value;
    }
    */

    // Use this for initialization
    void Start () {
        SelectPanel(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitButton.SetActive(!quitButton.activeSelf);
        }

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + fps;
    }
}
