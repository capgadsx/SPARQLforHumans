﻿using System.Collections.Generic;

namespace SparqlForHumans.Models
{
    public interface IHasAltLabel
    {
        IList<string> AltLabels { get; set; }
    }
}