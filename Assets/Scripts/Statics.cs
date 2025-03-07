using UnityEngine;

namespace MatchThreePrototype
{

    public class Statics
    {

        public static float DEFAULT_REMOVE_DURATION = .42f;

        public static float DEFAULT_MOVE_SPEED = 2000f;

        public static float LIFT_LOWER_STEP = 5f;//7f;
        public static Vector2 SCALE_HELD = new Vector3(1.45f, 1.45f, 1.45f);

        public static float ALPHA_ON = 1;
        public static float BLOCK_ALPHA_ON = .70f;//.65f; //.50f; //.65f;

        public static float HELD_ALPHA_ON = .70f;

        public static float ALPHA_OFF = 0;

        public static string PLAY_AREA_RECT = "PLAY_AREA_RECT";
        public static string PLAY_AREA_COLLIDER = "PLAY_AREA_COLLIDER";

        public static string PERCENT = "%";

        public static string METERS = "m";

        public static string LAYER_GUN_TARGET = "WeaponTarget";

        public static float Interpolate(float x, float x1, float x2, float y1, float y2)
        {
            return y1 + ((x - x1) / (x2 - x1) * (y2 - y1));
        }


        public static double LerpDouble(double a, double b, float t)
        {
            return (a + (b - a)* Mathf.Clamp01(t));
        }

        public static bool IsCloseEnough(Vector3 positionDestination, Vector3 positionMoving, float tolerance)
        {
            //float tolerance = .01f;

            float manhattanDistance = Mathf.Abs(positionDestination.x - positionMoving.x) + Mathf.Abs(positionDestination.y - positionMoving.y + Mathf.Abs(positionDestination.z - positionMoving.z));

            if (manhattanDistance < tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsCloseEnough(Vector2 positionDestination, Vector2 positionMoving)
        {
            float tolerance = .01f;

            float manhattanDistance = Mathf.Abs(positionDestination.x - positionMoving.x) + Mathf.Abs(positionDestination.y - positionMoving.y); //+ Mathf.Abs(positionDestination.z - positionMoving.z);

            if (manhattanDistance < tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Vector3 Vector3Zero()
        {
            return StaticVector3Zero;
        }
        private static Vector3 StaticVector3Zero = new Vector3(0, 0, 0);

        public static Vector2 Vector2Zero()
        {
            return StaticVector2Zero;
        }
        private static Vector2 StaticVector2Zero = new Vector2(0, 0);

        public static Vector3 Vector3One()
        {
            return StaticVector3One;
        }
        private static Vector3 StaticVector3One = new Vector3(1, 1, 1);




    }

}
