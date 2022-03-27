using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    [Serializable]
    public class SimplifiedPacejkaParameters
    {
        [Range(4f, 12f), Tooltip("Stiffness. Typical range: 4f-12f")]
        public float Stiffness = 10f;
        [Range(1f, 2f), Tooltip("Shape. Typical range: 1f-2f")]
        public float Shape = 1.5f;
        [Range(0.1f, 1.9f), Tooltip("Peak. Typical range: 0.1f-1.9f, 1.65f for the longitudinal force and 1.3f for the lateral force")]
        public float Peak = 1f;
        [Range(-10f, 1f), Tooltip("Curvature. Typical range: -10f-1f")]
        public float Curvature = 0.97f;

        /*public SimplifiedPacejkaParameters(float stiffness = 10f, float shape = 1.9f, float peak = 1f, float curvature = 0.97f)
        {
            Stiffness = stiffness;
            Shape = shape;
            Peak = peak;
            Curvature = curvature;
        }*/
    }
    
    [CreateAssetMenu(menuName = MENU_NAME + MODEL_NAME, fileName = MODEL_NAME + " " + FILE_NAME)]
    public class SimplifiedPacejkaFrictionModel : AbstractFrictionModel
    {
        [Header(MODEL_NAME)]
        public SimplifiedPacejkaParameters LongitudinalParameters;
        [Space]
        public SimplifiedPacejkaParameters LateralParameters;
        
        protected const string MODEL_NAME = "Simplified Pacejka";

        public override void GetLongitudinal(CustomWheel wheel, float deltaTime, out float longitudinal)
        {
            longitudinal = SimplifiedFormula(
                wheel.GroundData.Hit.force,
                wheel.SlipRatio,
                LateralParameters.Stiffness,
                LateralParameters.Shape,
                LateralParameters.Peak,
                LateralParameters.Curvature);
        }

        public override void GetLateral(CustomWheel wheel, float deltaTime, out float lateral)
        {
            // Slip angle normalized to -1 to 1f.
            var slip = wheel.SlipAngle / 90f;
            lateral = -SimplifiedFormula(
                wheel.GroundData.Hit.force,
                slip,
                LateralParameters.Stiffness,
                LateralParameters.Shape,
                LateralParameters.Peak,
                LateralParameters.Curvature);
        }
        
        /// <summary>
        /// Simplified Magic Formula from https://www.edy.es/dev/docs/pacejka-94-parameters-explained-a-comprehensive-guide/.
        /// Used for both longitudinal and lateral forces.
        /// </summary>
        /// <param name="Fz">vertical force in N (newtons)</param>
        /// <param name="slip">slip ratio in percentage (0..100, for longitudinal force) or slip angle in degrees (for lateral force)</param>
        /// <param name="B">Stiffness. Typical range: 4f-12f</param>
        /// <param name="C">Shape. Typical range: 1f-2f</param>
        /// <param name="D">Peak. Typical range: 0.1f-1.9f</param>
        /// <param name="E">Curvature. Typical range: -10f-1f</param>
        /// <returns>Friction Force</returns>
        public static float SimplifiedFormula(float Fz, float slip, float B = 10f, float C = 1.5f, float D = 1f, float E = 0.97f)
        {
            //Constants:
            //B = Stiffness
            //C = Shape, The Pacekja model specifies the shape as C=1.65 for the longitudinal force and C=1.3 for the lateral force.
            //D = Peak
            //E = Curvature

            //Variables
            //F = tire force in N (newtons)
            //Fz = vertical force in N (newtons)
            //slip = slip ratio in percentage (0..100, for longitudinal force) or slip angle in degrees (for lateral force)
            
            //Formula
            //F = Fz · D · sin(C · arctan(B·slip – E · (B·slip – arctan(B·slip))))

            return Fz * D * Mathf.Sin(C * Mathf.Atan(B * slip - E * (B * slip - Mathf.Atan(B * slip))));
        }
    }
}