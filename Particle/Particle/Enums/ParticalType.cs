
using System.ComponentModel;

namespace Particle.Enums
{
    public enum ParticleType
    {
        [Description("Rocket")]
        Rocket,
        [Description("Shooting Star")]
        ShootingStar,
        [Description("Circle")]
        Circle,
        [Description("Infinity")]        
        Infinity,
        [Description("Sinkansen")]
        Sinkansen,
        [Description("Blast")]
        Blast,
        [Description("Center Flower")]
        CenterFlower,
        [Description("Left to Right Curve")]
        LefttoRightCurve,
        [Description("Buttom to Top Curve")]
        ButtomtoTopCurve
    }
}
