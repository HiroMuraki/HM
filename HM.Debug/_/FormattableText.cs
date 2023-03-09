namespace HM.GameManager.GameModManager.ViewModels;

public sealed class FormattableText
{
    public string Text { get; } = string.Empty;

    public string Format(params object?[] args)
    {
        return string.Format(Text, args);
    }

    public override string ToString()
    {
        return Text;
    }

    public FormattableText(string text)
    {
        Text = text;
    }
}




