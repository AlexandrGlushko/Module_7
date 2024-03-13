using UnityEngine;
using UnityEngine.UI;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] Image _gameObject;
    [SerializeField] Sprite _spriteOn;
    [SerializeField] Sprite _spriteOff;
    private Sprite currentSprite;
    // Start is called before the first frame update
    void Start()
    {
        currentSprite = _spriteOn;
    }

    public void Flip()
    {
        if (_gameObject.sprite == currentSprite)
        {
            _gameObject.sprite = _spriteOff;
        }
        else
        {
            _gameObject.sprite = _spriteOn;
        }
    }
}
