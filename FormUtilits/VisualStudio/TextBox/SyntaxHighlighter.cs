using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace FormUtilits.VisualStudioControl;
public class SyntaxHighlighter : IDisposable
{
    //styles
    protected static readonly Platform platformType = PlatformType.GetOperationSystemPlatform();
    public readonly Style OrangeRedBoldStyle = new TextStyle(Brushes.OrangeRed, null, FontStyle.Bold);
    public readonly Style CornflowerBlueBoldStyle = new TextStyle(Brushes.CornflowerBlue, null, FontStyle.Bold);
    public readonly Style CadetBlueBoldStyle = new TextStyle(Brushes.CadetBlue, null, FontStyle.Bold);
    public readonly Style BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
    public readonly Style BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
    public readonly Style GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
    public readonly Style GreenBoldStyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
    public readonly Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
    public readonly Style MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
    public readonly Style RedStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
    //styles

    protected readonly Dictionary<string, SyntaxDescriptor> descByXMLfileNames =
        new Dictionary<string, SyntaxDescriptor>();

    protected readonly List<Style> resilientStyles = new List<Style>(5);

    protected Regex CSharpAttributeRegex,
                  CSharpClassNameRegex, CSharpStructNameRegex, CSharpEnumNameRegex, CSharpInterfaceNameRegex;

    protected Regex CSharpCommentRegex1,
                  CSharpCommentRegex2,
                  CSharpCommentRegex3;

    protected Regex CSharpKeywordRegex;
    protected Regex CSharpKeywordClassRegex;
    protected Regex CSharpKeywordEnumRegex;
    protected Regex CSharpKeywordStructRegex;
    protected Regex CSharpKeywordInterfaceRegex;
    protected Regex CSharpNumberRegex;
    protected Regex CSharpStringRegex;

    protected Regex ShaderCommentRegex1,
          ShaderCommentRegex2,
          ShaderCommentRegex3;

    protected Regex ShaderKeywordRegex, ShaderReturnKeywordRegex;
    protected Regex ShaderFunctionRegex, ShaderFunctionNameRegex;
    protected Regex ShaderStructRegex, ShaderStructNameRegex;
    protected Regex ShaderNumberRegex;
    protected Regex ShaderStringRegex;

    protected Regex HTMLAttrRegex,
                  HTMLAttrValRegex,
                  HTMLCommentRegex1,
                  HTMLCommentRegex2;

    protected Regex HTMLEndTagRegex;

    protected Regex HTMLEntityRegex,
                  HTMLTagContentRegex;

    protected Regex HTMLTagNameRegex;
    protected Regex HTMLTagRegex;

    protected FastColoredTextBox currentTb;

    public static RegexOptions RegexCompiledOption
    {
        get
        {
            if (platformType == Platform.X86)
                return RegexOptions.Compiled;
            else
                return RegexOptions.None;
        }
    }

    public SyntaxHighlighter(FastColoredTextBox currentTb) {
        this.currentTb = currentTb;
    }

    #region IDisposable Members

    public void Dispose()
    {
        foreach (SyntaxDescriptor desc in descByXMLfileNames.Values)
            desc.Dispose();
    }

    #endregion

    /// <summary>
    /// Highlights syntax for given language
    /// </summary>
    public virtual void HighlightSyntax(Language language, Range range)
    {
        switch (language)
        {
            case Language.CSharp:
                CSharpSyntaxHighlight(range);
                break;
            case Language.Shader:
                ShaderSyntaxHighlight(range);
                break;
            default:
                break;
        }
    }

    public void InitStyleSchema(Language lang)
    {
        switch (lang)
        {
            case Language.CSharp:
                StringStyle = BrownStyle;
                CommentStyle = GreenStyle;
                NumberStyle = MagentaStyle;
                AttributeStyle = GreenStyle;
                ClassNameStyle = GreenBoldStyle;
                EnumNameStyle = OrangeRedBoldStyle;
                StructNameStyle = CadetBlueBoldStyle;
                InterfaceNameStyle = CornflowerBlueBoldStyle;
                KeywordStyle = BlueStyle;
                KeywordClassStyle = GreenBoldStyle;
                KeywordEnumStyle = OrangeRedBoldStyle;
                KeywordStructStyle = CadetBlueBoldStyle;
                KeywordInterfaceStyle = CornflowerBlueBoldStyle;
                CommentTagStyle = GrayStyle;
                break;
            case Language.Shader:
                StringStyle = BrownStyle;
                CommentStyle = GreenStyle;
                NumberStyle = MagentaStyle;
                AttributeStyle = GreenStyle;
                StructNameStyle = CadetBlueBoldStyle;
                KeywordStyle = BlueStyle;
                ReturnKeywordStyle = CornflowerBlueBoldStyle;
                KeywordStructStyle = CadetBlueBoldStyle;
                CommentTagStyle = GrayStyle;
                ConfigStyle = RedStyle;
                FunctionStyle = OrangeRedBoldStyle;
                break;
        }
    }

