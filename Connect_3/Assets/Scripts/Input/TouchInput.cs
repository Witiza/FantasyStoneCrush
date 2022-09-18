using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT,
    LEFT,
    UP,
    DOWN
}
public class TouchInput : MonoBehaviour,IGameplayInput
{
    public GameEndEventBus GameWon;
    public GameEndEventBus GameLost;
    public EventBus MovesAdded;
    private bool _inputEnabled= true;
    private bool _gameEnded = false;
    public Vector2 _initialTouch { get; set; }
    [field: SerializeField]
    public float SwapDistance { get; set; }

    public event Action<Vector2> StartTouch;
    public void NotifyTouch(Vector2 pos) =>StartTouch?.Invoke(pos);

    public event Action<Direction> SwapTouch;
    public void NotifySwapTouch(Direction dir) =>SwapTouch?.Invoke(dir);    

    public event Action EndTouch;
    public void NotifyEndTouch() =>EndTouch?.Invoke();  

    private void MovesAddedEvent()
    {
        _inputEnabled = true;
        _gameEnded = false;
    }

    private void GameEndedEvent(GameEndInfo info)
    {
        _inputEnabled = false;
        _gameEnded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 &&_inputEnabled)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _initialTouch = touch.position;
                    NotifyTouch(touch.position);
                    break;
                case TouchPhase.Moved:
                    { 
                        Vector2 diff = touch.position - _initialTouch;
                        if ((diff).magnitude > SwapDistance)
                        {
                            if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                            {
                                if (diff.y > 0)
                                {
                                    NotifySwapTouch(Direction.UP);
                                }
                                else
                                {
                                    NotifySwapTouch(Direction.DOWN);
                                }
                            }
                            else
                            {
                                if (diff.x > 0)
                                {
                                    NotifySwapTouch(Direction.RIGHT);
                                }
                                else
                                {
                                    NotifySwapTouch(Direction.LEFT);
                                }
                            }
                        }
                    }
                    break;
                case TouchPhase.Canceled: case TouchPhase.Ended:
                    NotifyEndTouch();
                    break;
                default:
                    break;
            }

        }
    }

    void Awake()
    {
        GameWon.Event += GameEndedEvent;
        GameLost.Event += GameEndedEvent;
        MovesAdded.Event += MovesAddedEvent;
    }

    void OnDestroy()
    {
        GameWon.Event -= GameEndedEvent;
        GameLost.Event -= GameEndedEvent;
        MovesAdded.Event -= MovesAddedEvent;
    }

    public IEnumerator CoroutineTurnWait()
    {
        _inputEnabled = false;
        yield return new WaitForSeconds(1.3f);
        if(!_gameEnded)
            _inputEnabled = true;
    }
}
