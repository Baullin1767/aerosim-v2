using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private float _updateRate = 1;

    private int _frames;
    
    private void Start()
    {
        //Application.targetFrameRate = 60;
        
        StartCoroutine(nameof(CountFps));
    }

    private IEnumerator CountFps()
    {
        while (true)
        {
            _frames = 0;
            
            for (var elapsed = 0f; elapsed < _updateRate; elapsed += Time.deltaTime)
            {
                yield return null;
                _frames++;
            }

            _text.text = Mathf.RoundToInt(_frames / _updateRate).ToString();
        }
    }
}
