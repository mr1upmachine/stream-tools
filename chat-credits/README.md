# mr1upmachine's credits system

A chatter logging system that records who chatted during a stream, for use in an end-of-stream credits screen.

Creates a new file every time you start stream in the directory you specify during set up. The file is updated every time a chatter says their first words of the stream.

---

## Prerequisites

- [Streamer.bot](https://streamer.bot) installed (v1.0.4 or later)

## Installation

### 1. Copy the following text and use the "Import" button

```
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
```

<img src="assets/streamer-bot-import-button.png" width="600" alt="Advanced quote implementation in Streamer.bot">

### 2. Run this command in your Twitch chat

This command creates the necessary files and folders automatically.

```
!setcreditsdir C:\Users\Sean\Streaming\Documents\credits
```

If you'd rather set this value manually instead of using Twitch chat, you can set the global variable `creditsDir` to the desired path in the persisted global variables.

<img src="assets/credits-manual-example.png" width="600" alt="Manually set the global variable for credits in Streamer.bot">

---

## Commands

### !creditsnewfile

Creates a new file, in case the content changes dramatically mid stream.

**Usage:** `!creditsnewfile`

> **Permissions:** Moderators and broadcaster only.

### !creditsblocklistadd

Adds a username to the blocklist preventing them from showing up in the credits.

**Usage:** `!creditsblocklistadd <username>`

**Example:**

```
!creditsblocklistadd nightbot
```

> Accepts usernames with or without a leading `@`.

> **Permissions:** Moderators and broadcaster only.

---

### !creditsblocklistdel

Removes a username from the blocklist.

**Usage:** `!creditsblocklistdel <username>`

**Example:**

```
!creditsblocklistdel nightbot
```

> Accepts usernames with or without a leading `@`.

> **Permissions:** Moderators and broadcaster only.

---

### Configuration commands

These commands update the global variables that control where files are stored. Each confirms the change in chat.

> **Permissions:** Moderators and broadcaster only.

---

#### !setcreditsdir

Sets the folder where per-stream chatter log files are saved. Creates the folder if it does not already exist.

**Usage:** `!setcreditsdir <path>`

**Example:**

```
!setcreditsdir C:\Users\Sean\Streaming\Documents\credits
```
