﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.ShellLink
{
    [Flags]
    public enum FillAttributes : short
    {
        ForegroundBlue = 1 << 0,
        ForegroundGreen = 1 << 1,
        ForegroundRed = 1 << 2,
        ForegroundIntensity = 1 << 3,
        BackgroundBlue = 1 << 4,
        BackgroundGreen = 1 << 5,
        BackgroundRed = 1 << 6,
        BackgroundIntensity = 1 << 7,
    }
}
