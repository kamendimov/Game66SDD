# Game66 — Spec-Driven Development Document

## 1. Purpose
This document captures the product specification and implementation status for **Game66**, a C# WinForms implementation of the 66-style card game. It covers every user command issued from the start of the session through the addition of the `SetTwenty` and `SetForty` buttons.

---

## 2. Product Vision
Build a two-player card game (human vs. computer) where a human player competes against a computer opponent. The application manages a 24-card deck, trump selection, card replacement, round scoring, and special set declarations (20 / 40), rendered in a fixed-size WinForms window with custom-drawn cards.

---

## 3. Technology Constraints
- **Language:** C# (.NET 9.0)
- **UI Framework:** Windows Forms (WinForms)
- **Platform:** Windows
- **Form Size:** 1200 × 800 px
- **Form Border:** FixedSingle (maximize disabled)
- **Font:** Arial 11 pt
- **Card Rendering:** Custom painting using PNG images
- **Images:** 24 placeholder PNGs stored in `Images/` and included as `Content` with `CopyToOutputDirectory`

---

## 4. Functional Requirements

### 4.1 Form & Layout
- **REQ-UI-01:** Main form size is exactly `1200 × 800`.
- **REQ-UI-02:** Form border style is `FixedSingle`; maximize box is disabled.
- **REQ-UI-03:** Default font is Arial 11 pt.
- **REQ-UI-04:** User cards are displayed along the bottom of the screen.
- **REQ-UI-05:** Trump card is displayed at the top-right corner.
- **REQ-UI-06:** Played cards are displayed in the middle of the screen.
- **REQ-UI-07:** Scores are displayed at the top center.

### 4.2 Buttons & Controls
- **REQ-BTN-01:** `ResetGameButton` — resets the entire game state.
- **REQ-BTN-02:** `SetCardsButton` — shuffles the deck and distributes cards.
- **REQ-BTN-03:** `ChangeTrumpCardButton` — allows trump change when conditions are met.
- **REQ-BTN-04:** `SetTwentyButton` — calls `PlayTwenty()` and, if `UserPlayer.Score >= Game66.WIN_SCORE`, shows `"User Player won!"`.
- **REQ-BTN-05:** `SetFortyButton` — calls `PlayForty()` and, if `UserPlayer.Score >= Game66.WIN_SCORE`, shows `"User Player won!"`.
- **REQ-BTN-06:** `CloseGameButton` — calls `CloseTheGame()` to stop card dealing and clear hands.

### 4.3 Card Management
- **REQ-CARD-01:** The game uses a 24-card deck with indices 1–24.
- **REQ-CARD-02:** Each card has a name, value, index, and image path.
- **REQ-CARD-03:** Cards are rendered as PNG images via custom painting.
- **REQ-CARD-04:** Double-clicking a user card replaces it with a card from the undeck.
- **REQ-CARD-05:** Replacement cards are drawn starting from index 13, excluding the trump card.

### 4.4 Trump Mechanics
- **REQ-TRUMP-01:** One trump card is visible at the top-right.
- **REQ-TRUMP-02:** Trump can be changed only when:
  1. The user is currently winning, AND
  2. The user holds a zero-value card of the current trump type.

### 4.5 Playing & Computer Logic
- **REQ-PLAY-01:** User clicks a card to play it.
- **REQ-PLAY-02:** Played cards appear in the middle (user left, computer right).
- **REQ-PLAY-03:** Computer responds using:
  1. Matching suit with highest value if it can beat the user.
  2. Otherwise, matching suit with minimal greater value (if any).
  3. Otherwise, the smallest card in hand.
- **REQ-PLAY-04:** When a player reaches score >= `Game66.WIN_SCORE` after playing a card, a message box shows the winner:
  - If `ComputerPlayer.Score >= Game66.WIN_SCORE` → `"Computer Player won!"`
  - Else if `UserPlayer.Score >= Game66.WIN_SCORE` → `"User player won!"`

### 4.6 Scoring & Sets
- **REQ-SCORE-01:** Total scores are tracked for both players.
- **REQ-SCORE-02:** `PlayTwenty()` on `UserPlayer` awards 20 points for a non-trump 4-3 combination.
- **REQ-SCORE-03:** `PlayForty()` on `UserPlayer` awards 40 points for a trump 4-3 combination.
- **REQ-SCORE-04:** `SetTwentyButton` and `SetFortyButton` trigger `PlayTwenty()` and `PlayForty()` on `UserPlayer`.
- **REQ-SCORE-05:** `PlayTwenty()` on `UserPlayer` adds 20 points when:
  1. The user won the last round (`UserPlayer.LastRoundUserWon == true`)
  2. `UserPlayer` has a card with value 4 and a card with value 3
  3. Both cards are of the same suit
  4. Neither card is of the trump suit
