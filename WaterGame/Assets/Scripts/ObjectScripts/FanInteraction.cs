﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanInteraction : MonoBehaviour,ButtonTriggered
{
    [Header("References")]
    [SerializeField] Transform influenceDirection;
    [SerializeField] private TriggerEvent colTrigger;
    [SerializeField] private AudioSource soundSource;

    [Header("Parameters")]
    [SerializeField] float fanForce = 1.0f;


    [SerializeField] bool isActive;
    [SerializeField] ParticleSystem particles;

    [SerializeField] Animator anim;

    [SerializeField] ButtonObject[] connectedButtons;
    public ButtonObject[] ConnectedButtons { get { return connectedButtons; } }

    // Start is called before the first frame update
    void Start()
    {
        colTrigger.triggerEvent += TriggeredEvent;
        LinkButtons();

        StateChanged();
    }

    private void OnDestroy()
    {
        colTrigger.triggerEvent -= TriggeredEvent;
        UnlinkButtons();
    }

    void TriggeredEvent(TriggerState triggerState, Collider other)
    {
        if (triggerState != TriggerState.Exit)
        {
            if (other.CompareTag("Player") && PlayerState.currentPlayerState == PlayerMatterState.CLOUD)
            {
                if(isActive)
                {
                    PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();

                    Vector3 influenceVec = influenceDirection.transform.forward * fanForce;

                    //Influence movement of the player
                    pm.InfluenceVelocity(influenceVec);
                }
            }
        }

    }


    //Button interface implmentation
    public void ButtonActivated()
    {
        isActive = !isActive;
        StateChanged();
    }

    public void ButtonReleased()
    {
        isActive = !isActive;
        StateChanged();
    }

    public void LinkButtons()
    {
        if (connectedButtons.Length == 0)
        {
            return;
        }

        //Connect delegates
        for (int i = 0; i < connectedButtons.Length; i++)
        {
            connectedButtons[i].buttonActivated += ButtonActivated;
            connectedButtons[i].buttonReleased += ButtonReleased;
        }
    }
    public void UnlinkButtons()
    {
        if (connectedButtons.Length == 0)
        {
            return;
        }
        
        //Disconnect delegates
        for (int i = 0; i < connectedButtons.Length; i++)
        {
            connectedButtons[i].buttonActivated -= ButtonActivated;
            connectedButtons[i].buttonReleased  -= ButtonReleased;
        }
    }

    void StateChanged()
    {
        if (isActive)
        {
            particles.Play();
            anim.SetBool("Active", isActive);
            soundSource.Play();
        }
        else
        {
            particles.Stop();
            anim.SetBool("Active", isActive);
            soundSource.Stop();
        }


    }
}
