# HAP Video Converter (Windows)

This application converts all video files in the current folder to HAP codec using FFmpeg. It processes `.mp4`, `.mov`, and `.avi` files, allowing you to include or exclude audio from the output files.

## Features

- Converts video files to HAP codec
- Optionally includes or removes audio from the converted files
- Automatically downloads and uses FFmpeg
- Outputs files to a subfolder `/HAP`

## How to Use

1. **Download and extract the executable** from the release on this repository.
2. **Place the executable in the folder** that contains the videos you want to convert.
3. **Run the executable** by double-clicking it.
4. **Choose whether to include audio** by typing `Y` for Yes or `N` for No when prompted.
5. The app will process all `.mp4`, `.mov`, and `.avi` files in the current folder and convert them to HAP format.
6. Converted files will be saved in a `/HAP` subfolder.

## Requirements

- **Windows OS**: This app is Windows-compatible only.
- **FFmpeg**: The app will automatically download FFmpeg for you if not already present.

## Notes

- The first time you run the app, it will download FFmpeg to `MyDocuments/ffmpeg`.
- The app only processes `.mp4`, `.mov`, and `.avi` files. Ensure your videos are in one of these formats.
- Converted videos will be saved in a `/HAP` folder in the same directory where the app is run.

## Troubleshooting

- If you encounter any issues, make sure you have a stable internet connection for downloading FFmpeg on the first run.
- Ensure the video files are in the correct format and placed in the same directory as the executable.
