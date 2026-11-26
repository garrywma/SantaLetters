using Microsoft.Extensions.Logging;
using SantaLetters.Core.Services;
using SantaLetters.Services;
using SantaLetters.ViewModels;
using SantaLetters.Views;

namespace SantaLetters;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<IPresentCatalog, PresentCatalog>();
		builder.Services.AddSingleton<IQuestionBank, QuestionBank>();
		builder.Services.AddSingleton<ILetterStore>(_ => new JsonLetterStore(FileSystem.AppDataDirectory));
		builder.Services.AddSingleton<LetterDraft>();

		builder.Services.AddTransient<WelcomeViewModel>();
		builder.Services.AddTransient<PresentsViewModel>();
		builder.Services.AddTransient<LetterViewModel>();
		builder.Services.AddTransient<QuestionnaireViewModel>();
		builder.Services.AddTransient<SummaryViewModel>();
		builder.Services.AddTransient<WorkshopViewModel>();

		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<PresentsPage>();
		builder.Services.AddTransient<LetterPage>();
		builder.Services.AddTransient<QuestionnairePage>();
		builder.Services.AddTransient<SummaryPage>();
		builder.Services.AddTransient<WorkshopPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
