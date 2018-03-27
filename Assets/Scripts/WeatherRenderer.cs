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
    private GameObject quad;
    private MeshRenderer quadRenderer;
    private Camera cam;
    private bool isChangingWeather = false;

    private Texture2D randomWeatherTexture;

    private Texture2D prevWeatherTexture;
    private Texture2D nextWeatherTexture;

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
                
                startWeatherTextureChange(getCustomTextureInRightFormat());
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

    public void Awake()
    {
        cam = this.GetComponent<Camera>();
        quad = this.transform.Find("Quad").gameObject;
        quadRenderer = quad.GetComponent<MeshRenderer>();
        weatherVisualiserRenderer = weatherVisualiser.GetComponent<MeshRenderer>();
    }

    private Texture2D getCustomTextureInRightFormat()
    {
        BlendMaterial.SetTexture("_NextWeather", customWeatherTexture);
        BlendMaterial.SetFloat("_Alpha", 1f);
        Graphics.Blit(null, rt, BlendMaterial, 0);
        return GetTexture2D(rt);
    }

    private Texture2D GetTexture2D(RenderTexture rt)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        RenderTexture.active = currentActiveRT;
        tex.Apply();
        return tex;
    }

    private void setWeatherTexture(Texture2D texture)
    {
        if (clouds.weatherTexture.width != texture.width)
        {
            DestroyImmediate(clouds.weatherTexture);
            clouds.weatherTexture = new Texture2D(texture.width, texture.height);
        }
        Graphics.CopyTexture(texture, clouds.weatherTexture);
        clouds.weatherTexture.wrapMode = TextureWrapMode.Mirror;
        DestroyImmediate(texture);
        weatherVisualiserRenderer.sharedMaterial.SetTexture("_MainTex", clouds.weatherTexture);
    }

    private void startWeatherTextureChange(Texture2D texture)
    {
        if (prevWeatherTexture == null)
        {
            prevWeatherTexture = new Texture2D(size, size);
        }
        Graphics.CopyTexture(clouds.weatherTexture, prevWeatherTexture);
        nextWeatherTexture = texture;
        if (isChangingWeather)
        {
            StopCoroutine("LerpWeatherTexture");
        }
        StartCoroutine("LerpWeatherTexture");
    }

    public void GenerateWeatherTexture()
    {
        quadRenderer.sharedMaterial.SetVector("_Randomness", new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.value * 1.5f - 0.2f));
        DestroyImmediate(randomWeatherTexture);
        randomWeatherTexture = GetTexture2D(rt);
    }

    public void GenerateAndChangeWeatherTexture()
    {
        GenerateWeatherTexture();
        if (!useCustomWeatherTexture)
        {
            startWeatherTextureChange(randomWeatherTexture);
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
            setWeatherTexture(GetTexture2D(rt));
            yield return null;
        }
        if (nextWeatherTexture != null)
            setWeatherTexture(nextWeatherTexture);
        isChangingWeather = false;
    }

    void Start()
    {
        if (_BlendMaterial)
            DestroyImmediate(_BlendMaterial);
        if (clouds.weatherTexture != null)
        {
            DestroyImmediate(clouds.weatherTexture);
            clouds.weatherTexture = new Texture2D(size, size);
        }

        rt = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        rt.Create();
        cam.targetTexture = rt;
        cam.Render();

        useCustomWeatherTexture = useCustomTexture;
        if (useCustomWeatherTexture && customWeatherTexture != null)
        {
            if (isChangingWeather)
            {
                StopCoroutine("LerpWeatherTexture");
            }
            setWeatherTexture(getCustomTextureInRightFormat());
        }
        else
        {
            GenerateWeatherTexture();
            setWeatherTexture(randomWeatherTexture);
        }
    }

    void Update()
    {
        useCustomWeatherTexture = useCustomTexture;
    }
}
