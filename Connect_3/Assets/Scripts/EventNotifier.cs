using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotifier : MonoBehaviour
{
    [SerializeField] GameEndEventBus _gameWon;
    [SerializeField] GameEndEventBus _gameLost;
}
