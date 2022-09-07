using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum MovementType
{
    NULL,
    SWAP,
    DOWNWARDS,
    INITIAL,
    CHANGE
}
public class VisualTile : MonoBehaviour
{
    public Vector2 BoardPos;
    Vector2 original_world_pos;
    public TileType Type;
    float _tileSize;
    Sequence _sequence;
    ParticleSystem particles;
    bool destroyed = false;

    private void Awake()
    {
        _sequence = DOTween.Sequence(this);
        particles = GetComponent<ParticleSystem>();
        original_world_pos = GameObject.FindGameObjectWithTag("Controller").transform.position;
    }
    public void InitializeTile(Vector2 pos, float tileSize,MovementType movement = MovementType.INITIAL)
    {
        _tileSize = tileSize;
        SetBoardPosition(pos, movement);
    }
    public void SetBoardPosition(Vector2 pos,MovementType movementType)
    {
        if (!destroyed)
        {
            BoardPos = pos;
            switch (movementType)
            {
                case MovementType.NULL:
                    break;
                case MovementType.SWAP:
                    SwapMovement();
                    break;
                case MovementType.DOWNWARDS:
                    MoveDownwards();
                    break;
                case MovementType.INITIAL:
                    SetInitialWorldPosition();
                    break;
                case MovementType.CHANGE:
                    ChangeTileMovement();
                    break;
                default:
                    break;
            }
        }
    }

    public void MoveDownwards()
    {

        float y = original_world_pos.y + BoardPos.y * _tileSize;
        float x = original_world_pos.x + BoardPos.x * _tileSize;
        Vector3 movement = new Vector3(x, y, 0);
        _sequence.Append(transform.DOMove(movement, 0.3f));
    }

    public void SetInitialWorldPosition()
    {
        float y = original_world_pos.y + BoardPos.y * _tileSize;
        float x = original_world_pos.x + BoardPos.x * _tileSize;
        Vector3 initial_movement = new Vector3(x, y+10, 0);
        transform.position = initial_movement;
        _sequence.Append(transform.DOMoveY(y, 1));
    }

    public void SwapMovement()
    {
        float y = original_world_pos.y + BoardPos.y * _tileSize;
        float x = original_world_pos.x + BoardPos.x * _tileSize;
        Vector3 movement = new Vector3(x, y, 0);
        _sequence.Append(transform.DOMove(movement, 0.3f));
    }
    public void ChangeTileMovement()
    {
        float y = original_world_pos.y + BoardPos.y * _tileSize;
        float x = original_world_pos.x + BoardPos.x * _tileSize;
        transform.position = new Vector3(x, y, 0);
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        _sequence.Append(transform.DOScale(scale, 0.5f));
    }

    public void DestroyVisualTile(bool play_particles)
    {
        destroyed = true;
        _sequence.Kill(false);
        GetComponent<SpriteRenderer>().enabled = false;
        if (play_particles)
        {
            particles.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
