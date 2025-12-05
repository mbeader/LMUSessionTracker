window.addEventListener('DOMContentLoaded', function () {
	let check = document.querySelector('#autorefresh');
	check.checked = localStorage.getItem(check.id);
	if(check.checked)
		reload(check)
	check.addEventListener('click', autorefresh);
});

function reload(check) {
	setTimeout(function() {
		if(check.checked)
			window.location.reload();
	}, 10000);
}

function autorefresh(e) {
	let check = e.target;
	if(check.checked) {
		localStorage.setItem(check.id, check.checked);
		reload(check);
	} else {
		localStorage.removeItem(check.id);
	}
}