- **REQ-SCORE-06:** `PlayForty()` on `UserPlayer` adds 40 points when:
  1. The user won the last round (`UserPlayer.LastRoundUserWon == true`)
  2. `UserPlayer` has a card with value 4 and a card with value 3
  3. Both cards are of the same suit
  4. Both cards are of the trump suit

### 4.7 Game End
- **REQ-GAME-01:** `CloseTheGame()` sets `gameClosed = true` when `UserPlayer.Score >= 1`.
- **REQ-GAME-02:** When `gameClosed` is `true`, neither player receives new cards after playing.
- **REQ-GAME-03:** In `PlaySelectedCard`, if `gameClosed` is `true` and `UserPlayer.Score >= Game66.WIN_SCORE`, both players' cards are cleared.

---

## 5. Acceptance Criteria

| ID | Criterion | Status |
|----|-----------|--------|
| AC-01 | Form builds and runs at 1200×800 with FixedSingle border and Arial 11pt. | ✅ Met |
| AC-02 | `ResetGameButton` clears all game state (hands, scores, trump, current cards). | ✅ Met |
| AC-03 | `SetCardsButton` shuffles deck and deals 6 cards to each player plus a trump card. | ✅ Met |
| AC-04 | Double-click on a user card replaces it from undeck starting at index 13 (trump excluded). | ✅ Met |
| AC-05 | Trump card is displayed top-right and can be changed only under win + zero-value conditions. | ✅ Met |
| AC-06 | Computer plays matching suit, highest value if winning; minimal greater value if not; otherwise smallest card. | ✅ Met |
| AC-07 | Scores update after each round; total scores displayed top-center. | ✅ Met |
| AC-08 | `SetTwentyButton` calls `PlayTwenty()` and shows `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`. | ✅ Met |
| AC-09 | 24 PNG card images are present and rendered via custom painting. | ✅ Met |
| AC-10 | Selected card is highlighted with a yellow border. | ✅ Met |
| AC-11 | `CloseTheGame()` sets `gameClosed = true` when `UserPlayer.Score >= 1`; no new cards are dealt after that. | ✅ Met |
| AC-12 | In `PlaySelectedCard`, when `gameClosed` is `true` and `UserPlayer.Score >= Game66.WIN_SCORE`, both players' cards are cleared. | ✅ Met |
| AC-13 | When a player reaches score >= `Game66.WIN_SCORE` after playing a card, a message box shows the winner. | ✅ Met |
| AC-14 | `UserPlayer.PlayTwenty()` adds 20 points when the user won the last round and holds a non-trump value-4 and value-3 card of the same suit. | ✅ Met |
| AC-15 | `UserPlayer.PlayForty()` adds 40 points when the user won the last round and holds a trump value-4 and value-3 card of the same suit. | ✅ Met |
| AC-16 | `SetFortyButton` calls `PlayForty()` and shows `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`. | ✅ Met |

---

## 6. Architecture & Components

### 6.1 Directory Layout
```
Cantace/
├── PlayScreen.cs
├── PlayScreen.Designer.cs
├── Objects/
│   ├── Game66.cs
│   ├── Player.cs
│   ├── ComputerPlayer.cs
│   ├── UserPlayer.cs
│   └── Card.cs
├── Images/
│   └── (24 PNG card images)
├── Cantace.csproj
└── Game66_SDD.md
```

### 6.2 Component Responsibilities
- **PlayScreen** — Main form; handles painting, mouse clicks, button events, score rendering, and card display.
- **Game66** — Core game engine; deck management, distribution, replacement, play logic, scoring, trump changes, and state synchronization to `UserPlayer` (`LastRoundUserWon`, `TrumpCard`).
- **Player (abstract)** — Base class with `Cards`, `CurrentCard`, `IncrementScore`, `ResetScore`, and `public static GetCardSymbol(string)`.
- **ComputerPlayer** — AI strategy implementation.
- **UserPlayer** — Human player logic; holds `LastRoundUserWon`, `TrumpCard`, and implements `PlayTwenty()` and `PlayForty()` for set evaluation.
- **Card** — Data model (`CardName`, `CardValue`, `Index`, `ImagePath`).

---

