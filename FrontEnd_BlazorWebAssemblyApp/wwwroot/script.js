var isDarkMode = false;

function updateLightDarkInterface() {
	document.body.classList.remove('dark-mode');
	if (isDarkMode) {
		document.body.classList.add('dark-mode');
	}
}

function toggleDarkMode() {
	isDarkMode = !isDarkMode;
	updateLightDarkInterface();
}

function wireUpDarkModeButton() {
	const darkModeButtonId = document.getElementById('darkModeButton');
	darkModeButtonId.addEventListener('click', toggleDarkMode);
}

wireUpDarkModeButton();

var slider = document.getElementById('font-size-slider');

slider.addEventListener('input', function () {
	var size = slider.value;
	document.body.style.fontSize = size + "px";
});


function ExpandDetail(d) {
	if (document.getElementById(d).style.display == "none") {
		document.getElementById(d).style.display = "block";
	} else {
		document.getElementById(d).style.display = "none";
	}
}