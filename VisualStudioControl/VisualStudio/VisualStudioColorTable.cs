namespace VisualStudioControl;

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
    #endregion
}