# 🎅 Code Dojo: Letters to Santa

> Mrs Claus has been trying to modernise Santa's Christmas processes.
>
> One area causing real trouble is the sheer number of letters from children asking for presents.
> Children being children, the letters are often hard to read, and Santa doesn't want to give the wrong presents.
>
> Mrs Claus thinks children should have a **tablet app** to choose the presents they want and add anything
> else in a letter. It should also include a **naughty or nice questionnaire** to find out whether the child
> has been good this year.

Build that app in **.NET MAUI**, running on a **Meta (Facebook) Portal Mini**.

```
SantaLetters/
├── README.md         ← this brief
├── handout.pdf       ← 2-page printable attendee handout (source: handout.html)
├── starter/          ← give this to attendees (storybook theme)
├── starter-classic/  ← the original, simpler-themed starter
└── solution/         ← the facilitator's reference solution (don't peek!)
```

> 🎨 **Two looks:** the `main` branch has the original simple theme. The `deluxe-ux` branch has a
> "vintage storybook" theme in both `starter/` and `solution/`: parchment pages, Christmas fonts, twinkling
> fairy lights, falling snow, gift-wrapped present tiles, page-turn transitions and a wax-seal envelope
> animation when the letter is sent. On `deluxe-ux`, `starter-classic/` keeps the original starter for
> groups who'd rather start from something plainer.

All three folders have the same shape:

| Project | What it is |
| --- | --- |
| `SantaLetters` | The .NET MAUI app (targets Android for the Portal, plus Windows/iOS/Mac) |
| `SantaLetters.Core` | Plain .NET 10 library: models, present catalogue, questions and business rules |
| `SantaLetters.Tests` | xUnit tests for `SantaLetters.Core` |

---

## 🎯 The challenge

### User stories

1. **As a child** I can enter my name and age so that Santa knows who the letter is from.
2. **As a child** I can tap pictures of presents to choose up to **5**, so that I don't need to write neatly.
3. **As a child** I can write a letter to add anything else I'd like.
4. **As a child** I answer a short set of yes/no **Naughty or Nice** questions.
5. **As a child** I can see my letter, find out whether I'm on the Nice List, and **send it to Santa**.
6. **As Santa** I can open **Santa's Workshop** to read every letter with each child's naughty/nice verdict.
7. **As an elf** I can see a **production list** of how many of each present to make (nice children only!).

### Rules (already written as tests in `SantaLetters.Tests`)

| Kata | Class | Rule |
| --- | --- | --- |
| 1 | `NiceListCalculator` | Score 0-100 = weighted share of "nice" answers. Some questions are phrased naughtily (e.g. *"Have you told any fibs?"*), so **No** is the nice answer. Unanswered questions count as not nice. Verdicts: ≥85 Super Nice, ≥60 Nice, ≥35 Almost Nice, otherwise Naughty. |
| 2 | `LetterValidator` | Name required, age 1-16, 1-5 presents with no duplicates, letter ≤ 500 characters, every question answered. |
| 3 | `JsonLetterStore` | Letters are saved to a JSON file on the device, survive restarts, come back newest first, and can be deleted. |

### Suggested timeline (about 3 hours)

| Time | Activity |
| --- | --- |
| 0:00 | Brief, form pairs/mobs, check you can deploy the starter to a Portal (or emulator) |
| 0:20 | **Katas 1-3**: TDD in `SantaLetters.Core` (`dotnet test` should go green) |
| 1:00 | **Stage 1**: Presents page (tap-to-choose tiles) |
| 1:30 | **Stage 2**: Letter page + Naughty or Nice questionnaire |
| 2:10 | **Stage 3**: Summary + "Send to Santa" (validate, then save) |
| 2:30 | **Stage 4**: Santa's Workshop + elf production list |
| 2:50 | Show and tell on the Portals 🎄 |

### Stretch goals 🌟

