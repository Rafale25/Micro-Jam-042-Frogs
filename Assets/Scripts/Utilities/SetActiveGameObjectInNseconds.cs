using System.Collections;
using UnityEngine;

public class SetActiveGameObjectInNseconds : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private float _seconds;
    [SerializeField] private bool _active;

    void Start()
    {
        StartCoroutine(SetActiveInNseconds(_gameObject, _seconds, _active));
    }


    IEnumerator SetActiveInNseconds(GameObject obj, float seconds, bool active)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(active);
    }
}
