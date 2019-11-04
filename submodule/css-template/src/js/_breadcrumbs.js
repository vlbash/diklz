this.getBreadCrumbsList = function (cookieObj) {
  getCookie(cookieObj);
  var breadCrumbList;

  //check breadCrumbCookie
  function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    if (matches && decodeURIComponent(matches[1]) !== 'undefined') {
      breadCrumbList = JSON.parse(decodeURIComponent(document.cookie.split('=')[1]))
    } else {
      return;
    }
  }

  //check breadcrumb obj
  if (typeof breadCrumbList == 'undefined' || !breadCrumbList) {
    return;
  }

  //init
  var list = breadCrumbList,
    el = $('#bread-crumbs'),
    html = '';

  //add items to breadcrumbs
  if (list !== null && list.length > 0) {
    for (i = 0; list.length > i; i++) {
      var currItem = list[i],
        currName = currItem.Name,
        currLink = currItem.Link;
      if (i === 0) {
        html += '<li><a class="homepage-link" href="' + currLink + '" title="' + currName + '"><i class="icon-sm icon-home"></i></a></li>'
      } else if (i === list.length - 1) {
        html += '<li><span>' + currName + '</span></li>'
      } else {
        html += '<li><a href="' + currLink + '">' + currName + '</a></li>'
      }
    }
  }

  //hide homepage link
  var homePage = $(location).attr('origin') + '/';
  var currentPage = $(location).attr('href');
  if (homePage == currentPage) {
    $('#bread-crumbs').hide()
  }

  //set breadcrumbs template
  el.html(html);
}