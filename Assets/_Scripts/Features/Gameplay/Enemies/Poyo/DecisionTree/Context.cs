using UnityEngine;

namespace Poyo
{
    public class Context
    {
        public Transform Self { get; set; }
        public Transform Target { get; set; }
        public LineOfSight Sight { get; set; }
        public float Distance { get; set; }
        public float Angle { get; set; }
        public float AttackRange { get; set; }
        public LayerMask Obstacles { get; set; }
    }
}
