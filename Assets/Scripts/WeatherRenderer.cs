using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WeatherRenderer : MonoBehaviour
{
    public CloudsScript clouds;
    public int size = 512;
    public bool useCustomTexture = false;
    public Texture2D customWeatherTexture;
    public GameObject weatherVisualiser;
    public float blendTime = 30f;

    private MeshRenderer weatherVisualiserRenderer;

    private RenderTexture rt;
    private bool isChangingWeather = false;

    private RenderTexture prevWeatherTexture;
    private RenderTexture nextWeatherTexture;

    private bool _useUserWeatherTexture;

    public bool useCustomWeatherTexture
    {
        get { return _useUserWeatherTexture; }
        set
        {
            if (value == _useUserWeatherTexture)
                return;

            _useUserWeatherTexture = value;
            if (_useUserWeatherTexture && customWeatherTexture != null)
            {
                Graphics.Blit(rt, prevWeatherTexture);
                Graphics.Blit(customWeatherTexture, nextWeatherTexture);
                startWeatherTextureChange();
            } else
            {
                GenerateAndChangeWeatherTexture();
            }
        }
    }

    public Material BlendMaterial
    {
        get
        {
            if (!_BlendMaterial)
            {
                _BlendMaterial = new Material(Shader.Find("Hidden/WeatherBlender"));
                _BlendMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _BlendMaterial;
        }
    }
    private Material _BlendMaterial;

    public Material SystemMaterial
    {
        get
        {
            if (!_SystemMaterial)
            {
                _SystemMaterial = new Material(Shader.Find("Hidden/WeatherSystem"));
                _SystemMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _SystemMaterial;
        }
    }
    private Material _SystemMaterial;

    public void Awake()
    {
        weatherVisualiserRenderer = weatherVisualiser.GetComponent<MeshRenderer>();
    }

    private void setWeatherTexture()
    {
        clouds.CloudMaterial.SetTexture("_WeatherTexture", rt);
        weatherVisualiserRenderer.sharedMaterial.SetTexture("_MainTex", rt);
    }

    private void startWeatherTextureChange()
    {
        if (isChangingWeather)
        {
            StopCoroutine("LerpWeatherTexture");
        }
        StartCoroutine("LerpWeatherTexture");
    }

    public void GenerateWeatherTexture()
    {
        SystemMaterial.SetVector("_Randomness", new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.value * 1.5f - 0.2f));
        Graphics.Blit(rt, prevWeatherTexture);
        Graphics.Blit(null, rt, SystemMaterial, 0);
        Graphics.Blit(rt, nextWeatherTexture);
    }

    public void GenerateAndChangeWeatherTexture()
    {
        GenerateWeatherTexture();
        if (!useCustomWeatherTexture)
        {
            startWeatherTextureChange();
        }
    }

    IEnumerator LerpWeatherTexture()
    {
        isChangingWeather = true;
        for (float t = 0f; t <= blendTime; t += Time.deltaTime * (clouds.globalMultiplier == 0.0 ? blendTime : clouds.globalMultiplier))
        {
            BlendMaterial.SetTexture("_PrevWeather", prevWeatherTexture);
            BlendMaterial.SetTexture("_NextWeather", nextWeatherTexture);
            BlendMaterial.SetFloat("_Alpha", t / blendTime);
            Graphics.Blit(null, rt, BlendMaterial, 0);
            setWeatherTexture();
            yield return null;
        }
        Graphics.Blit(nextWeatherTexture, rt);
        setWeatherTexture();
        isChangingWeather = false;
    }

    void Start()
    {
        if (_BlendMaterial)
            DestroyImmediate(_BlendMaterial);
        if (_SystemMaterial)
            DestroyImmediate(_SystemMaterial);

        rt = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        prevWeatherTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        prevWeatherTexture.Create();
        nextWeatherTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        nextWeatherTexture.Create();

        clouds.CloudMaterial.SetTexture("_WeatherTexture", rt);

        useCustomWeatherTexture = useCustomTexture;
        if (useCustomWeatherTexture && customWeatherTexture != null)
        {
            if (isChangingWeather)
            {
                StopCoroutine("LerpWeatherTexture");
            }
            Graphics.Blit(rt, prevWeatherTexture);
            Graphics.Blit(customWeatherTexture, nextWeatherTexture);
            setWeatherTexture();
        }
        else
        {
            GenerateWeatherTexture();
            setWeatherTexture();
        }
    }

    void Update()
    {
        useCustomWeatherTexture = useCustomTexture;
    }
}
