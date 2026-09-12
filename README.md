# NoSmoothTime

This mod is for players who prefer a more dynamic in-game day-night cycle.
It smooths out the global lighting and can skip through the long night hours.

## Smoothing

Global lighting is updated on a per-frame basis rather than on a per-second one, thus eliminating visible light and shadow nudges.

### Disabled (Vanilla)
![Disabled](./assets/disabled.webp)

### Enabled
![Enabled](./assets/enabled.webp)

## Night Skip

Runs the clock faster between dusk and dawn, so a night at **60x** no longer takes 12 real minutes.
Daytime keeps the mission's own time factor.

![Night Skip](./assets/night_skip.webp)

> [!IMPORTANT]
> The clock is server-authoritative, so night skip only takes effect when you host the mission or play singleplayer.
> As a client you keep the server's clock; only the smoothing applies.
