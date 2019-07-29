﻿using System.Collections.Generic;

namespace SparqlForHumans.Models
{
    public class SubjectComparer : IEqualityComparer<Subject>
    {
        public bool Equals(Subject x, Subject y)
        {
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Subject obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}