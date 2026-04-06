# mr1upmachine's Streamer.bot Tools

## !quote

Search or retrieve quotes stored in Streamer.bot.

**Usage**

- `!quote <word>` — returns the first quote containing that word
- `!quote <number>` — returns the quote with that ID
- `!quote` — returns a random quote (configurable)

**Placeholders**

| Placeholder  | Description                   |
| ------------ | ----------------------------- |
| `%id%`       | Quote ID                      |
| `%quote%`    | Quote text                    |
| `%user%`     | User who was quoted           |
| `%game%`     | Game being played when quoted |
| `%platform%` | Platform (e.g. Twitch)        |
| `%date%`     | Date formatted as yyyy-MM-dd  |

**Inputs**

These values can be set either as a global variable or as an argument (which takes priority over globals).

| Variable             | Description                                   | Default                                   |
| -------------------- | --------------------------------------------- | ----------------------------------------- |
| `quoteTemplate`      | Template for a found quote                    | `Quote #%id%: %quote%`                    |
| `quoteNotFoundWord`  | Message when word search fails                | `No quotes found containing "%input%"`    |
| `quoteNotFoundId`    | Message when ID lookup fails                  | `No quote found with ID #%id%`            |
| `quoteNoQuotes`      | Message when quote list is empty              | `No quotes found`                         |
| `quoteUsage`         | Message when no input given and random is off | `Usage: !quote <word> or !quote <number>` |
| `quoteRandomOnEmpty` | Return a random quote when no input is given  | `true`                                    |

**Output**

Arguments that are set after the code is run.

| Argument          | Description                                                                   |
| ----------------- | ----------------------------------------------------------------------------- |
| `%quoteMessage%`  | The resulting message — use in a Send Message sub-action after this code runs |
| `%quoteExists%`   | Whether or not the quote was found                                            |
| `%quoteId%`       | Quote ID (only set if a quote was found)                                      |
| `%quote%`         | Quote text (only set if a quote was found)                                    |
| `%quoteUser%`     | User who was quoted (only set if a quote was found)                           |
| `%quoteGame%`     | Game being played when quoted (only set if a quote was found)                 |
| `%quotePlatform%` | Platform (only set if a quote was found)                                      |
| `%quoteDate%`     | Date formatted as yyyy-MM-dd (only set if a quote was found)                  |

**Examples**

The script will set an argument `%quoteMessage%` if you are sending a simple chat message.

<img src="assets/quote-basic-example.png" width="600" alt="Example quote implementation in Streamer.bot">

If you need to customize the messages, I encourage you to use the global variables feature and set one of the **inputs** above.

<img src="assets/quote-global-variable.png" width="600" alt="Example quote template variable in Streamer.bot">

If you need to do something more custom with the output, make sure to include logic to handle if the quote exists.

<img src="assets/quote-advanced-example.png" width="600" alt="Advanced quote implementation in Streamer.bot">
