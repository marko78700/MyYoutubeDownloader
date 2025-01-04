
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace MyYoutubeDownloader;

public static class YoutubeService
{
    private static YoutubeClient _youtubeClient = new YoutubeClient();
    private static string _title = "";
    private static readonly string FilePath = "/Users/";
    
    public static async Task DownloadVideo(string sVideoUrl)
    {
        await GetInfosVideo(sVideoUrl);
        var streamListVideo = await GetStreamListVideo(sVideoUrl);
        var streamListAudio = await GetStreamListAudio(sVideoUrl);
        
        if (streamListVideo.Count > 0 && streamListAudio.Count > 0)
        {
            var streamInfoVideo = streamListVideo[GetUserChoiceVideo(streamListVideo)];
            var streamInfoAudio = streamListAudio[GetUserChoiceAudio(streamListAudio)];

            try
            {
                var streamInfos = new IStreamInfo[] { streamInfoAudio, streamInfoVideo };
                Console.WriteLine("Downloading...");
                await _youtubeClient.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder($"{FilePath}{_title}.{streamInfoVideo.Container}").Build());
                Console.WriteLine("Finished :-)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during the download: {e.Message}");
                throw;
            }
        }
    }

    private static async Task GetInfosVideo(string sVideoUrl)
    {
        var video = await _youtubeClient.Videos.GetAsync(sVideoUrl);
        
        Console.WriteLine("Video informations :");
        Console.WriteLine($"Title : {video.Title}");
        Console.WriteLine($"Channel : {video.Author.ChannelTitle}");
        Console.WriteLine($"Duration : {video.Duration}");
        Console.WriteLine();
        
        _title = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
    }

    private static int GetUserChoiceVideo(List<IVideoStreamInfo> streamList)
    {
        var userChoice = -1;
        Console.WriteLine("Choose the video quality");
        foreach (var (stream,idx) in streamList.Select((stream, idx) => (stream, idx)))
        {
            Console.WriteLine($"{idx} : {stream.VideoQuality.ToString()} / {stream.Bitrate}");
        }
        do
        {
            var sInput = Console.ReadLine() ?? string.Empty;
            if(int.TryParse(sInput, out userChoice) && userChoice >= 0 && userChoice < streamList.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
            }
        
        } while (true);
        
        return userChoice;
    }
    
    private static int GetUserChoiceAudio(List<IAudioStreamInfo> streamList)
    {
        var userChoice = -1;
        Console.WriteLine("Choose the audio quality");
        foreach (var (stream,idx) in streamList.Select((stream, idx) => (stream, idx)))
        {
            Console.WriteLine($"{idx} : {stream.AudioCodec.ToString()} / {stream.Bitrate}");
        }
        
        do
        {
            var sInput = Console.ReadLine() ?? string.Empty;
            if(int.TryParse(sInput, out userChoice) && userChoice >= 0 && userChoice < streamList.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
            }
        
        } while (true);
        
        return userChoice;
    }

    private static async Task<List<IVideoStreamInfo>> GetStreamListVideo(string sVideoUrl)
    {
        try
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(sVideoUrl);
       
            var streamList = streamManifest
                .GetVideoStreams()
                .Where(s => s.Container == Container.Mp4)
                .ToList();
            
            if (streamList.Count == 0)
            {
                Console.WriteLine("No video streams found.");
            }

            return streamList;

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching video streams: {e.Message}");
            throw;
        }
    }
    
    private static async Task<List<IAudioStreamInfo>> GetStreamListAudio(string sVideoUrl)
    {
        try
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(sVideoUrl);
       
            var streamList = streamManifest
                .GetAudioStreams()
                .Where(s => s.Container == Container.Mp4)
                .ToList();
            
            if (streamList.Count == 0)
            {
                Console.WriteLine("No audio streams found.");
            }

            return streamList;

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching audio streams: {e.Message}");
            throw;
        }
        
    }
}