    protected void InitHTMLRegex()
    {
        HTMLCommentRegex1 = new Regex(@"(<!--.*?-->)|(<!--.*)", RegexOptions.Singleline | RegexCompiledOption);
        HTMLCommentRegex2 = new Regex(@"(<!--.*?-->)|(.*-->)",
                                      RegexOptions.Singleline | RegexOptions.RightToLeft | RegexCompiledOption);
        HTMLTagRegex = new Regex(@"<|/>|</|>", RegexCompiledOption);
        HTMLTagNameRegex = new Regex(@"<(?<range>[!\w:]+)", RegexCompiledOption);
        HTMLEndTagRegex = new Regex(@"</(?<range>[\w:]+)>", RegexCompiledOption);
        HTMLTagContentRegex = new Regex(@"<[^>]+>", RegexCompiledOption);
        HTMLAttrRegex =
            new Regex(
                @"(?<range>[\w\d\-]{1,20}?)='[^']*'|(?<range>[\w\d\-]{1,20})=""[^""]*""|(?<range>[\w\d\-]{1,20})=[\w\d\-]{1,20}",
                RegexCompiledOption);
        HTMLAttrValRegex =
            new Regex(
                @"[\w\d\-]{1,20}?=(?<range>'[^']*')|[\w\d\-]{1,20}=(?<range>""[^""]*"")|[\w\d\-]{1,20}=(?<range>[\w\d\-]{1,20})",
                RegexCompiledOption);
        HTMLEntityRegex = new Regex(@"\&(amp|gt|lt|nbsp|quot|apos|copy|reg|#[0-9]{1,8}|#x[0-9a-f]{1,8});",
                                    RegexCompiledOption | RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Highlights syntax for given XML description file
    /// </summary>
    public virtual void HighlightSyntax(string XMLdescriptionFile, Range range)
    {
        SyntaxDescriptor desc = null;
        if (!descByXMLfileNames.TryGetValue(XMLdescriptionFile, out desc))
        {
            var doc = new XmlDocument();
            string file = XMLdescriptionFile;
            if (!File.Exists(file))
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));

            doc.LoadXml(File.ReadAllText(file));
            desc = ParseXmlDescription(doc);
            descByXMLfileNames[XMLdescriptionFile] = desc;
        }

        HighlightSyntax(desc, range);
    }

    public virtual void AutoIndentNeeded(object sender, AutoIndentEventArgs args)
    {
        var tb = (sender as FastColoredTextBox);
        Language language = tb.Language;
        switch (language)
        {
            case Language.CSharp:
                CSharpAutoIndentNeeded(sender, args);
                break;
            case Language.Shader:
                CSharpAutoIndentNeeded(sender, args);
                break;
            default:
                break;
        }
    }

