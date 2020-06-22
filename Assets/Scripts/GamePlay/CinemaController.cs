using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GamePlay;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CinemaController : MonoBehaviour,MMEventListener<MMCameraEvent>, MMEventListener<TopDownEngineEvent>
{
    public bool FollowsPlayer { get; set; }

    public bool FollowsAPlayer = true;
    public bool ConfineCameraToLevelBounds = true;
    [MMReadOnly]
    public Character TargetCharacter;

    protected CinemachineVirtualCamera _virtualCamera;
    protected CinemachineConfiner _confiner;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confiner = GetComponent<CinemachineConfiner>();
    }

    private void Start()
    {
        if (_confiner != null && ConfineCameraToLevelBounds)
        {
            _confiner.m_BoundingVolume = LevelManagerGamePLay.Instance.BoundsCollider;
        }
    }
     public virtual void SetTarget(Character character)
        {
            TargetCharacter = character;
        }

        /// <summary>
        /// Starts following the LevelManager's main player
        /// </summary>
        public virtual void StartFollowing()
        {
            if (!FollowsAPlayer) { return; }
            FollowsPlayer = true;
            _virtualCamera.Follow = TargetCharacter.CameraTarget.transform;
            _virtualCamera.LookAt = TargetCharacter.CameraTarget.transform;
        }

        /// <summary>
        /// Stops following any target
        /// </summary>
        public virtual void StopFollowing()
        {
            if (!FollowsAPlayer) { return; }
            FollowsPlayer = false;
            _virtualCamera.Follow = null;
        }

        public virtual void OnMMEvent(MMCameraEvent cameraEvent)
        {
            switch (cameraEvent.EventType)
            {
                case MMCameraEventTypes.SetTargetCharacter:
                    SetTarget(cameraEvent.TargetCharacter);
                    break;
                case MMCameraEventTypes.SetConfiner:                    
                    if (_confiner != null)
                    {
                        _confiner.m_BoundingVolume = cameraEvent.Bounds;
                    }
                    break;
                case MMCameraEventTypes.StartFollowing:
                    if (cameraEvent.TargetCharacter != null)
                    {
                        if (cameraEvent.TargetCharacter != TargetCharacter)
                        {
                            return;
                        }
                    }
                    StartFollowing();
                    break;

                case MMCameraEventTypes.StopFollowing:
                    if (cameraEvent.TargetCharacter != null)
                    {
                        if (cameraEvent.TargetCharacter != TargetCharacter)
                        {
                            return;
                        }
                    }
                    StopFollowing();
                    break;
            }
        }

        public virtual void OnMMEvent(TopDownEngineEvent topdownEngineEvent)
        {
            if (topdownEngineEvent.EventType == TopDownEngineEventTypes.CharacterSwitch)
            {
                SetTarget(LevelManager.Instance.Players[0]);
                StartFollowing();
            }

            if (topdownEngineEvent.EventType == TopDownEngineEventTypes.CharacterSwap)
            {
                SetTarget(LevelManager.Instance.Players[0]);
                StartFollowing();
            }
        }

        protected virtual void OnEnable()
        {
            this.MMEventStartListening<MMCameraEvent>();
            this.MMEventStartListening<TopDownEngineEvent>();
        }

        protected virtual void OnDisable()
        {
            this.MMEventStopListening<MMCameraEvent>();
            this.MMEventStopListening<TopDownEngineEvent>();
        }
    
}
