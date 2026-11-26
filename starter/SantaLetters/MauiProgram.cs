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
				fonts.AddFont("MountainsOfChristmas-Bold.ttf", "ChristmasTitle");
				fonts.AddFont("MountainsOfChristmas-Regular.ttf", "ChristmasTitleLight");
				fonts.AddFont("CinzelDecorative-Bold.ttf", "StoryCaps");
				fonts.AddFont("IMFellDWPica-Regular.ttf", "StoryBody");
				fonts.AddFont("IMFellDWPica-Italic.ttf", "StoryItalic");
				fonts.AddFont("Kalam-Regular.ttf", "Handwriting");
				fonts.AddFont("Kalam-Bold.ttf", "HandwritingBold");
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
