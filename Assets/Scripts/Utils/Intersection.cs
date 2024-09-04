using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Utils
{
    public static class Intersection
    {
        // Given three colinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        public static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
                q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are colinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        public static int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            // for details of below formula.
            float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);

            if (val == 0) return 0;  // colinear

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        // The main function that returns true if line segment 'p1q1'
        // and 'p2q2' intersect.
        public static bool DoIntersect(Vector2 a, Vector2 b, Vector2 x, Vector2 y)
        {
            // Find the four orientations needed for general and
            // special cases
            int o1 = Orientation(a, b, x);
            int o2 = Orientation(a, b, y);
            int o3 = Orientation(x, y, a);
            int o4 = Orientation(x, y, b);

            if(a == x)
            {
                if (o2 == 0 && OnSegment(a, y, b))
                    return true;
                else
                    return false;
            }

            if (a == y)
            {
                if (o1 == 0 && OnSegment(a, x, b))
                    return true;
                else
                    return false;
            }


            if (b == x)
            {
                if (o2 == 0 && OnSegment(a, y, b))
                    return true;
                else
                    return false;
            }

            if (b == y)
            {
                if (o1 == 0 && OnSegment(a, x, b))
                    return true;
                else
                    return false;
            }


            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            /*
            if (o1 == 0 && o2 == 0 && o3 == 0 && o4 == 0)
                return true;
                */

             // Special Cases
             // p1, q1 and p2 are colinear and p2 lies on segment p1q1
             if (o1 == 0 && OnSegment(a, x, b)) return true;

             // p1, q1 and q2 are colinear and q2 lies on segment p1q1
             if (o2 == 0 && OnSegment(a, y, b)) return true;

             // p2, q2 and p1 are colinear and p1 lies on segment p2q2
             if (o3 == 0 && OnSegment(x, a, y)) return true;

             // p2, q2 and q1 are colinear and q1 lies on segment p2q2
             if (o4 == 0 && OnSegment(x, b, y)) return true;

            return false; // Doesn't fall in any of the above cases
        }


        public static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        public static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;          

            b1 = Sign(pt, v1, v2) < 0.0f;
            b2 = Sign(pt, v2, v3) < 0.0f;
            b3 = Sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public static bool PointInTriangle2(Vector2 s, Vector2 a, Vector2 b, Vector2 c)
        {
            float as_x = s.x - a.x;
            float as_y = s.y - a.y;

            bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

            if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab) return false;

            if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab) return false;

            return true;
        }
    }

}
