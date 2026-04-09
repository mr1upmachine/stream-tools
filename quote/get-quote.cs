using System;

public class CPHInline
{
    /*
     * https://github.com/mr1upmachine/streamerbot-tools/blob/main/get-quote.cs
     *
     * !quote <word>   — returns the first quote containing that word
     * !quote <number> — returns the quote with that ID
     * !quote          — shows usage message, or a random quote if quoteRandomOnEmpty is "true"
     *
     * MESSAGE TEMPLATES
     * -----------------
     * Templates are resolved in this order: action arg > global variable > default constant.
     *
     * Available placeholders:
     *   %id%       — quote ID number
     *   %quote%    — the quote text
     *   %user%     — the user who said the quote
     *   %game%     — the game being played when quoted (if none is found, set to "Unknown")
     *   %platform% — the platform (e.g. Twitch)
     *   %date%     — timestamp formatted as yyyy-MM-dd
     *
     * Global variables (set via Set Global Variable sub-action or a mod command):
     *   quoteTemplate        — template for a found quote
     *   quoteNotFoundWord    — message when no quote matches a word search
     *   quoteNotFoundId      — message when no quote matches a numeric ID
     *   quoteNoQuotes        — message when the quote list is empty
     *   quoteUsage           — message shown when no input is provided (only used if quoteRandomOnEmpty is false)
     *   quoteRandomOnEmpty   — if "true", !quote with no input returns a random quote instead of usage message
     *
     * Output:
     *   quoteMessage         — the resulting message (success or fail), set as an argument for use in subsequent sub-actions
     *   quoteExists          — whether or not the quote was found
     *   quoteId              — quote ID (only set if a quote was found)
     *   quote                — quote text (only set if a quote was found)
     *   quoteUser            — user who was quoted (only set if a quote was found)
     *   quoteGame            — game being played when quoted (only set if a quote was found)
     *   quotePlatform        — platform (only set if a quote was found)
     *   quoteDate            — date formatted as yyyy-MM-dd (only set if a quote was found)
     *
     * Example template:
     *   Quote #%id% (%user%, %game%): %quote%
     */
    private const string DefaultTemplate = "Quote #%id%: %quote%";
    private const string DefaultNotFoundWord = "No quotes found containing \"%input%\"";
    private const string DefaultNotFoundId = "No quote found with ID #%id%";
    private const string DefaultNoQuotes = "No quotes found";
    private const string DefaultUsage = "Usage: !quote <word> or !quote <number>";
    private const bool DefaultRandomOnEmpty = true;

    /* Resolve string variable from arguments or global context */
    private string ResolveString(string argKey, string globalKey, string defaultValue)
    {
        if (CPH.TryGetArg(argKey, out string argVal) && !string.IsNullOrWhiteSpace(argVal))
        {
            return argVal;
        }
        string global = CPH.GetGlobalVar<string>(globalKey, true);
        if (!string.IsNullOrWhiteSpace(global))
        {
            return global;
        }
        return defaultValue;
    }

    /* Resolve bool variable from arguments or global context */
    private bool ResolveBool(string argKey, string globalKey, bool defaultValue)
    {
        if (CPH.TryGetArg(argKey, out string argVal) && bool.TryParse(argVal, out bool parsed))
        {
            return parsed;
        }
        string global = CPH.GetGlobalVar<string>(globalKey, true);
        if (!string.IsNullOrWhiteSpace(global) && bool.TryParse(global, out bool globalVal))
        {
            return globalVal;
        }
        return defaultValue;
    }

    /* Format a quote using a template and QuoteData properties */
    private string FormatQuote(string template, QuoteData q)
    {
        return template
            .Replace("%id%", q.Id.ToString())
            .Replace("%quote%", q.Quote)
            .Replace("%user%", q.User ?? "")
            .Replace("%game%", q.GameName ?? "unknown")
            .Replace("%platform%", q.Platform ?? "")
            .Replace("%date%", q.Timestamp.ToString("yyyy-MM-dd"));
    }

