using UnityEngine;
using UnityEngine.UI;

namespace Draw
{
    public class DrawController : MonoBehaviour
    {
        [SerializeField] private RawImage drawImage;
        [SerializeField] private InputData inputData;
        [SerializeField] private Color color;

        private Texture2D texture;

        private void Awake()
        {
            Init();
            inputData.InputEvent += Drawing;
        }

        private void Init()
        {
            texture = new Texture2D(Screen.width, Screen.height);
            drawImage.texture = texture;
        }

        private void Drawing(Vector2 point)
        {
            texture.SetPixel((int)point.x, (int)point.y, color);
            texture.Apply();
        }
    }
}