using UnityEngine;

/// <summary>
/// Add to methods to expose a button in the editor, that when pressed, executes the method. 
/// WARNING: Does not work on methods that require params
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    private string buttonText = null;
    public string ButtonText => buttonText;

    private int spaceBefore;
    public int SpaceBefore => spaceBefore;

    private Color color = GUI.backgroundColor;
    public Color Color => color;

    /// <summary>
    ///  Button with custom text and set color.  NOTE: Only mark one color property as true. Later ones listed will overwrite previous ones.
    /// </summary>
    public EditorButtonAttribute(string buttonText, int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;

        if (white) color = Color.white;
        if (cyan) color = Color.cyan;
        if (blue) color = Color.blue;
        if (yellow) color = Color.yellow;
        if (green) color = Color.green;
        if (magenta) color = Color.magenta;
        if (red) color = Color.red;
        if (gray) color = Color.gray;
        if (black) color = Color.black;
    }
    /// <summary>
    /// Button with text as method name and set color.
    /// </summary>
    public EditorButtonAttribute(int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.spaceBefore = spaceBefore;

        if (white) color = Color.white;
        if (cyan) color = Color.cyan;
        if (blue) color = Color.blue;
        if (yellow) color = Color.yellow;
        if (green) color = Color.green;
        if (magenta) color = Color.magenta;
        if (red) color = Color.red;
        if (gray) color = Color.gray;
        if (black) color = Color.black;
    }
    /// <summary>
    /// Button with custom text and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with text as method name and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with custom text and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB, colorA);
    }
    /// <summary>
    /// Button with text as method name and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB, colorA);
    }
}