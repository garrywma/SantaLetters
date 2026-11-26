namespace SantaLetters.Controls;

/// <summary>
/// Base page for the storybook theme: each time a page appears its content gently
/// "turns in" (fades, slides and un-tilts) like the page of a book.
/// </summary>
public class StoryPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Content is not View content)
        {
            return;
        }

        content.AbortAnimation("PageTurn");
        content.Opacity = 0;
        content.TranslationX = 60;
        content.RotationY = -6;
        content.AnchorX = 0;

        new Animation
        {
            { 0, 1, new Animation(v => content.Opacity = v, 0, 1, Easing.CubicOut) },
            { 0, 1, new Animation(v => content.TranslationX = v, 60, 0, Easing.CubicOut) },
            { 0, 1, new Animation(v => content.RotationY = v, -6, 0, Easing.CubicOut) },
        }.Commit(content, "PageTurn", length: 420);
    }
}
