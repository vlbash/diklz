localStorage.formCache = localStorage.formCache || '{}';

localStorageData = JSON.parse(localStorage.formCache);

this.saveToStorage = function (key, value) {
     localStorageData[key] = value;
     localStorage.formCache = JSON.stringify(localStorageData);
}

// this.onLoadManageCollapseBody = function () {

//      if (typeof localStorageData.collapsed != 'undefined' && !localStorageData.collapsed.length) {

          
//           if (localStorageData.collapsed) {
//                body.addClass('collapsed');
//                bodyCollapsed = true;
//                self.destroyAsideScrollBar();

//           } else {
//                body.removeClass('collapsed');
//                bodyCollapsed = false;

//           }
//           setTimeout(function () {
//                // body.css('transition-duration','');
//                // asideWrap.css('transition-duration','');
//           }, 500)

//      }
// }


// Listen to changes and save

// this.saveToStorage = function (key, value) {
//      localStorageData[key] = value;
//      localStorage.formCache = JSON.stringify(localStorageData);
// }

// this.loadFromStorage = function (valueMap) {
//      Object.keys(valueMap).forEach(function (name) {
//           var elem = document.querySelector('[name="' + name + '"]');
//           if (!elem) return;
//           elem.value = valueMap[name];
//      });
// }
// this.clearLocalStorage = function(){
//      localStorage.formCache = localStorage.formCache || '{}';
//      localStorage.clear();
// }

// $(":reset").on('click',self.clearLocalStorage);