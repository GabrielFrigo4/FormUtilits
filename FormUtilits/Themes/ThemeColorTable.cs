namespace FormUtilits.Themes;
public class ThemeColorTable : ProfessionalColorTable
{
    private bool isDark = false;

    public ThemeColorTable(bool isDark)
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

    #region Button
    public override Color ButtonCheckedGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonCheckedGradientBegin;
        }
    }

    public override Color ButtonCheckedGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonCheckedGradientEnd;
        }
    }

    public override Color ButtonCheckedGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonCheckedGradientMiddle;
        }
    }

    public override Color ButtonCheckedHighlight
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonCheckedHighlight;
        }
    }

    public override Color ButtonCheckedHighlightBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonCheckedHighlightBorder;
        }
    }

    public override Color ButtonPressedBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedBorder;
        }
    }

    public override Color ButtonPressedGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedGradientBegin;
        }
    }

    public override Color ButtonPressedGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedGradientEnd;
        }
    }

    public override Color ButtonPressedGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedGradientMiddle;
        }
    }

    public override Color ButtonPressedHighlight
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedHighlight;
        }
    }

    public override Color ButtonPressedHighlightBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonPressedHighlightBorder;
        }
    }

    public override Color ButtonSelectedBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedBorder;
        }
    }

    public override Color ButtonSelectedGradientBegin
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedGradientBegin;
        }
    }

    public override Color ButtonSelectedGradientEnd
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedGradientEnd;
        }
    }

    public override Color ButtonSelectedGradientMiddle
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedGradientMiddle;
        }
    }

    public override Color ButtonSelectedHighlight
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedHighlight;
        }
    }

    public override Color ButtonSelectedHighlightBorder
    {
        get
        {
            if (isDark)
                return Color.FromArgb(23, 23, 23);
            else
                return base.ButtonSelectedHighlightBorder;
        }
    }
    #endregion Button

    #region Backgound
    #endregion

    #region Grip
    #endregion

    #region Separator
    #endregion 
}