    /* Set individual quote properties as arguments */
    private void SetQuoteArgs(QuoteData q)
    {
        CPH.SetArgument("quoteId", q.Id);
        CPH.SetArgument("quote", q.Quote);
        CPH.SetArgument("quoteUser", q.User ?? "");
        CPH.SetArgument("quoteGame", q.GameName ?? "unknown");
        CPH.SetArgument("quotePlatform", q.Platform ?? "");
        CPH.SetArgument("quoteDate", q.Timestamp.ToString("yyyy-MM-dd"));
    }

    /* Pick and return a random valid quote message */
    private string GetRandomQuote(string template, string noQuotes)
    {
        int count = CPH.GetQuoteCount();
        if (count == 0)
        {
            return noQuotes;
        }

        // Collect valid IDs first since there may be gaps from deleted quotes
        // HACK: check 10x past the quotes because Streamer.bot does not have a way to know all quote ids
        var validIds = new System.Collections.Generic.List<int>();
        for (int i = 1; i <= count * 10; i++)
        {
            try
            {
                CPH.GetQuote(i);
                validIds.Add(i);
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        if (validIds.Count == 0)
        {
            return noQuotes;
        }

        int pickId = validIds[new Random().Next(validIds.Count)];
        var pick = CPH.GetQuote(pickId);
        SetQuoteArgs(pick);
        return FormatQuote(template, pick);
    }

    public bool GetQuote()
    {
        // Get all the variables
        CPH.TryGetArg("rawInput", out string rawInput);
        string input = rawInput?.Trim() ?? "";

        string template = ResolveString("quoteTemplate", "quoteTemplate", DefaultTemplate);
        string notFoundWord = ResolveString("quoteNotFoundWord", "quoteNotFoundWord", DefaultNotFoundWord);
        string notFoundId = ResolveString("quoteNotFoundId", "quoteNotFoundId", DefaultNotFoundId);
        string noQuotes = ResolveString("quoteNoQuotes", "quoteNoQuotes", DefaultNoQuotes);
        string usage = ResolveString("quoteUsage", "quoteUsage", DefaultUsage);
        bool randomOnEmpty = ResolveBool("quoteRandomOnEmpty", "quoteRandomOnEmpty", DefaultRandomOnEmpty);

        // Handle !quote called with no input
        if (string.IsNullOrEmpty(input))
        {
            CPH.SetArgument("quoteMessage", randomOnEmpty ? GetRandomQuote(template, noQuotes) : usage);
            return false;
        }

        // Check if there are no quotes
        int count = CPH.GetQuoteCount();
        if (count == 0)
        {
            CPH.SetArgument("quoteMessage", noQuotes);
            return false;
        }

        // Attempt to get quote by id
        if (int.TryParse(input.TrimStart('#'), out int quoteId))
        {
            try
            {
                var q = CPH.GetQuote(quoteId);
                SetQuoteArgs(q);
                CPH.SetArgument("quoteMessage", FormatQuote(template, q));
                return true;
            }
            catch (Exception)
            {
                CPH.SetArgument("quoteMessage", notFoundId.Replace("%id%", quoteId.ToString()));
            }
            return false;
        }

        // Find first quote that matches
        for (int i = 1; i <= count; i++)
        {
            var q = CPH.GetQuote(i);
            if (q != null && q.Quote.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                SetQuoteArgs(q);
                CPH.SetArgument("quoteMessage", FormatQuote(template, q));
                return true;
            }
        }

        CPH.SetArgument("quoteMessage", notFoundWord.Replace("%input%", input));
        return false;
    }

    public bool Execute()
    {
        try
        {
            bool result = GetQuote();
            CPH.SetArgument("quoteExists", result);
            return result;
        }
        catch (Exception)
        {
            CPH.SetArgument("quoteMessage", "Something went wrong.");
            CPH.SetArgument("quoteExists", false);
            return false;
        }
    }
}