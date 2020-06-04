﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIAnimationType
{
    Move,
    Rotate,
    Scale,
    Fade
}

public class UITweener : MonoBehaviour
{
    public GameObject objectToAnimate;

    public UIAnimationType animationType;
    public LeanTweenType easeType;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public bool startOffset;
    public Vector3 from;
    public Vector3 to;

    public bool showOnEnable;

    private LTDescr tweenObject;

    public void OnEnable()
    {
        if(showOnEnable)
        {
            Show();
        }
    }

    public void Show()
    {
        HandleTween();
    }

    public void Disabe()
    {
        if (!gameObject.activeSelf) return;

        if (!pingpong && !loop)
        {
            SwapDirection();
            HandleTween();
        }
        else if(pingpong)
        {
            tweenObject.setLoopPingPong(1);
        }
        else if(loop)
        {
            tweenObject.loopCount = 1;
        }

        tweenObject.setOnComplete(() =>
        {
            SwapDirection();
            LeanTween.cancel(objectToAnimate);
            gameObject.SetActive(false);
        });
    }

    public void SwapDirection()
    {
        var tmp = from;
        from = to;
        to = from;
    }

    private void HandleTween()
    {
        if(objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationType)
        {
            case UIAnimationType.Move:
                Move();
                break;
            case UIAnimationType.Rotate:
                Rotate();
                break;
            case UIAnimationType.Scale:
                Scale();
                break;
            case UIAnimationType.Fade:
                Fade();
                break;
        }

        tweenObject.setDelay(delay);
        tweenObject.setEase(easeType);

        if (loop) tweenObject.loopCount = int.MaxValue; //tweenObject.setLoopType()

        if (pingpong) tweenObject.setLoopPingPong();
    }

    private void Move()
    {
        RectTransform rect = objectToAnimate.GetComponent<RectTransform>();

        if(startOffset)
        {
            rect.anchoredPosition = from;
        }

        tweenObject = LeanTween.move(rect, to, duration);
    }

    private void Rotate()
    {
        RectTransform rect = objectToAnimate.GetComponent<RectTransform>();

        if (startOffset)
        {
            rect.localRotation = Quaternion.Euler(from);
        }

        tweenObject = LeanTween.rotate(rect, to, duration);
    }

    private void Scale()
    {
        RectTransform rect = objectToAnimate.GetComponent<RectTransform>();

        if (startOffset)
        {
            rect.localScale = from;
        }

        tweenObject = LeanTween.scale(rect, to, duration);
    }

    private void Fade()
    {
        CanvasGroup canvasGroup = objectToAnimate.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objectToAnimate.AddComponent<CanvasGroup>();
        }

        if(startOffset)
        {
            canvasGroup.alpha = from.x;
        }

        tweenObject = LeanTween.alphaCanvas(canvasGroup, to.x, duration);
    }
}
