﻿using System;

namespace SparqlForHumans.Utilities
{
    public static class NumbersExtensions
    {
        public static double ToThreeDecimals(this double input)
        {
            return Math.Truncate(input * 1000) / 1000;
        }
    }
}