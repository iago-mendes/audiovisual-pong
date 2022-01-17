window.getWindowDimensions = () => (
	{
		x: window.innerWidth,
		y: window.innerHeight
	}
)

window.focusOnElement = (element) => element.focus()

window.playAudio = (elementId) => document.getElementById(elementId).play()

window.pauseAudio = (elementId) => document.getElementById(elementId).pause()

window.resetAudio = (elementId) => document.getElementById(elementId).currentTime = 0

window.getFileAsDataUri = async (fileUrl) => {
	let dataUri = ''

	await new Promise((resolve, reject) => {
		fetch(fileUrl)
			.then(res => res.blob())
			.then(res => {
				const reader = new FileReader()

				reader.addEventListener("load", function () {
					dataUri = this.result
					resolve()
				}, false)

				reader.readAsDataURL(res)
			})
	})

	return dataUri
}
