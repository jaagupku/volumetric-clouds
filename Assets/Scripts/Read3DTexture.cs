using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Read3DTexture : MonoBehaviour {

    public Texture2D cloudShapeSlices;
    public Texture2D cloudErasionSlices;
    Material material;
    Texture3D cloudShapeTexture;
    Texture3D cloudErasionTexture;
    float timeCounter = 0;
    Vector3 startPosition;

    // https://support.unity3d.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-
    Texture2D getReadableTexture(Texture2D texture)
    {
        // Create a temporary RenderTexture of the same size as the texture
        RenderTexture tmp = RenderTexture.GetTemporary(
                            texture.width,
                            texture.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

        // Blit the pixels on texture to the RenderTexture
        Graphics.Blit(texture, tmp);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmp;
        Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
        myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTexture2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmp);
        return myTexture2D;
    }

    Texture3D createTexture3DFrom2DSlices(Texture2D tex2d, int size)
    {
        Texture2D readableTexture2D = getReadableTexture(tex2d);

        Color[] colors = new Color[size * size * size];
        int idx = 0;
        for (int z = 0; z < size; ++z)
        {
            for (int y = 0; y < size; ++y)
            {
                for (int x = 0; x < size; ++x, ++idx)
                {
                    colors[idx] = readableTexture2D.GetPixel(x + z * size, y);
                }
            }
        }

        Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.ARGB32, true);
        texture3D.SetPixels(colors);
        texture3D.Apply();
        return texture3D;
    }

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
    
        cloudShapeTexture = createTexture3DFrom2DSlices(cloudShapeSlices, 128);
        cloudErasionTexture = createTexture3DFrom2DSlices(cloudErasionSlices, 32);

        material.SetTexture("_CloudShapeTexture", cloudShapeTexture);
        material.SetTexture("_CloudErasionTexture", cloudErasionTexture);

        startPosition = transform.position;
	}

    // Update is called once per frame
    void Update () {
        /*
        timeCounter += Time.deltaTime;

        float x = Mathf.Cos(timeCounter);
        float y = 0;
        float z = Mathf.Sin(timeCounter);

        transform.position = new Vector3(x + startPosition.x, y + startPosition.y, z + startPosition.z);
	    */
    }
}
