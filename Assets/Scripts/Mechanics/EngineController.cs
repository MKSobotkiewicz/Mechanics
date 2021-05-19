using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class EngineController : MonoBehaviour, IController
    {
        public bool NeutralSteering = false;
        public float Acceleration = 10;
        public float MaxSpeed = 10000;
        public Transform LeftTracksPoint;
        public Transform RightTracksPoint;
        public Track TracksPrefab;
        public TankWheel LeftEngineWheel;
        public TankWheel RightEngineWheel;
        private List<Track> tracks=new List<Track>();
        private bool tracksSet = false;

        public void Start()
        {
        }

        public void SetTracks()
        {
            if (tracksSet)
            {
                return;
            }
            tracksSet = true;
            var track = Instantiate(TracksPrefab, LeftTracksPoint);
            track.Side = SideE.Left;
            tracks.Add(track);
            track = Instantiate(TracksPrefab, RightTracksPoint);
            track.Side = SideE.Right;
            tracks.Add(track);
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}
