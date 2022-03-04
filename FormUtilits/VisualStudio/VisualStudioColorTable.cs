namespace FormUtilits.VisualStudioControl;
public class VisualStudioColorTable : ProfessionalColorTable
{
    private bool isDark = false;

    public VisualStudioColorTable(bool isDark)
    {
        this.isDark = isDark;
    }

    #region MenuStrip
    public override Color MenuBorder
    {
        get
        {
            if (isDark)
                return Color.DimGray;
            else
                return base.MenuBorder;
        }
    }

    public override Color MenuItemSelectedGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuItemSelectedGradientBegin;
        }
    }

    public override Color MenuItemSelectedGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuItemSelectedGradientEnd;
        }
    }

    public override Color MenuItemPressedGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.MenuItemPressedGradientBegin;
        }
    }

    public override Color MenuItemPressedGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.MenuItemPressedGradientEnd;
        }
    }

    public override Color MenuItemBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuItemBorder;
        }
    }

    public override Color MenuStripGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuStripGradientBegin;
        }
    }

    public override Color MenuStripGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuStripGradientEnd;
        }
    }

    public override Color MenuItemPressedGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuItemPressedGradientMiddle;
        }
    }

    public override Color MenuItemSelected
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.MenuItemSelected;
        }
    }
    #endregion

    #region ToolTrip
    public override Color ToolStripDropDownBackground
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ToolStripDropDownBackground;
        }
    }

    public override Color ToolStripBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripBorder;
        }
    }

    public override Color ToolStripContentPanelGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripContentPanelGradientBegin;
        }
    }

    public override Color ToolStripContentPanelGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripContentPanelGradientEnd;
        }
    }

    public override Color ToolStripPanelGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripPanelGradientBegin;
        }
    }

    public override Color ToolStripPanelGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripPanelGradientEnd;
        }
    }

    public override Color ToolStripGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripGradientBegin;
        }
    }

    public override Color ToolStripGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripGradientMiddle;
        }
    }

    public override Color ToolStripGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(40, 40, 40);
            else
                return base.ToolStripGradientEnd;
        }
    }
    #endregion

    #region Image
    public override Color ImageMarginGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ImageMarginGradientBegin;
        }
    }

    public override Color ImageMarginGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ImageMarginGradientMiddle;
        }
    }

    public override Color ImageMarginGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ImageMarginGradientBegin;
        }
    }
    #endregion
}