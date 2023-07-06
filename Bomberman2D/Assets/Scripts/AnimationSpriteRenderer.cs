using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] animationSprites;

    [SerializeField] private float animationRate = 0.25f;
    
    private int animationIndex;

    public bool isIdle = true;
    public bool isLoop = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateFrame), animationRate, animationRate);
    }

    void AnimateFrame()
    {
        //increment the index
        animationIndex++;

        //if looping and index has reached max length of sprites, reset it.
        if (isLoop && animationIndex >= animationSprites.Length)
            animationIndex = 0;

        if (isIdle)
            spriteRenderer.sprite = idleSprite;
        else if (animationIndex >= 0 && animationIndex < animationSprites.Length) //just a secure check to avoid array out of bounds
            spriteRenderer.sprite = animationSprites[animationIndex];
    }
}