## 7. Implementation Notes
- `EnableDefaultCompileItems` is toggled to `false` in the `.csproj` to explicitly include `Objects/**/*.cs`.
- Images are included as `Content` with `CopyToOutputDirectory`.
- `PlayCardCount` tracks played cards to calculate the next undealt index as `13 + PlayCardCount`.
- `CurrentCard` tracks the card played in the current round for both players.
- `ResetGame()` clears hands, scores, `CurrentCard`, and `TrumpCard`.
- `WIN_SCORE` is a public constant in `Game66` set to `66`, replacing the magic number in score comparisons.
- `UserPlayer.LastRoundUserWon` tracks whether the user won the most recent round; it is synchronized from `Game66` and reset in `ResetGame()` and `DistributeCards()`.
- `UserPlayer.TrumpCard` mirrors the current trump card; it is synchronized from `Game66` after distribution and trump changes.
- `PlayTwenty()` and `PlayForty()` are implemented on `UserPlayer`; they award 20 and 40 points respectively when `LastRoundUserWon` is true and the user holds the required card combination.
- `Game66.GetUserPlayer()` exposes the `UserPlayer` instance so the UI can call `PlayTwenty()` and `PlayForty()`.
- `Player.GetCardSymbol(string)` is a public static helper used by `UserPlayer.PlayTwenty()`, `UserPlayer.PlayForty()`, and `Game66` suit-matching logic.
- `ChangeTrumpCard()` enforces the winning + zero-value trump card condition.
- `CloseTheGame()` sets `gameClosed = true` when `UserPlayer.Score >= 1`. When `gameClosed` is true, no new cards are dealt after playing.
- In `PlaySelectedCard`, after scoring, if `gameClosed` is `true` and `UserPlayer.GetScore() >= Game66.WIN_SCORE`, both players' cards are cleared.
- `PlayScreen_MouseClick` shows a winner message box when either player reaches score >= `Game66.WIN_SCORE` after playing a card.
- `PlayTwenty()` and `PlayForty()` methods moved from `Game66` to `UserPlayer`; `Game66` synchronizes `LastRoundUserWon` and `TrumpCard` to `UserPlayer`.
- `SetTwentyButton_Click` calls `game66.GetUserPlayer().PlayTwenty()`, shows `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`, then calls `Invalidate()`.
- `SetFourtyButton_Click` calls `game66.GetUserPlayer().PlayForty()`, shows `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`, then calls `Invalidate()`.
- Computer response logic is split into helpers: `GetComputerPlayerCardToPlay`, `GetComputerPlayerCardToPlayByTrumpCard`, `GetComputerPlayerSmallestCard`.

---

## 8. Current Status
All features requested through the `SetTwenty` / `SetForty` button command are implemented and the project builds successfully with `dotnet build`. Button click handlers for `SetTwenty` and `SetForty` are present as empty stubs.
- Card images in `G:\PROJECTS\Game66\Images` have been replaced with the images from `G:\PROJECTS\CSoft\Cantace\Images`.
- `CloseTheGame()` method added to `Game66` to set `gameClosed = true` when `UserPlayer.Score >= 1`. When `gameClosed` is true, no new cards are dealt after playing.
- `PlaySelectedCard` updated to clear both players' cards when `gameClosed` is `true` and `UserPlayer.Score >= Game66.WIN_SCORE`.
- `CloseGameButton` added to `PlayScreen` to trigger `CloseTheGame()`.
- `PlayScreen_MouseClick` shows a winner message box when either player reaches score >= `Game66.WIN_SCORE` after playing a card.
- `PlayTwenty()` method moved to `UserPlayer` to award 20 points when the user won the last round and holds a non-trump value-4 and value-3 card of the same suit.
- `SetTwentyButton_Click` implemented to call `game66.GetUserPlayer().PlayTwenty()`, show `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`, and refresh the UI.
- `PlayForty()` method moved to `UserPlayer` to award 40 points when the user won the last round and holds a trump value-4 and value-3 card of the same suit.
- `SetFourtyButton_Click` implemented to call `game66.GetUserPlayer().PlayForty()`, show `"User Player won!"` when `UserPlayer.Score >= Game66.WIN_SCORE`, and refresh the UI.
- `GetCardSymbol()` moved from `UserPlayer` and `Game66` to `Player` as a `public static` method; all callers updated.

---

## 9. Out of Scope
- Full scoring validation for 20 / 40 sets.
- Game-over detection and winner announcement.
- Sound effects, animations, or advanced UI polish.
