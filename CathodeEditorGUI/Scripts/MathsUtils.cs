using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor
{
    public static class MathsUtils
    {
        /* Utility: convert Quaternion to Yaw/Pitch/Roll */
        public static (decimal, decimal, decimal) ToYawPitchRoll(this Quaternion q)
        {
            decimal yaw = Convert.ToDecimal(Math.Atan2(2 * (q.Y * q.W + q.X * q.Z), 1 - 2 * (q.Y * q.Y + q.X * q.X)) * (180 / Math.PI));
            decimal pitch = Convert.ToDecimal(Math.Asin(2 * (q.X * q.W - q.Z * q.Y)) * (180 / Math.PI));
            decimal roll = Convert.ToDecimal(Math.Atan2(2 * (q.Z * q.W + q.X * q.Y), 1 - 2 * (q.X * q.X + q.Z * q.Z)) * (180 / Math.PI));

            return (yaw, pitch, roll);
        }
    }
}
