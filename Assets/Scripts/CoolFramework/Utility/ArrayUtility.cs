using System;
using System.Collections;
using System.Collections.Generic;

namespace CoolFramework
{
    public static class ArrayUtility
    {
        public static bool Contains<T>(T[] _array, T _element)
        {
            return Array.IndexOf(_array, _element ) >= 0;
        }
    }
}
