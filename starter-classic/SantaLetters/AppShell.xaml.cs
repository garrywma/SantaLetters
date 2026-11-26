using SantaLetters.Views;

namespace SantaLetters;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(PresentsPage), typeof(PresentsPage));

		// TODO: register routes for each new page you add, e.g.
		// Routing.RegisterRoute(nameof(LetterPage), typeof(LetterPage));
	}
}
