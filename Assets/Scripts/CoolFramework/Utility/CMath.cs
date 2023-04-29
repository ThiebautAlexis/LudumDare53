using System;
using UnityEngine;

namespace CoolFramework.Utility
{
    public static class CMath
    {
        #region Float
        /// <summary>
        /// Round and simplify the float to N number to the decimal.
        /// </summary>
        /// <param name="_value">Value to round</param>
        /// <param name="_decimals">Number of decimal.</param>
        /// <returns></returns>
        public static float RoundToDecimals(this float _value, int _decimals = 2)
        {
            float _factor = Mathf.Pow(10, _decimals); 
            return ((float)Mathf.RoundToInt(_value * _factor)) / _factor; 
        }
        #endregion

        #region Int 
        /// <summary>
        /// Convert the float to an int that Keeps N number after the point.
        /// Ex : 12.345 with 2 decimals => 1235; 
        /// </summary>
        /// <param name="_value">Value to convert</param>
        /// <param name="_decimals">Numer of decimals to keep.</param>
        /// <returns></returns>
        public static int ConvertToIntN(this float _value, int _decimals = 2)
        {
            float _factor = Mathf.Pow(10, _decimals);
            return Mathf.RoundToInt(_value * _factor);
        }
        #endregion

        #region bool 
        public static bool AreEquals(float _x, float _y, float _z)
        {
            return _x == _y && _y == _z; 
        }

        public static bool AreEquals(int _x, int _y, int _z)
        {
            return _x == _y && _y == _z;
        }
        #endregion 
    }
}
