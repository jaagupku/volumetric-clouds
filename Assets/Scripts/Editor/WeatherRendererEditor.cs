using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WeatherRenderer))]
public class WeatherRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WeatherRenderer weatherRenderer = (WeatherRenderer)target;
        if (GUILayout.Button("Generate new weather texture"))
        {
            weatherRenderer.GenerateAndChangeWeatherTexture();
        }
    }
}
