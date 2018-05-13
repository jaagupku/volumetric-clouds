using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WeatherScript : MonoBehaviour
{
    public CloudScript clouds;
    public int size = 512;
    public bool useCustomTexture = false;
    public Texture2D customWeatherTexture;
    public GameObject weatherVisualiser;
    public float blendTime = 30f;

    private MeshRenderer weatherVisualiserRenderer;

    private RenderTexture rt; // weather texture at the moment
    private bool isChangingWeather = false;

    private RenderTexture prevWeatherTexture; // previous weather texture
    private RenderTexture nextWeatherTexture; // next weather texture

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
                StartWeatherTextureChange();
            }
            else
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

    // sets weather textures on clouds and visualiser object
    private void setWeatherTexture()
    {
        clouds.CloudMaterial.SetTexture("_WeatherTexture", rt);
        weatherVisualiserRenderer.sharedMaterial.SetTexture("_MainTex", rt);
    }

    // starts weather texture change routine
    public void StartWeatherTextureChange()
    {
        if (isChangingWeather)
        {
            StopCoroutine("LerpWeatherTexture");
        }
        StartCoroutine("LerpWeatherTexture");
    }

    // generates new weather texture
    public void GenerateWeatherTexture()
    {
        SystemMaterial.SetVector("_Randomness", new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.value * 1.5f - 0.2f));
        Graphics.Blit(rt, prevWeatherTexture);
        Graphics.Blit(null, rt, SystemMaterial, 0);
        Graphics.Blit(rt, nextWeatherTexture);
    }

    // calls StartWeatherTextureChange() and GenerateWeatherTexture()
    public void GenerateAndChangeWeatherTexture()
    {
        GenerateWeatherTexture();
        if (!useCustomWeatherTexture)
        {
            StartWeatherTextureChange();
        }
    }

    // lerps between previous and next weather texture
    IEnumerator LerpWeatherTexture()
    {
        isChangingWeather = true;
        for (float t = 0f; t <= blendTime; t += Time.deltaTime * (clouds.globalMultiplier == 0.0 ? blendTime : Mathf.Abs(clouds.globalMultiplier)))
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
        rt.wrapMode = TextureWrapMode.Mirror;
        rt.Create();

        prevWeatherTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        prevWeatherTexture.wrapMode = TextureWrapMode.Mirror;
        prevWeatherTexture.Create();
        nextWeatherTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        nextWeatherTexture.wrapMode = TextureWrapMode.Mirror;
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