﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MW5.Plugins;
using MW5.Plugins.Concrete;
using MW5.Plugins.Interfaces;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Tools.XPMenus;

namespace MW5.UI
{
    public class Toolbar: IToolbar
    {
        private MainFrameBarManager _manager;
        private readonly CommandBar _commandBar;
        private readonly Bar _bar;

        internal Toolbar(MainFrameBarManager manager, Bar bar)
        {
            _manager = manager;
            _bar = bar;
            _commandBar = _manager.GetBarControl(_bar);

            if (bar == null || _commandBar == null)
            {
                throw new NullReferenceException("Internal toolbar reference is null.");
            }
        }

        public string Name
        {
            get { return _bar.BarName; }
            set { _bar.BarName = value; }
        }

        public IMenuItemCollection Items
        {
            get { return new MenuItemCollection(_bar.Items); }
        }

        public bool Visible
        {
            get { return _commandBar.Visible; }
            set { _commandBar.Visible = value; }
        }

        public object Tag
        {
            get { return Metadata.Tag; }
            set { Metadata.Tag = value; }
        }

        public ToolbarDockState DockState
        {
            get { return (ToolbarDockState) _manager.GetBarControl(_bar).DockState; }
            set { _manager.GetBarControl(_bar).DockState = (CommandBarDockState)value; }
        }

        public void AddSeparator(int beforeItemIndex)
        {
            _bar.SeparatorIndices.Add(beforeItemIndex);
        }

        public void ClearSeparators()
        {
            _bar.SeparatorIndices.Clear();
        }

        private MenuItemMetadata Metadata
        {
            get
            {
                var metadata = _commandBar.Tag as MenuItemMetadata;
                if (metadata == null)
                {
                    throw new ApplicationException("Tag must have an instance of MenuItemMetadata class.");
                }
                return metadata;
            }
        }

        public PluginIdentity PluginIdentity
        {
            get { return Metadata.PluginIdentity; }
        }
    }
}
