![PadPoll icon](./.github/assets/icon.svg)

# PadPoll

A simple application to test polling rate of controllers on Windows and Linux. It relies on low-level APIs provided by 
the operating systems to get the most accurate polling rate possible. On Windows it uses [GameInput API](https://learn.microsoft.com/en-us/gaming/gdk/docs/features/common/input/overviews/input-overview?view=gdk-2510) and on Linux it uses [evdev](https://docs.kernel.org/input/input.html#evdev).

> [!NOTE]
> The metrics provided by this application are intended as high-quality references rather than absolute laboratory values. Results may vary based on factors like OS scheduling and system load. However, because PadPoll utilizes the same low-level APIs used by modern games and engines to process controller input, these measurements provide a realistic representation of the latency you can expect during actual gameplay.

## Usage

### Windows

> [!IMPORTANT]
> As PadPoll currently relies on the GameInput API, it is only compatible with Windows 10 19H1 or later including all Windows 11 versions.

1. Ensure you have installed the [Microsoft GameInput API](https://learn.microsoft.com/en-us/gaming/gdk/docs/features/common/input/overviews/input-overview?view=gdk-2510), you can install it by executing:
```powershell
winget install Microsoft.GameInput
```

2. Download the latest Windows executable from the [releases page](https://github.com/PabloStarOk/padpoll/releases).
3. Run the executable and follow the on-screen instructions to test your controller's polling rate.

### Linux

1. Download the latest Linux executable from the [releases page](https://github.com/PabloStarOk/padpoll/releases).
2. Ensure the file is executable:
```bash
chmod +x padpoll
```
3. Run the executable and follow the on-screen instructions to test your controller's polling rate.

## Understand Metrics

PadPoll calculates and displays the following metrics:

- `Test Duration`: The total time taken to complete the polling test.
- `Collected Samples`: The total number of input samples collected during the test.

### Polling Rate

- `Average Hz`: The average polling rate in Hertz.
- `Peak Hz`: The highest polling rate recorded during the test.
- `Accuracy (%)`: The ratio between the measured average Hertz and the user-defined expected rate. This metric is calculated only when the user specifies an expected polling rate.
- `Consistency (%)`: The percentage of samples that arrived within a tight timing window of the expected interval. Higher values indicate a stable, predictable input stream (e.g., An input each 16ms for 60-Hz controller means 100% consistency). This metric is calculated only when the user specifies an expected polling rate.

### Latency

- `Average`: The average latency in milliseconds.
- `Minimum`: The minimum latency recorded during the test in milliseconds.
- `Maximum`: The maximum latency recorded during the test in milliseconds.
- `1% Lows Average`: The average of the worst 1% of latency samples in milliseconds, indicating the typical worst-case latency you can expect during gameplay.
- `Jitter (Standard Deviation)`: Calculated as the standard deviation of latency samples. It represents the overall stability of the input timing; a lower value indicates that input delivery is tightly clustered around the average. Measured in milliseconds.

## License

This project is open source and available under the [MIT License](LICENSE).
