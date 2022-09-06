using UnityEngine;

namespace WebCameraInputSystem.Sample
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _text;
        private int _count = 0;

        private void Awake()
        {
            _text.text = _count.ToString();
        }
        public void Up()
        {
            _count++;
            _text.text = _count.ToString();
        }
        public void Down()
        {
            _count--;
            _text.text = _count.ToString();
        }
    }
}
