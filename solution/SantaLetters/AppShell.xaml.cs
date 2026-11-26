using SantaLetters.Views;

namespace SantaLetters;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(PresentsPage), typeof(PresentsPage));
		Routing.RegisterRoute(nameof(LetterPage), typeof(LetterPage));
		Routing.RegisterRoute(nameof(QuestionnairePage), typeof(QuestionnairePage));
		Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
		Routing.RegisterRoute(nameof(WorkshopPage), typeof(WorkshopPage));
	}
}
