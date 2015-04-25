﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW5.Plugins.Symbology.Views
{
    public enum RasterStyleCommand
    {
        ProjectionDetails = 0,
        BuildOverviews = 1,
        ClearOverviews = 2,
        GenerateColorScheme = 3,
        CalculateMinMax = 4,
        Apply = 5,
        ClearColorAdjustments = 6,
    }
}
