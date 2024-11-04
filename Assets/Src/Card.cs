using System;
using DG.Tweening;
using UnityEngine;
using UniRx;
using Zenject;

/// <summary>
/// Represents a card in the game, handling its visual state, interactions, and animations.
/// </summary>
public class Card : MonoBehaviour, IDisposable
{
    private ControlManager _controlManager;
    
    // The 3D model of the card.
    [SerializeField]
    private GameObject model;
    
    [SerializeField]
    private ParticleSystem matchedParticleSystem;
    
    // Sprites for the front and back faces of the card.
    public Sprite frontSprite;
    public Sprite backSprite;
    
    // Flags to track the card's flipped and matched states.
    private bool _isFlipped;
    private bool _isMatched;
    
    // Observable properties to react to card state changes.
    public ReactiveProperty<bool> isFlipped = new(false);
    public readonly Subject<Card> OnCardFlipped = new();
    
    // Materials for the front and back faces of the card.
    private Material _frontMaterial;
    private Material _backMaterial;
    
    // Unique identifier for the card.
    public int CardId { get; set; }

    [Inject]
    public void Construct(ControlManager controlManager)
    {
        _controlManager = controlManager;
    }
    
    /// <summary>
    /// Initializes the card by setting up reactive subscriptions and assigning textures to materials.
    /// </summary>
    private void Start()
    {
        // Subscribe to the isFlipped property to notify when the card is flipped.
        isFlipped.Subscribe(flip =>
        {
            if (flip)
            {
                OnCardFlipped.OnNext(this);
            }
        }).AddTo(this);

        // Assign textures to the front and back materials of the card.
        _frontMaterial = model.GetComponent<MeshRenderer>().materials[2];
        _frontMaterial.mainTexture = frontSprite.texture;

        if (_backMaterial)
        {
            _backMaterial = model.GetComponent<MeshRenderer>().materials[1];
            _backMaterial.mainTexture = backSprite.texture;
        }
    }

    /// <summary>
    /// Handles the click event on the card, flipping it if conditions are met.
    /// </summary>
    public void OnClick()
    {
        // Check if the card can be flipped based on its state and the game's control state.
        if (_isFlipped || _isMatched || !_controlManager.CanFlip)
        {
            return;
        }
        Flip();
    }

    /// <summary>
    /// Flips the card, changing its visual state and triggering the flip animation.
    /// </summary>
    public void Flip()
    {
        // Toggle the flipped state and update the observable property.
        _isFlipped = !_isFlipped;
        isFlipped.Value = _isFlipped;

        // Perform the flip animation.
        transform.DORotate(new Vector3(0, _isFlipped ? 180 : 0, 0), 0.5f)
            .SetEase(Ease.OutBack);
    }

    /// <summary>
    /// Marks the card as matched, changing its state to prevent further interactions.
    /// </summary>
    public void SetMatched()
    {
        _isMatched = true;
        matchedParticleSystem.Play();
    }
    
    /// <summary>
    /// Sets the sprite for the front face of the card.
    /// </summary>
    /// <param name="sprite">The new front sprite.</param>
    public void SetFrontSprite(Sprite sprite)
    {
        frontSprite = sprite;
    }
    
    /// <summary>
    /// Sets the sprite for the back face of the card.
    /// </summary>
    /// <param name="sprite">The new back sprite.</param>
    public void SetBackSprite(Sprite sprite)
    {
        backSprite = sprite;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}