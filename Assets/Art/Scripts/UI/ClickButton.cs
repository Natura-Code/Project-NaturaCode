using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _deafult, _pressed;
    [SerializeField] private AudioClip _clikc, _unClikc;
    [SerializeField] private AudioSource _source;
    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
        _source.PlayOneShot(_clikc);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _deafult;
        _source.PlayOneShot(_unClikc);
    }

    public void iWasClicked()
    {
        Debug.Log("Button was clicked!");
    }

    public void playGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void quitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }

}
