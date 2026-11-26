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
		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<PresentsPage>();

		// TODO: register your new pages and view models here so Shell can inject their dependencies.

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
