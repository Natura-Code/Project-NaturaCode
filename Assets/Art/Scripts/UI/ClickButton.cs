using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _pressed, _unPressed;
    [SerializeField] private AudioClip _audioPressed, _audioUnPressed;
    [SerializeField] private AudioSource _source;
    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
        _source.PlayOneShot(_audioPressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _unPressed;
        _source.PlayOneShot(_audioUnPressed);
    }

    public void iWasClicked()
    {

    }
}

