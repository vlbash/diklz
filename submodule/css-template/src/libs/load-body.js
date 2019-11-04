document.addEventListener("DOMContentLoaded", loadBody);
document.addEventListener("onload", afterLoadBody);

function loadBody() {
     localStorage.formCache = localStorage.formCache || '{}';
     var localStorageData = JSON.parse(localStorage.formCache);
     if (typeof localStorageData.collapsed != 'undefined' && !localStorageData.collapsed.length) {
          if (localStorageData.collapsed)
               document.body.classList.add('collapsed');
          document.getElementById('aside-wrapper').style.transitionDuration = '.15s';

          setTimeout(function () {
               document.body.classList.remove('before-load');
          }, 350)
     }
}

function afterLoadBody() {
     document.getElementById('aside-wrapper').style.opacity = '1';
     setTimeout(function () {
          document.getElementById('aside-wrapper').style.transitionDuration = '';
     }, 350)

}