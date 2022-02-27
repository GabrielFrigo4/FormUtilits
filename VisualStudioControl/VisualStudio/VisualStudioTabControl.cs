using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Security.Permissions;
using System.Windows.Forms;

namespace VisualStudioControl;
public class VisualStudioTabControl : TabControl
{
    /// <summary>
    ///     Format of the title of the TabPage
    /// </summary>
    private readonly StringFormat CenterSringFormat = new StringFormat
    {
        Alignment = StringAlignment.Near,
        LineAlignment = StringAlignment.Center
    };

    /// <summary>
    ///     The Theme of the VisualStudioTabControl
    /// </summary>
    private VisualStudioTabControlTheme theme = VisualStudioTabControlTheme.Light;

    [Category("Colors"), Browsable(true), Description("The Theme of the VisualStudioTabControl")]
    public VisualStudioTabControlTheme Theme
    {
        get
        {
            return theme;
        }

        set
        {
            theme = value;
            switch (theme)
            {
                case VisualStudioTabControlTheme.Light:
                    HeaderColor = Color.White;
                    ActiveColor = Color.LightGray;
                    DesactiveColor = Color.FromArgb(236, 236, 236);
                    HorizontalLineColor = Color.FromArgb(1, 122, 204);
                    ClosingButtonColor = Color.Black;
                    ClosingButtonColorMouseUp = Color.Red;
                    SelectedTextColor = Color.Black;
                    TextColor = Color.Black;
                    BackTabColor = Color.WhiteSmoke;
                    break;
                case VisualStudioTabControlTheme.Blue:
                    HeaderColor = Color.FromArgb(54, 78, 114);
                    ActiveColor = Color.FromArgb(245, 204, 132);
                    DesactiveColor = Color.FromArgb(65, 90, 130);
                    HorizontalLineColor = Color.FromArgb(245, 204, 132);
                    ClosingButtonColor = Color.Black;
                    ClosingButtonColorMouseUp = Color.IndianRed;
                    SelectedTextColor = Color.Black;
                    TextColor = Color.White;
                    BackTabColor = Color.FromArgb(64, 88, 124);
                    break;
                case VisualStudioTabControlTheme.Dark:
                    HeaderColor = Color.FromArgb(45, 45, 48);
                    ActiveColor = Color.FromArgb(78, 78, 84);
                    DesactiveColor = Color.FromArgb(54, 54, 58);
                    HorizontalLineColor = Color.LightBlue;
                    ClosingButtonColor = Color.White;
                    ClosingButtonColorMouseUp = Color.FromArgb(200, 0, 0);
                    SelectedTextColor = Color.White;
                    TextColor = Color.White;
                    BackTabColor = Color.FromArgb(28, 28, 28);
                    break;
            }
        }
    }

    /// <summary>
    ///     The color of the active tab header
    /// </summary>
    private Color activeColor = Color.LightGray;

    /// <summary>
    ///     The color of the active tab header
    /// </summary>
    private Color desactiveColor = Color.FromArgb(236, 236, 236);

    /// <summary>
    ///     The color of the background of the Tab
    /// </summary>
    private Color backTabColor = Color.FromArgb(28, 28, 28);

    /// <summary>
    ///     The color of the border of the control
    /// </summary>
    private Color borderColor = Color.FromArgb(30, 30, 30);

    /// <summary>
    ///     Color of the closing button
    /// </summary>
    private Color closingButtonColor = Color.White;

    /// <summary>
    ///     Color of the closing button
    /// </summary>
    private Color closingButtonColorMouseUp = Color.Red;

    /// <summary>
    ///     Message for the user before losing
    /// </summary>
    private string closingMessage = string.Empty;

    /// <summary>
    ///     The color of the tab header
    /// </summary>
    private Color headerColor = Color.FromArgb(45, 45, 48);

    /// <summary>
    ///     The color of the horizontal line which is under the headers of the tab pages
    /// </summary>
    private Color horizLineColor = Color.FromArgb(0, 122, 204);

    /// <summary>
    ///     A random page will be used to store a tab that will be deplaced in the run-time
    /// </summary>
    private TabPage? predraggedTab;

    /// <summary>
    ///     The color of the text
    /// </summary>
    private Color textColor = Color.FromArgb(255, 255, 255);

    /// <summary>
    /// Selected tab text color
    /// </summary>
    public Color selectedTextColor = Color.FromArgb(255, 255, 255);

