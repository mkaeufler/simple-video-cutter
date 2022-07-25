using FFmpeg.NET;
using FFmpeg.NET.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoCutter.FFmpegNET
{
    internal class FFmpegArgumentBuilder
    {
        public static string BuildArgumentsSingleCutOperation(string inputFileFullPath, string outputFileFullPath,
            TimeSpan start, TimeSpan end, bool lossless)
        {
            var commandBuilder = new StringBuilder();

            // Warning! There is a difference when placing 'ss' before or after 'i'.
            // See more in https://trac.ffmpeg.org/wiki/Seeking
            // Using output seeking since input seeking might cause an issue where the start time of the resulting
            // video equals the original timestamp of the starting cut instead of 0:00.

            if (lossless)
            {
                // Maybe also use -noaccurate_seek ?
                commandBuilder.AppendFormat(" -i \"{0}\" ", inputFileFullPath);
                commandBuilder.AppendFormat(" -codec copy -copyts ");
                commandBuilder.AppendFormat(CultureInfo.InvariantCulture, " -ss {0:0.000} ", start.TotalSeconds);
                commandBuilder.AppendFormat(CultureInfo.InvariantCulture, " -to {0:0.000} ", end.TotalSeconds);
                commandBuilder.AppendFormat(" -map 0:v -map 0:a ");
            }
            else
            {
                commandBuilder.AppendFormat(" -i \"{0}\" ", inputFileFullPath);
                commandBuilder.AppendFormat(" -copyts ");
                commandBuilder.AppendFormat(CultureInfo.InvariantCulture, " -ss {0:0.000} ", start.TotalSeconds);
                commandBuilder.AppendFormat(CultureInfo.InvariantCulture, " -to {0:0.000} ", end.TotalSeconds);
                commandBuilder.AppendFormat(" -map 0:v -map 0:a ");
            }

            return commandBuilder.AppendFormat(" \"{0}\" ", outputFileFullPath).ToString();
        }
    }
}