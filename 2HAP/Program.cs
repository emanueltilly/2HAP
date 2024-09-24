using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace FFmpegHapConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RenderHeader();

            // Step 1: Set a cross-platform path for FFmpeg binaries
            string ffmpegPath = GetFFmpegPath();
            Directory.CreateDirectory(ffmpegPath);

            // Step 2: Check if FFmpeg is already downloaded, if not, download it
            if (!Directory.EnumerateFiles(ffmpegPath).Any())
            {
                Console.WriteLine("Downloading the latest version of FFmpeg...");
                await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegPath);
            }
            else
            {
                Console.WriteLine("FFmpeg is already downloaded and will be reused.");
            }

            // Set the path for FFmpeg executables
            FFmpeg.SetExecutablesPath(ffmpegPath);

            string codec = "hap";

            // Step 3: Ask user if they want to include audio
            Console.WriteLine("Do you want to include audio? (Y/N):");
            bool includeAudio = Console.ReadLine().ToUpper() == "Y";

            // Step 4: Get all video files in the current directory
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] videoFiles = Directory.GetFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(file => file.EndsWith(".mp4") || file.EndsWith(".mov") || file.EndsWith(".avi"))
                .ToArray();

            if (videoFiles.Length == 0)
            {
                Console.WriteLine("No video files found in the current directory.");
                return;
            }

            // Step 5: Create output subfolder based on codec choice
            string outputDirectory = Path.Combine(currentDirectory,"HAP");
            Directory.CreateDirectory(outputDirectory);

            // Step 6: Process each video file and measure time
            foreach (var videoFile in videoFiles)
            {
                string outputFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(videoFile) + ".mov");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine($"Processing: {Path.GetFileName(videoFile)}");

                // Start timing the conversion
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var conversion = FFmpeg.Conversions.New()
                    .AddParameter($"-i \"{videoFile}\"")        // Input video file
                    .AddParameter($"-c:v {codec}");            // Codec for video

                // Optionally remove audio
                if (!includeAudio)
                {
                    conversion.AddParameter("-an");           // Remove audio
                }

                // Set output file path
                conversion.SetOutput(outputFileName);

                // Execute conversion
                await conversion.Start();

                // Stop timing and calculate the elapsed time
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                Console.WriteLine($"Finished processing {Path.GetFileName(videoFile)} -> {outputFileName}");
                Console.WriteLine($"Conversion time: {elapsedTime}");
            }

            Console.WriteLine("\nAll conversions finished.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task RenderHeader()
        {
            Console.WriteLine("░▒▓███████▓▒░  ░▒▓█▓▒░░▒▓█▓▒░  ░▒▓██████▓▒░  ░▒▓███████▓▒░  \r\n       ░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ \r\n       ░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ \r\n ░▒▓██████▓▒░  ░▒▓████████▓▒░ ░▒▓████████▓▒░ ░▒▓███████▓▒░  \r\n░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░        \r\n░▒▓█▓▒░        ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░        \r\n░▒▓████████▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░        ");
            Console.WriteLine("");
            Console.WriteLine("Release 0.1.0.0");
            Console.WriteLine("");
        }

        private static string GetFFmpegPath()
        {
            if (OperatingSystem.IsWindows())
            {
                // Use AppData folder for Windows
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ffmpeg");
            }
            else
            {
                // Use Home directory for Linux and macOS
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ffmpeg");
            }
        }
    }
}
