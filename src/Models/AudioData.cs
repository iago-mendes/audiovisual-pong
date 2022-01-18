using Microsoft.JSInterop;

namespace audiovisual_pong.Models
{
	public class AudioData {
		IJSRuntime JSRuntime { get; set; }
		public int[] FrequencyData { get; private set; } = new int[0];
		public int Amplitude { get; private set; } = 0; // 0 - 255

		public AudioData(IJSRuntime JSRuntime) {
			this.JSRuntime = JSRuntime;
		}
		public async Task UpdateAudioData() {
			await UpdateFrequencyData();
			UpdateAmplitude();

			// DEBUG
			Console.Write("FrequencyData: ");
			for (int i = 0; i < FrequencyData.Length; i++)
				Console.Write($"{FrequencyData[i]} ");
			Console.WriteLine();
			Console.WriteLine($"Amplitude: {Amplitude}");
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

			int amplitudeSum = 0;
			for (int i = 0; i < FrequencyData.Length; i++)
				amplitudeSum += FrequencyData[i];
			
			int amplitude = amplitudeSum / FrequencyData.Length;
			this.Amplitude = amplitude;
		}
	}
}