    protected void CSharpAutoIndentNeeded(object sender, AutoIndentEventArgs args)
    {
        //block {}
        if (Regex.IsMatch(args.LineText, @"^[^""']*\{.*\}[^""']*$"))
            return;
        //start of block {}
        if (Regex.IsMatch(args.LineText, @"^[^""']*\{"))
        {
            args.ShiftNextLines = args.TabLength;
            return;
        }
        //end of block {}
        if (Regex.IsMatch(args.LineText, @"}[^""']*$"))
        {
            args.Shift = -args.TabLength;
            args.ShiftNextLines = -args.TabLength;
            return;
        }
        //label
        if (Regex.IsMatch(args.LineText, @"^\s*\w+\s*:\s*($|//)") &&
            !Regex.IsMatch(args.LineText, @"^\s*default\s*:"))
        {
            args.Shift = -args.TabLength;
            return;
        }
        //some statements: case, default
        if (Regex.IsMatch(args.LineText, @"^\s*(case|default)\b.*:\s*($|//)"))
        {
            args.Shift = -args.TabLength / 2;
            return;
        }
        //is unclosed operator in previous line ?
        if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|foreach|while|[\}\s]*else)\b[^{]*$"))
            if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)")) //operator is unclosed
            {
                args.Shift = args.TabLength;
                return;
            }
    }

    public static SyntaxDescriptor ParseXmlDescription(XmlDocument doc)
    {
        var desc = new SyntaxDescriptor();
        XmlNode brackets = doc.SelectSingleNode("doc/brackets");
        if (brackets != null)
        {
            if (brackets.Attributes["left"] == null || brackets.Attributes["right"] == null ||
                brackets.Attributes["left"].Value == "" || brackets.Attributes["right"].Value == "")
            {
                desc.leftBracket = '\x0';
                desc.rightBracket = '\x0';
            }
            else
            {
                desc.leftBracket = brackets.Attributes["left"].Value[0];
                desc.rightBracket = brackets.Attributes["right"].Value[0];
            }

            if (brackets.Attributes["left2"] == null || brackets.Attributes["right2"] == null ||
                brackets.Attributes["left2"].Value == "" || brackets.Attributes["right2"].Value == "")
            {
                desc.leftBracket2 = '\x0';
                desc.rightBracket2 = '\x0';
            }
            else
            {
                desc.leftBracket2 = brackets.Attributes["left2"].Value[0];
                desc.rightBracket2 = brackets.Attributes["right2"].Value[0];
            }

            if (brackets.Attributes["strategy"] == null || brackets.Attributes["strategy"].Value == "")
                desc.bracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;
            else
                desc.bracketsHighlightStrategy = (BracketsHighlightStrategy)Enum.Parse(typeof(BracketsHighlightStrategy), brackets.Attributes["strategy"].Value);
        }

        var styleByName = new Dictionary<string, Style>();

        foreach (XmlNode style in doc.SelectNodes("doc/style"))
        {
            Style s = ParseStyle(style);
            styleByName[style.Attributes["name"].Value] = s;
            desc.styles.Add(s);
        }
        foreach (XmlNode rule in doc.SelectNodes("doc/rule"))
            desc.rules.Add(ParseRule(rule, styleByName));
        foreach (XmlNode folding in doc.SelectNodes("doc/folding"))
            desc.foldings.Add(ParseFolding(folding));

        return desc;
    }

    protected static FoldingDesc ParseFolding(XmlNode foldingNode)
    {
        var folding = new FoldingDesc();
        //regex
        folding.startMarkerRegex = foldingNode.Attributes["start"].Value;
        folding.finishMarkerRegex = foldingNode.Attributes["finish"].Value;
        //options
        XmlAttribute optionsA = foldingNode.Attributes["options"];
        if (optionsA != null)
            folding.options = (RegexOptions)Enum.Parse(typeof(RegexOptions), optionsA.Value);

        return folding;
    }

    protected static RuleDesc ParseRule(XmlNode ruleNode, Dictionary<string, Style> styles)
    {
        var rule = new RuleDesc();
        rule.pattern = ruleNode.InnerText;
        //
        XmlAttribute styleA = ruleNode.Attributes["style"];
        XmlAttribute optionsA = ruleNode.Attributes["options"];
        //Style
        if (styleA == null)
            throw new Exception("Rule must contain style name.");
        if (!styles.ContainsKey(styleA.Value))
            throw new Exception("Style '" + styleA.Value + "' is not found.");
        rule.style = styles[styleA.Value];
        //options
        if (optionsA != null)
            rule.options = (RegexOptions)Enum.Parse(typeof(RegexOptions), optionsA.Value);

        return rule;
    }

    protected static Style ParseStyle(XmlNode styleNode)
    {
        XmlAttribute typeA = styleNode.Attributes["type"];
        XmlAttribute colorA = styleNode.Attributes["color"];
        XmlAttribute backColorA = styleNode.Attributes["backColor"];
        XmlAttribute fontStyleA = styleNode.Attributes["fontStyle"];
        XmlAttribute nameA = styleNode.Attributes["name"];
        //colors
        SolidBrush foreBrush = null;
        if (colorA != null)
            foreBrush = new SolidBrush(ParseColor(colorA.Value));
        SolidBrush backBrush = null;
        if (backColorA != null)
            backBrush = new SolidBrush(ParseColor(backColorA.Value));
        //fontStyle
        FontStyle fontStyle = FontStyle.Regular;
        if (fontStyleA != null)
            fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontStyleA.Value);

        return new TextStyle(foreBrush, backBrush, fontStyle);
    }

    protected static Color ParseColor(string s)
    {
        if (s.StartsWith("#"))
        {
            if (s.Length <= 7)
                return Color.FromArgb(255,
                                      Color.FromArgb(Int32.Parse(s.Substring(1), NumberStyles.AllowHexSpecifier)));
            else
                return Color.FromArgb(Int32.Parse(s.Substring(1), NumberStyles.AllowHexSpecifier));
        }
        else
            return Color.FromName(s);
    }

    public void HighlightSyntax(SyntaxDescriptor desc, Range range)
    {
        //set style order
        range.tb.ClearStylesBuffer();
        for (int i = 0; i < desc.styles.Count; i++)
            range.tb.Styles[i] = desc.styles[i];
        // add resilient styles
        int l = desc.styles.Count;
        for (int i = 0; i < resilientStyles.Count; i++)
            range.tb.Styles[l + i] = resilientStyles[i];
        //brackets
        char[] oldBrackets = RememberBrackets(range.tb);
        range.tb.LeftBracket = desc.leftBracket;
        range.tb.RightBracket = desc.rightBracket;
        range.tb.LeftBracket2 = desc.leftBracket2;
        range.tb.RightBracket2 = desc.rightBracket2;
        //clear styles of range
        range.ClearStyle(desc.styles.ToArray());
        //highlight syntax
        foreach (RuleDesc rule in desc.rules)
            range.SetStyle(rule.style, rule.Regex);
        //clear folding
        range.ClearFoldingMarkers();
        //folding markers
        foreach (FoldingDesc folding in desc.foldings)
            range.SetFoldingMarkers(folding.startMarkerRegex, folding.finishMarkerRegex, folding.options);

        //
        RestoreBrackets(range.tb, oldBrackets);
    }

    protected void RestoreBrackets(FastColoredTextBox tb, char[] oldBrackets)
    {
        tb.LeftBracket = oldBrackets[0];
        tb.RightBracket = oldBrackets[1];
        tb.LeftBracket2 = oldBrackets[2];
        tb.RightBracket2 = oldBrackets[3];
    }

    protected char[] RememberBrackets(FastColoredTextBox tb)
    {
        return new[] { tb.LeftBracket, tb.RightBracket, tb.LeftBracket2, tb.RightBracket2 };
    }

    public void CSharpUpdateSyntaxHighlight()
    {
        //roslyn.Run().Wait();

        //#region type
        //string regexClass = @"";
        //string regexStruct = @"";
        //string regexInterface = @"";
        //string regexEnum = @"";

        //List<string> types = new List<string>();
        //foreach (var @class in roslyn.classes)
        //{
        //    if (!types.Contains(@class))
        //    {
        //        types.Add(@class);

        //        if (regexClass != @"")
        //            regexClass = regexClass + @"|" + @class;
        //        else
        //            regexClass = regexClass + @class;
        //    }
        //}
        //foreach (var @struct in roslyn.structs)
        //{
        //    if (!types.Contains(@struct))
        //    {
        //        types.Add(@struct);

        //        if (regexStruct != @"")
        //            regexStruct = regexStruct + @"|" + @struct;
        //        else
        //            regexStruct = regexStruct + @struct;
        //    }
        //}
        //foreach (var @interface in roslyn.interfaces)
        //{
        //    if (!types.Contains(@interface))
        //    {
        //        types.Add(@interface);

        //        if (regexInterface != @"")
        //            regexInterface = regexInterface + @"|" + @interface;
        //        else
        //            regexInterface = regexInterface + @interface;
        //    }
        //}
        //foreach (var @enum in roslyn.enums)
        //{
        //    if (!types.Contains(@enum))
        //    {
        //        types.Add(@enum);

        //        if (regexEnum != @"")
        //            regexEnum = regexEnum + @"|" + @enum;
        //        else
        //            regexEnum = regexEnum + @enum;
        //    }
        //}
        //types.Clear();

        //CSharpKeywordClassRegex = new Regex($@"\b({regexClass})\b", RegexCompiledOption);
        //CSharpKeywordEnumRegex = new Regex($@"\b({regexEnum})\b", RegexCompiledOption);
        //CSharpKeywordStructRegex = new Regex($@"\b({regexStruct})\b", RegexCompiledOption);
        //CSharpKeywordInterfaceRegex = new Regex($@"\b({regexInterface})\b", RegexCompiledOption);
        //#endregion
    }

    protected void InitCSharpRegex()
    {
        CSharpStringRegex =
            new Regex(
                @"
                        # Character definitions:
                            '
                            (?> # disable backtracking
                              (?:
                                \\[^\r\n]|    # escaped meta char
                                [^'\r\n]      # any character except '
                              )*
                            )
                            '?
                            |
                            # Normal string & verbatim strings definitions:
                            (?<verbatimIdentifier>@)?         # this group matches if it is an verbatim string
                            ""
                            (?> # disable backtracking
                              (?:
                                # match and consume an escaped character including escaped double quote ("") char
                                (?(verbatimIdentifier)        # if it is a verbatim string ...
                                  """"|                         #   then: only match an escaped double quote ("") char
                                  \\.                         #   else: match an escaped sequence
                                )
                                | # OR
            
                                # match any char except double quote char ("")
                                [^""]
                              )*
                            )
                            ""
                        ",
                RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace |
                RegexCompiledOption
                ); //thanks to rittergig for this regex

        CSharpCommentRegex1 = new Regex(@"//.*$", RegexOptions.Multiline | RegexCompiledOption);
        CSharpCommentRegex2 = new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexCompiledOption);
        CSharpCommentRegex3 = new Regex(@"(/\*.*?\*/)|(.*\*/)",
                                        RegexOptions.Singleline | RegexOptions.RightToLeft | RegexCompiledOption);

        CSharpNumberRegex = new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b",
                                      RegexCompiledOption);
        CSharpAttributeRegex = new Regex(@"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline | RegexCompiledOption);
        CSharpClassNameRegex = new Regex(@"\b(class)\s+(?<range>\w+?)\b", RegexCompiledOption);
        CSharpStructNameRegex = new Regex(@"\b(struct)\s+(?<range>\w+?)\b", RegexCompiledOption);
        CSharpEnumNameRegex = new Regex(@"\b(enum)\s+(?<range>\w+?)\b", RegexCompiledOption);
        CSharpInterfaceNameRegex = new Regex(@"\b(interface)\s+(?<range>\w+?)\b", RegexCompiledOption);
        CSharpKeywordRegex = new Regex(
                @"\b(abstract|add|alias|as|ascending|async|await|base|bool|break|by|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|descending|do|double|dynamic|else|enum|equals|event|explicit|extern|false|finally|fixed|float|for|foreach|from|get|global|goto|group|if|implicit|in|int|interface|internal|into|is|join|let|lock|long|nameof|namespace|new|null|object|on|operator|orderby|out|override|params|partial|private|protected|public|readonly|ref|remove|return|sbyte|sealed|select|set|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|value|var|virtual|void|volatile|when|where|while|yield)\b|#region\b|#endregion\b|#if\b|#endif\b",
                RegexCompiledOption);
    }

    /// <summary>
    /// Highlights C# code
    /// </summary>
    /// <param name="range"></param>
    public virtual void CSharpSyntaxHighlight(Range range)
    {
        /* range */
        range.tb.CommentPrefix = "//";
        range.tb.LeftBracket = '(';
        range.tb.RightBracket = ')';
        range.tb.LeftBracket2 = '{';
        range.tb.RightBracket2 = '}';
        range.tb.BracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;

        range.tb.AutoIndentCharsPatterns
            = @"
                ^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
                ^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
                ";

        //clear style of changed range
        range.ClearStyle(StringStyle, CommentStyle, NumberStyle, AttributeStyle, ClassNameStyle, StructNameStyle, EnumNameStyle, InterfaceNameStyle, KeywordStyle, KeywordClassStyle, KeywordEnumStyle, KeywordStructStyle, KeywordInterfaceStyle);
        
        //init c# regex
        if (CSharpStringRegex == null)
            InitCSharpRegex();

        //string highlighting
        range.SetStyle(StringStyle, CSharpStringRegex);
        //comment highlighting
        range.SetStyle(CommentStyle, CSharpCommentRegex1);
        range.SetStyle(CommentStyle, CSharpCommentRegex2);
        range.SetStyle(CommentStyle, CSharpCommentRegex3);
        //number highlighting
        range.SetStyle(NumberStyle, CSharpNumberRegex);
        //attribute highlighting
        range.SetStyle(AttributeStyle, CSharpAttributeRegex);
        //class name highlighting
        range.SetStyle(ClassNameStyle, CSharpClassNameRegex);
        range.SetStyle(EnumNameStyle, CSharpEnumNameRegex);
        range.SetStyle(StructNameStyle, CSharpStructNameRegex);
        range.SetStyle(InterfaceNameStyle, CSharpInterfaceNameRegex);
        //keyword highlighting
        range.SetStyle(KeywordStyle, CSharpKeywordRegex);

        foreach (var r in range.GetRanges(CSharpKeywordClassRegex))
        {
            r.SetStyle(KeywordClassStyle, CSharpKeywordClassRegex);
        }
        foreach (var r in range.GetRanges(CSharpKeywordEnumRegex))
        {
            r.SetStyle(KeywordEnumStyle, CSharpKeywordEnumRegex);
        }
        foreach (var r in range.GetRanges(CSharpKeywordStructRegex))
        {
            r.SetStyle(KeywordStructStyle, CSharpKeywordStructRegex);
        }
        foreach (var r in range.GetRanges(CSharpKeywordInterfaceRegex))
        {
            r.SetStyle(KeywordInterfaceStyle, CSharpKeywordInterfaceRegex);
        }

        //find document comments
        foreach (Range r in range.GetRanges(@"^\s*///.*$", RegexOptions.Multiline))
        {
            //remove C# highlighting from this fragment
            r.ClearStyle(StyleIndex.All);
            //do XML highlighting
            if (HTMLTagRegex == null)
                InitHTMLRegex();
            //
            r.SetStyle(CommentStyle);
            //tags
            foreach (Range rr in r.GetRanges(HTMLTagContentRegex))
            {
                rr.ClearStyle(StyleIndex.All);
                rr.SetStyle(CommentTagStyle);
            }
            //prefix '///'
            foreach (Range rr in r.GetRanges(@"^\s*///", RegexOptions.Multiline))
            {
                rr.ClearStyle(StyleIndex.All);
                rr.SetStyle(CommentTagStyle);
            }
        }

        //clear folding markers
        range.ClearFoldingMarkers();
        //set folding markers
        range.SetFoldingMarkers("{", "}"); //allow to collapse brackets block
        range.SetFoldingMarkers(@"#region\b", @"#endregion\b"); //allow to collapse #region blocks
        range.SetFoldingMarkers(@"/\*", @"\*/"); //allow to collapse comment block
    }

    protected void InitShaderRegex()
    {
        ShaderStringRegex =
            new Regex(
                @"
                        # Character definitions:
                            '
                            (?> # disable backtracking
                              (?:
                                \\[^\r\n]|    # escaped meta char
                                [^'\r\n]      # any character except '
                              )*
                            )
                            '?
                            |
                            # Normal string & verbatim strings definitions:
                            (?<verbatimIdentifier>@)?         # this group matches if it is an verbatim string
                            ""
                            (?> # disable backtracking
                              (?:
                                # match and consume an escaped character including escaped double quote ("") char
                                (?(verbatimIdentifier)        # if it is a verbatim string ...
                                  """"|                         #   then: only match an escaped double quote ("") char
                                  \\.                         #   else: match an escaped sequence
                                )
                                | # OR
            
                                # match any char except double quote char ("")
                                [^""]
                              )*
                            )
                            ""
                        ",
                RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace |
                RegexCompiledOption
                ); //thanks to rittergig for this regex

        ShaderCommentRegex1 = new Regex(@"//.*$", RegexOptions.Multiline | RegexCompiledOption);
        ShaderCommentRegex2 = new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexCompiledOption);
        ShaderCommentRegex3 = new Regex(@"(/\*.*?\*/)|(.*\*/)",
                                        RegexOptions.Singleline | RegexOptions.RightToLeft | RegexCompiledOption);

        ShaderNumberRegex = new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b",
                                      RegexCompiledOption);
        ShaderStructNameRegex = new Regex(@"\b(struct)\s+(?<range>\w+?)\b", RegexCompiledOption);

        ShaderStructRegex = new Regex(
            @"\b(sampler1D|sampler2D|sampler3D|samplerCube|sampler1DShadow|sampler2DShadow|samplerCubeShadow|sampler1DArray|sampler2DArray|sampler1DArrayShadow|sampler2DArrayShadow|isampler1D|isampler2D|isampler3D|isamplerCube|isampler1DArray|isampler2DArray|usampler1D|usampler2D|usampler3D|usamplerCube|usampler1DArray|usampler2DArray|sampler2DRect|sampler2DRectShadow|isampler2DRect|usampler2DRect|samplerBuffer|isamplerBuffer|usamplerBuffer|sampler2DMS|isampler2DMS|usampler2DMS|sampler2DMSArray|isampler2DMSArray|usampler2DMSArray|samplerCubeArray|samplerCubeArrayShadow|isamplerCubeArray|usamplerCubeArray|image1D|iimage1D|uimage1D|image2D|iimage2D|uimage2D|image3D|iimage3D|uimage3D|image2DRect|iimage2DRect|uimage2DRect|imageCube|iimageCube|uimageCube|imageBuffer|iimageBuffer|uimageBuffer|image1DArray|iimage1DArray|uimage1DArray|image2DArray|iimage2DArray|uimage2DArray|imageCubeArray|iimageCubeArray|uimageCubeArray|image2DMS|iimage2DMS|uimage2DMS|image2DMSArray|iimage2DMSArray|uimage2DMSArray|atomic_uint|mat2|mat3|mat4|dmat2|dmat3|dmat4|mat2x2|mat2x3|mat2x4|dmat2x2|dmat2x3|dmat2x4|mat3x2|mat3x3|mat3x4|dmat3x2|dmat3x3|dmat3x4|mat4x2|mat4x3|mat4x4|dmat4x2|dmat4x3|dmat4x4|vec2|vec3|vec4|ivec2|ivec3|ivec4|bvec2|bvec3|bvec4|dvec2|dvec3|dvec4|float|double|int|void|bool||uint|uvec2|uvec3|uvec4)\b", 
            RegexCompiledOption);

        ShaderFunctionNameRegex = new Regex(@"\b(struct)\s+(?<range>\w+?)\b", RegexCompiledOption);

        ShaderFunctionRegex = new Regex(
            @"\b(radians|degrees|sin|cos|tan|asin|acos|atan|sinh|cosh|tanh|asinh|acosh|atanh|pow|exp|log|exp2|log2|sqrt|inversqrt|abs|sign|floor|trunc|round|roundEven|ceil|fract|mod|modf|min|max|clamp|mix|step|smoothstep|isnan|isinf|floatBitsToInt|floatBitsToUInt|intBitsToFloat|uintBitsToFloat|fma|frexp|ldexp|packUnorm2x16|packSnorm2x16|packUnorm4x8|packSnorm4x8|unpackUnorm2x16|unpackSnorm2x16|unpackUnorm4x8|unpackSnorm4x8|packDouble2x32|unpackDouble2x32|packHalf2x16|unpackHalf2x16|length|distance|dot|cross|normalize|faceforward|reflect|refract|matrixCompMult|outerProduct|transpose|determinant|inverse|lessThan|lessThanEqual|greaterThan|greaterThanEqual|equal|notEqual|any|all|not|uaddCarry|usubBorrow|umulExtended|imulExtended|bitfieldExtract|bitfieldInsert|bitfieldReverse|findLSB|bitCount|findMSB|textureSize|textureQueryLod|textureQueryLevels|texture|textureProj|textureLod|textureOffset|texelFetch|texelFetchOffset|textureProjOffset|textureLodOffset|textureProjLod|textureProjLodOffset|textureGrad|textureGradOffset|textureProjGrad|textureProjGradOffset|textureGather|textureGatherOffset|textureGatherOffsets|texture1D|texture1DProj|texture1DLod|texture1DProjLod|texture2D|texture2DProj|texture2DLod|texture2DProjLod|texture3D|texture3DProj|texture3DLod|texture3DProjLod|textureCube|textureCubeLod|shadow1D|shadow2D|shadow1DProj|shadow2DProj|shadow1DLod|shadow2DLod|shadow1DProjLod|shadow2DProjLod|atomicCounterIncrement|atomicCounterDecrement|atomicCounter|atomicAdd|atomicMin|atomicMax|atomicAnd|atomicOr|atomicXor|atomicExchange|atomicCompSwap|imageSize|imageLoad|imageStore|imageAtomicAdd|imageAtomicMin|imageAtomicMax|imageAtomicAnd|imageAtomicOr|imageAtomicXor|imageAtomicExchange|imageAtomicCompSwap|dFdx|dFdy|fwidth|interpolateAtCentroid|interpolateAtSample|interpolateAtOffset|noise1|noise2|noise3|noise4|EmitStreamVertex|EndStreamPrimitive|EmitVertex|EndPrimitive|barrier|memoryBarrier|memoryBarrierAtomicCounter|memoryBarrierBuffer|memoryBarrierShared|memoryBarrierImage|groupMemoryBarrier)\b",
            RegexCompiledOption);

        ShaderKeywordRegex = new Regex(
                @"\b(coherent|attribute|const|uniform|varying|buffer|shared|volatile|restrict|readonly|writeonly|struct|layout|centroid|flat|smooth|noperspective|patch|sample|break|continue|do|for|while|switch|case|default|if|else|subroutine|in|out|inout|true|false|invariant|discard|return|lowp|mediump|highp|precision)\b|#define\b|#undef\b|#if\b|#ifdef\b|#ifndef\b|#else\b|#elif\b|#endif\b|#error\b|#pragma\b|#extension\b|#version\b|#line\b",
                RegexCompiledOption);

        ShaderReturnKeywordRegex = new Regex(
                @"\b(gl_NumWorkGroups|gl_WorkGroupSize|gl_WorkGroupID|gl_LocalInvocationID|gl_GlobalInvocationID|gl_LocalInvocationIndex|gl_VertexID|gl_InstanceID|gl_PerVertex|gl_Position|gl_PointSize|gl_ClipDistance|gl_in|gl_PrimitiveIDIn|gl_InvocationID|gl_Layer|gl_ViewportIndex|gl_PatchVerticesIn|gl_InvocationID|gl_out|gl_TessLevelOuter|gl_TessLevelInner|gl_PatchVerticesIn|gl_PrimitiveID|gl_TessCoord|gl_FragCoord|gl_FrontFacing|gl_ClipDistance|gl_PointCoord|gl_PrimitiveID|gl_SampleID|gl_SamplePosition|gl_SampleMaskIn|gl_Layer|gl_ViewportIndex|gl_FragDepth|gl_SampleMask|gl_VertexIndex)\b",
                RegexCompiledOption);
    }

    /// <summary>
    /// Highlights Shader code
    /// </summary>
    /// <param name="range"></param>
    public virtual void ShaderSyntaxHighlight(Range range)
    {
        /* range */
        range.tb.CommentPrefix = "//";
        range.tb.LeftBracket = '(';
        range.tb.RightBracket = ')';
        range.tb.LeftBracket2 = '{';
        range.tb.RightBracket2 = '}';
        range.tb.BracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;

        range.tb.AutoIndentCharsPatterns
            = @"
                ^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
                ^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
                ";

        //clear style of changed range
        range.ClearStyle(StringStyle, CommentStyle, NumberStyle, AttributeStyle, ClassNameStyle, StructNameStyle, EnumNameStyle, InterfaceNameStyle, KeywordStyle, ReturnKeywordStyle, KeywordStructStyle);

        //init c# regex
        if (CSharpStringRegex == null)
            InitShaderRegex();

        //
        //comment highlighting
        range.SetStyle(CommentStyle, ShaderCommentRegex1);
        range.SetStyle(CommentStyle, ShaderCommentRegex2);
        range.SetStyle(CommentStyle, ShaderCommentRegex3);
        //string highlighting
        range.SetStyle(StringStyle, ShaderStringRegex);
        //number highlighting
        range.SetStyle(NumberStyle, ShaderNumberRegex);
        //class name highlighting
        range.SetStyle(StructNameStyle, ShaderStructNameRegex);
        //keyword highlighting
        range.SetStyle(KeywordStyle, ShaderKeywordRegex);
        range.SetStyle(ReturnKeywordStyle, ShaderReturnKeywordRegex);
        //function highlighting
        range.SetStyle(FunctionStyle, ShaderFunctionRegex);
        //struct highlighting
        range.SetStyle(StructNameStyle, ShaderStructRegex);

        //dinamic names
        foreach (var r in currentTb.Range.GetRanges(CSharpKeywordStructRegex))
        {
            r.SetStyle(KeywordStructStyle, CSharpKeywordStructRegex);
        }

        //find document comments
        foreach (Range r in range.GetRanges(@"^\s*///.*$", RegexOptions.Multiline))
        {
            //remove C# highlighting from this fragment
            r.ClearStyle(StyleIndex.All);
            //do XML highlighting
            if (HTMLTagRegex == null)
                InitHTMLRegex();
            //
            r.SetStyle(CommentStyle);
            //tags
            foreach (Range rr in r.GetRanges(HTMLTagContentRegex))
            {
                rr.ClearStyle(StyleIndex.All);
                rr.SetStyle(CommentTagStyle);
            }
            //prefix '///'
            foreach (Range rr in r.GetRanges(@"^\s*///", RegexOptions.Multiline))
            {
                rr.ClearStyle(StyleIndex.All);
                rr.SetStyle(CommentTagStyle);
            }
        }

        //clear folding markers
        range.ClearFoldingMarkers();
        //set folding markers
        range.SetFoldingMarkers("{", "}"); //allow to collapse brackets block
        range.SetFoldingMarkers(@"/\*", @"\*/"); //allow to collapse comment block
    }

    #region Styles

    /// <summary>
    /// String style
    /// </summary>
    public Style StringStyle { get; set; }

    /// <summary>
    /// Comment style
    /// </summary>
    public Style CommentStyle { get; set; }

    /// <summary>
    /// Number style
    /// </summary>
    public Style NumberStyle { get; set; }

    /// <summary>
    /// C# attribute style
    /// </summary>
    public Style AttributeStyle { get; set; }

    /// <summary>
    /// Class name style
    /// </summary>
    public Style ClassNameStyle { get; set; }

    /// <summary>
    /// Enum name style
    /// </summary>
    public Style EnumNameStyle { get; set; }

    /// <summary>
    /// Struct name style
    /// </summary>
    public Style StructNameStyle { get; set; }

    /// <summary>
    /// Interface name style
    /// </summary>
    public Style InterfaceNameStyle { get; set; }

    /// <summary>
    /// Keyword style
    /// </summary>
    public Style KeywordStyle { get; set; }

    /// <summary>
    /// Return Keyword style
    /// </summary>
    public Style ReturnKeywordStyle { get; set; }

    /// <summary>
    /// Function style
    /// </summary>
    public Style FunctionStyle { get; set; }

    /// <summary>
    /// Keyword Class c# style
    /// </summary>
    public Style KeywordClassStyle { get; set; }

    /// <summary>
    /// Keyword Enum c# style
    /// </summary>
    public Style KeywordEnumStyle { get; set; }

    /// <summary>
    /// Keyword Struct c# style
    /// </summary>
    public Style KeywordStructStyle { get; set; }

    /// <summary>
    /// Keyword Enum c# style
    /// </summary>
    public Style KeywordInterfaceStyle { get; set; }

    /// <summary>
    /// Style of tags in comments of C#
    /// </summary>
    public Style CommentTagStyle { get; set; }

    /// <summary>
    /// Style of configs in Shader
    /// </summary>
    public Style ConfigStyle { get; set; }

    #endregion
}

/// <summary>
/// Language
/// </summary>
public enum Language
{
    CSharp,
    Shader,
    Normal,
}
