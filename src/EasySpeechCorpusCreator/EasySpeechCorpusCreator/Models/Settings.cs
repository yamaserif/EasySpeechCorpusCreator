using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static EasySpeechCorpusCreator.Consts.SettingsConst;

namespace EasySpeechCorpusCreator.Models
{
    public class Settings
    {
        public string ProjectPass { get; set; }

        public Format Format { get; set; }
        public string ExternalSoftware { get; set; }
        public string Device { get; set; }
        public int SamplingRate { get; set; }
        public string SaveVoiceFormat { get; set; }

        [JsonIgnore]
        public int DeviceId {
            get
            {
                var devices = AudioUtil.GetRecordingDevices();
                return devices.IndexOf(this.Device);
            }
        }

        public Settings(
            string projectPass = SettingsConst.DEFAULT_PROJECT_PASS,
            Format format = SettingsConst.DEFAULT_FORMAT,
            string externalSoftware = SettingsConst.DEFAULT_EXTERNAL_SOFTWARE,
            string device = SettingsConst.DEFAULT_DEVICE,
            int samplingRate = SettingsConst.DEFAULT_SAMPLING_RATE,
            string saveVoiceFormat = SettingsConst.DEFAULT_SAVE_VOICE_FORMAT)
        {
            this.ProjectPass = System.IO.Path.GetFullPath(projectPass);
            this.Format = format;
            this.ExternalSoftware = externalSoftware;
            this.Device = device;
            this.SamplingRate = samplingRate;
            this.SaveVoiceFormat = saveVoiceFormat;
        }
    }
}
