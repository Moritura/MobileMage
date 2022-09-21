using UnityEngine;
using UnityEngine.UI;

namespace Draw
{
    public class ViewLine : MonoBehaviour
    {
        [SerializeField] private RawImage drawImage;
        [SerializeField] private LineController lineController;
        [SerializeField] private Color color;

        private Texture2D texture;

        private void Awake()
        {
            Init();
            lineController.EndLineEvent += Drawing;
        }

        private void Init()
        {
            texture = new Texture2D(Screen.width, Screen.height);
            drawImage.texture = texture;
        }

        private void Drawing(LineData data)
        {
            foreach (var point in data.Points)
            {
                texture.SetPixel(point.x, point.y, color);
            }

            texture.Apply();
        }
    }
}