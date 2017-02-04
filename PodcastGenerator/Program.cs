using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PodcastGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var title = "Le donjon de Naheulbeuk";
            var baseUrl = "https://ceres.pas-bien.net/naheulbeuk/";
            var copyright = "Pen of chaos";
            var keyword = "pen,chaos,naheulbeuk";
            var dir = @"P:\naheulbeuk\";

            var files = Directory.GetFiles(dir, "*.mp3", SearchOption.TopDirectoryOnly).OrderByDescending(l => l).ToList();

            var result = new StringBuilder();
            var refDate = DateTime.Now;

            result.AppendFormat(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<rss xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" version=""2.0"">
	<channel>
		<title>{0}</title>
		<link>{1}</link>
		<description>{0}</description>
		<language>fr</language>
		<copyright>{2}</copyright>
		<lastBuildDate>" + refDate.ToString("R") + @"</lastBuildDate>
		<generator>{2}</generator>
		<image>
			<url>{1}image.png</url>
			<title>{0}</title>
		    <link>{1}</link>
		</image>
		<itunes:author>{2}</itunes:author>
		<itunes:category text=""Arts""/>
		<itunes:explicit>no</itunes:explicit>
		<itunes:image href=""{1}image.png"" />
		<itunes:owner>
			<itunes:email>vincent.remond@gmail.com</itunes:email>
			<itunes:name>{2}</itunes:name>
		</itunes:owner>
		<itunes:subtitle>{0}</itunes:subtitle>
		<itunes:summary>{0}</itunes:summary>",title, baseUrl, copyright);


            foreach (var file in files)
            {
                refDate = refDate.AddDays(-1);

                var fileInfo = new FileInfo(file);
                Console.WriteLine(file);

                var f = TagLib.File.Create(fileInfo.FullName, TagLib.ReadStyle.Average);
                

                var name = fileInfo.Name.Split('.')[0].Replace("-", " ");
                var link = $"{baseUrl}{fileInfo.Name}";
                var pubDate = refDate.ToString("R");
                var length = fileInfo.Length.ToString();
                var duration = f.Properties.Duration.ToString("hh\\:mm\\:ss");

                result.AppendFormat(@"
		<item>
			<title>{0}</title>
			<link>{1}</link>
			<description>{2}</description>
			<author>{3}</author>
			<category>Arts</category>
			<enclosure url=""{1}"" length=""{6}"" type=""audio/mpeg""  />
			<guid>{1}</guid>
			<pubDate>{5}</pubDate>
			<itunes:author>{3}</itunes:author>
			<itunes:explicit>no</itunes:explicit>
			<itunes:keywords>{4}</itunes:keywords>
			<itunes:subtitle>{0}</itunes:subtitle>
			<itunes:summary>{2}</itunes:summary>
			<itunes:duration>{7}</itunes:duration>
		</item>
", name, link, name, copyright, keyword, pubDate, length, duration);

            }

            result.AppendLine(@"	</channel>
</rss>");
            File.WriteAllText(Path.Combine(dir, "episodes.xml"), result.ToString());
            Console.ReadLine();
        }
    }
}
