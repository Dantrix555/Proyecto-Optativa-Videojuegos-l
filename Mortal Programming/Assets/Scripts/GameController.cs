using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject _playerX;
    public GameObject _playerZero;
    public Text _readyText;
    public Text _gameOverText;
    void Start()
    {
        _readyText.gameObject.SetActive(true);
        Debug.Log(_playerZero.transform.position);
        Debug.Log(_playerX.transform.position);
        StartCoroutine(ActivatePlayers());
    }

    IEnumerator ActivatePlayers()
    {
        yield return new WaitForSeconds(2);
        _playerX.SetActive(true);
        _playerZero.SetActive(true);
        _readyText.gameObject.SetActive(false);
    }

    public void GameOver(string _winner)
    {
        _gameOverText.gameObject.SetActive(true);
        if(_winner  == "Zero")
        {
            _playerX.SetActive(false);
        }
        else
        {
            _playerZero.SetActive(false);
        }
    }
}
