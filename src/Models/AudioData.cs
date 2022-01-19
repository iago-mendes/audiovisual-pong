using Microsoft.JSInterop;

namespace audiovisual_pong.Models
{
	public class AudioDataModel {
		IJSRuntime JSRuntime { get; set; }
		public int[] FrequencyData { get; private set; } = new int[0];
		public int Amplitude { get; private set; } = 0; // 0 - 255
		public int BassAmplitude { get; private set; } = 0; // 0 - 255
		public int MiddleAmplitude { get; private set; } = 0; // 0 - 255
		public int TrebbleAmplitude { get; private set; } = 0; // 0 - 255

		public AudioDataModel(IJSRuntime JSRuntime) {
			this.JSRuntime = JSRuntime;
		}
		public async Task UpdateAudioData() {
			await UpdateFrequencyData();
			UpdateAmplitude();
		}
		public void Reset() {
			this.FrequencyData = new int[0];
			this.Amplitude = 0;
			this.BassAmplitude = 0;
			this.MiddleAmplitude = 0;
			this.TrebbleAmplitude = 0;
		}
		private async Task UpdateFrequencyData() {
			string[] frequencyDataString = (await JSRuntime.InvokeAsync<string>("getAudioFrequencyData")).Split(';');
			this.FrequencyData = new int[frequencyDataString.Length];
			
			for (int i = 0; i < frequencyDataString.Length; i++)
				this.FrequencyData[i] = Int32.Parse(frequencyDataString[i]);
		}
		private void UpdateAmplitude() {
			if (FrequencyData.Length == 0) {
				this.Amplitude = 0;
				return;
			}

			int bassAmplitudeSum = 0;
			int middleAmplitudeSum = 0;
			int trebbleAmplitudeSum = 0;
			for (int i = 0; i < FrequencyData.Length; i++) {
				if (i < FrequencyData.Length/3)
					bassAmplitudeSum += FrequencyData[i];
				else if (i < 2 * FrequencyData.Length/3)
					middleAmplitudeSum += FrequencyData[i];
				else
					trebbleAmplitudeSum += FrequencyData[i];
			}
			
			int bassAmplitude = bassAmplitudeSum / (FrequencyData.Length / 3);
			int middleAmplitude = middleAmplitudeSum / (FrequencyData.Length / 3);
			int trebbleAmplitude = trebbleAmplitudeSum / (FrequencyData.Length / 3);
			int amplitude = (bassAmplitude + middleAmplitude + trebbleAmplitude) / 3;

			this.BassAmplitude = bassAmplitude;
			this.MiddleAmplitude = middleAmplitude;
			this.TrebbleAmplitude = trebbleAmplitude;
			this.Amplitude = amplitude;
		}
	}
}