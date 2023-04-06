using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class NoiseGeneration : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private int width = 256;

    [SerializeField] 
    private int height = 256;

    [SerializeField]
    private float scale = 30f;

    [SerializeField]
    private float offsetX;

    [SerializeField]
    private float offsetY;

    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private string name = "Noise1";

    private Renderer renderer;

    private Player_Controls controls;

    private Texture2D currentTex;

    private void Awake()
    {
        renderer= GetComponent<Renderer>();

        offsetX = Random.Range(0, 999999);
        offsetY = Random.Range(0, 999999);

        controls = new();

        controls.Player1.RightSpell.performed += CaptureScreenShot;

        controls.Player1.Enable();
    }

    private void Update()
    {
        if(isActive)
        {
            currentTex = NewNoiseTexture();

            renderer.material.mainTexture = currentTex;
        }
    }

    //Creates new texture and goes through each pixel giving it a random value.
    private Texture2D NewNoiseTexture()
    {
        Texture2D texture = new(width, height, TextureFormat.RGB24, false);

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Color color = NoiseColor(x, y);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        return texture;
    }

    //Creates color using perlin noise.
    private Color NoiseColor(int x, int y)
    {
        float xPerlin = (float)x / width * scale + offsetX;
        float yPerlin = (float)y / height * scale + offsetY;

        float perlinNoise = Mathf.PerlinNoise(xPerlin, yPerlin);

        Color color = new(perlinNoise, perlinNoise, perlinNoise);

        return color;
    }

    private void CaptureScreenShot(InputAction.CallbackContext context)
    {
        byte[] bytes = currentTex.EncodeToPNG();

        var directionPath = Application.dataPath + "/Images/NoiseImages/";

        if(!Directory.Exists(directionPath))
        {
            Directory.CreateDirectory(directionPath);
        }

        File.WriteAllBytes(directionPath + name + ".png", bytes);

        Debug.Log("Done with saving png!");
    }
}