    ///<summary>
    /// Shows closing buttons
    /// </summary> 
    bool showClosingButton = false;

    /// <summary>
    ///     Init
    /// </summary>
    public VisualStudioTabControl()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw
            | ControlStyles.OptimizedDoubleBuffer,
            true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Normal;
        AllowDrop = true;
        DrawMode = TabDrawMode.OwnerDrawFixed;
    }

    [Category("bool"), Browsable(true), Description("Show the close button of the tabPage")]
    public bool ShowClosingButton
    {
        get
        {
            return showClosingButton;
        }
        set
        {
            showClosingButton = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the selected page")]
    public Color ActiveColor
    {
        get
        {
            return this.activeColor;
        }

        set
        {
            this.activeColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the selected page")]
    public Color DesactiveColor
    {
        get
        {
            return this.desactiveColor;
        }

        set
        {
            this.desactiveColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the background of the tab")]
    public Color BackTabColor
    {
        get
        {
            return this.backTabColor;
        }

        set
        {
            this.backTabColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the border of the control")]
    public Color BorderColor
    {
        get
        {
            return this.borderColor;
        }

        set
        {
            this.borderColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the closing button")]
    public Color ClosingButtonColor
    {
        get
        {
            return this.closingButtonColor;
        }

        set
        {
            this.closingButtonColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the closing button")]
    public Color ClosingButtonColorMouseUp
    {
        get
        {
            return this.closingButtonColorMouseUp;
        }

        set
        {
            this.closingButtonColorMouseUp = value;
        }
    }

    [Category("Options"), Browsable(true), Description("The message that will be shown before closing.")]
    public string ClosingMessage
    {
        get
        {
            return this.closingMessage;
        }

        set
        {
            this.closingMessage = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the header.")]
    public Color HeaderColor
    {
        get
        {
            return this.headerColor;
        }

        set
        {
            this.headerColor = value;
        }
    }

    [Category("Colors"), Browsable(true),
     Description("The color of the horizontal line which is located under the headers of the pages.")]
    public Color HorizontalLineColor
    {
        get
        {
            return this.horizLineColor;
        }

        set
        {
            this.horizLineColor = value;
        }
    }

    [Category("Options"), Browsable(true), Description("Show a Yes/No message before closing?")]
    public bool ShowClosingMessage { get; set; }

    [Category("Colors"), Browsable(true), Description("The color of the title of the page")]
    public Color SelectedTextColor
    {
        get
        {
            return this.selectedTextColor;
        }

        set
        {
            this.selectedTextColor = value;
        }
    }

    [Category("Colors"), Browsable(true), Description("The color of the title of the page")]
    public Color TextColor
    {
        get
        {
            return this.textColor;
        }

        set
        {
            this.textColor = value;
        }
    }

    /// <summary>
    ///     Update font
    /// </summary>
    protected override void OnFontChanged(EventArgs e)
    {
        Font font = new Font(this.Font.FontFamily, this.Font.Size, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
        IntPtr hFont = font.ToHfont();
        NativeMethods.SendMessage(this.Handle, NativeMethods.WM_SETFONT, hFont, (IntPtr)(-1));
        NativeMethods.SendMessage(this.Handle, NativeMethods.WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
        this.UpdateStyles();
        if (this.Visible)
        {
            this.Invalidate();
        }
    }

    /// <summary>
    ///     Sets the Tabs on the top
    /// </summary>
    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Top;
    }

    /// <summary>
    ///     Drags the selected tab
    /// </summary>
    /// <param name="drgevent"></param>
    protected override void OnDragOver(DragEventArgs drgevent)
    {
        TabPage? draggedTab = null;
        if (drgevent.Data != null)
        {
            draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
        }
        var pointedTab = GetPointedTab();

        if (ReferenceEquals(draggedTab, predraggedTab) && pointedTab != null)
        {
            drgevent.Effect = DragDropEffects.Move;

            if (!ReferenceEquals(pointedTab, draggedTab) && draggedTab != null)
            {
                this.ReplaceTabPages(draggedTab, pointedTab);
            }
        }

        this.Refresh();
        base.OnDragOver(drgevent);
    }

    /// <summary>
    ///     Handles the selected tab|closes the selected page if wanted.
    /// </summary>
    /// <param name="e"></param>
    TabPage? mySelectedTab = null;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        predraggedTab = GetPointedTab();
        var p = e.Location;
        if (ShowClosingButton && predraggedTab != null)
        {
            var r = GetRectangleTab(predraggedTab);
            r.Offset(r.Width - 19, r.Height / 2 - 9);
            r.Width = 14;
            r.Height = 14;
            if (!r.Contains(p))
            {
                mySelectedTab = predraggedTab;
                return;
            }

            bool mySelectedTabSet = false;
            if (mySelectedTab != predraggedTab && TabCount > 1)
            {
                mySelectedTabSet = true;
            }

            if (this.ShowClosingMessage)
            {
                if (DialogResult.Yes == MessageBox.Show(this.ClosingMessage, "Close", MessageBoxButtons.YesNo))
                {
                    if (this.TabPages.Contains(predraggedTab) && this.TabCount > 1)
                    {
                        this.TabPages.Remove(predraggedTab);
                    }
                    else if (TabCount == 1)
                    {
                        this.TabPages.Clear();
                    }
                }
            }
            else
            {
                if (this.TabPages.Contains(predraggedTab) && this.TabCount > 1)
                {
                    this.TabPages.Remove(predraggedTab);
                }
                else if (TabCount == 1)
                {
                    this.TabPages.Clear();
                }
            }

            if (mySelectedTab != null && mySelectedTabSet)
            {
                SelectedTab = mySelectedTab;
                SelectedTab.Select();
            }
        }

        base.OnMouseDown(e);
        this.Refresh();
    }

    /// <summary>
    ///     Holds the selected page until it sets down
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && predraggedTab != null)
        {
            this.DoDragDrop(predraggedTab, DragDropEffects.Move);
        }

        base.OnMouseMove(e);
        this.Refresh();
    }

    /// <summary>
    ///     Refresh paint
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        this.Refresh();
    }

    /// <summary>
    ///     Abandons the selected tab
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
        predraggedTab = null;
        base.OnMouseUp(e);
    }

    /// <summary>
    ///     Draws the control
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(PaintEventArgs e)
    {
        var Drawer = e.Graphics;

        Drawer.SmoothingMode = SmoothingMode.HighQuality;
        Drawer.PixelOffsetMode = PixelOffsetMode.HighQuality;
        Drawer.TextRenderingHint = TextRenderingHint.SystemDefault;
        Drawer.Clear(this.headerColor);
        try
        {
            SelectedTab.BackColor = this.backTabColor;
        }
        catch
        {
            // ignored
        }

        try
        {
            SelectedTab.BorderStyle = BorderStyle.None;
        }
        catch
        {
            // ignored
        }

        for (var i = 0; i < TabCount; i++)
        {
            var Header = new Rectangle(
               new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 1),
               new Size(GetTabRect(i).Width + 2, GetTabRect(i).Height + 3));

            //DrawTabPage
            void DrawTabPage(Color tabColor, Rectangle HeaderRec, TabPage tabPage, Graphics DrawerTap)
            {
                // Draws the back of the color when it is selected
                DrawerTap.FillRectangle(
                    new SolidBrush(tabColor),
                    new Rectangle(HeaderRec.X, HeaderRec.Y, HeaderRec.Width, HeaderRec.Height));

                // Draws the title of the page
                DrawerTap.DrawString(
                    tabPage.Text,
                    Font,
                    new SolidBrush(this.selectedTextColor),
                    HeaderRec,
                    this.CenterSringFormat);
            }

            //DrawButton
            void DrawButton(Color buttonColor, Rectangle ButtonRect, Rectangle HeaderRec, Graphics DrawerTap)
            {
                Brush ClosingColorBrush = new SolidBrush(this.closingButtonColor);
                Brush ClosingBackColorBrush = new SolidBrush(buttonColor);

                DrawerTap.FillRectangle(ClosingBackColorBrush, ButtonRect);

                e.Graphics.DrawString("X", new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold), ClosingColorBrush, HeaderRec.Right - 19, HeaderRec.Height / 2 - 8);
                predraggedTab = GetPointedTab();

                DrawerTap.DrawRectangle(new Pen(ClosingColorBrush, 2), ButtonRect);
            }

            // Draws the back of the header 
            Drawer.FillRectangle(new SolidBrush(this.headerColor), Header);

            if (i == SelectedIndex)
            {
                // Drawd tabPage
                DrawTabPage(this.activeColor, Header, TabPages[i], Drawer);

                // Draws the line top of the color when it is selected
                Drawer.DrawLine(new Pen(this.horizLineColor, 4.5f), new Point(Header.X, Header.Y), new Point(Header.X + Header.Width, Header.Y));

                // Draws the closing button
                if (this.ShowClosingButton)
                {
                    var ButtonRect = this.GetTabRect(i);
                    ButtonRect.Offset(ButtonRect.Width - 19, Header.Height / 2 - 9);
                    ButtonRect.Width = 14;
                    ButtonRect.Height = 14;

                    if (GetPointedButtonClose(ButtonRect) && TabPages[i] == GetPointedTab())
                    {
                        DrawButton(closingButtonColorMouseUp, ButtonRect, Header, Drawer);
                    }
                    else if (TabPages[i] == GetPointedTab())
                    {
                        DrawButton(this.activeColor, ButtonRect, Header, Drawer);
                    }
                }
            }
            else if (TabPages[i] == GetPointedTab())
            {
                // Drawd tabPage
                DrawTabPage(this.activeColor, Header, TabPages[i], Drawer);

                // Draws the closing button
                if (this.ShowClosingButton)
                {
                    var ButtonRect = this.GetTabRect(i);
                    ButtonRect.Offset(ButtonRect.Width - 19, Header.Height / 2 - 9);
                    ButtonRect.Width = 14;
                    ButtonRect.Height = 14;

                    if (GetPointedButtonClose(ButtonRect))
                    {
                        DrawButton(closingButtonColorMouseUp, ButtonRect, Header, Drawer);
                    }
                    else
                    {
                        DrawButton(this.activeColor, ButtonRect, Header, Drawer);
                    }
                }
            }
            else
            {
                // Drawd tabPage
                DrawTabPage(this.desactiveColor, Header, TabPages[i], Drawer);
            }

            // Draws the horizontal line
            Drawer.DrawLine(new Pen(this.horizLineColor, 5), new Point(0, ItemSize.Height + 4), new Point(Width, ItemSize.Height + 4));

            // Draws the background of the tab control
            Drawer.FillRectangle(new SolidBrush(this.backTabColor), new Rectangle(0, ItemSize.Height + 4, Width, Height - ItemSize.Height));

            // Draws the border of the TabControl
            Drawer.DrawRectangle(new Pen(this.borderColor, 2), new Rectangle(0, 0, Width, Height));
            Drawer.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }
    }

    /// <summary>
    ///     Gets the pointed tab
    /// </summary>
    /// <returns></returns>
    private TabPage? GetPointedTab()
    {
        for (var i = 0; i <= this.TabPages.Count - 1; i++)
        {
            if (this.GetTabRect(i).Contains(this.PointToClient(Cursor.Position)))
            {
                return this.TabPages[i];
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets the TabPage Rectangle
    /// </summary>
    /// <param name="tabPage"></param>
    /// <returns></returns>
    private Rectangle GetRectangleTab(TabPage tabPage)
    {
        for (var i = 0; i < this.TabCount; i++)
        {
            if (tabPage == this.TabPages[i])
            {
                return this.GetTabRect(i);
            }
        }
        return default(Rectangle);
    }

    /// <summary>
    ///     Gets the pointed button close
    /// </summary>
    /// <param name="buttonRect"></param>
    /// <returns></returns>
    private bool GetPointedButtonClose(Rectangle buttonRect)
    {
        if (buttonRect.Contains(this.PointToClient(Cursor.Position)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///     Swaps the two tabs
    /// </summary>
    /// <param name="Source"></param>
    /// <param name="Destination"></param>
    private void ReplaceTabPages(TabPage Source, TabPage Destination)
    {
        var SourceIndex = this.TabPages.IndexOf(Source);
        var DestinationIndex = this.TabPages.IndexOf(Destination);

        this.TabPages[DestinationIndex] = Source;
        this.TabPages[SourceIndex] = Destination;

        if (this.SelectedIndex == SourceIndex)
        {
            this.SelectedIndex = DestinationIndex;
        }
        else if (this.SelectedIndex == DestinationIndex)
        {
            this.SelectedIndex = SourceIndex;
        }

        this.Refresh();
    }
}

public enum VisualStudioTabControlTheme
{
    Light,
    Dark,
    Blue,
}