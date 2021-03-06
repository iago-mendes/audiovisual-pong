let audioContext = null
let audioBuffer = null
let audioSource = null
let audioAnalyser = null
let hasAudioStarted = false
let isAudioPaused = false

function initWebAudioApi() {
	try {
		window.AudioContext = window.AudioContext || window.webkitAudioContext
		audioContext = new AudioContext()

		audioAnalyser = audioContext.createAnalyser()
		audioAnalyser.connect(audioContext.destination)
	}
	catch(e) {
		alert('Web Audio API is not supported in this browser!')
	}
}

function configSource() {
	audioSource = audioContext.createBufferSource()
	audioSource.buffer = audioBuffer
	audioSource.connect(audioAnalyser)
}

window.loadAudio = async (audioUrl) => {
	try {
		initWebAudioApi()

		await new Promise((resolve, reject) => {
			const request = new XMLHttpRequest()

			request.open('GET', audioUrl, true)
			request.responseType = 'arraybuffer'
			request.onload = async () => {
				await audioContext.decodeAudioData(request.response)
				.then(buffer => {
					audioBuffer = buffer
					resolve()
				})
				.catch(() => reject())
			}

			request.send()
		})

		configSource()
	
		const audioDuration = audioBuffer.duration.toFixed(0)
		return audioDuration
	} catch (error) {
		return '0'
	}
}

window.playAudio = () => {
	if (!hasAudioStarted) {
		audioSource.start(0)
		hasAudioStarted = true
	}
	
	if (isAudioPaused) {
		isAudioPaused = false
		audioContext.resume()
	}
}

window.pauseAudio = () => {
	audioContext.suspend()
	isAudioPaused = true
}

window.resetAudio = () => {
	if (audioSource)
		audioSource.stop()

	configSource()

	hasAudioStarted = false
}

window.getAudioFrequencyData = () => {
	let audioFrequencyData = new Uint8Array(audioAnalyser.frequencyBinCount)

	audioAnalyser.getByteFrequencyData(audioFrequencyData)
	const audioFrequencyDataString = audioFrequencyData.join(';')

	return audioFrequencyDataString
}
