using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems
{
    public sealed class BarkAccentSystem : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _random = default!;

        private static readonly IReadOnlyList<string> Barks = new List<string>{
            " Гав!", " ГАВ", " вуф-вуф"  // Corvax-Localization
        }.AsReadOnly();

        private static readonly IReadOnlyDictionary<string, string> SpecialWords = new Dictionary<string, string>()
        {
            //Corvax-Localization-Start
            { "га", "гаф" },
            { "Га", "Гаф" },
            { "угу", "вуф" },
            { "Угу", "Вуф" },
            //Corvax-Localization-End
        };

        public override void Initialize()
        {
            SubscribeLocalEvent<BarkAccentComponent, AccentGetEvent>(OnAccent);
        }

        public string Accentuate(string message)
        {
            foreach (var (word, repl) in SpecialWords)
            {
                message = message.Replace(word, repl);
            }

            return message.Replace("!", _random.Pick(Barks))
                .Replace("l", "r").Replace("L", "R")
                .Replace("л", "р").Replace("Л", "Р");
        }

        private void OnAccent(EntityUid uid, BarkAccentComponent component, AccentGetEvent args)
        {
            args.Message = Accentuate(args.Message);
        }
    }
}
