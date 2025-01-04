// See https://aka.ms/new-console-template for more information

using MyYoutubeDownloader;

Console.WriteLine("################################");
Console.WriteLine("Welcome on MyYoutubeDownloader!");
Console.WriteLine("################################");
Console.WriteLine("");
Console.WriteLine("Insert a Youtube URL to download the video");
var sUrl = Console.ReadLine() ?? string.Empty;
await YoutubeService.DownloadVideo(sUrl);




