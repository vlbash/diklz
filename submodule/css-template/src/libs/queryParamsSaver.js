localStorage.formCache = localStorage.formCache || '{}';

var data = JSON.parse(localStorage.formCache);

// Restore
loadFromStorage(data);

// Listen to changes and save
document.addEventListener('change',
    function(event) {
        saveToStorage(event.target.name, event.target.value);
    });

function saveToStorage(key, value) {
    data[key] = value;
    localStorage.formCache = JSON.stringify(data);
}

function loadFromStorage(valueMap) {
    Object.keys(valueMap).forEach(function(name) {
        var elem = document.querySelector('[name="' + name + '"]');
        if (!elem) return;
        elem.value = valueMap[name];
    });
}

$(":reset").click(function() {
    localStorage.formCache = localStorage.formCache || '{}';
    localStorage.clear();
});