- 🗂️ Category filter chips on the presents page (`PresentCategory` already exists).
- 🔒 Parental PIN to open Santa's Workshop, so children can't peek at the naughty list.
- 🔊 Text-to-speech (`TextToSpeech.Default.SpeakAsync`) so younger children hear each question.
- 🎤 Voice dictation for the letter (basic mic capture works on Portal; there's no Google speech service).
- ✨ Animations: falling snow, a sleigh flying off when the letter is sent.
- 🌍 Localisation for children around the world.
- 📊 Santa dashboard: letters by age, the most-wanted present, naughty/nice trends.
- 🧪 More tests, e.g. view model tests for the "max 5 presents" rule.

---

## 🧰 Setup

### Prerequisites

- .NET 10 SDK with the MAUI workloads:
  ```powershell
  dotnet workload install maui-android
  ```
  If a build fails with **NETSDK1147 "... must be installed: wasm-tools"**, run `dotnet workload restore`
  in the project folder (from an elevated prompt on Windows). The Mono toolchain that Android/iOS need
  ships in that workload.
- Android SDK (Visual Studio installs it; otherwise `dotnet build -t:InstallAndroidDependencies`).
- A USB-C cable for each Portal.

### Run the tests

```powershell
cd starter      # or solution
dotnet test SantaLetters.Tests
```

### Run the app

```powershell
# Fastest inner loop on a laptop (Windows):
dotnet build SantaLetters -f net10.0-windows10.0.19041.0 -t:Run

# On a Portal or Android emulator:
dotnet build SantaLetters -f net10.0-android -t:Run
```

Without a Portal, create an Android emulator with a **1280×800 landscape tablet** profile on **API 29**
(the Portal Mini's Android version).

---

## 📺 Deploying to a Portal Mini

Portals were discontinued at the end of 2022, but Meta has since enabled ADB so owners can sideload apps.

1. On the Portal: **Settings → Debug → ADB Enabled** (enter the PIN if prompted).
2. Connect USB-C to the back of the Portal and tap **Allow** on the "Allow USB debugging?" prompt.
3. Check it's visible: `adb devices -l`. If it's missing, toggle **ADB Enabled** again (it can race the USB connection).
4. Deploy: `dotnet build SantaLetters -f net10.0-android -t:Run`
   or install an APK: `adb install -r <path>\com.northpole.santaletters-Signed.apk`

> ⚠️ Facilitator: do a full test deploy to **every** Portal before the day. Firmware versions vary.

### Portal design constraints (worth putting on a slide)

| Constraint | What it means in MAUI |
| --- | --- |
| Android 10 (API 29), **no Google Play services** | No Firebase, no Google Maps, no Google sign-in. MAUI itself is fine. |
| 8" 1280×800, **landscape-first** | `ScreenOrientation.SensorLandscape` on `MainActivity`; design for wide layouts. |
| Used from 50-100cm, by children | Touch targets ≥ 64dp (primary buttons 96dp), body text ≥ 18-24sp. |
| System back/home/status pills overlay the **top ~64dp** | Keep content out of the top 64dp and use a dark header so the white pills stay visible (see `HeaderBand` in `Christmas.xaml`). |
| Launcher needs a **PNG** icon in `mipmap-xxxhdpi` | MAUI's `MauiIcon` generates this; adaptive-only icons appear blank. |
| Emoji support stops at **Emoji 12** | Avoid newer emoji (🪆, 🪥, 🫶 …) or they show as blank boxes. |
| No contacts or accounts APIs | Keep everything local to the app. |
| Small 8" screen, so content can overflow | Keep primary actions (Next, Send) in a fixed footer or header, not at the bottom of a `ScrollView`. |

> 💡 **Tappable tiles tip:** for a grid of selectable cards, use `CollectionView` with `SelectionMode="Single"` and
> clear `SelectedItem` after handling it (see `PresentsViewModel.TappedPresent`). It works for touch, mouse and
> accessibility, and avoids fragile `RelativeSource` command bindings from inside a `DataTemplate`.

---

## 🗺️ What's in the starter

✅ Provided:
- Christmas theme (`Resources/Styles/Christmas.xaml`) with Portal-friendly styles: `HeaderBand`, `PrimaryButton`, `SecondaryButton`, `RoundButton`, `Card`, `LetterPaper`, `Heading`, `Body`, `ErrorText`, `Footer`.
  - Storybook extras (`deluxe-ux`): the `BerryButton`, `Handwriting`, `Flourish`, `WaxSeal` and `WaxSealText` styles, and brushes such as `PageBrush`, `ParchmentBrush`, `NightBrush` and `BaubleBrush`.
  - Reusable controls in `Controls/`: `StoryPage` (a base page with a page-turn transition), `StoryHeader` (a title band with fairy lights, snow and an `Accessory` slot), `SnowView` and `FairyLightsView`.
  - `Converters/CategoryBrushConverter` gives each present category its own gradient.
  - Fonts from Google Fonts (Mountains of Christmas, Cinzel Decorative, IM Fell DW Pica, Kalam) are under the SIL Open Font License or the Apache License. The licence texts are in `Resources/Fonts/Licenses/`.- Working **Welcome page** (name and age) using CommunityToolkit.Mvvm and Shell navigation.
- `LetterDraft`: a singleton holding the letter while the child moves between pages.
- Domain models, a present catalogue (24 presents, all with Portal-safe emoji) and 9 Nice List questions.
- Dependency injection already wired up for the services.
- 27 tests describing the business rules (26 fail until you finish the katas).

🛠️ To build: the three katas, then Presents → Letter → Questionnaire → Summary → Santa's Workshop.

## 🧑‍🏫 Reference solution notes

`solution/` implements everything above with MVVM (CommunityToolkit.Mvvm source generators), Shell routing
and constructor-injected pages and view models. Letters are stored locally in
`FileSystem.AppDataDirectory/letters-to-santa.json`. Points worth discussing at the retrospective:

- Why put the rules in `SantaLetters.Core`? They're testable without a device, and the tests run in milliseconds.
- Why are some questions phrased "naughtily", and how does `NiceAnswer` handle that?
- Should children see their verdict? The reference only shows a friendly message and keeps the score for Santa.
- What would you change to send letters to a real North Pole API instead of local storage?
