﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW5.Plugins.Interfaces
{
    public interface IMenu: IToolbar
    {
        IDropDownMenuItem Plugins { get; }

        IDropDownMenuItem Tiles { get; }

        IMenuItem FindItem(string key);
    }
}
