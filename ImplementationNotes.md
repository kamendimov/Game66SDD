# Game66 — Implementation Notes

This document records key implementation decisions and behaviors for the Game66 project.

## Project Configuration
- `EnableDefaultCompileItems` is toggled to `false` in the `.csproj` to explicitly include `Objects/**/*.cs`.
- Images are included as `Content` with `CopyToOutputDirectory` in the project file.

## Game Logic Notes
- `PlayCardCount` tracks played cards to calculate the next undealt index as `13 + PlayCardCount`.
- `CurrentCard` tracks the card played in the current round for both players.
- `ResetGame()` clears hands, scores, `CurrentCard`, and `TrumpCard`.
- `ChangeTrumpCard()` enforces the winning + zero-value trump card condition.

## Computer Player Strategy
- Computer response logic is split into helpers:
  - `GetComputerPlayerCardToPlay` — matching suit with highest value if it can beat the user.
  - `GetComputerPlayerCardToPlayByTrumpCard` — matching suit with minimal greater value (if any).
  - `GetComputerPlayerSmallestCard` — smallest card in hand when no matching suit is available.
