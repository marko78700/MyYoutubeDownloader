# MyYoutubeDownloader

![License](https://img.shields.io/github/license/marko78700/MyYoutubeDownloader?style=flat-square)
![GitHub issues](https://img.shields.io/github/issues/marko78700/MyYoutubeDownloader?style=flat-square)
![GitHub repo size](https://img.shields.io/github/repo-size/marko78700/MyYoutubeDownloader?style=flat-square)
![Platform](https://img.shields.io/badge/platform-.NET-blue?style=flat-square)

A minimal .NET console app that downloads a YouTube video by URL. The app is interactive: you run it, paste a YouTube URL, then choose which video and audio stream indexes to download/merge.

Important: Use this tool in accordance with YouTube's Terms of Service and only download content you have the right to access.

Table of contents
- Description
- How it works
- Requirements
- Build & Run
- Usage (interactive)
- File output location
- Troubleshooting
- Contributing
- License

Description
---
MyYoutubeDownloader is a simple cross-platform .NET console application that:
- Prompts the user for a YouTube URL.
- Displays video metadata (title, channel, duration).
- Lists available MP4 video streams and MP4 audio streams.
- Lets the user pick one video stream index and one audio stream index.
- Downloads and merges the selected audio+video streams into a single file.

How it works
---
- The app uses YoutubeExplode to fetch video metadata and stream manifests.
- It filters available streams to MP4 containers and shows them with indexes.
- The user types the index number for the video stream and the audio stream.
- The application downloads and merges the two streams using YoutubeExplode.Converter's ConversionRequestBuilder (ffmpeg is required for conversion/merging).

Requirements
---
- .NET SDK (6.0 or later recommended; adjust to the framework targeted by the project)
- ffmpeg installed and available in PATH (required for merging/conversion via the converter)
- Internet connection

Build & Run
---
1. Clone repository:
   git clone https://github.com/marko78700/MyYoutubeDownloader.git
   cd MyYoutubeDownloader

2. Build:
   dotnet build

3. Run from the project folder:
   dotnet run --project ./MyYoutubeDownloader

(Or run the published executable after publishing a release build:
   dotnet publish -c Release -r <RID> -o ./publish
Replace <RID> with win-x64, linux-x64, osx-x64, etc.)

Usage (interactive)
---
1. Start the app:
   dotnet run --project ./MyYoutubeDownloader

2. When prompted, paste a YouTube URL and press Enter.

3. The program prints video information (title, channel, duration) and lists available video stream options (index, quality, bitrate), then prompts:
   Choose the video quality

   Enter the index (e.g., 0) and press Enter.

4. Then it lists audio stream options (index, codec, bitrate), prompts:
   Choose the audio quality

   Enter the index and press Enter.

5. The app downloads and merges the selected audio+video streams. Progress is printed to the console.

File output location
---
- Currently the output path is hardcoded in the code to:
  /Users/<sanitized_title>.<video_container_extension>

- To change the output location, edit YoutubeService.cs and modify the FilePath constant:
  private static readonly string FilePath = "/Users/";

- The file name is based on the video title with invalid filename characters removed (joined with underscores).

Notes & troubleshooting
---
- If ffmpeg is not installed or not in PATH, merging may fail. Install ffmpeg and ensure running `ffmpeg -version` succeeds.
- If the program prints "No video streams found." or "No audio streams found.", the video may be unavailable in MP4 container streams; you can extend the code to support other containers if needed.
- If you get permissions errors writing to /Users/, change FilePath to a folder where you have write access.
- If the video is age-restricted/private, you may need cookies support (this project currently does not implement cookies authentication).
- The app currently requires interactive console input — there are no command-line flags.

Contributing
---
Contributions, fixes and improvements are welcome. Suggested workflow:
1. Fork the repo.
2. Create a feature branch.
3. Open a pull request with a clear description of changes.

If you want non-interactive usage or CLI options (output path, audio-only, playlist support), consider opening an issue or submitting a PR.

License
---
This project is released under the MIT License. See LICENSE file.