using EasySpeechCorpusCreator.Consts;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EasySpeechCorpusCreator.Business
{
    public static class AudioUtil
    {
        public static AudioStatus Status { get; set; } = AudioStatus.Idling;

        // 録音
        public static WaveInEvent? WaveIn { get; set; }
        public static WaveFileWriter? WaveWriter { get; set; }
        public static WaveOutEvent? WaveOut { get; set; }
        public static AudioFileReader? AudioReader { get; set; }

        public enum AudioStatus
        {
            Idling,
            Recording,
            Playing
        }

        public static void StartRecording(string savePath, int deviceNumber, int sampleRate)
        {
            if (AudioUtil.Status == AudioStatus.Playing)
            {
                AudioUtil.StopAudio();
                Thread.Sleep(100);
            }
            else if (AudioUtil.Status == AudioStatus.Recording)
            {
                AudioUtil.StopRecording();
                Thread.Sleep(100);
            }

            AudioUtil.Status = AudioStatus.Recording;

            AudioUtil.WaveIn = new WaveInEvent();
            AudioUtil.WaveIn.DeviceNumber = deviceNumber;
            AudioUtil.WaveIn.WaveFormat = new WaveFormat(sampleRate, WaveInEvent.GetCapabilities(deviceNumber).Channels);

            var directoryName = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directoryName) && directoryName != null)
            {
                Directory.CreateDirectory(directoryName);
            }

            AudioUtil.WaveWriter = new WaveFileWriter(savePath, AudioUtil.WaveIn.WaveFormat);

            AudioUtil.WaveIn.DataAvailable += (_, e) =>
            {
                AudioUtil.WaveWriter?.Write(e.Buffer, 0, e.BytesRecorded);
                AudioUtil.WaveWriter?.Flush();
            };
            AudioUtil.WaveIn.RecordingStopped += (_, __) =>
            {
                AudioUtil.WaveWriter?.Flush();
            };

            AudioUtil.WaveIn.StartRecording();
        }

        public static void StopRecording()
        {
            if (AudioUtil.Status == AudioStatus.Recording)
            {
                AudioUtil.WaveIn?.StopRecording();
                AudioUtil.WaveIn?.Dispose();
                AudioUtil.WaveIn = null;

                AudioUtil.WaveWriter?.Close();
                AudioUtil.WaveWriter?.Dispose();
                AudioUtil.WaveWriter = null;

                AudioUtil.Status = AudioStatus.Idling;
            }
        }

        public static void PlayAudio(string audioPath)
        {
            if (AudioUtil.Status == AudioStatus.Playing)
            {
                AudioUtil.StopAudio();
                Thread.Sleep(100);
            }
            else if (AudioUtil.Status == AudioStatus.Recording)
            {
                AudioUtil.StopRecording();
                Thread.Sleep(100);
            }

            AudioUtil.Status = AudioStatus.Playing;

            AudioUtil.WaveOut = new WaveOutEvent();
            AudioUtil.AudioReader = new AudioFileReader(audioPath);
            AudioUtil.WaveOut.Init(AudioUtil.AudioReader);
            AudioUtil.WaveOut.Play();
        }

        public static void StopAudio()
        {
            if (AudioUtil.Status == AudioStatus.Playing)
            {
                AudioUtil.WaveOut?.Stop();
                AudioUtil.WaveOut?.Dispose();
                AudioUtil.WaveOut = null;

                AudioUtil.AudioReader?.Close();
                AudioUtil.AudioReader?.Dispose();
                AudioUtil.AudioReader = null;

                AudioUtil.Status = AudioStatus.Idling;
            }
        }

        public static BitmapSource? ExWaveImage(string audioPath)
        {
            BitmapSource? returnImg = null;

            if (File.Exists(audioPath)){
                using (var waveStream = new AudioFileReader(audioPath))
                {
                    var maxPeakProvider = new MaxPeakProvider();
                    var settings = new SoundCloudBlockWaveFormSettings(
                        Color.FromArgb(102, 102, 102),
                        Color.FromArgb(103, 103, 103),
                        Color.FromArgb(179, 179, 179),
                        Color.FromArgb(218, 218, 218)
                    )
                    {
                        Name = "SoundCloud Light Blocks",
                        TopHeight = 300,
                        BottomHeight = 300
                    };

                    var renderer = new WaveFormRenderer();
                    var image = renderer.Render(waveStream, maxPeakProvider, settings);

                    // 見やすいように加工
                    using (var bitmap = new Bitmap(image))
                    {
                        var heightCenter = bitmap.Height / 2;
                        var width = bitmap.Width;
                        var outWidthSize = 30;
                        var outWidthSizeHalf = outWidthSize;

                        var isBreak = false;
                        while (outWidthSizeHalf < heightCenter && !isBreak)
                        {
                            isBreak = false;

                            var checkHeight = heightCenter - outWidthSizeHalf;
                            for (var i = 0; i < width; i += 1)
                            {
                                var checkPixel = bitmap.GetPixel(i, checkHeight);
                                if (checkPixel.ToArgb() != Color.White.ToArgb())
                                {
                                    outWidthSizeHalf += outWidthSize;
                                    isBreak = true;
                                    break;
                                }
                            }

                            if (!isBreak)
                            {
                                break;
                            }
                        }

                        var rect = new Rectangle(0, heightCenter - outWidthSizeHalf, width, outWidthSizeHalf * 2);
                        using (var trimmingBmp = bitmap.Clone(rect, bitmap.PixelFormat))
                        using (var resizeBmp = new Bitmap(trimmingBmp, width, outWidthSize))
                        using (var ms = new MemoryStream())
                        {
                            resizeBmp.Save(ms, ImageFormat.Bmp);
                            ms.Seek(0, SeekOrigin.Begin);
                            returnImg = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        }
                    }
                }
            }

            return returnImg;
        }
    }
}
