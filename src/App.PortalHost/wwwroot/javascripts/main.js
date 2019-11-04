var MainTemplates = function () {

     var self = this,
          htmlEl = document.getElementsByTagName("html")[0],
          body = $(document.body),
          bodyCollapsed = false,
          docWindow = $(window),
          winScrollTimer,
          winResizeTimer,
          windowHeight,
          windowWidth,
     
          mainWrapper = $('#wrapper'),
          rightBtnMenu = $('#content-btn') || false,
     
          headerWrap = $('#header-wrapper') || false,
     
          asideWrap = $('#aside-wrapper') || false,
          aside = $('#aside') || false,
          asideMenu = $('#aside-menu') || false,
          asideSwitcher = $('#aside-switcher') || false,
          asideScrollBar = null,
     
          selectScrollBar = null,
          selectDuration = 0,
          selectOpen = false,
     
          dialogWrapper = $('#dialog-wrapper') || false,
          dialogOpen = false,
     
          modal = $('#modal'),
          modalContainer = $('#modal .modal-container'),
          modalOpenBtn = $('[data-modal]'),
          modalCloseBtn = $('#modal-close'),
     
          headerMainMenu = $('#header-main-menu') || false,
          headerAccountMenu = $('#header-account-menu') || false,
          footerWrapper = $('#footer-wrapper') || false,
     
          timeOutInterval = 500,// interval for setTimeOut
     
          searchFormOpen = false, //todo
          searchPresetsOpen = false, //todo
     
          contentWrapper = $('#content-wrapper') || false,
          content = $('#content') || false,
     
          contentListConstArr = [],
     
          contentListConst,
          currLoaderParent,
          openTab,
          dataTabOpen = false,
          datePickerOpen = false,////todo
          localStorageData,
         
          userDisplaySettingsSaved = {
               windowWidth: 0, //1600
               setCurrDate: false,
               showTooltip: true,
               saveUser: true
          },
     
          presetFilterJson = '',
          defaultPresetFilter = false,
     
          testVar;
     
     
     // device detection
     var isTouchable = false;
     
     if (
          /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(
               navigator.userAgent
          ) ||
          /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(
               navigator.userAgent.substr(0, 4)
          )
     ) {
          isTouchable = true;
     }

     this.isScreenDeskTop = function(){
          if($('#is-screen-desktop').is(':visible')){
               return true;
          }
          return false;
     }
     this.isScreenTabletBig = function(){
          if($('#is-screen-tablet-big').is(':visible')){
               return true;
          }
          return false;
     }
     this.isScreenTablet = function(){
          if($('#is-screen-tablet').is(':visible')){
               return true;
          }
          return false;
     }
     this.isScreenMobileBig = function(){
          if($('#is-screen-mobile-big').is(':visible')){
               return true;
          }
          return false;
     }
     this.isScreenMobile = function(){
          if($('#is-screen-mobile').is(':visible')){
               return true;
          }
          return false;
     }

     moment.locale('uk');
     
     
     this.sortArrObjByMethod = function (arr, method) {
     
         function sortByMethod(arr1, arr2) {
             var a = arr1[method],
                 b = arr2[method],
                 temp;
     
             a = +a;
             b = +b;
             if (a != a || b != b) {//проверка на NaN, если NaN - сортируй, как string, иначе - как number
                 temp = arr1[method] - arr2[method];
             } else {
                 temp = a - b;
             }
             return temp;
         }
         var tempArr = arr.sort(sortByMethod);
     
         return tempArr;
     }
     
     this.addAttr = function (el, attrName, attrVal) {
         el.attr(attrName, attrVal);
     }
     this.addData = function (el, dataName, dataVal) {
         el.data(dataName, dataVal);
     }
     
     this.calculateWindowHeight = function() {
         return $(window).height();
     }
     this.calculateWindowWidth = function() {
         return $(window).width();
     }
     
     
     
     this.clearElEventListeners = function (el) {
         $(el).off();
         $(el).find('*').each(function () {
             $(this).off();
         });
     }
     
     this.generateId = function(){
         return '_' + Math.random().toString(36).substr(2, 9);
     }
     
     this.isElValueEmpty = function (el) {
         if($.trim($(el).val()) == ''){
             return true;
         }
         return false;
     }
     
     this.isStringEmpty = function name(str) {
         if($.trim(str) == ''){
             return true;
         }
         return false;
     }
     
     this.returnTagName = function (el) {
         return  $(el).prop('tagName').toLowerCase();
     }

     this.toggleActiveClass = function (el) {
         if ($(el).hasClass('active')) {
             self.removeActiveClass(el);
         } else {
             self.addActiveClass(el);
         }
     }
     
     this.addNotActiveClass = function (el) {
         $(el).addClass('not-active');
     }
     
     this.removeNotActiveClass = function (el) {
         $(el).removeClass('not-active');
     }
     this.addActiveClass = function (el) {
         $(el).addClass('active');
     }
     
     this.addNotDisabledClass = function (el) {
         $(el).addClass('disabled');
     }
     this.removeNotDisabledClass = function (el) {
         $(el).removeClass('disabled');
     }
     this.addDisabledClass = function (el) {
         $(el).addClass('disabled');
     }
     
     this.removeActiveClass = function (el) {
         $(el).removeClass('active');
     }
     
     this.toggleOpenClass = function (el) {
         if ($(el).hasClass('open')) {
             self.removeOpenClass(el);
         } else {
             self.addOpenClass(el);
         }
     }
     
     this.addOpenClass = function (el) {
         $(el).addClass('open');
     }
     
     this.removeOpenClass = function (el) {
         $(el).removeClass('open');
     }
     
     this.addTestClass = function (el) {
         $(el).addClass('test');
     }
     this.removeTestClass = function (el) {
         $(el).removeClass('test');
     }
     
     this.listColumnSwitch = function (el) {
         $(el).on('click', function () {
             $(mainWrapper).toggleClass("wrapper-column wrapper-list");
             self.updateNiceScroll();
         })
     }
     
     
     this.addEmptyNotEmptyClass = function (el) {
         if (!el.is(':visible')) {
             return
         }
     
         if (self.isElValueEmpty(el)) {
             $(el).removeClass('not-empty');
             $(el).addClass('empty');
     
         } else {
             $(el).removeClass('empty');
             $(el).addClass('not-empty');
     
         }
     
         if ($(el).closest('.datepicker-group')) {
             self.addDatePickerGroupEmptyNotEmptyClass(el);
         }
     }
     
     
     
     this.addDatePickerGroupEmptyNotEmptyClass = function (el) {
     
         var parent = $(el).closest('.datepicker-group'),
             children = $(parent).find('.input ,.select'),
             notEmpty = false;
     
         $(children).each(function () {
             if (!$(this).is(':visible') && self.returnTagName == 'input') {
                 return
             }
             if (!self.isElValueEmpty($(this))) {
                 notEmpty = true
             }
         })
     
         if (notEmpty) {
             $(parent).removeClass('empty');
             $(parent).addClass('not-empty');
     
         } else {
             $(parent).removeClass('not-empty');
             $(parent).addClass('empty');
         }
     
     }
     
     
     
     if ($('*').is(".btn-column-list")) {
         $('.btn-column-list').each(function () {
             self.listColumnSwitch($(this));
         })
     }
     if (mainWrapper) {
         
         $(mainWrapper).find('.btn-column-list').each(function(){
             if(mainWrapper.hasClass('wrapper-list')){
                 $(this).removeClass('icon-list').addClass('icon-columns'); 
             }
             if(mainWrapper.hasClass('wrapper-column')){
                 $(this).removeClass('icon-columns').addClass('icon-list');
             }
             $(this).on('click', function(){
                 if(mainWrapper.hasClass('wrapper-list')){
                     $(this).removeClass('icon-list').addClass('icon-columns'); 
                 }
                 if(mainWrapper.hasClass('wrapper-column')){
                     $(this).removeClass('icon-columns').addClass('icon-list');
                 }
             })
         });
     }

     // body.niceScroll({
     //      cursorcolor: "#e0e0e0",
     //      cursorwidth: "8px"
     // });
     this.updateNiceScroll = function () {
          body.getNiceScroll().resize();
     }
     
     
     this.createSelectScrollBar = function (el) { // может быть только один на странице в одно и тоже время - когда один элемент открываетсяы, иной закрывается, следить за вложенными элементыыми
          selectScrollBar = new PerfectScrollbar(el, {
               wheelSpeed: 0.5,
               minScrollbarLength: 20,
               maxScrollbarLength: 60
          });
     };
     this.destroySelectScrollBar = function () {
          if (selectScrollBar) {
               selectScrollBar.destroy();
               selectScrollBar = null;
          }
     };
     
     this.scrollTop = function () {
          var scrollTopButton = $('#scroll-top');
     
          //window scroll event
          $(window).scroll(function () {
               if ($(window).scrollTop() > windowHeight/4) {
                    $(scrollTopButton).removeClass('hidden');
               } else {
                    $(scrollTopButton).addClass('hidden');
               }
          })
     
          //button-scrollTop listener
          $(scrollTopButton).on('click', function(){
               $('html, body').animate({scrollTop : 0},600);
               $(this).addClass('hidden');
          })
     }

     this.showHideHeader = function () {
         if (docWindow.scrollTop() > headerWrap.outerHeight()) {
             headerWrap.addClass("top-fixed");
             aside.addClass("top-fixed");
             asideSwitcher.addClass("top-fixed");
     
             headerWrap.find('.menu-horizontal, .menu-vertical, .menu-side').each(function () { //скрываю все подменю
                 self.closeAllOpenLi($(this)); //menu.js
             })
     
             headerWrap.on('mouseover mouseenter', self.showHeader);
             headerWrap.on('mouseleave', self.hideHeader);
             setTimeout(function () {
                 headerWrap.css('transition-duration', '.35s');
             }, timeOutInterval)
         } else {
             headerWrap.removeClass("top-fixed");
             aside.removeClass("top-fixed");
             asideSwitcher.removeClass("top-fixed");
             setTimeout(function () {
                 headerWrap.css('transition-duration', '');
             }, timeOutInterval)
         }
     }
     
     this.showHeader = function () {
         if (!aside.hasClass('top-fixed')) {
             return;
         }
         headerWrap.css('top', '0');
         asideWrap.css('top', headerWrap.outerHeight())
         asideSwitcher.css('height', headerWrap.outerHeight())
     
     }
     
     this.hideHeader = function () {
         headerWrap.css('top', '');
         asideWrap.css('top', '');
         asideSwitcher.css('height', '')
         if (headerWrap.hasClass("top-fixed")) {
             headerWrap.find('.menu-horizontal, .menu-vertical, .menu-side').each(function () {
                 self.closeAllOpenLi($(this)); //menu.js
             })
         }
     
     }

     this.checkCollapsedBody = function(){
         if ($(body).hasClass('collapsed')) {
             bodyCollapsed = true; 
         }
     }
     
     
     this.createAsideScrollBar = function () {
         if (bodyCollapsed) {
             return;
         }
         if (aside){ //доробити
             asideScrollBar = new PerfectScrollbar(aside.get(0), {
                 wheelSpeed: .1,
                 minScrollbarLength: 20,
                 maxScrollbarLength: 60
             });
         }
     }
     
     this.destroyAsideScrollBar = function () {
         if (!asideScrollBar)
             return;
     
         asideScrollBar.destroy();
         asideScrollBar = null;
     }
     
     
     this.toggleCollapseBody = function () {
         if (body.hasClass('collapsed')) {
             self.unCollapseBody();
         } else {
             self.collapseBody();
         }
     }
     this.collapseBody = function () {
         self.saveToStorage('collapsed', true);
         bodyCollapsed = true;
         self.destroyAsideScrollBar();
         body.addClass('collapsed');
         self.closeAllOpenLi(asideMenu);
     
     }
     this.unCollapseBody = function () {
         self.saveToStorage('collapsed', false);
         bodyCollapsed = false;
         self.createAsideScrollBar();
         body.removeClass('collapsed');
         self.onLoadActiveLi(asideMenu);
     }
     
     
     this.showHideCollapsedArrow = function () {
     
         var asideMenuChildren = asideMenu.children(),
             asideMenuChildrenLength = asideMenuChildren.length,
             currChild,
             asideMenuHeight = 0,
             arrowHeight = parseInt($('#aside-arrow-top').css('height'))*2,
             headerHeight = $('header').outerHeight();
         if (!asideMenuChildrenLength)
             return;
     
         for (var i = 0; i < asideMenuChildrenLength; i++) {
             currChild = asideMenuChildren[i];
             asideMenuHeight += $(currChild).outerHeight();
         }
     
     
         if(windowHeight < asideMenuHeight + arrowHeight + headerHeight){
             $(aside).addClass('show-arrow')
         }else{
             $(aside).removeClass('show-arrow')
         }
     
     
     
     
     }
     
     
     
     
     this.manageAside = function () {
     
         if (bodyCollapsed) {
     
             this.showHideCollapsedArrow();
     
         } else {
             self.createAsideScrollBar();
         }
     
     
     }
     
     $(asideSwitcher).on('click', function () {
         self.toggleCollapseBody();
     })

     this.showHideFooter = function () {
         if (self.returnScrollHeight() - self.returnWindowHeight() - docWindow.scrollTop() < ($(footerWrapper).outerHeight() /12)) {///??????????????
             self.addActiveClass(footerWrapper)
         }else{
             self.removeActiveClass(footerWrapper)
         }
     
     }

     /*
     
     
     menuNavigation - функция управления меню
     
     параметры 
     nav - элемент, которым надо управлять
     autoHide (bool) - закрывается при клике вне элемента
     asideCollapsedMenu (bool) - боковое меню изменяет размер 
     closeSiblings (bool) - при открытии элемента закрывать другие
     
     
     manage open/close menu
     nav - menu which manage
     autoHide (true or false) - should menu close on body click or not, not required
     */
     
     this.menuNavigation = function (nav, autoHide, asideCollapsedMenu, closeSiblings) {
         self.addNavChevrons(nav, closeSiblings);
     
         if (autoHide) {
             self.onBodyClickHideNav(nav);
         }
     
         $(nav).on('click keypress', function (event) {
             
             var target = $(event.target),
                 currLi;
             if ( self.returnTagName(target) == 'a' || target.closest('a').length != 0) {
                 return;
             } else {
                 currLi = target.closest('li');
                 if (currLi.hasClass('open')) {
                     self.closeAllOpenLi(currLi);
                 } else {
                     if (closeSiblings) {
                         self.closeAllOpenLi(currLi.parent().find('.open'));
     
                     }
                     self.openOpenLi(currLi);
                 }
             }
         });
     
         // todo
         if (asideCollapsedMenu) {
     
             $(nav).children('li').children('.aside-menu-li-inner').on('mouseleave', function () {
     
                 if (!bodyCollapsed)
                     return;
     
                 self.removeOpenClass(nav.find('.open'));
             })
     
         }
     }
     
     
     this.addNavChevrons = function (nav, closeSiblings) {
         var needChevrons = $(nav).find('div.menu-item').closest('li');
         for (var i = 0, max = needChevrons.length; i < max; i++) {
             if ($(needChevrons[i]).find('ul').length) {
                 if (closeSiblings) {
                     $(needChevrons[i]).find('div.menu-item').addClass('menu-item-chevron');
     
                 } else {
                     $(needChevrons[i]).find('div.menu-item').addClass('menu-item-nested');
     
                 }
             }
         }
     }
     
     
     
     this.onBodyClickHideNav = function (nav) {
     
         body.on('click', function (event) {
             var target = $(event.target),
                 targets = target.parents();
             targets.push(target); //add target could be what we need
             for (var i = 0, max = targets.length; i < max; i++) {
                 if ($(targets[i]).is(nav)) {
                     return;
                 }
             }
             self.closeAllOpenLi(nav);
         })
     }
     
     
     this.closeAllOpenLi = function (currLi) {
         currLi.find('.open').each(function () {
             self.removeOpenClass($(this));
         })
     
         self.removeOpenClass(currLi);
         if (currLi.find('ul.menu-side').length) {
     
             self.hideSideMenu(currLi.find('ul.menu-side'));
         }
     }
     
     
     this.openOpenLi = function (currLi) {
     
         if (currLi.children().children('ul.menu-side').length) {
     
             self.addOpenClass(currLi);
             self.toggleSideMenu(currLi);
         } else if (currLi.children().children('ul').length) {
             self.addOpenClass(currLi);
             self.toggleOpenClass(currLi.children().children('ul'));
         }
     }
     
     /*
     to show active link in menu ol page loading
     */
     
     this.onLoadActiveLi = function (nav) {
         if (!$(nav).find('.active').length ) {
             return;
         }
     
         $(nav).find('.active').parentsUntil(nav).each(function () {
             if ($(this).parent(nav).length && self.returnTagName($(this)) == 'li') {
                 self.addActiveClass($(this)); //присваиваю класс active верхнему li
             }
     
             if(bodyCollapsed){
                 return;
             }
             if ( self.returnTagName($(this)) == 'ul' ||  self.returnTagName($(this)) == 'li') {
                 self.addOpenClass($(this));
             }
             
         })
     
     }
     
     
     
     
     
     /*
     on opening - give width before managing height
     on closing - hide side menu after  managing height
     timeOutTime - css transition-duration
     */
     
     this.hideSideMenu = function (sideMenu) {
     
         self.removeOpenClass(sideMenu);
         setTimeout(hideSideMenuAfterTimeOut, timeOutInterval);
     
         function hideSideMenuAfterTimeOut() {
             sideMenu.css({
                 'opacity': '',
                 'max-width': '0'
             })
         }
     }
     
     this.toggleSideMenu = function (el) {
     
         var sideMenu = el.children().children('ul.menu-side');
     
         if ($(el).hasClass('open')) {
             sideMenu.css({
                 'opacity': '1',
                 'max-width': ''
             });
     
             self.addOpenClass(sideMenu);
         } else {
     
             self.hideRightMenu(sideMenu);
         }
     }
     
     
     /*
     adding menus you want managing
     
     */
     
     if (asideMenu) {
         self.menuNavigation(asideMenu, false, true, false);
         docWindow.on('load', function () {
             self.onLoadActiveLi(asideMenu);
         });
     }
     if ($(headerMainMenu)) {
         self.menuNavigation(headerMainMenu, true, false, true);
     }
     if ($(headerAccountMenu)) {
         self.menuNavigation(headerAccountMenu, true, false, true);
     }

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

     
     this.formatTextDate = function (el) {
       var text = '';
       if (typeof $(el).data('date') !== 'undefined') {
         text = self.convertDate($(el).data('date'));
         if (typeof $(el).data('date-end') !== 'undefined') {
           text = text + ' - ' + self.convertDate($(el).data('date-end'));
         }
       } else if (typeof $(el).data('date-end') !== 'undefined') {
         text = self.convertDate($(el).data('date-end'))
       }
     
       $(el).text(text);
     
     }
     
     
     /// _autocomplete
     
     this.createJsGridDatePicker = function (el, date) {
       $(el).datepicker({
         dateFormat: 'dd.mm.yy',
         firstDay: 1,
       });
       if (date) {
         $(el).datepicker({
           'setDate': self.convertDate(date),
           'defaultDate': self.convertDate(date)
         });
       }
     }
     
     this.convertDate = function (date) {
       date = $.trim(date);
     
       var fullDate = new Date(date),
         year = fullDate.getFullYear(),
         month = fullDate.getMonth() + 1,
         day = fullDate.getDate();
     
       if (month < 10) {
         month = '0' + month;
       }
       if (day < 10) {
         day = '0' + day;
       }
     
       if (fullDate == 'Invalid Date') {
         return date;
       }
     
       return day + '.' + month + '.' + year;
     }
     
     
     this.convertDateForMoment = function (val) {
       var returnedArr = val.split('.'),
         day = returnedArr[0],
         month = returnedArr[1],
         year = returnedArr[2];
     
     
       return [year, month, day];
     }
     
     
     
     this.returnDateToIsoString = function (val) {
       var returnedDate = val.split('.'),
         day = +returnedDate[0],
         month = +returnedDate[1] - 1,
         year = +returnedDate[2];
     
       return new Date(year, month, day).toISOString();
     }
     
     
     this.normalizeDate = function (date) {
     
       var reg = /^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$/g;
       if (reg.test(date)) {
         return date;
       } else {
         return self.convertDate(date);
       }
     
     }
     this.checkDatepickerPair = function (el) {
     
       if ($(el).closest('.dates-group')) {
         var parent,
           childrenInputFrom,
           childrenInputTo,
           hiddenInputFrom,
           hiddenInputTo,
           maxValue,
           minValue;
     
         parent = $(el).closest('.dates-group');
         hiddenInputFrom = $(parent).find('.data-range-result[name$="_From"]');
         hiddenInputTo = $(parent).find('.data-range-result[name$="_To"]');
     
         minValue = $(hiddenInputFrom).val();
         maxValue = $(hiddenInputTo).val();
     
         childrenInputTo = $(parent).find('[id$="_To"]');
         childrenInputFrom = $(parent).find('[id$="_From"]');
         
         $(childrenInputTo).datepicker("option", "minDate", new Date(minValue));
         $(childrenInputFrom).datepicker("option", "maxDate", new Date(maxValue));
       }
     }
     this.createDatePicker = function (el) {
     
       if ($(el).val()) {
         $(el).val(self.normalizeDate($(el).val()));
       }
     
       var elName = $(el).attr('name');
       $(el).attr('name', '');
       $(el).parent().append('<input class = "data-range-result" type = "hidden" name = "' + elName + '"/>')
       $(el).datepicker({
         showButtonPanel: true,
         closeText: 'Закрити',
         prevText: '&#x3c;Поп',
         nextText: 'Наст&#x3e;',
         currentText: 'Сьогодні',
         monthNames: ['Січень', 'Лютий', 'Березень', 'Квітень', 'Травень', 'Червень',
           'Липень', 'Серпень', 'Вересень', 'Жовтень', 'Листопад', 'Грудень'
         ],
         monthNamesShort: ['Січ', 'Лют', 'Бер', 'Квіт', 'Трав', 'Черв',
           'Лип', 'Серп', 'Вер', 'Жовт', 'Лист', 'Груд'
         ],
         dayNames: ['неділя', 'понеділок', 'вівторок', 'среда', 'четвер', 'п\'ятница', 'субота'],
         dayNamesShort: ['нд', 'пн', 'вт', 'ср', 'чт', 'пт', 'сб'],
         dayNamesMin: ['Нд', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
         weekHeader: 'Ти',
         dateFormat: 'dd.mm.yy',
         firstDay: 1,
         isRTL: false,
         showMonthAfterYear: false,
         yearSuffix: '',
     
         defaultDate: new Date(),
     
         input: function () {
           // console.log('datepickerValue', $(this).val())
         },
     
         beforeShow: function (input, inst) {
           datePickerOpen = true; //todo
           if ($(this).hasClass('datepicker-input-select')) {
             $(this).parent().addClass('select-hidden')
           };
           $(this).closest('.input-col-2').addClass('dates-group');
         },
     
         onClose: function (input, inst) {
     
     
           if ($(this).hasClass('datepicker-input-select')) {
     
             if (self.isElValueEmpty($(this))) {
               $(this).parent().removeClass('select-hidden');
             }
           }
     
           $.when(self.setDataPickerHiddenValue($(this)))
             .done(self.checkDatepickerPair($(this)))
             .done(self.addEmptyNotEmptyClass($(this)));
           // pair check
           datePickerOpen = false; //todo
         }
       });
       self.setDataPickerHiddenValue(el, true);
     
       $('.datepicker-input-select').on('focus', function () {
         self.closeAllSelect(body);
       })
     }
     
     
     this.setDataPickerHiddenValue = function (el, onLoad) {
     
       var parent = $(el).closest('.datepicker-group'),
         dataPickerInput = $(parent).find('.hasDatepicker'),
         selectBefore = $(parent).find('.select-range-before') || false,
         selectAfter = $(parent).find('.select-range-after') || false,
         selectInput = $(parent).find('.select-range-input') || false,
         inputResult,
         inputValue;
       //todo: предустановленные фильтры
     
       if (onLoad) {
         inputValue = '';
         inputResult = '';
         if ($(dataPickerInput).val()) {
           inputValue = $(dataPickerInput).val();
           if (!self.isStringEmpty(inputValue)) {
             emptyDate = true;
             inputValue = self.returnDateToIsoString(inputValue);
             if (selectBefore.length || selectAfter.length) {
               // if
               inputResult = inputValue + '&' + inputValue;
             } else {
               inputResult = inputValue;
             }
           }
         }
       } else {
         inputValue = $(dataPickerInput).val();
     
         var currMsg = self.validateDate(dataPickerInput);
         if (currMsg) {
     
           self.showNotValidMsg(el, currMsg);
           return;
         }
     
         if (self.isStringEmpty(inputValue)) {
           if (selectBefore.length || selectAfter.length) {
             if (!self.isElValueEmpty(selectBefore) || !self.isElValueEmpty(selectAfter)) {
               inputResult = new Date().toISOString();
             }
     
             if (selectBefore.length && selectAfter.length) {
               inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + self.addDate(inputResult, ($(selectAfter).val()));
             } else if (selectBefore.length) {
               inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + inputResult;
             } else if (selectAfter.length) {
               inputResult = inputResult + '&' + self.addDate(inputResult, ($(selectAfter).val()));
             }
           } else if (selectInput.length) {
     
             if (!self.isElValueEmpty(selectInput)) {
               inputResult = self.subtractDate(new Date().toISOString(), ($(selectInput).val()))
             } else {
               inputResult = '';
             }
           } else {
             inputResult = '';
           }
         } else {
     
           inputResult = self.returnDateToIsoString(inputValue);
     
           if (selectBefore.length || selectAfter.length) {
             if (selectBefore.length && selectAfter.length) {
               inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + self.addDate(inputResult, ($(selectAfter).val()));
             } else if (selectBefore.length) {
               inputResult = self.subtractDate(inputResult, ($(selectBefore).val())) + '&' + inputResult;
             } else if (selectAfter.length) {
               inputResult = inputResult + '&' + self.addDate(inputResult, ($(selectAfter).val()));
             }
           }
           if (selectInput.length) {
             self.removeAttrSelected(selectInput);
             $(parent).removeClass('select-hidden');
           }
         }
     
       }
       $(parent).find('.data-range-result').val(inputResult);
     
     }
     
     
     
     this.subtractDate = function (date, str) {
       switch (str) {
         case 'day':
           return moment(date).subtract(1, 'days').toISOString();
     
         case '3day':
           return moment(date).subtract(3, 'days').toISOString();
     
         case 'week':
           return moment(date).subtract(1, 'week').toISOString();
     
         case 'month':
           return moment(date).subtract(1, 'month').toISOString();
     
         case 'quarter':
           return moment(date).subtract(3, 'month').toISOString();
     
         case 'halfYear':
           return moment(date).subtract(6, 'month').toISOString();
     
         case 'year':
           return moment(date).subtract(1, 'year').toISOString();
     
         default:
           return date;
       }
     }
     
     
     
     this.addDate = function (date, str) {
       switch (str) {
         case 'day':
     
           return moment(date).add(1, 'days').toISOString();
     
         case '3day':
           return moment(date).add(3, 'days').toISOString();
     
         case 'week':
           return moment(date).add(1, 'week').toISOString();
     
         case 'month':
           return moment(date).add(1, 'month').toISOString();
     
         case 'quarter':
           return moment(date).add(3, 'month').toISOString();
     
         case 'halfYear':
           return moment(date).add(6, 'month').toISOString();
     
         case 'year':
           return moment(date).add(1, 'year').toISOString();
     
         default:
           return date;
       }
     }
     
     
     
     this.setDatePickerInputSelectValue = function (el) {
     
       $(el).parent().find('.hasDatepicker').val('');
     
       self.setDataPickerHiddenValue($(this));
     
     }
     
     
     this.createDatePickerInputSelect = function (el) {
       var parent = $(el).closest('.datepicker-group');
       $.when($(el).after(self.createDatepickerSelect(false, false, true)))
         .done(
           function () {
             $(parent).addClass('datepicker-input-select-wrapper');
             self.createDatePicker(el);
             $(parent).find('.select-range-input').on('change', function () {
               $(parent).find('.hasDatepicker').val('');
               self.setDataPickerHiddenValue($(this));
             });
           }
         )
     }
     
     
     //dateRangeBefore
     
     
     this.createDateRangeBefore = function (el) {
       var parent = $(el).closest('.datepicker-group');
     
       $.when($(el).before(self.createDatepickerSelect(true, false, false)))
         .done(
           function () {
             self.createDatePicker(el);
             $(parent).addClass('date-range-before-wrapper');
             $(parent).find('.select-range-before').on('change', function () {
               self.setDataPickerHiddenValue($(this));
             });
           }
         )
     }
     
     //dateRangeAfter
     this.createDateRangeAfter = function (el) {
       var parent = $(el).closest('.datepicker-group');
       $.when($(el).after(self.createDatepickerSelect(false, true, false)))
         .done(
           function () {
             $(parent).addClass('date-range-after-wrapper');
             self.createDatePicker(el);
             $(parent).find('.select-range-after').on('change', function () {
               self.setDataPickerHiddenValue($(this));
             });
           }
         )
     }
     
     //dateRangeBeforeAfter
     
     this.createDateRangeBeforeAfter = function (el) {
       var parent = $(el).closest('.datepicker-group');
     
       $.when(
         $(el).before(self.createDatepickerSelect(true, false, false)).after(self.createDatepickerSelect(false, true, false))
     
       ).done(
         function () {
     
           $(parent).addClass('date-range-before-after-wrapper');
           self.createDatePicker(el);
           $(parent).find('.select-range-before').on('change', function () {
             self.setDataPickerHiddenValue($(this));
           });
           $(parent).find('.select-range-after').on('change', function () {
             self.setDataPickerHiddenValue($(this));
           });
         }
       )
     
     }
     
     this.createDatepickerSelect = function (selectBefore, selectAfter, selectInput) {
       var selectClass = 'select';
       if (selectBefore) {
         selectClass += " select-range-before";
       }
       if (selectAfter) {
         selectClass += " select-range-after";
       }
       if (selectInput) {
         selectClass += " select-range-input";
       }
       var select = '<select class = "' + selectClass + '">' +
         '<option value = ""></option>' +
         '<option value = "day">1 день</option>' +
         '<option value = "3day">3 дні</option>' +
         '<option value = "week">тиждень</option>' +
         '<option value = "month">місяц</option>' +
         '<option value = "quarter">3 місяці</option>' +
         '<option value = "halfYear">6 місяців</option>' +
         '<option value = "year">рік</option>' +
         '</select>';
       return select;
     }
     
     // if ($('input').is('.date-range-before-after')) {
     //       $('.date-range-before-after').each(function () {
     //             self.createDateRangeBeforeAfter($(this));
     //       })
     // }
     
     // if ($('input').is('.date-range-before')) {
     //       $('.date-range-before').each(function () {
     //             self.createDateRangeBefore($(this));
     //       })
     // }
     
     // if ($('input').is('.date-range-after')) {
     //       $('.date-range-after').each(function () {
     //             self.createDateRangeAfter($(this));
     //       })
     // }
     
     // if ($('input').is('.datepicker')) {
     //       $('.datepicker').each(function () {
     //             self.createDatePicker($(this));
     //       })
     // }
     
     
     this.findFormatDateParagraph = function (el) {
       $(el).find('.format-date').each(function () {
         self.formatTextDate($(this));
       })
     
     }

     ///https://ru.stackoverflow.com/questions/564448/
     
     this.closeAllSelect = function () {
        $('.select-gap').each(function () {
           self.closeDefinedSelect($(this));
        });
     };
     
     this.closeDefinedSelect = function (el) {
     
        if (!$(el).hasClass('select-gap')) {
           el = $(el).parent().find('.select-gap');
        }
     
        el.removeClass('on');
     
        var selectGroupInner = el.closest('.select-group-inner'),
           selectList = el.next('.select-list');
     
        if ($.trim(selectGroupInner.find('select').val()) != '' || $.trim(selectGroupInner.find('.select-gap').text()) != '') {
           selectGroupInner.addClass('not-empty');
        } else {
           selectGroupInner.removeClass('not-empty');
        }
        if ($(selectGroupInner).closest('.datepicker-group')) {
           self.addDatePickerGroupEmptyNotEmptyClass(selectGroupInner)
     
        }
     
        selectGroupInner.removeClass('on');
        selectList.slideUp(selectDuration);
        self.destroySelectScrollBar();
        selectList.removeAttr('id');
        selectOpen = false;
     };
     
     this.selectWrap = function (select) {
     
        if ($(select).is(':hidden') || !$(select).is(':visible')) {
           return
        }
        select.hide();
        // Wrap all
        select.wrap('<div class="select-group-inner"></div>');
     
        // Add decorative line
        $('<span>', {
           class: 'select-group-bar'
        }).insertAfter(select);
     
        var selectGroupBar = select.next('.select-group-bar');
     
        // Wrap all in select box
        $('<div>', {
           class: 'select-gap'
        }).insertAfter(selectGroupBar);
        var selectGap = selectGroupBar.next('.select-gap');
        // caret = selectGap.find('.caret');
     
        // Add ul list
        $('<ul>', {
           class: 'select-list'
        }).insertAfter(selectGap);
     
        self.createSelectList(select);
     };
     
     this.createSelectList = function (select) {
        var selectOption = select.find('option'),
           selectGroupInner = select.closest('.select-group-inner'),
           selectOptionLength = selectOption.length,
           selectWarning = false,
           selectGap = selectGroupInner.find('.select-gap'),
           selectList = selectGap.next('.select-list'),
           selectedOption = selectOption.filter(':selected');
        (selectGap).attr('tabindex', '0')
        if (selectedOption.length) {
           selectGap.text(selectOption.eq(selectedOption.index()).text());
           selectGroupInner.removeClass('select-readonly');
           if (!self.isStringEmpty($(selectGap).text())) {
              $(selectGroupInner).addClass('not-empty');
           }
        } else {
           selectGap.text('');
           selectGroupInner.addClass('select-readonly');
           selectGroupInner.removeClass('not-empty');
           return;
        }
        if ($(select).is('[readonly]')) {
           $(selectGroupInner).addClass('select-readonly')
           return;
        }
        if ($(selectGroupInner).closest('.datepicker-group')) {
           self.addDatePickerGroupEmptyNotEmptyClass(selectGroupInner)
     
        }
     
        selectDuration = select.css('transition-duration');
     
        if (select.hasClass('select-warning')) {
           selectWarning = true;
        }
     
        // Add li - option items
        for (var i = 0; i < selectOptionLength; i++) {
           $('<li>', {
                 class: 'select-item',
                 html: $('<span>', {
                    text: selectOption.eq(i).text()
                 })
              })
              .attr('data-value', selectOption.eq(i).val())
              .attr('data-disabled', selectOption.eq(i).attr('disabled'))
              .appendTo(selectList);
        }
        var selectItem = selectList.find('li');
        selectList.slideUp(0);
     
        selectGap.on('click keydown', function () {
           if (!$(this).hasClass('on')) {
              if (selectOpen) {
                 self.closeAllSelect();
              }
     
              $(this).addClass('on');
              selectGroupInner.addClass('on');
              selectOpen = true;
     
              selectList.slideDown(selectDuration);
     
              self.createSelectScrollBar(selectList.get(0));
     
              selectItem.off().on('click keydown', function (e) {
                 if (selectWarning && select.val()) {
                    createSelectDialog(e, $(this));
                 } else {
                    setSelectValue($(this));
                 }
              });
           } else {
              self.closeDefinedSelect($(this));
           }
        });
     
        function setSelectValue(el) {
           var chooseItem = el.data('value');
           $(select).val(chooseItem).attr('selected', 'selected');
           selectGap.text(el.find('span').text());
           self.closeDefinedSelect(selectGap);
           $(select).trigger('change');
        }
     
        function createSelectDialog(e, el) {
           var msg = '<p>При зміні сутності, введенні права доступу для полей сутності видаляться автоматично</p>';
     
           function confirmDialog() {
              return setSelectValue(el);
           }
     
           function refuseDialog() {
              return self.closeAllSelect();
           }
           self.createDialog(e, msg, confirmDialog, refuseDialog);
        }
     };
     
     this.removeAttrSelected = function (select) {
        $(select).val('');
        $(select).find('option').removeAttr('selected');
        select.closest('.select-group-inner').find('.select-gap').text('');
        self.closeDefinedSelect(select);
     }
     
     this.updateSelectList = function (select) {
        self.clearSelectGroupInner(select);
        self.createSelectList(select);
     };
     
     this.clearSelectGroupInner = function (select) {
        var selectGroupInner = select.closest('.select-group-inner');
        self.clearElEventListeners(selectGroupInner);
        $(selectGroupInner).find('.select-list').html('');
     }
     
     
     this.setSelectValue = function (select, value) {
     
        $(select).find('option').each(function () {
           $(this).removeAttr('selected');
        })
        $(select).find('[value = ' + value + ']').attr('selected', 'selected');
        self.updateSelectList(select);
     }
     
     this.returnSelectedOptionText = function (select) {
        var options = $(select).find('option'),
           currOption,
           selectValue = $(select).val();
        for (var i = 0; i < options.length; i++) {
           currOption = options[i];
           if ($(currOption).val() == selectValue) {
              return $(currOption).text();
           }
        }
     }

     this.createAccordion = function (options) {
         var settings = $.extend({
             speed: 300,
             accordionItemSelector: ".accordion-item",
             accordionItemHeaderSelector: ".accordion-item-header",
             accordionItemContentSelector: ".accordion-item-content"
         }, options);
         var $this = $(this);
     
         $(document).on('click', settings.accordionItemSelector, function (e) {
             var $this = $(this);
     
             if (!$this.hasClass("active")) {
     
                 $this.find(settings.accordionItemContentSelector).slideDown(settings.speed);
                 $this.addClass("active");
     
             } else {
                 $this.find(settings.accordionItemContentSelector).slideUp(settings.speed);
                 $this.removeClass("active");
             }
             setTimeout(function () {
                 self.updateNiceScroll($this);
             }, 300);
         });
         $(document).on('click', settings.accordionItemHeaderSelector + " a", function (e) {
             e.stopPropagation();
         });
         $(document).on('click', settings.accordionItemContentSelector, function (e) {
             e.stopPropagation();
         });
         $(document).keypress(function (e) {
             if (e.which == 13 && $(e.target).parents($this)) {
                 $(e.target).trigger('click');
             }
         });
     }
     
     if ($('.accordion').is(':not(.not-active)')) {
         $('.accordion').each(function () {
             self.createAccordion($(".accordion"));
         })
     }

     this.createDialog = function(e, msg, yesCallback, noCallback) {
       var dialogInner = $("#dialog-inner");
     
         dialogOpen = true;
     
       if (yesCallback && noCallback){
         dialogInner
           .find(".btn-holder")
           .html(
             "<button id='dialog-refuse' class='btn btn-danger btn-outline'>Відмовитися</button><button id='dialog-confirm' class='btn btn-secondary btn-outline'>Підтвердити</button>"
           );
     
         setYesCallbackListener();
         setNoCallbackListener();
       } else if (noCallback) {
         dialogInner
           .find(".btn-holder")
           .html(
             "<button id='dialog-refuse' class='btn btn-danger btn-outline'>OK</button>"
           );
           
         setNoCallbackListener();
       } else if (yesCallback) {
         dialogInner
         .find(".btn-holder")
         .html("<button id='dialog-confirm' class='btn btn-secondary btn-outline'>Підтвердити</button>");
     
         setYesCallbackListener();
       } else {
         dialogInner
           .find(".btn-holder")
           .html(
             "<button id='dialog-confirm' class='btn btn-secondary btn-outline'>OK</button>"
           );
     
         setYesCallbackListener();
       }
     
       // if (e) {
       //   var X = e.pageX,
       //     Y = e.pageY,
       //     dialoginnerWidth = dialogInner.outerWidth(),
       //     dialoginnerHeight = dialogInner.outerHeight(),
       //     winWidth = self.calculateWindowWidth();
       //     winHeight = self.calculateWindowHeight();
     
       //   if (X + dialoginnerWidth > winWidth) {
       //     X = winWidth - dialoginnerWidth - 15;
       //   }
       //   if (Y + dialoginnerHeight > winHeight) {
       //     Y = winHeight - dialoginnerHeight - 15;
       //   }
       //   dialogInner.css({
       //     position: "absolute",
       //     top: Y - dialoginnerHeight + "px",
       //     left: X + "px"
       //   });
       // }
     
       // dialogInner.draggable();
     
       $("#dialog-text").html(msg);
     
       self.showDialogWrapper();
     
       function setYesCallbackListener() {
         $("#dialog-confirm")
           .off()
           .on("click", function() {
             self.hideDialogWrapper();
             if (yesCallback) yesCallback();
           });
       }
     
       function setNoCallbackListener() {
         $("#dialog-refuse")
           .off()
           .on("click", function() {
             self.hideDialogWrapper();
             noCallback();
           });
       }
     };
     
     this.showDialogWrapper = function() {
       self.addOpenClass(dialogWrapper);
       self.addActiveClass(dialogWrapper);
     };
     
     this.hideDialogWrapper = function() {
       self.removeActiveClass(dialogWrapper);
       setTimeout(function() {
         $("#dialog-confirm").off();
         $("#dialog-refuse").off();
         $("#dialog-text").html("");
         $("#dialog-btn").html("");
         // $("#dialog-inner").css({
         //   top: "",
         //   left: ""
         // });
         // $("#dialog-inner").draggable("destroy");
         self.removeOpenClass(dialogWrapper);
         dialogOpen = false;
       }, timeOutInterval);
     };

     this.selectDataTemplate = function (data, searchPanel) {
       switch (data.type) {
     
         case 'dataGroup':
           var groupLabel = '',
             dataGroupHtml;
     
           if (typeof data.labelName !== 'undefined') {
             groupLabel = '<label> ' + data.labelName + ' </label>';
           }
     
           dataGroupHtml =
             '<div class="input-col-2">' + groupLabel + '<div class="input-col-2-inner">';
     
           var temp;
           for (var h = 0; h < data.children.length; h++) {
             temp = self.selectDataTemplate(data.children[h], searchPanel);
             dataGroupHtml += temp;
           }
     
           dataGroupHtml += '</div></div>';
           
           return dataGroupHtml;
     
         case 'select':
           return self.createSelectTemplate(data);
     
         case 'datepicker':
           return self.createDatePickerTemplate(data, searchPanel);
     
         case 'hidden':
           return self.createHiddenInputTemplate(data);
     
         case 'datepickerBefore':
           return self.createDatePickerBeforeTemplate(data);
     
         case 'datepickerAfter':
           return self.createDatePickerAfterTemplate(data);
     
         case 'datepickerBeforeAfter':
           return self.createDatepickerBeforeAfterTemplate(data);
     
         case 'checkbox':
           return self.createCheckboxTemplate(data);
     
         case 'checkbox-tree':
           return self.createCheckboxTreeTemplate(data);
     
         case 'checkbox-tree-child':
           return self.createCheckboxTreeTemplate(data);
     
         case 'text':
           return self.createTextInputTemplate(data);
     
         default:
           return self.createTextInputTemplate(data);
       }
     }
     
     
     this.createTextInputTemplate = function (data) {
       var validateAttr = '';
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       }
     
       return '<div class="input-group">' +
         '<input class = "input" type="text" autocomplete="off" name = "' +
         data.name +
         '"' + validateAttr + '/>' +
         '<span class="input-group-bar">' +
         '</span><label>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     }
     
     
     this.createHiddenInputTemplate = function (data) {
       var val;
       if (typeof data.value !== 'undefined') {
         val = data.value;
       } else {
         val = '';
       }
       return '<input type="hidden" name = "' + data.name + '" value = "' + val + '"/>';
     };
     
     this.createCheckboxTemplate = function (data) {
       var value = '';
       if (data.value) {
         value = ' checked';
       }
     
       return '<div class="checkbox-group">' +
         '<div class="checkbox-group-inner">' +
         '<input id = "' +
         data.id +
         '" class="checkbox" type="checkbox" name="' +
         data.name +
         '"' +
         value +
         '/>' +
         '<label for="' +
         data.id +
         '">' +
         data.labelName +
         '</label>' +
         '</div><span class="checkbox-group-bar"></span></div>';
     };
     
     this.createCheckboxTreeTemplate = function (data) {
       var value = '';
       if (data.value) {
         value = ' checked';
       }
     
       return '<li class="tree-group"><span>'+ data.Code +'</span>' +
         '<input id = "' +
         data.Code +
         '" class="" type="checkbox" name="' +
         data.Name +
         '"' +
         value +
         '/>' +
         '<label for="' +
         data.Code +
         '">' +
         data.Name +
         '</label>' +
         '</li>';
     };
     
     this.createDatePickerTemplate = function (data, searchPanel) {
     
       var searchForm = '';
       if (searchPanel) {
         searchForm = '-input-select'
       }
     
       var validateAttr = '';
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       };
     
       return '<div class="datepicker-group input-group">' +
         '<input class = "input datepicker' + searchForm + '" type="text"' + 'id="' + data.name + '"' + 'name="' +
         data.name + '"' + validateAttr + '/>' +
         '<span class="input-group-bar"></span>' +
         '<label for="' + data.name + '"' + '>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     
     };
     
     
     this.createDatePickerAfterTemplate = function (data) {
       var validateAttr = '';
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       };
     
       return '<div class="datepicker-group input-group">' +
         '<input class = "input date-range-after" type="text" name="' +
         data.name +
         '"' + validateAttr + '/>' +
         '<span class="input-group-bar"></span>' +
         '<label>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     };
     
     
     
     
     
     this.createDatePickerBeforeTemplate = function (data) {
       var validateAttr = '';
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       };
     
       return '<div class="datepicker-group input-group">' +
         '<input class = "input date-range-before" type="text" name="' +
         data.name +
         '"' + validateAttr + '/>' +
         '<span class="input-group-bar"></span>' +
         '<label>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     };
     
     
     
     this.createDatepickerBeforeAfterTemplate = function (data) {
       var validateAttr = '';
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       };
       return '<div class="datepicker-group input-group">' +
         '<input class = "input date-range-before-after" type="text" name="' +
         data.name +
         '"' + validateAttr + '/>' +
         '<span class="input-group-bar"></span>' +
         '<label>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     };
     
     this.createSelectTemplate = function (data) {
     
       var validateAttr = '',
         onchangeUrl = '';
     
       if (typeof data.validate !== 'undefined') {
         validateAttr = self.createValidateAttr(data.validate);
       }
     
       if (typeof data.onchangeUrl !== 'undefined' || $.trim(data.onchangeUrl) != '') {
         onchangeUrl = ' onchange-url = "' + data.onchangeUrl + '"';
       }
     
       var optionsHtml = '<option value = ""></option>',
         currOption;
     
     
       if (typeof data.selectOptions === 'string') {
         data.selectOptions = JSON.parse(data.selectOptions);
       }
     
       if (data.selectOptions && typeof data.selectOptions != typeof undefined) {
         for (var k = 0, maxK = data.selectOptions.length; k < maxK; k++) {
           currOption = data.selectOptions[k];
           optionsHtml += '<option ';
           if (currOption.Disabled) {
             optionsHtml += 'disabled ';
           }
           if (currOption.Selected) {
             optionsHtml += 'selected ';
           }
           optionsHtml += 'value="' + currOption.Value + '">' + currOption.Text + '</option>';
         }
       }
     
     
       return '<div class="select-group">' +
         '<select name="' +
         data.name +
         '" class="select"' +
         onchangeUrl +
         validateAttr + '>' +
         optionsHtml +
         '</select><label>' +
         data.labelName +
         '</label><span class="js-validation"></span></div>';
     };
     
     
     
     this.activateFormControls = function (el) {
       if ($(el).find('.datepicker')) {
         $(el).find('.datepicker').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.createDatePicker($(this));
         });
       }
       if ($(el).find('.datepicker-input-select')) {
         $(el).find('.datepicker-input-select').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.createDatePickerInputSelect($(this));
         });
       }
       if ($(el).find('.date-range-before')) {
         $(el).find('.date-range-before').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.createDateRangeBefore($(this));
         });
       }
       if ($(el).find('.date-range-before-after')) {
         $(el).find('.date-range-before-after').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.createDateRangeBeforeAfter($(this));
         });
       }
       if ($(el).find('.date-range-after')) {
         $(el).find('.date-range-after').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.createDateRangeAfter($(this));
         });
       }
       if ($(el).find('.select')) {
         $(el).find('.select').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.selectWrap($(this));
         });
       }
       if ($(el).find('.input')) {
         $(el).find('.input').each(function () {
           if ($(this).hasClass('active')) {
             return
           }
           self.addActiveClass($(this));
           self.initInputAfterLoad($(this));
         });
       }
     };

     //todo list
     /*
     1. при сохранении предфильтров 
     1.1. инпуты 
     1.1.1.- если не датапикер  - тупо сохранить
     1.1.2. - если датапикер:
     1.1.2.1 - если выбрана дата - записать дату
     1.1.2.2. - если выбран селект - записать селект
     различия по имени, наверное, или по формату
     
     2. при выборе 
     2.1.
     2.1.1. датапикеры - если дата "с  - по" добавить проверку, чтоб дата "по" была не больше, чем дата "с"
     2.1.2. - при двойном формате даты предусмотреть, что первой дата может быть введена, как в первом инпуте, тка и во втором
     
     3. предфильтра после перемещения удаляется не тот фильтр
     
     4. клик по предфильтру
     
     
     
     
     */
     
     var FormObject = function (el) {
          var _this = this;
          this.form = el;
          this.filters = [];
          this.filled = false;
          this.formWrapper = $(_this.form).closest('.content-search');
          this.formHolder = $(_this.form).find('.content-search-form-holder');
          this.formPreset = $(_this.form).find('.content-search-form-preset');
          this.formSwitcher = $(_this.formWrapper).find('.content-search-switcher');
          this.formPresetSwitcher = $(_this.formWrapper).find('.content-preset-switcher');
     
          this.mainInput = $(_this.formWrapper).find('.content-search-main-input') || false;
          this.mainText = $(_this.formWrapper).find('.content-search-main-text') || false;
          this.shownFilters = [];
          this.hasFields = false;
          this.hasPresetFilters = false;
          this.dirty = false;
     
          this.submitBtn = $(_this.formHolder).find('.search-form-btn-submit');
          this.saveFilterBtn = $(_this.formHolder).find('.search-form-btn-save-filter');
          this.clearFormBtn = $(_this.formHolder).find('.search-form-btn-clean');
     
          this.savePresetFilterForm = $(_this.formHolder).find('.preset-filter-wrapper');
          this.savePresetFilterNameInput = $(_this.savePresetFilterForm).find('.preset-filter-name-input');
          this.savePresetFilterDefaultCheckbox = $(_this.savePresetFilterForm).find('.preset-filter-default-checkbox');
          this.savePresetFilterSaveBtn = $(_this.savePresetFilterForm).find('.preset-filter-save');
          this.savePresetFilterCancelBtn = $(_this.savePresetFilterForm).find('.preset-filter-cancel');
     
          this.savePresetFilterFormOpen = false;
     
          this.parametersFilterHolder = $(_this.formWrapper).find('.content-filter-parameters');
          this.clearAllFiltersBtn = $(_this.parametersFilterHolder).find('.content-filter-parameters-clear-all');
     
     };

     this.createContentSearchForm = function (data, submitBtnFunction) {
       if (!data || typeof data == 'undefined') {
         return;
       }
       //create formObj
       var thisForm = new FormObject($('form[name="' + data.formConfig.formName + '"]'));
     
       thisForm.mainInputConfig = data.mainInputConfig || false;
       thisForm.submitFunction = submitBtnFunction || false;
       thisForm.presetFiltersUrl = data.presetFiltersUrl || false;
       thisForm.formMethod = data.formConfig.formMethod || 'post';
       thisForm.controls = [];
     
       // add attributes to form
       if (data.formConfig.formDataAjaxUpdate) {
         self.addAttr(thisForm.form, 'data-ajax-update', data.formConfig.formDataAjaxUpdate);
       }
     
       self.addAttr(thisForm.form, 'data-ajax-complete', 'mt.searchFormCompleteFunction($(this))'); // возможно можно переделать
     
       // create presetFilters
       if (thisForm.presetFiltersUrl && thisForm.presetFiltersUrl.actionName && thisForm.presetFiltersUrl.controllerName) {
         thisForm.hasPresetFilters = true;
         self.getPresetFilters(thisForm, true);
       } else {
         self.submitMainInput(thisForm);
       }
     
       //create MainInput
       if (thisForm.mainInputConfig) {
         self.createMainInput(thisForm);
       } else {
         self.createMainText(thisForm);
       }
     
       //check&set PresetSettings
       if (thisForm.controls) {
     
         //check if we has presetcontrols
         if (getPresetContolsLength(thisForm.presetFiltersUrl)) {
           $(thisForm.formHolder).addClass('has-presettings');
           //get inputConfig
           $.when(self.getConstrolsConfig(thisForm))
             .done(
               function () {
                 //check getted input config
                 if (thisForm.controls.length > 0) {
                   //activate form if we have saved preseting&inputconfig
                   self.activateSearchForm(thisForm)
                 } else {
                   //active form from index object
                   thisForm.controls = data.formConfig.controls.controlsConfig;
                   $.when(thisForm.controls = data.formConfig.controls.controlsConfig)
                     .done(
                       self.activateSearchForm(thisForm)
                     );
                 }
               }
             );
         } else {
     
           //activate form without presettings from index
           thisForm.controls = data.formConfig.controls.controlsConfig;
           self.activateSearchForm(thisForm);
         }
     
         //function check object length
         function getPresetContolsLength(obj) {
           var size = 0,
             key;
           for (key in obj) {
             if (obj.hasOwnProperty(key)) size++;
           }
           return size;
         };
     
       }
     
       // activatePresetFiltersSwitcher
       $(thisForm.formPresetSwitcher).on('click', function () {
         self.toggleSearchFormPresetFilters(thisForm);
       });
     
       //activateSearchForm;
       self.addActiveClass(thisForm.formWrapper);
     };
     
     //get Input config to generate searchForm
     this.getConstrolsConfig = function (formObj) {
       var controllerName = formObj.presetFiltersUrl.controllerName,
         actionName = formObj.presetFiltersUrl.actionName,
         url,
     
         journalName = journalName = controllerName + '-' + actionName,
         queryData;
     
       url = controllerName + '/GenerateInputConfig';
       queryData = {
         journalName: journalName
       }
       
       return $.ajax({
         type: "post",
         url: url,
         data: queryData,
         success: function (data) {
           data = JSON.parse(data);
           formObj.controls = data;
         },
         error: function (data) {
           console.log('error', data)
         }
       });
     }
     
     this.getDefaultControls = function (formObj, controlsConfig) {
       formObj.controls = controlsConfig;
     
       $.when(formObj.controls = controlsConfig)
         .done(
           self.activateSearchForm(formObj)
         );
     }
     
     //add event listeners for search form
     this.activateSearchForm = function (formObj) {
       self.addActiveClass(formObj.formSwitcher);
       self.addActiveClass(formObj.formWrapper);
       $(formObj.formSwitcher).on('click', function () {
         if (formObj.filled) {
           self.toggleSearchFormHolder(formObj);
         } else {
           self.showLoader(formObj.formHolder);
           $.when($(formObj.formHolder).find('ul').html(self.createFormTemplate(formObj)))
             .done(self.hideLoader(formObj.formHolder))
             .done(self.toggleSearchFormHolder(formObj))
             .done(
               self.watchDirtyForm(formObj),
               self.activateFormControls($(formObj.formHolder).find('ul')),
               self.createSearchFormBtnsClickListeners(formObj),
               formObj.filled = true,
               setTimeout(function () {
                 self.updateNiceScroll();
               }, timeOutInterval)
             );
         }
       });
     }

     
     this.createMainText = function (formObj) {//filterForm !!!!!!!!!!!
     
          var text = formObj.mainInputText || 'Для формування звіту виберіть параметри';
          $(formObj.mainText).text(text);
     };
     
     this.createMainInput = function (formObj) {//searchForm
     
          var mainInput = formObj.mainInput,
               mainInputData = formObj.mainInputConfig;
     
          if (mainInputData.labelName) {
               self.addAttr(mainInput, 'placeholder', mainInputData.labelName);
          } else {
               self.addAttr(mainInput, 'placeholder', 'Пошук');
          }
          if (mainInputData.type) {
               self.addAttr(mainInput, 'type', mainInputData.type);
          } else {
               self.addAttr(mainInput, 'type', 'text');
          }
     
          mainInput.val('');
     
          self.manageMainInput(formObj);
     };
     
     
     this.manageMainInput = function (formObj) {
     
          $(formObj.mainInput).keydown(function (e) {
               if (e.keyCode === 13) {
                    e.preventDefault();
     
                    if (formObj.filled) {
                         self.clearSearchForm(formObj, true);
                    }
     
                    self.submitMainInput(formObj);
               }
          });
     
          $(formObj.formWrapper).find('.content-search-submit-main-input').each(function () {
               $(this).on('click', function () {
                    if (formObj.filled) {
                         self.clearSearchForm(formObj, true);
                    }
                    self.submitMainInput(formObj);
               });
          });
     
     };
     
     this.submitMainInput = function (formObj) {
     
          var query = self.prepareSearchQueryData([{
               'name': formObj.mainInputConfig.mainInputLookUp[0],
               'value': $(formObj.mainInput).val()
          }]);
     
     
          if (!self.isElValueEmpty(formObj.mainInput)) {
               formObj.shownFilters = [{
                    'name': formObj.mainInputConfig.mainInputLookUp[0],
                    'shownName':formObj.mainInputConfig.labelName,
                    'value': $(formObj.mainInput).val(),
                    'shownValue': $(formObj.mainInput).val()
               }];
     
          } else {
               formObj.shownFilters = [];
          }
     
          self.ajaxSubmitSearchForm(formObj, query);
     
     };
     
     this.clearMainInput = function (formObj) {
     
          $(formObj.mainInput).val('');
     }

     this.prepareParametersFilter = function (formObj) {
          if (formObj.shownFilters.length) {
               $(formObj.clearAllFiltersBtn).on('click', function () {
                    self.clearAllSearchFormFilters(formObj);
               });
               $.when($(formObj.parametersFilterHolder).find('ul').html(self.createPresetFilters(formObj.shownFilters))).done(
                    $(formObj.parametersFilterHolder).find('.content-filter-parameters-item span').each(function () {
                         self.setRemoveFilterListener($(this), formObj.shownFilters, formObj);
                    })
               )
               self.addOpenClass(formObj.parametersFilterHolder);
               self.addActiveClass(formObj.parametersFilterHolder);
               
          } else {
               $(formObj.parametersFilterHolder).find('ul').html('');
               self.removeOpenClass(formObj.parametersFilterHolder);
               self.removeActiveClass(formObj.parametersFilterHolder);
          }
     }
     this.createPresetFilters = function(arr) {
          var htmlInner = '',
               currFilter;
          for (var i = 0; i < arr.length; i++) {
               currFilter = arr[i];
               htmlInner += '<li class = "content-filter-parameters-item"> <p>' +
                    currFilter.shownName + ': ' + currFilter.shownValue +
                    '</p> <span data-remove = "' + currFilter.name + '"><i class="icon-xxs icon-close"></i></span>'
          }
          return htmlInner;
     }
     
     this.setRemoveFilterListener = function(el, arr, formObj) {
          var index,
               name = 'name';
          $(el).on('click', function () {
               index = self.findObjIndexInArr(arr, name, $(el).data('remove'));
               arr.splice(index, 1);
               var queryData = self.prepareSearchQueryData(arr);
               self.ajaxSubmitSearchForm(formObj, queryData);
          });
     }
     
     this.clearAllSearchFormFilters = function (formObj) {
          formObj.shownFilters = [];
          var queryData = self.prepareSearchQueryData(formObj.shownFilters);
          self.ajaxSubmitSearchForm(formObj, queryData);
     }
     
     this.createFormTemplate = function (formObj) {
     
          var inputConfig = formObj.controls,
               currData,
               tempHtml,
               inputLength = inputConfig.length,
               html = '';
     
          for (var i = 0; i < inputLength; i++) {
               currData = inputConfig[i];
               // if (i == 0) {
               //      self.configureMainInput(formObj, currData);
               // }
     
               formObj.hasFields = true; // дойдет сюда только если есть какие-то поля, кроме главного;
     
               tempHtml = self.selectDataTemplate(currData, true);
               
               html += '<li class="content-search-form-inner">' + tempHtml + '</li>';
          }
     
          return html;
     };

     this.addActiveClassToFormSwitcher = function (formObj) {
     
          if (formObj.hasFields) {
               self.addActiveClass(formObj.formSwitcher);
          }
     };
     
     
     this.toggleSearchFormHolder = function (formObj) {
     
          if ($(formObj.formSwitcher).hasClass('open')) {
               self.closeSearchFormHolder(formObj);
          } else {
               self.openSearchFormHolder(formObj);
          }
     };
     
     this.openSearchFormHolder = function (formObj) {
     
          $(formObj.form).find('input').each(function () {
               self.addEmptyNotEmptyClass($(this));
          });
     
          // self.closeSearchFormPreset();
          self.addOpenClass(formObj.formHolder);
          self.addOpenClass($(formObj.formHolder).parent());
          self.addOpenClass(formObj.formSwitcher);
     
     
          setTimeout(function () {
               searchFormOpen = formObj;
          }, timeOutInterval); // todo
     };
     
     this.closeSearchFormHolder = function (formObj) {
     
          if (!formObj) {
               formObj = searchFormOpen;
          }
     
          self.removeOpenClass(formObj.formHolder);
          self.removeOpenClass($(formObj.formHolder).parent());
          self.removeOpenClass(formObj.formSwitcher);
          searchFormOpen = false;
     
          $(formObj.formHolder).find('.js-validation').each(function () {
               self.removeActiveClass($(this));
               setTimeout(function () {
                    $(this).text('');
               }, timeOutInterval)
          });
          self.closeNewPresetFilterForm(formObj);
     };
     
     this.closeSearchFormHolderAndPresetFilters = function () {
     
          if (searchFormOpen) {
               self.closeSearchFormHolder();
          }
          if (searchPresetsOpen) {
               self.closePresetFiltersForm();
          }
     
     }
     
     this.toggleSearchFormPresetFilters = function (formObj) {
     
          if ($(formObj.formPresetSwitcher).hasClass('open')) {
     
               self.closePresetFiltersForm(formObj);
          } else {
               self.openPresetFiltersForm(formObj);
          }
     
     };
     
     this.openPresetFiltersForm = function (formObj) {
          self.getPresetFilters(formObj, false); 
          self.addOpenClass(formObj.formPresetSwitcher);
          self.addActiveClass($(formObj.formPreset).parent());
          self.addOpenClass(formObj.presetWrapper);
     
          self.createSelectScrollBar($(formObj.formPreset).find('ul').get(0));
          setTimeout(function () {
               searchPresetsOpen = formObj;
          }, timeOutInterval); // todo
     }
     
     this.closePresetFiltersForm = function (formObj) {
     
          if (!formObj) {
               formObj = searchPresetsOpen;
          }
          self.removeOpenClass(formObj.formPresetSwitcher);
          self.removeActiveClass($(formObj.formPreset).parent());
     
          self.removeOpenClass(formObj.formPreset);
     
          self.destroySelectScrollBar();
     
          searchPresetsOpen = false;
          if (formObj.presetSettingActive) {
               self.deActivatePresetFiltersSetting(formObj);
          }
          // self.updatePresetFilters(formObj, true); //quiet update
     }

     this.submitSearchForm = function (formObj) {
     
          if (!self.validateForm($(formObj.formHolder).find('ul'))) {
               return;
          }
     
          self.showLoader($($(formObj.form).data('ajax-update')));
     
          self.clearMainInput(formObj);
     
          self.closeSearchFormHolder(formObj);
          // self.closeSearchFormPreset(formObj);
          self.clearPaging(formObj.form);
     
          formObj.shownFilters = self.findDataForPresetFilters(formObj);
     
          var queryData = self.prepareSearchQueryData(formObj.shownFilters);
     
          self.ajaxSubmitSearchForm(formObj, queryData);
     };
     
     
     this.submitDefaultFilter = function (formObj) {
          self.showLoader($($(formObj.form).data('ajax-update')));
          var filters = formObj.filters,
               defaultFilter = false;
          for (var i = 0; i < filters.length; i++) {
               if (filters[i].default) {
                    defaultFilter = filters[i];
                    break;
               }
          }
          if (defaultFilter) {
               formObj.shownFilters = defaultFilter.fields;
               var query = self.prepareSearchQueryData(defaultFilter.fields);
               self.ajaxSubmitSearchForm(formObj, query);
          } else {
               self.submitMainInput(formObj);
          }
     };
     
     
     this.findDataForPresetFilters = function (formObj) {
     
          var arr = [];
     
          var selectGroupChildren = $(formObj.formHolder).find('.select-group');
          var checkboxGroupChildren = $(formObj.formHolder).find('.checkbox-group');
          var inputGroupChildren = $(formObj.formHolder).find('.input-group');
     
     
          $(selectGroupChildren).each(function () {
               if ($(this).parent().is('.preset-filter-inner')) {
                    return
               }
               setSelectGroupValue($(this))
          })
     
          $(checkboxGroupChildren).each(function () {
               if ($(this).parent().is('.preset-filter-inner')) {
                    return
               }
               setCheckBoxGroupValue($(this))
          })
     
          $(inputGroupChildren).each(function () {
               if ($(this).parent().is('.preset-filter-inner')) {
                    return
               }
               if ($(this).hasClass('datepicker-group')) {
                    setDatePickerGroupValue($(this));
               } else {
                    setInputGroupValue($(this))
               }
     
          })
     
          if (arr.length) {
               return arr;
          }
          return false;
     
     
          function setSelectGroupValue(el) {
     
               var select = $(el).find('select');
     
               if (!self.isElValueEmpty(select)) {
                    arr.push({
                         'name': $(select).attr('name'),
                         'shownName': $(select).closest('.select-group').find('label').text(),
                         'type': 'select',
                         'value': $(select).val(),
                         'shownValue': self.returnSelectedOptionText(select)
                    })
               }
          }
     
     
     
          function setCheckBoxGroupValue(el) {
               var checkbox = $(el).find('input:checkbox');
               if ($(checkbox).prop('checked')) {
                    arr.push({
                         'name': $(checkbox).attr('name'),
                         'shownName': $(checkbox).parent().find('label').text(),
                         'type': 'checkbox',
                         'value': true,
                         'shownValue': 'Так'
                    })
               }
          }
     
          function setInputGroupValue(el) {
               var input = $(el).find('input');
     
               if (!self.isElValueEmpty(input)) {
                    arr.push({
                         'name': $(input).attr('name'),
                         'shownName': $(input).parent().find('label').text(),
                         'type': 'input',
                         'value': $(input).val(),
                         'shownValue': $(input).val()
     
                    })
               }
          }
     
          function setDatePickerGroupValue(el) {
               var hiddenInput = $(el).find('.data-range-result');
               if (!self.isElValueEmpty(hiddenInput)) {
                    if (!self.isElValueEmpty($(el).find('.select-range-input'))) {
                         arr.push({
                              'name': $(hiddenInput).attr('name'),
                              'shownName': $(hiddenInput).parent().find('label').text(),
                              'type': 'datePickerSelect',
                              'value': $(el).find('.select-range-input').val(),
                              'shownValue': self.returnSelectedOptionText($(el).find('.select-range-input')) + " тому"
                         })
                    } else if (!self.isElValueEmpty($(el).find('.hasDatepicker'))) {
                         arr.push({
                              'name': $(hiddenInput).attr('name'),
                              'shownName': $(hiddenInput).parent().find('label').text(),
                              'type': 'datePickerInput',
                              'value': $(hiddenInput).val(),
                              'shownValue': $(el).find('.hasDatepicker').val()
                         })
                    }
               }
          }
     
     }
     
     
     this.submitPresetFilter = function (formObj, filterBtn) {
          var id = 'filterId';
          var index = self.findObjIndexInArr(formObj.filters, id, $(filterBtn).attr('id')),
               queryData = self.prepareSearchQueryData(formObj.filters[index].fields);
          self.showLoader($($(formObj.form).data('ajax-update')));
     
          formObj.shownFilters = formObj.filters[index].fields;
     
          self.closePresetFiltersForm(formObj);
     
          self.ajaxSubmitSearchForm(formObj, queryData);
     };
     
     
     this.prepareSearchQueryData = function (fieldsArr) {
     
          var query = {
               "X-Requested-With": "XMLHttpRequest"
          };
     
          for (var i = 0; i < fieldsArr.length; i++) {
               if (fieldsArr[i].type == 'datePickerSelect') {
                    query[fieldsArr[i].name] = self.subtractDate(new Date().toISOString(), fieldsArr[i].value);
               } else {
                    query[fieldsArr[i].name] = fieldsArr[i].value
               }
          }
          return query;
     }
     
     this.ajaxSubmitSearchForm = function (formObj, queryData) {
          self.showLoader();
          var requestType = formObj.formMethod,
               url = $(formObj.form).attr('action'),
               formResult = $(formObj.form).data('ajax-update');
     
          self.clearPaging(formObj.form);
          $.ajax({
               type: requestType,
               url: url,
               data: {
                    paramList: queryData,
                    options: null
               },
               success: function (data) {
                    $(formResult).html(data);
                    self.searchFormCompleteFunction(formObj.form);
                    self.prepareParametersFilter(formObj);
               },
               error: function (data) {
                    var msg = '<p>Виникла помилка при виведенні данних пошуку</p>';
                    self.createDialog(false, msg);
               }
          });
     
     }
     
     this.searchFormCompleteFunction = function (form) {
     
          var result = $($(form).data('ajax-update'));
          $.when(self.initMask(result))
               .done(self.findFormatDateParagraph(result))
               .done(self.findGrids(result))
               .done(self.createPaging(result))
               .done(
                    self.setDeleteContentListItem(result),
                    self.updateNiceScroll(),
                    self.createCheckboxLabel(result),
                    self.setToolTipsForChild(result),
                    self.hideLoader(result)
               );
     };
     
     this.addSearchFormEnterListener = function (formObj) {
     
          $(document).keydown(function (e) {
               if (e.keyCode === 13 && searchFormOpen) {
                    e.preventDefault();
                    self.submitSearchForm(formObj);
               }
          });
     };

     this.setDefaultPresetFilter = function (formObj, el) {
          var inputId = '',
               inputChecked = false,
               id = 'filterId',
               index = self.findObjIndexInArr(formObj.filters, id, $(el).attr('id'));
     
          if ($(el).find('input:checkbox').prop('checked')) {
               inputId = $(el).find('input:checkbox').attr('id');
               inputChecked = true;
          }
     
          for (var i = 0; i < formObj.filters.length; i++) {
               formObj.filters[i].default = false;
          }
     
          $(formObj.presetWrapper).find('.checkbox').each(function () {
               $(this).prop('checked', false);
          });
     
          if (inputChecked) {
               $('#' + inputId).prop('checked', true);
               formObj.filters[index].default = true;
          }
     
          self.updatePresetFilters(formObj, true);
     };

     this.deletePresetFilter = function (e, formObj, el) {
     
          var msg = "<p>Підтвердіть видалення фільтру</p>",
               id = 'filterId',
               index = self.findObjIndexInArr(formObj.filters, id, $(el).attr('id'));
     
          self.createDialog(e, msg, deleteFilter, noRemoveFilter);
     
          function deleteFilter() {
     
               formObj.filters.splice(index, 1);
               self.clearElEventListeners(el);
               $(el).remove();
               self.reOrderPresetFilters(formObj);
     
               self.updatePresetFilters(formObj);
     
          }
     
          function noRemoveFilter() {
               return;
          }
     
     
     };

     this.savePresetFilter = function (formObj) {
     
          var runOk = true;
     
          if (self.validateForm(formObj.savePresetFilterForm)) {
     
               var presetFilterName = $.trim($(formObj.savePresetFilterNameInput).val());
     
               for (var f = 0; f < formObj.filters.length; f++) {
                    if ($.trim(formObj.filters[f].name) == presetFilterName) {
                         runOk = false;
                         var msg = "<p>Фільтр з таким ім\'ям вже збережений</p>";
                         self.createDialog(false, msg, confirmDialog);
                         break;
                    }
               }
     
               function confirmDialog() {
                    $(formObj.savePresetFilterNameInput).focus();
               }
               if (!runOk)
                    return;
     
               var presetFiltersData = self.findDataForPresetFilters(formObj);
     
               if (presetFiltersData) {
                    var currFilter = {};
                    currFilter.name = $(formObj.savePresetFilterNameInput).val();
                    currFilter.default = $(formObj.savePresetFilterDefaultCheckbox).prop('checked');
                    currFilter.order = formObj.filters.length;
                    currFilter.fields = presetFiltersData;
     
                    var filterId = 'filter' + self.generateId();
                    currFilter.filterId = filterId;
     
                    if (currFilter.default) {
                         for (var i = 0; i < formObj.filters.length; i++) {
                              formObj.filters[i].default = false;
                         }
     
                         //удалить дефолтный фильтр, если есть
                    }
     
                    formObj.filters.push(currFilter);
                    self.updatePresetFilters(formObj);
     
               } else {
                    self.createDialog(false, "Виберіть параметри фільтру")
               }
               self.closeNewPresetFilterForm(formObj);
          }
     }

     this.createSortablePresetFilters = function (formObj) {
     
          // $(formObj.presetWrapper).sortable({
          //      cursor: "move",
          //      axis: "y",
          //      items: "li:not(.content-search-form-preset-header)",
          //      start: startSortable,
          //      stop: stopSorting
          // }).disableSelection();
     
     
          // function startSortable(e, ui) {
          //      $(ui.item).css('transition-duration', '0s');
          // }
     
          // function stopSorting(e, ui) {
     
     
          //      self.reOrderPresetFilters(formObj);
     
          //      self.updatePresetFilters(formObj, true); //quiet update
          //      setTimeout(function () {
          //           $(ui.item).css('transition-duration', '');
          //      }, timeOutInterval)
     
          // }
     }
     
     this.destroySortablePresetFilters = function (formObj) {
          //$(formObj.presetWrapper).sortable("destroy")
     }

     this.openNewPresetFilterForm = function (formObj) {
     
          formObj.savePresetFilterFormOpen = true;
     
          self.addActiveClass($(formObj.savePresetFilterForm));
          $(formObj.savePresetFilterNameInput).focus();
     
          $(formObj.savePresetFilterCancelBtn).on('click', function (e) {
               e.preventDefault();
               self.closeNewPresetFilterForm(formObj)
          });
     
          $(formObj.savePresetFilterSaveBtn).on('click', function (e) {
               e.preventDefault();
               self.savePresetFilter(formObj);
          })
     
     }
     
     this.closeNewPresetFilterForm = function (formObj) {
     
          if (!formObj.savePresetFilterFormOpen) {
               return;
          }
          $(formObj.savePresetFilterNameInput).val('');
          self.removeActiveClass($(formObj.savePresetFilterForm));
          formObj.savePresetFilterFormOpen = false;
     }

     this.reOrderPresetFilters = function (formObj) {
     
          var filterBtn = $(formObj.presetWrapper).find('.preset-btn'),
               currBtn,
               currObjIndex,
               id = 'filterId';
     
          for (var i = 0; i < filterBtn.length; i++) {
               currBtn = filterBtn[i];
               currObjIndex = self.findObjIndexInArr(formObj.filters, id, $(currBtn).attr('id'));
               formObj.filters[currObjIndex].order = i
          }
          if (!formObj.filters || !formObj.filters.length) {
               self.closePresetFiltersForm(formObj);
          }
     }
     
     this.findObjIndexInArr = function (arr, prop, value) {
          var valueTrimmed = $.trim(value);
          for (var i = 0; i < arr.length; i++) {
               var el = arr[i];
               if ($.trim(el[prop]) === valueTrimmed) {
                    return i;
               }
          }
     };
     
     this.sortPresetFiltersByOrder = function (arr) {
     
          return self.sortArrObjByMethod(arr, 'order');
     };

     this.getPresetFilters = function (formObj, onLoad) {
     
       var controllerName = formObj.presetFiltersUrl.controllerName,
         actionName = formObj.presetFiltersUrl.actionName,
         url,
     
         journalName = journalName = controllerName + '-' + actionName,
         queryData;
     
     
       url = controllerName + '/GetPresettings';
       queryData = {
         journalName: journalName
       }
     
     
     
       $.ajax({
         type: "POST",
         url: url,
         data: queryData,
         success: function (data) {
           if (typeof data == 'string' && data.length) { //get
             presetFilterJson = data;
             data = JSON.parse(data);
             formObj.filters = self.sortPresetFiltersByOrder(data.filters);
             
             if (onLoad) {
               //activatePresetFiltersWrapper
               self.submitDefaultFilter(formObj);
             }
     
             formObj.presetWrapper = $(formObj.form).find('.content-search-form-preset')[0];
             self.buildPresetFilters(formObj);
           }
           if (formObj.filters.length <= 0) {
             self.submitSearchForm(formObj);
           }
     
         },
         error: function (data) {
     
           var msg = "<p>Виникла помилка при генерації збережених фільтрів</p><p>Обновіть сторінку</p>";
           self.createDialog(false, msg);
           console.error(data);
         }
       });
     }

     this.setPresetFilters = function (formObj, str, setSuccessFunction, setErrorFunction) {
     
          var url,
               controllerName = formObj.presetFiltersUrl.controllerName,
               actionName = formObj.presetFiltersUrl.actionName,
               journalName = journalName = controllerName + '-' + actionName,
               queryData;
     
          if (str) { //set
               url = controllerName + '/SetPresettings';
               queryData = {
                    journalName: journalName,
                    presettingsJson: str
               }
          }
     
          $.ajax({
               type: "POST",
               url: url,
               data: queryData,
               success: function (data) {
                    if (data.setSuccess) { //set
                         return setSuccessFunction()
                    } else {
                         return setErrorFunction()
                    }
     
               },
               error: function (data) {
                    console.error(data);
                    return setErrorFunction()
               }
          });
     }
     
     
     this.updatePresetFilters = function (formObj, quiet) {
     
          if (!quiet) {
               quiet = false;
          }
     
     
          var needActivation = false;
     
          if (formObj.filtersChanging) {
               var msg = "<p>Система зайнята.</p><p>Зачекайте та спробуйте ще раз.</p>"
               self.createDialog(false, msg);
               return;
          }
     
          if (formObj.presetSettingActive) {
               needActivation = true;
               self.deActivatePresetFiltersSetting(formObj);
               self.addActiveClass(formObj.presetWrapper);
     
          }
     
          if (!quiet) {
               self.showLoader(formObj.presetWrapper);
          }
     
          formObj.filtersChanging = true;
     
          var data = {};
          data.filters = formObj.filters;
     
          
          data = JSON.stringify(data);
     
          self.setPresetFilters(formObj, data, successCallback, errorCallback)
          function successCallback() {
     
               self.reBuildPresetFilters(formObj);
     
               if (needActivation) {
                    self.activatePresetFiltersSetting(formObj);
               }
     
               presetFilterJson = data;
          }
     
          function errorCallback() {
               //
               var unsavedFilters = JSON.parse(presetFilterJson);
               formObj.filters = self.sortPresetFiltersByOrder(unsavedFilters.filters);
               var interval = setInterval(function () { //при удалении открывается диалоговое окно, надо дать время, чтоб оно успело закрыться
                    if (!dialogOpen) {
                         var msg = "<p>Виникла помилка при збереженні фільтра</p>";
     
                         self.reBuildPresetFilters(formObj)
                         self.createDialog(false, msg, false, errorSaveFilter);
                         clearInterval(interval);
     
                    }
               }, 66);
     
          }
     
          function errorSaveFilter() {
               return;
          }
     }

     this.buildPresetFilters = function (formObj) {
     
          var htmlInner = '';
     
          self.removeActiveClass(formObj.formPresetSwitcher);
          if (!formObj.filters.length) {
               return;
          }
     
          self.sortArrObjByMethod(formObj.filters, 'order');
     
          self.addActiveClass(formObj.formPresetSwitcher);
     
          for (var i = 0; i < formObj.filters.length; i++) {
               htmlInner += self.createFilter(formObj.filters[i]);
          }
          $.when($(formObj.presetWrapper).find('ul').html(htmlInner)).done(
               self.createCheckboxLabel(formObj.presetWrapper),
               self.setPresetFiltersClickListener(formObj)
          );
     }
     
     this.createFilter = function (data) {
     
          var checked = '';
     
          if (typeof data.default !== 'undefined' && data.default) {
               checked = ' checked ';
          }
     
          return (
               '<li class="btn btn-outline preset-btn" ' + 'id="'+ data.filterId + '"' +
               ' <div class="checkbox-group-inner"><input type="checkbox" ' +
               checked +
               '/></div>' +
               '<p>' +
               data.name +
               '</p>' +
               '<span class = "remove-preset-btn icon icon-sm icon-remove" data-title = "Видалити фільтр"></span></>'
          );
     };
     
     
     
     this.setPresetFiltersClickListener = function (formObj) {
     
          formObj.presetSettingActive = false;
     
          $(formObj.presetWrapper).find('.preset-filters-settings').on('click', function () {
               self.activatePresetFiltersSetting(formObj);
          })
     
          $(formObj.presetWrapper).find('.preset-filters-settings-cancel').on('click', function () {
     
               self.deActivatePresetFiltersSetting(formObj);
          })
     
     
     
          $(formObj.presetWrapper).find('.preset-btn').each(function () {
               var filterBtn = $(this);
               $(filterBtn).find('span').on('click', function (e) {
                    //deleting
                    e.stopPropagation();
                    self.deletePresetFilter(e, formObj, filterBtn);
               });
     
               $(filterBtn).find('.checkbox-group-inner').on('click', function (e) {
                    //setDefault
                    e.stopPropagation();
               });
     
               $(filterBtn).find('label').on('click', function (e) {
                    if (!formObj.presetSettingActive) {
                         e.preventDefault();
                         return;
                    }
               })
     
     
               $(filterBtn).find('input').on('change', function () {
                    self.setDefaultPresetFilter(formObj, filterBtn);
               });
     
               $(filterBtn).on('click', function () { //submit searchForm
                    if (!formObj.presetSettingActive) { //todo нафига эта проверка??? не знаю пока, проверить надо
                         $.when(self.submitPresetFilter(formObj, filterBtn))
                              .done(
                                   self.closePresetFiltersForm(formObj)
                              )
     
                         //self.submitPresetFilter(formObj, filterBtn);
                    } 
                    
     
               });
          });
     };
     
     
     
     this.reBuildPresetFilters = function (formObj) {
     
          self.clearElEventListeners(formObj.presetWrapper);
          self.hideLoader(formObj.presetWrapper);
          $(formObj.presetWrapper).find('ul').html('');
          formObj.filtersChanging = false;
          self.buildPresetFilters(formObj);
     }
     
     
     
     this.activatePresetFilterBtn = function (formObj) {
     
          if (formObj.dirty) {
               self.removeNotDisabledClass(formObj.saveFilterBtn)
          } else {
               self.addNotDisabledClass(formObj.saveFilterBtn)
          }
     }
     
     
     
     this.activatePresetFiltersSetting = function (formObj) {
     
          formObj.presetSettingActive = true;
          self.createSortablePresetFilters(formObj);
          self.addActiveClass(formObj.presetWrapper);
     }
     
     this.deActivatePresetFiltersSetting = function (formObj) {
     
          formObj.presetSettingActive = false;
          self.removeActiveClass(formObj.presetWrapper);
          self.destroySortablePresetFilters(formObj);
     }


     this.watchDirtyForm = function (formObj) {
     
       // $(formObj.formHolder).find('select').each(function () {
       //      $(this).on('change focus', function () {
       //           //formObj.dirty = checkDirty();
       //           self.activatePresetFilterBtn(formObj);
       //      });
       // });
     
     
       $(formObj.formHolder).on('click focus blur change', function () {
         formObj.dirty = checkDirty();
         self.activatePresetFilterBtn(formObj);
         //check datepicker
         var searchdate = body.find('#ui-datepicker-div, .ui-datepicker-calendar, .ui-state-default');
         $(searchdate).on('blur focus change click', function () {
           formObj.dirty = checkDirty();
           self.activatePresetFilterBtn(formObj);
         });
       });
     
       function checkDirty() {
         var temp = false;
         var inputsSelects = $(formObj.formHolder).find('select, input, .select-group, .input-group, .select-group-inner, .datepicker-input-select, .datepicker-input-select-wrapper');
         for (var i = 0; i < inputsSelects.length; i++) {
     
           var currInputsSelect = inputsSelects[i];
     
           var checkClasscurrInputsSelect = $(currInputsSelect).hasClass('not-empty');
     
           if (checkClasscurrInputsSelect == true) {
             temp = true;
           } else if (checkClasscurrInputsSelect == true) {
             temp = false;
           }
     
         }
         return temp;
       }
     };
     
     
     this.createSearchFormBtnsClickListeners = function (formObj) {
     
       $(formObj.clearFormBtn).on('click', function () {
     
         self.clearSearchForm(formObj);
         $('.hasDatepicker').datepicker("option", "minDate", null); // чистим датапикер на форме
         $('.hasDatepicker').datepicker("option", "maxDate", null);
       })
     
       $(formObj.saveFilterBtn).on('click', function () {
     
         if (formObj.dirty) {
           clickFunc();
         }
     
         function clickFunc() {
           if (!formObj.savePresetFilterFormOpen) {
             self.openNewPresetFilterForm(formObj);
     
           } else {
             setTimeout(clickFunc, 66); //TODO
           }
         }
       })
       if (formObj.submitFunction) {
     
         $(formObj.submitBtn).on('click', function () {
           if (self.validateForm(formObj.form)) {
             self.showLoader($(formObj.form.data('ajax-update')));
             formObj.submitFunction(formObj);
           }
         });
       } else {
     
         $(formObj.submitBtn).on('click', function (e) {
           e.preventDefault();
           self.submitSearchForm(formObj);
         });
       }
     
     }
     
     
     this.clearSearchForm = function (formObj, doNotClearMainInput) {
     
       $(formObj.formHolder).find('input').each(function () {
         $(this).val('');
         self.addEmptyNotEmptyClass($(this));
       });
       $(formObj.formHolder).find('select').each(function () {
         self.removeAttrSelected($(this));
       });
     
       if (!doNotClearMainInput) {
         if (formObj.mainInput) {
           $(formObj.mainInput).val('');
         }
       }
     };
     
     
     // previous code
     // this.watchDirtyForm = function (formObj) {
     
     //      $(formObj.formHolder).find('select').each(function () {
     //           $(this).on('change', function () {
     //                formObj.dirty = checkDirty();
     //                self.activatePresetFilterBtn(formObj);
     //           });
     //      });
     //      $(formObj.formHolder).find('input').each(function () {
     //           $(this).on('blur', function () {
     //                formObj.dirty = checkDirty();
     //                self.activatePresetFilterBtn(formObj);
     //           });
     //      });
     
     //      function checkDirty() {
     //           var temp = false;
     //           var inputsSelects = $(formObj.formHolder).find('select, input');
     //           for (var i = 0; i < inputsSelects.length; i++) {
     //                if (!self.isElValueEmpty(inputsSelects[i])) temp = true;
     //           }
     //           return temp;
     //      }
     // };
     
     
     // this.createSearchFormBtnsClickListeners = function (formObj) {
     
     //      $(formObj.clearFormBtn).on('click', function () {
     
     //           self.clearSearchForm(formObj);
     
     //      })
     
     //      $(formObj.saveFilterBtn).on('click', function () {
     
     //           if (formObj.dirty) {
     //                clickFunc();
     //           }
     
     //           function clickFunc() {
     //                if (!formObj.savePresetFilterFormOpen) {
     //                     self.openNewPresetFilterForm(formObj);
     
     //                } else {
     //                     setTimeout(clickFunc, 66); //TODO
     //                }
     //           }
     //      })
     //      if (formObj.submitFunction) {
     
     //           $(formObj.submitBtn).on('click', function () {
     //                if (self.validateForm(formObj.form)) {
     //                     self.showLoader($(formObj.form.data('ajax-update')));
     //                     formObj.submitFunction(formObj);
     //                }
     //           });
     //      } else {
     
     //           $(formObj.submitBtn).on('click', function (e) {
     //                e.preventDefault();
     //                self.submitSearchForm(formObj);
     //           });
     //      }
     
     // }
     
     
     // this.clearSearchForm = function (formObj, doNotClearMainInput) {
     
     //      $(formObj.formHolder).find('input').each(function () {
     //           $(this).val('');
     //           self.addEmptyNotEmptyClass($(this));
     //      });
     //      $(formObj.formHolder).find('select').each(function () {
     //           self.removeAttrSelected($(this));
     //      });
     
     //      if (!doNotClearMainInput) {
     //           if (formObj.mainInput) {
     //                $(formObj.mainInput).val('');
     //           }
     //      }
     // };
     
     this.manageContentTabs = function () {
     
       var contentItems = content.find('.content-item');
       // contentItemHolder = contentItemWrapper.find('.content-item-holder');
       if (contentItems.length > 1) {
         self.createContentItemSwitcher(contentItems);
       }
       self.manageRightBtnMenu();
       self.addOpenClass($(contentItems)[0]);
     }
     
     this.manageRightBtnMenu = function () {
       if ($(rightBtnMenu).find('*').is(':visible')) {
     
         $(content).addClass('has-btn');
       } else {
         $(content).removeClass('has-btn');
       }
     }
     
     
     this.createContentItemSwitcher = function (contentItems) {
       $(content).addClass('has-switcher');
       var contentSwitcher = $('#content-switcher');
       var currBtn,
         currEl;
       for (var i = 0, maxI = contentItems.length; i < maxI; i++) {
         currEl = contentItems[i];
     
         if ($(contentSwitcher).hasClass('content-switcher-row')) {
           currBtn = $('<a class = "btn btn-tab"  data-tooltipright = "1" data-title = "' + $(currEl).find('h2').text() + '" id = "' + $(currEl).find('h2').attr('id') + '"></a>').appendTo($(contentSwitcher));
           //var contentHeader = $(currEl).find('.tab-header i').html();
     
           //$(contentHeader).appendTo(currBtn);
           $(currEl).find('.content-item-header i').clone().appendTo(currBtn);
           //$(currEl).find('h2').remove();
           $(currEl).find('.content-item-header i').remove();
     
         } else {
           currBtn = $('<a class = "btn btn-square btn-fill btn-secondary" data-tooltipright = "1" data-title = "' + $(currEl).find('h2').text() + '"></a>').appendTo($(contentSwitcher));
           $(currEl).find('.content-item-header i').clone().appendTo(currBtn);
           $(currEl).find('.content-item-header i').remove();
         }
     
     
         self.contentItemAddEventListener(contentItems, currBtn, currEl)
       }
       var contentSwitcherChildrens = $('.content-switcher-row').children(),
         gridColumns = "";
       for (var i = 0; contentSwitcherChildrens.length > i; i++) {
         gridColumns += "64px ";
       }
       $('.content-switcher-row').css('grid-template-columns', gridColumns);
       self.addActiveClass($(contentSwitcher).find('a').eq(0));
     };
     
     this.contentItemAddEventListener = function (contentItems, currBtn, currEl) {
     
       if ($(currBtn).is('[data-title]')) {
         self.tooltipEl(currBtn); //todo;
       }
     
       $(currBtn).on('click', function () {
         self.showHideContentItems(contentItems, currEl)
       })
     };
     
     
     this.showHideContentItems = function (contentItems, currEl) {
       var contentSwitcher = $('#content-switcher');
       if ($(currEl).hasClass('open')) {
         return;
       }
       $(contentItems).each(function () {
     
         self.removeOpenClass($(this));
         $(contentSwitcher).find('a').each(function () {
           self.removeActiveClass($(this));
         })
       });
     
       self.addOpenClass($(currEl));
       self.addActiveClass($(contentSwitcher).find('a').eq($(currEl).index()));
     
       openTab = currEl;
       self.tryLoadContainer($(currEl).find('.content-partial:not([data-tab-empty])'));
       //console.log($(currEl).find('.content-partial').not('[data-tab-empty]'));
       if (!($(currEl).index())) { //первый элемент уже загружен на нем не отрабытывает tryLoadContainer, а там запускается manageOpenTab
         self.manageOpenTab();
       }
       if ($(openTab).find('[data-tab-empty]').length) {
         self.deleteRightBtnMenuAdditionalBtn();
       }
     
       setTimeout(function () {
         self.updateNiceScroll();
         self.initAllInputTextareaAfterLoad();
         self.onLoadCheckAllInputs();
       }, 500);
     }
     
     
     //якщо у сontent-detailes не буде вміщуватись значення по ширині
     this.manageDetailesContent = function () {
       if (!$("div").is(".content-details")) {
         return;
       }
       var contentDetails = $(".content-details"),
         contentDetailsLength = contentDetails.length,
         currDetails,
         currDetailsItem,
         contentDetailsWidth,
         contentDetailsItemWidth;
     
       for (var i = 0; i < contentDetailsLength; i++) {
         currDetails = contentDetails[i];
     
         currDetailsItem = $(currDetails).find(".content-details-group");
         contentDetailsWidth = contentDetails.width();
         contentDetailsItemWidth = currDetailsItem.width();
         currDetailsItem.removeClass("content-item-hover");
     
         if (contentDetailsWidth < contentDetailsItemWidth) {
           currDetailsItem.addClass("content-item-hover");
     
         } else {
           currDetailsItem.removeClass("content-item-hover");
     
         }
       }
     }

     this.initAllInputTextareaAfterLoad = function () {
         if ($('input').is('.input')) {
             $('.input').each(function () {
                 self.initInputAfterLoad($(this));
             })
         }
         if ($('textarea').is('.textarea')) {
             $('.input').each(function () {
                 self.initTextareaAfterLoad($(this));
             })
         }
     
     }
     
     
     
     this.initTextareaAfterLoad = function (textarea) {
         $('textarea').on('blur', function () {
             self.addEmptyNotEmptyClass($(this));
         })
         $('textarea').on('focus', function () {
             self.closeAllSelect(body);
         })
     }
     
     this.initInputAfterLoad = function (input) {
     
         $(input).on('blur', function () {
             self.addEmptyNotEmptyClass($(this));
         })
     
         $(input).on('focus', function () {
             var inputValue = $.trim($(this).val());
             if (inputValue == '0' || inputValue == '0.00' || inputValue == '0,00' || inputValue == '0,0' || inputValue == '0.0') {
                 $(this).select();
             };
             self.closeAllSelect(body);
         })
         self.addEmptyNotEmptyClass(input);
     
     }
     
     
     
     
     this.onLoadCheckAllInputs = function () {
         $('input').each(function () {
             self.addEmptyNotEmptyClass($(this));
             if ($(this).is('[readonly]')) {
                 $(this).addClass('input-readonly')
             }
             if ($(this).hasClass('input-file')) {
                 self.inputFileCustomization($(this));
             }
         })
         $('textarea').each(function () {
             self.addEmptyNotEmptyClass($(this));
         })
     }
     
     
     
     this.inputFileCustomization = function (input) {
         var label = $(input).next('label'),
             labelVal = $(label).val();
         $(input).on('change', function (e) {
             var fileName = '';
     
             if (this.files && this.files.length > 1) {
                 fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
             } else if (e.target.value) {
                 fileName = e.target.value.split('\\').pop();
             }
     
             if (fileName) {
                 $('.content-upload-filename').html(fileName);
             } else {
                 //$(label).html(labelVal);
             }
         });
     }

     this.checkboxGroupAnimation = function(el) {
       var temp = true;
       el.on("change", function() {
         if (temp) {
           var checkboxBar = el
               .closest(".checkbox-group")
               .find(".checkbox-group-bar"),
             dur = checkboxBar.css("transition-duration");
           temp = false;
           dur = dur.slice(0, -1);
           dur = +dur * 1000;
           checkboxBar.addClass("on");
           setTimeout(function() {
             checkboxBar.removeClass("on");
             temp = true;
           }, dur);
         }
       });
     };
     
     this.createCheckboxLabel = function(el) {
       $(el)
         .find("input.checkbox")
         .each(function() {
           if (!$(this).is(":visible")) {
             return;
           }
           var checboxId = "checkbox" + self.generateId();
           $(this)
             .addClass("checkbox")
             .attr("id", checboxId)
             .after('<label for = "' + checboxId + '"></label>');
         });
     
       $(el)
         .find("input.check-box")
         .each(function() {
           if (!$(this).is(":visible")) {
             return;
           }
           var checboxId = "checkbox" + self.generateId();
           $(this)
             .addClass("checkbox")
             .attr("id", checboxId)
             .after('<label for = "' + checboxId + '"></label>');
         });
     };
     
     // if($('input').is('.checkbox')){
     //     $('.checkbox').each(function(){
     //         self.checkboxGroupAnimation($(this));
     //     })
     // }
     
     if ($("input").is(".check-box")) {
       $(".check-box").each(function() {
         var checboxId = "checkbox" + self.generateId();
         $(this)
           .addClass("checkbox")
           .attr("id", checboxId)
           .after('<label for = "' + checboxId + '"></label>');
       });
     }

     this.setLabelMultiSelect = function (el) {
     
         var ms = el,
             msWrap = $(ms).siblings('.ms-options-wrap'),
             msHiddenLabel = $(msWrap).find('button span.ms-hidden-label');
     
         $(ms).bind('change', function (e) {
             e.stopPropagation();
             var msEl = $(this),
                 msSelected = $(msEl).siblings('.ms-options-wrap.ms-has-selections');
     
             setTimeout(function () {
                 var labelArr = [],
                     listItems = [],
                     labelHtml,
     
                     //find & get data from control button
                     labelData = $(msHiddenLabel).html().split(', '),
     
                     //find & get data from selected options
                     selectedList = $(msWrap).find('.ms-options ul li.selected');
     
                 for (var i = 0; selectedList.length > i; i++) {
                     var t = $(selectedList[i]).text()
                     listItems.push(t)
                 }
     
                 //check quantity of items & set labelArr
                 if (listItems.length > labelData.length) {
                     labelArr = labelData;
                 } else if (listItems.length == labelData.length) {
                     labelArr = listItems;
                 } else {
                     $(msSelected).find('button .ms-label').remove();
                 }
     
                 //clear html data after change
                 labelHtml = '';
                 for (var i = 0; labelArr.length > i; i++) {
                     //clear control button after change
                     $(msSelected).find('button .ms-label').remove();
                     //prepare new html data
                     labelHtml += '<div class="ms-label">' + labelArr[i] + '</div>';
                 }
     
                 //toggle control button visibility
                 if ($(msWrap).hasClass('ms-has-selections')) {
                     $(msHiddenLabel).addClass('hidden');
                 } else {
                     $(msHiddenLabel).removeClass('hidden');
                 }
     
                 self.addMslabel(msEl, labelHtml)
             }, 0);
     
     
         })
     }
     
     this.addMslabel = function (item, data) {
         //add ms-label
         var t = $(item).siblings('.ms-options-wrap.ms-has-selections').find('button');
         $(data).appendTo(t);
     }
     
     if ($('select').is('[multiple].multiselect')) {
         $('[multiple].multiselect').each(function () {
             var selectPlaceholder = $(this).attr('data-placeholder');
             $(this).multiselect({
                 columns: 1,
                 selectAll: true,
                 texts: {
                     placeholder: selectPlaceholder,
                     selectAll: 'Обрати все',
                     unselectAll: 'Видалити всі позначки'
                 }
             });
             self.setLabelMultiSelect($(this));
         })
     }
     
     if ($('select').is('[multiple].multiselect-search')) {
         $('[multiple].multiselect-search').each(function () {
             var selectPlaceholder = $(this).attr('data-placeholder');
             $(this).multiselect({
                 columns: 1,
                 search: true,
                 selectAll: true,
                 texts: {
                     placeholder: selectPlaceholder,
                     search: 'Пошук',
                     selectAll: 'Обрати все',
                     unselectAll: 'Видалити всі позначки'
                 }
             });
             self.setLabelMultiSelect($(this));
         })
     }
     
     if ($('.ms-options-wrap .ms-options')) {
         setTimeout(function () {
             $('.ms-options-wrap .ms-options').each(function () {
                 self.createSelectScrollBar($(this).get(0));
             })
         }, 0)
     }

     this.openModal = function () {
        $(modal).addClass('open');
        $(modalCloseBtn).on('click', function () {
           self.closeModal();
        });
     }
     
     this.closeModal = function () {
        $(modal).removeClass('open');
        self.clearElEventListeners($(modalContainer));
        setTimeout(function () {
           self.toggleModalClass(modal);
           self.clearModal();
        }, 300)
     }
     
     this.clearModal = function () {
        $(modalContainer).html('');
        $(modalContainer).removeAttr('data-tab-container-url');
     }
     
     this.getModalContent = function (url) {
        $(modalContainer).attr('data-tab-container-url', url);
        //console.log(self.tryLoadContainer($(modal).find('[data-tab-container-url]')))
        self.tryLoadContainer($(modal).find('[data-tab-container-url]'));
     
     }
     
     this.getModalPosition = function (el) {
        var pos = $(el).attr('data-modal');
        switch (pos) {
           case 'modal-right':
              self.toggleModalClass(modal, pos)
              break;
           case 'modal-left':
              self.toggleModalClass(modal, pos)
              break;
           case 'modal-center':
              self.toggleModalClass(modal, pos)
              break;
           case 'modal-top':
              self.toggleModalClass(modal, pos)
              break;
           case 'modal-bottom':
              self.toggleModalClass(modal, pos)
              break;
           case 'modal-fs':
              self.toggleModalClass(modal, pos)
              break;
           default:
              self.toggleModalClass(modal, 'modal-center')
        }
     }
     
     this.toggleModalClass = function (el, name) {
        if ($(el).is('[class^="modal-"]')) {
           $(el).removeClass(function (index, className) {
              return (className.match(/(^|\s)modal-\S+/g) || []).join(' '); //https://stackoverflow.com/questions/2644299/jquery-removeclass-wildcard
           });
        } else {
           $(el).addClass(name);
        }
     }
     
     //activate modal btn
     if (modalOpenBtn) {
        $(modalOpenBtn).on('click', function () {
           var dataUrl = $(this).attr('data-tab-container-url');
           self.getModalPosition($(this));
           $.when(self.getModalContent(dataUrl))
              .done(self.closeAllOpenLi(headerAccountMenu))
              .done(function () {
                 setTimeout(function () {
                    self.openModal();
                 }, 300)
              })
        })
     }
     
     //add eventListenet for modal btn
     this.activateModalBtn = function (el) {
        $(el).find('[data-modal]').each(function () {
           $(this).on('click', function () {
              var dataUrl = $(this).attr('data-tab-container-url');
              self.getModalPosition($(this));
              $.when(self.getModalContent(dataUrl))
                 .done(self.closeAllOpenLi(headerAccountMenu))
                 .done(function () {
                    setTimeout(function () {
                       self.openModal();
                    }, 300)
                 })
           })
        })
     }

     ﻿this.fastSelectControl = function (el) {
     
         el = $(el)
         el.fastselect({
             placeholder: '',
         searchPlaceholder: '',
         noResultsText: 'Результат не знайдено',
             
         });
         if (el.attr("readonly") !== undefined) {
             el.prev(".fstControls").find("button, input").remove();
         };
         el.on('change', function () {
             console.log('onchange', el.val())
         })
     
         el.on('focus',function(){
             console.log('focus', el.val())
         })
         el.on('blur',function(){
             console.log('blur', el.val())
         })
     
     
     }
     
     if ($('select').is('.fastselect')) {
         $('.fastselect').each(function () {
             self.fastSelectControl($(this));
         })
     }

     this.onChangeUrl = function (onChangeEl, onchangeUrl) {
        var url;
        var args = {};
        if (onchangeUrl) {
           url = onchangeUrl;
           args[$(onChangeEl).attr('name')] = $(onChangeEl).val();
        } else {
           url = $(onChangeEl).attr("onchange-url");
           $("[onchange-url='" + url + "']").each(function () {
              args[$(this).attr('name')] = $(this).val();
           });
        }
        var changedInput = body.find('[onchange-url]');
        if ($(changedInput).val().length > 0) {
           $.ajax({
              url: url,
              type: "POST",
              data: args,
              success: function (data) {
                 response = $.parseJSON(JSON.stringify(data));
                 $.each(Object.keys(response), function (index, key) {
                    var tVal = response[key];
                    //console.log(tVal);
                    if (tVal) {
                       var elements;
                       if (key.match('^#')) {
                          elements = $(key);
                       } else {
                          elements = $("[name='" + key + "']");
                       }
     
                       $.each(elements, function (index, element) {
                          var tTag = self.returnTagName(element);
     
                          if (tTag === 'select') {
     
                             var opt;
                             var options = $(elements).find('option');
                             var parentSelect = $(elements).closest('.select-group-inner');
                             var parentReadOnly = $(parentSelect).hasClass('select-readonly');
                             if (parentReadOnly) {
                                $(element).html('');
                                for (var i = 0; i < tVal.length; i++) {
                                   opt = document.createElement('option');
                                   opt.value = tVal[i].value;
                                   opt.innerHTML = tVal[i].text;
                                   if (tVal[i].selected) {
                                      opt.setAttribute('selected', 'selected');
                                   }
                                   if (tVal[i].disabled) {
                                      opt.setAttribute('disabled', 'disabled');
                                   }
                                   $(element).append(opt);
                                }
                             } else {
                                for (var i = 0; i < options.length; i++) {
                                   opt = options[i];
     
                                   if (tVal == opt.value) {
                                      $(opt).prop('selected', 'selected');
                                      opt.setAttribute('selected', 'selected');
                                   }
                                }
                             }
     
                             if (tVal.length <= 0) {
                                $(element).html('');
                             }
     
                             self.updateSelectList($(element));
                          } else if (tTag === 'input') {
                             $(element).val(tVal);
                             self.addEmptyNotEmptyClass($(element));
                          } else {
                             $(element).text(tVal);
                          }
                       });
                    } else if(tVal == null){
                       // var elements1;
                       // if (key.match('^#')) {
                       //    elements1 = $(key);
                       // } else {
                       //    elements1 = $("[name='" + key + "']");
                       // }
                       // $.each(elements1, function (index, element1) {
                       //    console.log($(element1));
                       //    $(element1).val('');
                       //    $(element1).text('');
                       // });
     
                       
                    }
                 });
              }
           });
        }
     
     }
     
     $(document).on('change', '[onchange-url]', function (e) {
        self.onChangeUrl($(this));
     });

     this.setDataTabGroup = function (el) {
     
          if ($(el).find('[ data-tab-open]').length) {
               $(el).find('[ data-tab-open]').on('click', function () {
                    self.showDataTabGroup(el)
               })
          }
     
     
          if($(el).find('select').length){
               $(el).find('select').each(function(){
                    $(this).addClass('.select');
                    self.selectWrap($(this));
               })
          }
     
     
          if($(el).find('input').length){
               $(el).find('input').each(function(){
                    $(this).addClass('input');
                    self.initInputAfterLoad($(this));
               });
          }
     }
     
     this.showDataTabGroup = function (el) {
          dataTabOpen = true;
          self.addOpenClass(el);
     }
     
     
     
     this.hideDataTabGroup = function () {
          $('.data-tab-group').each(function(){
               self.removeOpenClass($(this));
          })
          dataTabOpen = false;
     
     
     }
     
     
     this.manageOpenTab = function () {
          self.deleteRightBtnMenuAdditionalBtn();
          if ($(openTab).find('[data-tab]').length) {
               $(openTab).find('[data-tab]').each(function () {
                    self.addRightBtnMenuAdditionalBtn($(this));
               });
          }
     
     }
     
     this.deleteRightBtnMenuAdditionalBtn = function () {
          if ($(rightBtnMenu).find('[data-tab]').length) {
               $(rightBtnMenu).find('[data-tab]').each(function () {
                    self.clearElEventListeners($(this));
                    $(this).off();
                    $(this).detach();
               })
          }
          if ($(rightBtnMenu).find('[data-tab-main]').length) {
               $(rightBtnMenu).find('[data-tab-main]').each(function () {
                    if ($(openTab).index()) {
                         $(this).css('display', 'none');
                    } else {
                         $(this).css('display', '');
                    }
               })
          }
          self.manageRightBtnMenu();
     }
     
     this.addRightBtnMenuAdditionalBtn = function (el) {
          var cloneEl = $(el).clone();
          //console.log($(el));
          // cloneElTagName = $(cloneEl).prop("tagName").toLowerCase();
          $(cloneEl).appendTo(rightBtnMenu);
     
     
          if ($(cloneEl).hasClass('data-tab-group')) {
               self.setDataTabGroup(cloneEl);
               self.setToolTipsForChild(cloneEl);
          } else {
               self.tooltipEl(cloneEl);
          }
     
          self.manageRightBtnMenu();
     }

     this.autocompleteEl = function (el) {
        var url = $(el).attr("autocomplete-url"); //url to contoller where we can receive array with properties
        var idinputname = $(el).attr("autocomplete-idinput-name"); //set name & id for hidden input near lookup
        var initid = $(el).attr("autocomplete-init-id"); //set value for hidden input when we have saved data
        var onsuccess = $(el).attr("autocomplete-onsuccess"); // testing => ?? why
        var onselect = $(el).attr("autocomplete-onselect"); //call function onchange => select event autocomplete jquery-ui
        var onblur = $(el).attr("autocomplete-onblur"); //testing => after focus ?? why
        var paramsstr = $(el).attr("autocomplete-params"); //testing => ?? why
        var allowcreate = $(el).attr("autocomplete-allowcreate") === ""; //testing => add this block and after next focus value will saved ?? why
        params = paramsstr ? paramsstr.split(",") : null;
        var form = $(el).closest("form");
     
        //get name from lookup input
        var name = idinputname ? idinputname : $(el).attr("name").replace("Name", 'Id');
        if (!name.endsWith('Id')) {
           name += 'Id';
        }
     
        //find hidden input near lookup
        var hidden = $(form).find("input[name='" + name + "']");
        if ($(hidden).length === 0) {
     
           //set attr name & id for hidden input
           $(el).after('<input type="hidden" name="' + name + '" id="' + name + '"/>');
           hidden = $(form).find("input[name='" + name + "']");
     
           //set value for hidden input
           hidden.val(initid ? initid : '');
        }
     
        // if datePicker set datepicker value
        //
        // if checkbox set
     
        //autocomplete - function from jquery ui
        $(el).autocomplete({
           source: function (request, response) {
              // A request object, with a single term property, which refers to the value currently in the text input. 
              var data = {};
              if (params) {
                 console.log(params);
                 $.each(params, function (i, p) {
                    data[p] = $('#' + p).val();
     
                 });
              }
              data['term'] = request.term;
     
              $.ajax({
                 url: url,
                 type: "POST",
                 dataType: "json",
                 data: data,
                 success: function (data) {
                    //receive array with properties
                    if (onsuccess) {
                       new Function('data', onsuccess)(data);
                       console.log(data) ///?????? onsuccess
                    }
                    if (data.length === 0 && !allowcreate) {
                       data.push({
                          label: 'Відповідності не знайдено',
                          value: '',
                          option: ''
                       });
                    }
                    if (!allowcreate) {
                       hidden.val('');
                    }
                    for (var k = 0; k < data.length; k++) {
                       item = data[k];
                       if (item.label == request.term) {
                          //set value for hidden option
                          hidden.val(item.option);
                          break;
                       }
                    };
                    response(
     						// method applies a function to each item in an array or object and maps the results into a new array ?? why
                       $.map(data, function (item) {
                          return {
                             label: item.label,
                             value: item.value,
                             option: item.option,
                             data: item.data
                          };
                       }));
                 }
              });
           },
           classes: {
              "ui-autocomplete": "input-group-autocomplete"
           },
           minLength: 0,
           select: function (event, ui) {
              if (ui.item.option === '') {
                 ui.item.value = '';
              }
              hidden.val(ui.item ? ui.item.option : '');
              if (onselect) {
                 new Function('item', onselect)(ui.item);
              }
           },
           create: function (e) {
              $('.ui-helper-hidden-accessible').remove();
           }
        }).focus(function () {
           var term = $(this).val();
           $(this).data('uiAutocomplete').search(term ? term : '');
        });
     
        // $(el).on('blur', function () {
        //     if (hidden) {
        //         //if hidden input
        //         if (!$(el).val()) {
        //             $(hidden).val('');
     
        //         } else {
        //             var v = $(hidden).val();
        //             if (v === "" && !allowcreate) {
        //                 this.value = '';
        //             }
        //         }
     
        //         if (onblur) {
        //             new Function('value', onblur)($(hidden).val());
        //         }
        //     }
        // });
     
     }
     
     
     if ($('*').is('[autocomplete-url]')) {
        $('[autocomplete-url]').each(function () {
           self.autocompleteEl($(this));
        })
     }

     $.widget("custom.combobox", {
         _create: function () {
             this.wrapper = $("<span>")
                 .addClass("custom-combobox")
                 .insertAfter(this.element);
     
             this.element.hide();
             this._createAutocomplete();
             this._createShowAllButton();
         },
     
         _createAutocomplete: function () {
             var selected = this.element.children(":selected"),
                 value = selected.val() ? selected.text() : "";
     
             this.input = $("<input>")
                 .appendTo(this.wrapper)
                 .val(value)
                 .attr("title", "")
                 .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                 .autocomplete({
                     delay: 0,
                     minLength: 0,
                     source: $.proxy(this, "_source")
                 })
                 .tooltip({
                     classes: {
                         "ui-tooltip": "ui-state-highlight"
                     }
                 });
     
             this._on(this.input, {
                 autocompleteselect: function (event, ui) {
                     ui.item.option.selected = true;
                     this._trigger("select", event, {
                         item: ui.item.option
                     });
                 },
     
                 autocompletechange: "_removeIfInvalid"
             });
         },
     
         _createShowAllButton: function () {
             var input = this.input,
                 wasOpen = false;
     
             $("<a>")
                 .attr("tabIndex", -1)
                 .attr("title", "Show All Items")
                 .tooltip()
                 .appendTo(this.wrapper)
                 .button({
                     icons: {
                         primary: "ui-icon-triangle-1-s"
                     },
                     text: false
                 })
                 .removeClass("ui-corner-all")
                 .addClass("custom-combobox-toggle ui-corner-right")
                 .on("mousedown", function () {
                     wasOpen = input.autocomplete("widget").is(":visible");
                 })
                 .on("click", function () {
                     input.trigger("focus");
     
                     // Close if already visible
                     if (wasOpen) {
                         return;
                     }
     
                     // Pass empty string as value to search for, displaying all results
                     input.autocomplete("search", "");
                 });
         },
     
         _source: function (request, response) {
             var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
             response(this.element.children("option").map(function () {
                 var text = $(this).text();
                 if (this.value && (!request.term || matcher.test(text)))
                     return {
                         label: text,
                         value: text,
                         option: this
                     };
             }));
         },
     
         _removeIfInvalid: function (event, ui) {
     
             // Selected an item, nothing to do
             if (ui.item) {
                 return;
             }
     
             // Search for a match (case-insensitive)
             var value = this.input.val(),
                 valueLowerCase = value.toLowerCase(),
                 valid = false;
             this.element.children("option").each(function () {
                 if ($(this).text().toLowerCase() === valueLowerCase) {
                     this.selected = valid = true;
                     return false;
                 }
             });
     
             // Found a match, nothing to do
             if (valid) {
                 return;
             }
     
             // Remove invalid value
             this.input
                 .val("")
                 .attr("title", value + " didn't match any item")
                 .tooltip("open");
             this.element.val("");
             this._delay(function () {
                 this.input.tooltip("close").attr("title", "");
             }, 2500);
             this.input.autocomplete("instance").term = "";
         },
     
         _destroy: function () {
             this.wrapper.remove();
             this.element.show();
         }
     });
     
     //$(".combobox").combobox();

     this.createValidateAttr = function (data) {
       var validateAttr = " validate "
       for (var i = 0; i < data.length; i++) {
         validateAttr += "validate-" + data[i] + ' ';
       }
       return validateAttr;
     }
     
     this.returnValidateMsg = function (property) {
       switch (property) {
         case 'required':
           return "Обов'язкове поле";
     
         case 'not-empty':
           return "Обов'язкове поле";
     
         case 'number':
           return 'Значення має бути числом';
     
         case 'phone':
           return 'Невірно введений номер';
     
         case 'email':
           return 'Невірний формат email';
     
         case 'equal-to':
           return 'Значення не співпадають';
     
         case 'length-max':
           return 'Максимальна кількість знаків ';
     
         case 'length-min':
           return 'Мінімальна кількість знаків ';
     
         case 'range':
           return 'Значення має бути у діапазоні від ';
     
         case 'url':
           return 'Невірний формат url-адреси';
     
         case 'creditcard':
           return 'Невірний номер картки';
     
         case 'pattern':
           return 'Недопустиме значення';
     
         case 'date':
           return 'Невірний формат дати';
     
       }
     
     }
     
     this.findValidateForm = function (el) {
       if (self.returnTagName(el) == 'form') {
         submitFunction(el);
       } else {
         $(el).find('form').each(function () {
           submitFunction($(this));
         })
       }
       $(el).find('[data-val-required]:not([type="checkbox"])').each(function () {
         currEl = $(this);
         currEl.siblings('label').append('*');
       })
     
       function submitFunction(form) {
       var submitBtn = $(form).find('input[type = "submit"]') || $(form).find('button[type = "submit"]') || $(form).find('.submit-form-button');
         $(submitBtn).off().on('click', function (e) {
           if (!$(form).hasClass('no-validate')) {
             self.checkFormBeforeSubmit(e, form);
           } else {
             $(form).submit();
           }
         })
       }
     }
     
     this.checkFormBeforeSubmit = function(e, form){
       e.preventDefault();
       if (self.validateForm(form)) {
         $(form).submit();
       } else{
         $('html, body').animate({
           scrollTop: ($('.asp-validation.active').first().offset().top-120)
         }, 700);
       }
     }
     
     
     this.showNotValidMsg = function (el, msg) {
       var elTagName = self.returnTagName(el);
       var elParent;
       if (elTagName == 'input') {
         elParent = $(el).closest('.input-group');
       }
       if (elTagName == 'select') {
         elParent = $(el).closest('.select-group');
       }
     
       var jsValidationMsgEl = $(elParent).find('.asp-validation');
     
       if (!jsValidationMsgEl.length) {
         return;
       }
     
       $(jsValidationMsgEl).text(msg);
     
       self.addActiveClass(jsValidationMsgEl);
     
       $(elParent).on('mouseover', destroyNotValidMsg);
       $(elParent).find('input').each(function () {
         $(this).bind('focus', destroyNotValidMsg);
       });
       //$(body).on('click', destroyNotValidMsg);
     
       function destroyNotValidMsg() {
         self.removeActiveClass(jsValidationMsgEl);
         setTimeout(function () {
           $(elParent).find('.asp-validation').text('');
     
         }, timeOutInterval)
         $(elParent).off('mouseover', destroyNotValidMsg);
         $(elParent).find('input').each(function () {
           $(this).unbind('focus', destroyNotValidMsg)
         });
     
       }
     }
     
     this.validateForm = function (form) {
       if (!form || !form.length) {
         return;
       }
       var validToken = true,
         currMsg,
         currEl;
     
       $(form).find('[data-val-required]').each(function () {
         currEl = $(this);
         var testMsg = $(this).attr('data-val-required');
         //console.log('data-val-required =',testMsg);
         currMsg = self.validateRequired(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[no-validate]').each(function () {
         currEl = $(this);
         validToken = true;
       })
     
       $(form).find('[validate-not-empty]').each(function () {
         currEl = $(this);
         currMsg = self.validateNotEmpty(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[validate-number]').each(function () {
         currEl = $(this);
         currMsg = self.validateNumber(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-phone]').each(function () {
         currEl = $(this);
         var testMsg = $(this).attr('data-val-phone');
         //console.log('data-val-phone =',testMsg);
         currMsg = self.validatePhone(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-email]').each(function () {
         currEl = $(this);
         currMsg = self.validateEmail(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-equalto-other]').each(function () {
         var currEl,
           currElVal,
           currElAttr,
           equalElName,
           equalElId,
           equalElVal;
     
         currEl = $(this);
         currElVal = currEl.val();
         currElAttr = currEl.attr('data-val-equalto-other');
     
         equalElName = currElAttr.replace(/[^\w\s]/gi, '');
         equalElId = '#' + equalElName;
         equalElVal = $(equalElId).val();
         currMsg = self.validateEqualTo(currElVal, equalElVal);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
           self.showNotValidMsg($(equalElId), currMsg);
         }
       })
     
       $(form).find('[data-val-length-max]').each(function () {
         var currEl,
           currElVal,
           currElLength,
           currElLengthMax;
     
         currEl = $(this);
         var testMsg = $(this).attr('data-val-length');
         //$(this).addClass('test')
         //console.log('data-val-length-max = ',testMsg);
         currElVal = currEl.val();
         currElLength = currElVal.length;
     
         currElLengthMax = currEl.attr('data-val-length-max');
         currMsg = self.validateLengthMax(currElLength, currElLengthMax);
         if (currMsg) {
           validToken = false;
           currMsg += currElLengthMax;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-length-min]').each(function () {
         var currEl,
           currElVal,
           currElLength,
           currElLengthMin;
     
         currEl = $(this);
         var testMsg = $(this).attr('data-val-length');
         //console.log('data-val-length-min = ',testMsg);
         currElVal = currEl.val();
         currElLength = currElVal.length;
     
         currElLengthMin = currEl.attr('data-val-length-min');
         currMsg = self.validateLengthMin(currElLength, currElLengthMin);
     
         if (currMsg) {
           validToken = false;
           currMsg += currElLengthMin;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-range]').each(function () {
         var currEl,
           currElRangeMax,
           currElRangeMin;
     
         currEl = $(this);
         var testMsg = $(this).attr('data-val-range');
         //console.log('data-val-range =', testMsg);
         currElRangeMax = +currEl.attr('data-val-range-max');
         currElRangeMin = +currEl.attr('data-val-range-min');
     
         currMsg = self.validateRange(currEl, currElRangeMin, currElRangeMax);
     
         if (currMsg) {
           validToken = false;
           currMsg += currElRangeMin + ' до ' + currElRangeMax;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-url]').each(function () {
         currEl = $(this);
         currMsg = self.validateUrl(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       $(form).find('[data-val-creditcard]').each(function () {
         currEl = $(this);
         currMsg = self.validateCreditCard(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       // $(form).find('[data-val-regex-pattern]').each(function () {
       //   var currEl,
       //     currElPattern;
     
       //   currEl = $(this);
       //   var testMsg = $(this).attr('data-val-regex');
       //   console.log('data-val-regex =',testMsg);
       //   currElPattern = currEl.attr('data-val-regex-pattern');
       //   currMsg = self.validatePattern(currEl, currElPattern);
     
       //   if (currMsg) {
       //     validToken = false;
       //     self.showNotValidMsg(currEl, currMsg);
       //   }
       // })
     
       $(form).find('[validate-date]').each(function () {
         currEl = $(this);
         currMsg = self.validateDate(currEl);
         if (currMsg) {
           validToken = false;
           self.showNotValidMsg(currEl, currMsg);
         }
       })
     
       return validToken;
     }
     
     
     this.validateRequired = function (el) {
       if (self.isElValueEmpty(el)) {
         return self.returnValidateMsg('required')
       } else {
         return false;
       }
     }
     
     this.validateNotEmpty = function (el) {
       if (self.isElValueEmpty(el)) {
         return self.returnValidateMsg('not-empty')
       } else {
         return false;
       }
     }
     
     this.validateNumber = function (el) {
       var regexp = /^(0|[1-9]\d*)([\,\.]{1}\d+)?$/
     
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         if (!regexp.test($(el).val())) {
           return self.returnValidateMsg('number')
         } else {
           return false;
         }
       }
     }
     
     this.validatePhone = function (el) {
       var regexp = /^[0-9]{12}$/;
     
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         var currTrimTel = $(el).val().replace(/\D+/g, '');
         if (!regexp.test(currTrimTel)) {
           return self.returnValidateMsg('phone')
         } else {
           return false;
         }
       }
     }
     
     this.validateEmail = function (el) {
       var regexp = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         if (!regexp.test($(el).val())) {
           return self.returnValidateMsg('email')
         } else {
           return false;
         }
       }
     }
     
     this.validateEqualTo = function (elVal, equalVal) {
       if (elVal !== equalVal) {
         return self.returnValidateMsg('equal-to');
       } else {
         return false;
       }
     }
     
     this.validateLengthMax = function (el, lengthMax) {
       if (el > lengthMax) {
         return self.returnValidateMsg('length-max');
       } else {
         return false;
       }
     }
     
     this.validateLengthMin = function (el, lengthMin) {
       if (el < lengthMin && el > 0) {
         return self.returnValidateMsg('length-min');
       } else {
         return false;
       }
     }
     
     this.validateRange = function (el, rangeMin, rangeMax) {
       elVal = +el.val();
       elValLength = el.val().length;
       if (elValLength == 0) {
         return false;
       } else if (elVal < rangeMin || elVal > rangeMax) {
         return self.returnValidateMsg('range');
       } else {
         return false;
       }
     }
     
     this.validateUrl = function (el) {
       var regexp = /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/g
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         if (!regexp.test($(el).val())) {
           return self.returnValidateMsg('url')
         } else {
           return false;
         }
       }
     }
     
     this.validateCreditCard = function (el) {
       var regexp = /^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/g
     
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         var currTrimVal = $(el).val().replace(/\D+/g, '');
         if (!regexp.test(currTrimVal)) {
           return self.returnValidateMsg('creditcard')
         } else {
           return false;
         }
       }
     }
     
     // this.validatePattern = function (el, regexPattern) {
     //   var regexp = new RegExp(regexPattern);
     //   var currTrimVal = $(el).val().replace(/\s+/g, '');
     //   if (self.isElValueEmpty(el)) {
     //     return false;
     //   } else {
     //     if (!regexp.test(currTrimVal)) {
     //       return self.returnValidateMsg('pattern')
     //     } else {
     //       return false;
     //     }
     //   }
     // }
     
     this.validateDate = function (el) {
       var regexp = /^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$/g
       if (self.isElValueEmpty(el)) {
         return false;
       } else {
         if (!regexp.test($(el).val())) {
           return self.returnValidateMsg('date')
         } else {
           return false;
         }
       }
     }
     
     $(".phone").inputmask({
       alias: "phone",
       mask: "+380(99)999-99-99",
       showMaskOnHover: false,
       removeMaskOnSubmit: false
     });

     this.createPaging = function (el) {
       var pagingContainer = $(el).find('[data-pgparams]');
     
       if (!pagingContainer)
         return;
     
       var pgparams = pagingContainer.data('pgparams');
       if (pgparams) {
         if ($(el).find('.pagination')) {
           $(el).find('.pagination a').each(function () {
             var link = $(this).attr('href') + '&' + pgparams;
             $(this).on('click', function (e) {
               e.preventDefault();
               self.showLoader(el);
               self.doPaging(link, el);
               self.updateNiceScroll();
             })
           })
         }
         if ($(el).find('.content-list-sortable')) {
           $(el).find('.content-list-sortable a').each(function () {
             var link = $(this).attr('href') + '&' + pgparams;
             $(this).on('click', function (e) {
               
               e.preventDefault();
               e.stopPropagation();
     
               self.showLoader(el);
               self.doPaging(link, el);
               self.updateNiceScroll();
             })
           })
         }
       }
     }
     
     
     this.doPaging = function (link, el) {
       $.ajax({
         url: link,
         type: "GET",
         success: function (data) {
           $.when(self.clearPaging(el))
             .done($(el).html(data))
             .done(self.createPaging(el))
             .done(self.findGrids($(el)), self.initMask(el))
             .done(self.setDeleteContentListItem($(el)))
             .done(self.createCheckboxLabel(el))
             .done(self.updateNiceScroll())
             .done(self.hideLoader(el));
         },
         error: function (data) {
           console.error('doPaging error', data)
         }
       });
     
     }
     
     
     
     this.clearPaging = function (el) {
       var pagingParent;
       if ($(el).data('ajax-update')) {
         pagingParent = $(el).data('ajax-update');
       } else {
         pagingParent = $(el);
       }
       self.clearElEventListeners(pagingParent);
     }
     
     
     this.findPaging = function () {
       if ($('*').is('[data-pgparams]')) {
         $('[data-pgparams]').each(function () {
           self.createPaging($(this));
         })
       }
     }

     //https://stackoverflow.com/questions/1318076/jquery-hasattr-checking-to-see-if-there-is-an-attribute-on-an-element
     
     this.deleteContentListItem = function (btn) {
       $(btn).on("click", function (e) {
         e.preventDefault();
     
         var disableAttr = $(this).attr("disabled");
         if (typeof disableAttr !== typeof undefined && disableAttr !== false) {
           return;
         }
     
         $(this).attr("disabled", "disabled");
     
         var row = $(e.target).closest(".content-list");
         var url = $(this).attr("href");
         var msg = "<p>Ви впевнені що хочете видалити запис?</p>";
     
         self.createDialog(e, msg, confirmDialog, refuseDialog);
     
         function confirmDialog() {
           $.ajax({
             type: "Post",
             url: url,
             complete: function (jqXHR, textStatus, errorThrown) {
               console.log(jqXHR.status);
               console.log(textStatus);
               console.log(errorThrown);
             },
             error: function (data) {
               console.log('error', data);
               $(btn).removeAttr("disabled");
               msg = "<p>Помилка видалення</p>";
               self.createDialog(e, msg, refuseDialog);
             },
             success: function (data) {
               console.log('success', data);
               $(btn).removeAttr("disabled");
               row.remove();
               self.updateNiceScroll();
               if ($('.btn-add-state').length) {
                 self.checkBtnState();
               }
               // if (data.success === true) {
               // } else {
               //   $(btn).removeAttr('disabled');
               //   msg = "<p>Помилка видалення</p>";
               //   self.createDialog(e, msg, refuseDialog);
               // }
             }
           });
         }
     
         function refuseDialog() {
           $(btn).removeAttr("disabled");
         }
       });
     };
     
     this.setDeleteContentListItem = function (el) {
       $(el)
         .find(".content-list-delete a")
         .each(function () {
           self.deleteContentListItem($(this));
         });
     };
     
     if ($("div").is(".content-list-delete")) {
       self.setDeleteContentListItem(body);
     }
     
     this.checkBtnState = function () {
       var urlBtnAddState = 'ApplicationSetting/IsPossibleToAddNew';
     
       checkState(urlBtnAddState);
     
       function checkState(url) {
         $.ajax({
           type: "Post",
           url: url,
           error: function (data) {},
           success: function (data) {
             if (data) {
               enableBtn();
             } else {
               disableBtn();
             }
           }
         });
       }
       var btnAdd = $(rightBtnMenu).find('.btn-add-state');
     
       function disableBtn() {
         $(btnAdd).addClass("btn-disabled");
       }
     
       function enableBtn() {
         $(btnAdd).removeClass("btn-disabled");
       }
     }

     this.tryLoadContainer = function (element, url) {
       self.showLoader(element);
       if (element) {
         self.updateNiceScroll();
         $(element).each(function (index, value) {
           var cUrl = url ? url : $(value).attr("data-tab-container-url");
           var replace = $(value).attr("data-replace");
           if (cUrl.length) {
             $.ajax({
               type: "Get",
               url: cUrl,
               success: function (data) {
                 if (replace === "") { //todo доработать в случае, если замены нне предусматривается
                   $.when($(value).replaceWith(data))
                     .done(self.initMask(element), self.findFormatDateParagraph(element))
                     .done(self.findGrids(element))
                     .done(self.findPaging(element))
                     .done(
                       function () {
                         self.manageOpenTab();
                         self.showHideFooter();
                         self.updateNiceScroll();
                         self.hideLoader(element);
                         self.setToolTipsForChild(element);
                         self.findValidateForm(element);
                         self.setDeleteContentListItem(element);
                         self.manageContentReload(element);
                         self.initTextareaAfterLoad();
                       }
                     );
                 } else {
                   $.when($(value).html(data))
                     .done(self.initMask(element), self.findFormatDateParagraph(element))
                     .done(self.findGrids(element))
                     .done(self.createCheckboxLabel(element))
                     .done(self.findPaging(element))
                     .done(
                       function () {
                         self.manageOpenTab();
                         self.showHideFooter();
                         self.updateNiceScroll();
                         self.hideLoader(element);
                         self.setToolTipsForChild(element);
                         self.findValidateForm(element);
                         self.setDeleteContentListItem(element);
                         self.manageContentReload(element);
                         self.initTextareaAfterLoad();
                         self.activateModalBtn($(this));
                       }
                     );
                 }
                 //self.tryLoadContainer($($(value)[0]));
                 //self.updateNiceScroll();
     
                 return true;
               },
               error: function () {
                 $(value).html('');
                 self.hideLoader(element);
                 self.closeModal();
                 msg = "<p>Помилка! Перезагрузіть сторінку.</p>";
                 self.createDialog(true, msg);
                 return true;
               }
             });
           }
         });
       }
     };

     body.on('click', function (event) {
     
       // console.log(event.target);////TEMP
       var target = event.target;
       if (dialogOpen) {
         return;
       }
       if (searchFormOpen || searchPresetsOpen) {
     
         if ($(target).is($('.content-search-form')) || $(target).closest('form').is($('.content-search-form'))) {
           return;
         }
         if (datePickerOpen) {
           return;
         }
         self.closeSearchFormHolderAndPresetFilters();
     
       }
     
       if (selectOpen) {
         if ($(target).is($('.select-list')) || $(target).is($('.select-gap')) || $(target).closest('ul').is('.select-list')) {
           return;
     
           //todo: check this func
         }
         self.closeAllSelect();
       }
       if (dataTabOpen) {
         if ($(target).closest('.data-tab-group').is('[data-tab]')) {
           return;
         }
         self.hideDataTabGroup();
       }
     
     
       // $("#ui-datepicker-div").on('click', function (event) {
       //   event.stopPropagation();
       // });
     })

     this.initMask = function (el) {
       var tagName;
     
       $(el).find('.mask').each(function () {
         tagName = self.returnTagName($(this));
     
         if (tagName == 'input') {
           self.setInputMask($(this));
         }
         if (tagName == 'p') {
           self.setMaskedParagraph($(this));
         }
       });
     };
     
     this.setInputMask = function (el) {
       var inputParent = $(el).parent(),
         elName = $(el).attr('name'),
         elDd = $(el).attr('id'),
         clonedInput,
         position;
     
       $(inputParent).append('<input id = "' + elDd + '" class = "data-mask" type = "hidden" name = "' + elName + '"/>');
     
       $(el).attr('name', '');
       $(el).attr('id', '');
     
       clonedInput = $(inputParent).find('[name = ' + elName + ']');
     
       if (!self.isElValueEmpty(el)) {
         setInputValue();
       }
       setMaskedInputListeners();
     
       function setMaskedInputListeners() {
         var _value;
     
         $(el).focus(function () {
           _value = $(el).val();
           _value = _value.replace(/\s/g, '');
           _value = _value.replace(/,/g, '.');
           $(el).val(_value);
         });
     
         $(el).blur(setInputValue);
     
         $(el).on('input', function () {
           position = self.getCaretPosition($(el)[0]);
           _value = $(el).val();
     
           _value = _value.replace(/,/g, '.');
     
           var regexp = /^[0-9]{0,12}([\.][0-9]{0,2})?$/;
     
           if (!regexp.test(_value)) {
             _value = _value.substring(0, position - 1) + _value.substring(position, _value.length);
             window.setTimeout(function () {
               self.setCaretPosition($(el)[0], position - 1);
               position = self.getCaretPosition($(el)[0]);
             }, 0);
           }
           $(el).val(_value);
         });
       }
     
       function setInputValue() {
         var _value = $(el).val(),
           _hiddenValue = _value.replace(/\s/g, '');
         _hiddenValue = _hiddenValue.replace(/,/g, '.');
     
         var _arr = _hiddenValue.split('.');
         var _int = _arr[0];
         var _dec = _arr[1];
     
         if ($(el).hasClass('mask-money-dec')) {
           _dec = self.returnDec(_dec, 2);
         }
         _int = _int.replace(/(\d)(?=(\d\d\d)+([^\d]|$))/g, '$1 ');
     
         _value = _int + '.' + _dec;
     
         $(clonedInput).val(_hiddenValue);
     
         $(el).val(_value);
       }
     
       // function getCaretPosition(ctrl) {
       //      if (ctrl.selectionStart || ctrl.selectionStart == '0') {
       //           return ctrl.selectionStart;
       //      } else {
       //           return 0;
       //      }
       // }
     
       // function setCaretPosition(elem, caretPos) {
       //      if (elem != null) {
       //           if (elem.createTextRange) {
       //                var range = elem.createTextRange();
       //                range.move('character', caretPos);
       //                range.select();
       //           } else {
       //                if (elem.selectionStart) {
       //                     elem.focus();
       //                     elem.setSelectionRange(caretPos, caretPos);
       //                } else elem.focus();
       //           }
       //      }
       // }
     };
     
     
     this.getCaretPosition = function (ctrl) {
       if (ctrl.selectionStart || ctrl.selectionStart == '0') {
         return ctrl.selectionStart;
       } else {
         return 0;
       }
     }
     
     this.setCaretPosition = function (elem, caretPos) {
       if (elem != null) {
         if (elem.createTextRange) {
           var range = elem.createTextRange();
           range.move('character', caretPos);
           range.select();
         } else {
           if (elem.selectionStart) {
             elem.focus();
             elem.setSelectionRange(caretPos, caretPos);
           } else elem.focus();
         }
       }
     }
     
     this.setMaskedParagraph = function (el) {
       var value = $(el).text(),
         arr,
         int,
         dec;
       value = value.replace(/,/g, '.');
       arr = value.split('.');
       int = arr[0];
       dec = arr[1];
     
       if ($(el).hasClass('mask-money-dec')) {
         dec = self.returnDec(dec, 2);
       }
     
       int = int.replace(/(\d)(?=(\d\d\d)+([^\d]|$))/g, '$1 ');
     
       $(el).text(int + '.' + dec);
     };
     
     this.returnDec = function (str, decLength) {
       if (decLength == 2) {
         var temp;
         if (!str || typeof str == 'undefined' || str.length == 0 || isNaN(+str)) {
           return '00';
         }
         if (str.length == 1) {
           return str + '0';
         }
         if (str.length > 2) {
           str = str.split('');
           temp = +str[2];
           if (temp > 4) {
             str[1] = +str[1] + 1;
           }
           return str[0] + str[1];
         }
         return str;
       }
     };

     this.setGrid = function(el) {
     
         var gridChildren = $(el).children(),
         gridChildrenLength = gridChildren.length,
         currGridChild,
         currGrid,
         gridColumns = "",
         gridTemplate = " ";
     
       if (!gridChildrenLength) {
         return;
       }
     
       for (var i = 0; i < gridChildrenLength; i++) {
         currGridChild = gridChildren[i];
     
         if ($(currGridChild).data("grid")) {
           currGrid = $(currGridChild).data("grid");
           currGrid = +currGrid;
         } else {
           currGrid = 1;
         }
     
         if (currGrid > 3) {
           currGrid = 3;
         }
         if (currGrid < 1) {
           currGrid = 1;
         }
     
         gridTemplate = gridTemplate + "_" + i + " ";
         gridColumns += "1fr ";
         if (currGrid > 1) {
           gridTemplate = gridTemplate + "_" + i + " ";
           gridColumns += "1fr ";
         }
         if (currGrid > 2) {
           gridTemplate = gridTemplate + "_" + i + " ";
           gridColumns += "1fr ";
         }
       
       }
     
       $(el).css({
         "grid-template-columns": gridColumns,
         "grid-template-areas": '"' + gridTemplate + '"'
       });
     
       if ($(el).closest(".content-list-header")) {
         $(el)
           .closest(".content-list-header")
           .siblings(".content-list")
           .each(function() {
             $(this)
               .find(".grid-container-nested")
               .each(function() {
                 $(this).css({
                   "grid-template-columns": gridColumns,
                   "grid-template-areas": '"' + gridTemplate + '"'
                 });
               });
           });
       }
       self.manageGridContent();
     };
     
     
     this.manageGridContent = function() {
       if (!$("div").is(".grid")) {
         return;
       }
     
       var grid = $(".grid"),
         gridLength = grid.length,
         currGrid,
         currGridItem,
         currGridItemWidth,
         currGridItemHeight,
         currGridInner,
         currGridInnerWidth,
         currGridInnerHeight;
     
       for (var i = 0; i < gridLength; i++) {
         currGrid = grid[i];
         currGridItem = $(currGrid).children(".grid-inner");
         currGridItem.removeClass("grid-hover");
         currGridItem.removeClass("grid-hover-height");
         currGridInner = currGridItem.children("p:not(.mask-money-dec)");
         
         
     
         currGridItemWidth = currGridItem.outerWidth();
         currGridInnerWidth = currGridInner.width();
     
         currGridItemHeight = currGridItem.outerHeight();
         currGridInnerHeight = currGridInner.height();
         
         if(currGridInner.length>1){
           for (var k = 0; k < currGridInner.length; k++){
             var gridInnerItem = currGridInner[k];
             var gridInnerItemWidth = $(gridInnerItem).width();
             var gridInnerItemHeight = $(gridInnerItem).height();
             if(currGridInnerWidth < gridInnerItemWidth){
               currGridInnerWidth = gridInnerItemWidth;
             }
             currGridInnerHeight += gridInnerItemHeight;
           }
         }
         currGridItem.removeClass("grid-hover");
         currGridItem.removeClass("grid-hover-height");
         
         if (currGridInnerHeight > currGridItemHeight && currGridInnerWidth > currGridItemWidth) {
           currGridItem.addClass("grid-hover-height");
         } else if (currGridInnerHeight > currGridItemHeight) {
           currGridItem.addClass("grid-hover-height");
         } else if (currGridInnerWidth > currGridItemWidth) {
           currGridItem.addClass("grid-hover");
         } else {
           currGridItem.removeClass("grid-hover");
           currGridItem.removeClass("grid-hover-height");
         }
       }
     };
     
     
     this.findGrids = function(el) {
       $(el)
         .find(".grid-container")
         .each(function() {
           self.setGrid($(this));
     
           //find content-list-link
           setTimeout(function () {
             if ($("a").is(".content-list-link")) {
               self.addContentListLink(body);
             }
           }, 0);
         });
     };
     
     if ($("div").is(".grid-container")) {
       $(".grid-container").each(function() {
         self.setGrid($(this));
       });
     }
     
     //set content-list-link href attr
     this.addContentListLink  = function (el) {
       $(el)
         .find(".content-list-link")
         .each(function () {
           var link = $(this).siblings('.content-list-edit').children('.btn-link').attr('href');
         $(this).attr('href', link);
         });
     };

     this.createMessenger = function () {
         var frameWrapper = $('#frame-wrapper'),
             widgetHidden = true,
             closeButton = frameWrapper.find('.frame-close'),
             win = $(window),
             winHeight = 0,
             winWidth = 0,
             resizeTimer,
             frame = '<iframe class = "frame" src="' + frameWrapper.data('url') + '"  frameborder="0" scrolling="no"></iframe>',
             wrapperWidth = parseInt(frameWrapper.css('width')),
             wrapperHeight = parseInt(frameWrapper.css('height'));
     
     
         $('#show-messenger').on('click', function () {
             if (!widgetHidden)
                 return;
             $.when(createFrame()).then(showFrame()).then(self.closeAllOpenLi($('#header-account-menu')));
         })
     
         $(closeButton).on('click', function () {
             hideFrame();
             setTimeout(function () {
                 destroyFrame()
             }, timeOutInterval);
         })
     
     
         function showFrame() {
             startFunction();
             frameWrapper.css({
                 'z-index': '2000',
                 'opacity': '',
                 'max-width': '80%',
                 'transition': '.05s',
                 'top': '50%',
                 'transform': 'translateY(-' + frameWrapper.height() / 2 + 'px)'
             }).animate({
                 right: 0
             }, timeOutInterval);
     
         }
     
         function hideFrame() {
             widgetHidden = true;
             frameWrapper.animate({
                 opacity: 0
             }, 500, function () {
                 frameWrapper.css({
                     'top': '',
                     'right': '-390px',
                     'max-width': '0',
                     'left': '',
                     'z-index': '-1',
                     'transition': ''
                 })
             })
     
         }
     
         function createFrame() {
             widgetHidden = false;
             frameWrapper.append(frame);
         }
     
         function destroyFrame() {
             $('.frame').remove();
     
         }
     
         function startFunction() {
             if (widgetHidden)
                 return;
             setWinSize();
             setFrameSize();
         }
     
         function setWinSize() {
             winHeight = win.height();
             winWidth = win.width();
         };
     
         function setFrameSize() {
             if (winHeight < wrapperHeight) {
                 frameWrapper.css({
                     'height': winHeight + 'px',
                     'transform': 'translateY(-' + winHeight / 2 + 'px)'
                 });
             } else if (wrapperHeight != frameWrapper.height()) {
                 frameWrapper.css({
                     'top': '50%',
                     'height': '',
                     'transform': 'translateY(-' + frameWrapper.height() / 2 + 'px)'
                 });
     
             }
             if (winWidth < wrapperWidth) {
                 frameWrapper.css({
                     'width': winWidth + 'px',
                     'left': '0'
                 });
             } else if (frameWrapper.width() < wrapperWidth) {
                 frameWrapper.css({
                     'width': wrapperWidth + 'px'
                 });
             }
     
         };
     
         function dragTopLimit(top) {
             if (top < 0) {
                 top = 0;
             }
             if (top > (winHeight - frameWrapper.height())) {
                 top = winHeight - frameWrapper.height();
             }
             return top + frameWrapper.height() / 2
         };
     
         function dragLeftLimit(left) {
             if (left < 0) {
                 left = 0;
             }
             if (left > (winWidth - frameWrapper.width())) {
                 left = winWidth - frameWrapper.width();
             }
             return left;
         };
         frameWrapper.draggable({
             drag: function (event, ui) {
                 ui.position.top = dragTopLimit(ui.position.top);
                 ui.position.left = dragLeftLimit(ui.position.left);
             }
         });
         win.on('resize', function (e) {
             if (widgetHidden)
                 return;
             clearTimeout(resizeTimer);
             resizeTimer = setTimeout(function () {
                 startFunction();
             }, 62);
         });
     
     
     }
     
     
     
     if ($('span').is('#show-messenger')) {
         self.createMessenger()
     }

     this.tooltipEl = function (el) {
          // if(!userDisplaySettingsSaved.ShowTooltip){
          //      return;
          // }
          $(el).attr('title', $(el).data('title'));
          $(el).removeData('title');
     
          if ($(el).data('tooltipleft')) {
               $(el).tooltip({
                    position: {
                         my: "right-15 center",
                         at: "left center"
                    },
                    classes: {
                         "ui-tooltip": "tooltip tooltip-left"
                    }
               });
          } else if ($(el).data('tooltipright')) {
               $(el).tooltip({
                    position: {
                         my: "left+15 center",
                         at: "right center"
                    },
                    classes: {
                         "ui-tooltip": "tooltip tooltip-right"
                    }
               });
          } else {
               $(el).tooltip();
          }
     }
     
     
     this.setTooltips = function () {
          $('[data-title]').each(function () {
     
               self.tooltipEl($(this));
          })
     }
     
     this.setToolTipsForChild = function (el) {
          $(el).find('[data-title]').each(function () {
     
               self.tooltipEl($(this));
          })
     }
     
     
     
     if ($('*').is('[data-title]')) {
          self.setTooltips();
     }

     this.showLoader = function (el) {
          var curElParent = $(el).parent();
          if (!curElParent.find('.loader-wrapper').length) {
               curElParent.addClass('loader-parent');
               curElParent.append('<div class="loader-wrapper"><div class="loader"></div></div>');
          } else {
               $('.loader-wrapper').remove();
               curElParent.removeClass('loader-parent');
          }
     }
     
     this.hideLoader = function (el) {
          var curElParent = $(el).parent();
          if ($(curElParent).hasClass('loader-parent')) {
               $('.loader-wrapper').remove();
               curElParent.removeClass('loader-parent');
          }
     }

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

     this.onAjaxComplete = function(xhr, status, el, url) {
       var typeStatus, errorsMsg, currError, errorsArr, validationEl, invalidMsg;
       errorsArr = [];
       typeStatus = JSON.parse(xhr.responseText);
       if (status === "success" && typeStatus.success === true) {
         self.getAjaxContainer(el, url);
       } else if (status === "success" && typeStatus.success === false) {
         for (key in typeStatus.errors) {
           currError = typeStatus.errors[key];
           invalidMsg = "<li>" + currError.item1 + " " + currError.item2 + "</li>";
           errorsArr.push(invalidMsg);
         }
         errorsMsg = errorsArr.join("");
         showValidationErrors(el, errorsMsg);
       }
     
       function showValidationErrors(element, msg) {
         validationEl = $(element).find(".validation-summary-errors ul");
         validationEl.html("" + msg + "");
       }
     };
     
     this.getAjaxContainer = function(el, url) {
       self.showLoader();
       el.load(url, function() {
         self.manageContentReload(el);
       });
     };
     
     this.manageContentReload = function(el) {
       //init template elements&components after reload
       self.findGrids(el);
       self.createCheckboxLabel(el);
       self.initTextareaAfterLoad();
       self.setDeleteContentListItem(el);
     
       if ($(el).find("data-pgparams")) {
         $("[data-pgparams]").each(function () {
           self.createPaging($(el));
         });
       }
       if ($(el).find("select").length) {
         $(el)
           .find("select")
           .not(".multiselect")
           .not(".standard-select")
           .each(function() {
             $(this).addClass("select");
             self.selectWrap($(this));
           });
       }
     
       if ($("input").is(".input")) {
         $(".input").each(function() {
           self.initInputAfterLoad($(this));
         });
       }
       if ($("input").is(".input")) {
         $(".input").each(function() {
           self.onLoadCheckAllInputs($(this));
         });
       }
     
       if ($("input").is(".datepicker")) {
         $(".datepicker").each(function() {
           self.createDatePicker($(this));
         });
       }
       if ($(el).find("[multiple].multiselect").length) {
         $(el)
           .find("[multiple].multiselect")
           .each(function() {
             $("[multiple].multiselect").each(function() {
               var selectPlaceholder = $(this).attr("data-placeholder");
               $(this).multiselect({
                 columns: 1,
                 selectAll: true,
                 texts: {
                   placeholder: selectPlaceholder,
                   selectAll: "Обрати все",
                   unselectAll: "Видалити всі позначки"
                 }
               });
               self.setLabelMultiSelect($(this));
             });
           });
       }
       self.updateNiceScroll(el);
       self.hideLoader();
     };

     this.buildContentTree = function (template, items, el, input, form, btn, status, changedItems) {
       self.createTreeData(template, el);
       self.setCheckedData(items, el);
       if (typeof status === "boolean") {
         
         self.setChangedItems(changedItems, el, status);
         self.editTreeContentListeners(el, status);
       }
       if (form) {
         var url = $(form).attr('action');
         self.submitTreeContent(btn, input, form, url, status);
       }
     }
     
     this.createTreeData = function (data, container) {
       var a = JSON.parse(data);
     
       $(container).html(self.createTreeDataTemplate(a.FirstLevels))
       self.addEventListenerContentTree(container);
     }
     
     this.createTreeDataTemplate = function (data) {
       var currDataObj = data,
         currEl;
     
       var html = '<ul class = "parent-item">';
     
       for (var k = 0; k < currDataObj.length; k++) {
         currEl = currDataObj[k];
         if (currEl.ChildItems && currEl.ChildItems.length) {
           html += '<li class="child-item"><div class="header-item">' +
             '<input id = "' +
             currEl.Code +
             '" class="checkbox header-checkbox" type="checkbox" name="' +
             currEl.Name +
             '"' +
             '/>' +
             '<label for="' +
             currEl.Code +
             '">' + currEl.Code + ' ' +
             currEl.Name;
             if (currEl.Info != null)
             {
               html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
             }
             html += '</label><button type="button" class="btn btn-square btn-tiny icon-sm btn-parent"></button></div>';
           html += self.createTreeDataTemplate(currEl.ChildItems);
         } else if (currEl.Caption !== null && currEl.Caption !== undefined) {
           html += '<li class="child-item last-item">' +
             '<input id = "' +
             currEl.Code +
             '" class="checkbox caption-checkbox" type="checkbox" name="' +
             currEl.Name +
             '"' +
             '/>' +
             '<label for="' +
             currEl.Code +
             '">' + currEl.Code + ' ' +
             currEl.Name;
             if (currEl.Info != null)
             {
               html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
             }
             html += '</label><input class="caption-input" type="text" disabled>';
         } else {
           html += '<li class="child-item last-item">' +
             '<input id = "' +
             currEl.Code +
             '" class="checkbox" type="checkbox" name="' +
             currEl.Name +
             '"' +
             '/>' +
             '<label for="' +
             currEl.Code +
             '">' + currEl.Code + ' ' +
             currEl.Name;
             if (currEl.Info != null)
             {
               html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
             }
             html += '</label>';
         }
         html += '</li>'
       }
       html += '</ul>'
       return html
     }
     
     // this.addEventListenerContentTree = function (el) {
     //   var parentBtn,
     //     parentHeader,
     //     parentCheckbox,
     //     childItem,
     //     childCheckbox,
     //     inputCaption;
     
     //   parentBtn = $(el).find('.btn-parent');
     //   parentHeader = $(el).find('.header-item');
     //   parentCheckbox = $(parentHeader).find('input[type="checkbox"]');
     //   childItem = $(el).find('.child-item');
     //   childCheckbox = $(childItem).find('input[type="checkbox"]');
     //   inputCaption = $(childCheckbox).siblings('input.caption-input');
     //   $(el).on("change", "input[type='checkbox']", function () {
     //     var headerItem = $(this).closest('.header-item');
     //     var childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']").not('.caption-checkbox, .checkbox-disabled');
     //     if ($(this).is(':checked')) {
     //       childInputs.prop('checked', true);
     //     } else {
     //       childInputs.prop('checked', false);
     //     }
     //   });
     
     //   if ($(childCheckbox).parent().is('.caption-item')) {
     
     //     $(childCheckbox).on('change', function () {
     //       var currInputCaption,
     //         currDisabledInput;
     //       currInputCaption = $(this).siblings('input.caption-input');
     //       currDisabledInput = currInputCaption.is('[disabled]');
     
     //       if (currDisabledInput) {
     //         $(currInputCaption).removeAttr('disabled');
     //       } else {
     //         $(currInputCaption).attr('disabled', 'disabled');
     //       }
     //       if ($(this).is(':not(:checked)')) {
     //         var currParentHeader = $(this).parents('.parent-item').prev('.header-item');
     //         $(currParentHeader).find("input[type='checkbox']").prop('checked', false);
     //       }
     //     });
     
     //     $(inputCaption).on('change', function () {
     //       var inputCaptionVal = $(this).val();
     //       var InputcaptionCheckbox = $(this).siblings('input[type="checkbox"]');
     //       $(InputcaptionCheckbox).attr('value', inputCaptionVal);
     //     });
     //   }
     
     //   $(parentBtn).on('click', function () {
     //     $(this).toggleClass('is-transform');
     //     $(this).parent(parentCheckbox).next().slideToggle();
     //   })
     // }
     this.addEventListenerContentTree = function (el) {
       var parentBtn,
           parentHeader,
           parentCheckbox,
           childItem,
           childCheckbox,
           inputCaption;
       parentBtn = $(el).find('.btn-parent');
       parentHeader = $(el).find('.header-item');
       parentCheckbox = $(parentHeader).find('input[type="checkbox"]');
       childItem = $(el).find('.child-item');
       childCheckbox = $(childItem).find('input[type="checkbox"]');
       inputCaption = $(childCheckbox).siblings('input.caption-input');
       $(el).on("change", "input[type='checkbox']", function () {
           var headerItem = $(this).closest('.header-item');
           var childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']");
           // if ($(this).is(':checked')) {
           // //     childInputs.prop('checked', true);
           // } else {
             if ($(this).is(':not(:checked)')) {
               childInputs.prop('checked', false);
               var caption = childInputs.siblings('input.caption-input');
               for (var i = 0; i < caption.length; i++) {   
                 $(caption[i]).attr('disabled', 'disabled');
             }
           }
           if ($(this).is(':not(:checked)')) {
               var currParentHeaderNotChecked = $(this).parents('.parent-item').prev('.header-item');
               for (i = 0; i < currParentHeaderNotChecked.length; i++) {
                   var childInputs = $(currParentHeaderNotChecked[i]).next(".parent-item")
                       .find("input[type='checkbox']");
                   var checkInputs = false;
                   for (var j = 0; j < childInputs.length; j++) {
                       if (childInputs[j].checked == true) {
                           checkInputs = true;
                       }
                   }
                   if (checkInputs != true)
                       $(currParentHeaderNotChecked[i]).find("input[type='checkbox']").prop('checked', false);
               }
           }
       });
       if ($(childCheckbox).parent().is('.last-item')) {
           $(childCheckbox).on('change', function () {
               var currInputCaption,
                   currDisabledInput;
               currInputCaption = $(this).siblings('input.caption-input');
               currDisabledInput = currInputCaption.is('[disabled]');
               if (currDisabledInput) {
                   $(currInputCaption).removeAttr('disabled');
               } else {
                   $(currInputCaption).attr('disabled', 'disabled');
               }
               if ($(this).is(':checked')) {
                   var currParentHeader = $(this).parents('.parent-item').prev('.header-item');
                   $(currParentHeader).find("input[type='checkbox']").prop('checked', true);
               }
           });
           $(inputCaption).on('change', function () {
               var inputCaptionVal = $(this).val();
               var InputcaptionCheckbox = $(this).siblings('input[type="checkbox"]');
               $(InputcaptionCheckbox).attr('value', inputCaptionVal);
           });
       }
       $(parentBtn).on('click', function () {
           $(this).toggleClass('is-transform');
           $(this).parent(parentCheckbox).next().slideToggle();
       })
       $(".info-tree").on('click', function(e){
           e.preventDefault();
           mt.createDialog(null ,$(this).attr("title"));
       })
     }
     
     this.setCheckedData = function (data, container) {
       if (data !== null && data.length > 0) {
         var checkedItemsObj = JSON.parse(data);
         for (item in checkedItemsObj) {
           var currItem = checkedItemsObj[item]
           var currId = currItem.id;
           var currVal = currItem.value;
           var trimCurrId = currId.replace(/(?:\.)/g, '\\.')
     
           var findItem = $(container).find("#" + trimCurrId);
           $(findItem).prop('checked', true);
           if ($(findItem).hasClass('caption-checkbox')) {
             $(findItem).val(currVal);
             $(findItem).siblings('.caption-input').val(currVal).removeAttr('disabled');
           }
         }
       }
     }
     
     this.setChangedItems = function(data, container, status){
       if (data !== null && data.length > 0) {
         var checkedItemsObj = JSON.parse(data);
         for (item in checkedItemsObj) {
           var currItem = checkedItemsObj[item]
           var currId = currItem.id;
           var currVal = currItem.value;
           var trimCurrId = currId.replace(/(?:\.)/g, '\\.')
     
           var findItem = $(container).find("#" + trimCurrId);
           
           if (status){
             $(findItem).prop('checked', false);
             $(findItem).addClass('checkbox-deleted');
           } else{
             $(findItem).prop('checked', true);
             $(findItem).addClass('checkbox-added');
           }
           if ($(findItem).hasClass('caption-checkbox')) {
             $(findItem).val(currVal);
             $(findItem).siblings('.caption-input').val(currVal).removeAttr('disabled');
           }
         }
       }
     }
     
     this.getTreeContentData = function (form, input, status) {
       var resultContainer = $(form).find('#content-tree'),
         objJson = [];
       if (status === true) {
         checkedArr = $(resultContainer).find('.checkbox-deleted').not('.checkbox-disabled');
       } else if(status === false){
         checkedArr = $(resultContainer).find('.checkbox-added').not('.checkbox-disabled');
       } else {
         checkedArr = $(resultContainer).find('input:checked');
       }
       for (var k = 0; k < checkedArr.length; k++) {
         var currInput = checkedArr[k],
           currInputId = $(currInput).attr('id'),
           currInputVal = $(currInput).attr('value'),
           pushTo = {
             id: currInputId,
             value: currInputVal
           };
         objJson.push(pushTo);
       }
       var obj = JSON.stringify(objJson);
       $(form).find(input).val(obj);
     }
     
     this.submitTreeContent = function (btn, input, form, url, status) {
       $(btn).on('click', function (e) {
         e.preventDefault();
         self.getTreeContentData(form, input, status);
         
         if (self.validateForm(form, url)) {
           $.ajax({
               type: 'post',
               url: url,
               data: $(form).serialize(),
             })
             .done(function (result) {
               if (result.status === "success") {          
                 goBackWithRefresh();           
                 return false;
                 function goBackWithRefresh() { // https://stackoverflow.com/questions/25639290/windows-history-back-location-reload-jquery
                   if ('referrer' in document) {
                     window.location = document.referrer + result.tab;
                   } else {
                     window.history.back(-1);
                   }              
                 }            
               } else {
                 console.log('response_status', result.status)
               }
             })
         } else {
           $('html, body').animate({
             scrollTop: ($('.asp-validation.active').first().offset().top - 180)
           }, 700);
         }
       });
     }
     
     this.editTreeContentListeners = function (el, status) {
       var unCheckedInputs = $('#content-tree .checkbox:not(:checked)'),
         checkedInputs = $('#content-tree .checkbox:checked');
     
       if (status) {
         $(unCheckedInputs).not('.checkbox-deleted').addClass('checkbox-disabled');
         if($(unCheckedInputs).hasClass('caption-checkbox')){
           $(unCheckedInputs).siblings('.caption-input').attr('disabled', true);
         };
       } else {
         $(checkedInputs).not('.checkbox-added').addClass('checkbox-disabled');
         if($(checkedInputs).hasClass('caption-checkbox')){
           $(checkedInputs).not('.checkbox-added').siblings('.caption-input').attr('disabled', true);
         };
       }
     
       $(el).on('change', "input[type='checkbox']", function () {
         var headerItem = $(this).closest('.header-item'),
           childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']").not('.caption-checkbox, .checkbox-disabled'),
           currParentHeader = $(this).parents('.parent-item').prev('.header-item');
     
         if (status) {
           self.removeContentItemClass($(this), childInputs);
           if ($(this).is(':not(:checked)')) {
             $(currParentHeader).find("input[type='checkbox']").not('.checkbox-disabled').addClass('checkbox-deleted');
           }
     
         } else {
           self.addContentItemClass($(this), childInputs);
           if ($(this).is(':not(:checked)')) {
             $(currParentHeader).find("input[type='checkbox']").removeClass('checkbox-added');
           }
         }
     
       });
     }
     
     this.removeContentItemClass = function (el, items) {
       if ($(el).is(':checked')) {
         $(el).removeClass('checkbox-deleted');
         $(items).removeClass('checkbox-deleted');
       } else {
         $(el).addClass('checkbox-deleted');
         $(items).addClass('checkbox-deleted');
       }
     }
     this.addContentItemClass = function (el, items) {
       if ($(el).is(':checked')) {
         $(el).addClass('checkbox-added');
         $(items).addClass('checkbox-added');
       } else {
         $(el).removeClass('checkbox-added');
         $(items).removeClass('checkbox-added');
       }
     }
      this.currentTab = function(){  
         var currentUrl = $(location).attr('href');
         var tabId = currentUrl.split('#')[1];
         $("#" + tabId).trigger("click");
     }

     
     /**
      * размер страницы с учетом прокрутки
      */
     
     this.returnScrollHeight = function () {
     
         return Math.max(
             document.body.scrollHeight, document.documentElement.scrollHeight,
             document.body.offsetHeight, document.documentElement.offsetHeight,
             document.body.clientHeight, document.documentElement.clientHeight
         );
     }
     
     
     /**
      * высота видимой области окна
      */
     this.returnWindowHeight = function(){
         return window.innerHeight;
     }
     
     /**
      * // вся ширина окна
      * 
      */
     
     this.returnWindowInnerWidth = function(){
         return window.innerWidth;
     }
     /**
      * // ширина минус прокрутка
      */
     this.returnClientWidth = function(){
         document.documentElement.clientWidth
     }

     var userDisplaySettings = {
       userDisplaySettingsConfig: [
         {
           labelName: "Оберіть ширину екрану",
           name: "windowWidth",
           type: "select",
           selectOptions: [
             {
               Disabled: false,
               Group: null,
               Selected: false,
               Text: "На всю ширину",
               Value: 0
             },
             {
               Disabled: false,
               Group: null,
               Selected: true,
               Text: "Широка таблиця",
               Value: 1
             },
             {
               Disabled: false,
               Group: null,
               Selected: false,
               Text: "Вузька таблиця",
               Value: 2
             }
           ]
         },
         {
           labelName: "Установити поточну дату",
           name: "setCurrDate",
           type: "checkbox",
           id: "SetCurrDate",
           value: true
         },
         {
           labelName: "Показувати підказки",
           name: "showTooltip",
           type: "checkbox",
           id: "ShowTooltip",
           value: false
         },
         {
           labelName: "Зберігати дані користувача при виході",
           name: "saveUser",
           type: "checkbox",
           id: "saveUser",
           value: true
         }
       ]
     };
     
     this.createContentSettingsForm = function(data) {
       if (!data || typeof data == "undefined" || self.isStringEmpty(data)) {
         return;
       }
     
       var settingsControls = data.userDisplaySettingsConfig;
       if (!settingsControls || !settingsControls.length) {
         return;
       }
     
       //userDisplaySettingsSaved=>userDisplaySettings.userDisplaySettingsConfig
     
       var html =
         '<div class = "settings-inner">' +
         "<h3>" +
         '<i class="icon icon-lg icon-settings"></i>Налаштування користувача</h3>';
     
       for (var i = 0; i < settingsControls.length; i++) {
         html += self.selectDataTemplate(settingsControls[i]);
       }
     
       html += "</div>";
       var modalContainer = $("#modal").find(".modal-container");
     
       $.when($(modalContainer).html(html)).done(
         self.activateFormControls(modalContainer)
       );
     
       self.openModal();
     
       $(modalContainer)
         .find(".checkbox")
         .on("change", function() {
           var elName = $(this).attr("name");
     
           if (this.checked) {
             userDisplaySettingsSaved[elName] = true;
           } else {
             userDisplaySettingsSaved[elName] = false;
           }
           self.saveToStorage("user-settings", userDisplaySettingsSaved);
     
           // for (var key in settingsControls) {
           //   if (settingsControls[key].name == "setCurrDate") {
           //     settingsControls[key].value = userDisplaySettingsSaved.setCurrDate;
           //   } else if (settingsControls[key].name == "showTooltip") {
           //     settingsControls[key].value = userDisplaySettingsSaved.showTooltip;
           //   } else if (settingsControls[key].name == "saveUser") {
           //     settingsControls[key].value = userDisplaySettingsSaved.saveUser;
           //   }
           // }
         });
     
       $(modalContainer)
         .find(".select")
         .on("change", function() {
           var elName = $(this).attr("name");
           var selectedValue = $(this).val();
     
           userDisplaySettingsSaved[elName] = selectedValue;
     
           self.saveToStorage("user-settings", userDisplaySettingsSaved);
         });
     };
     
     this.updateUserDisplaySettings = function() {
     
       var obgConfig = userDisplaySettings.userDisplaySettingsConfig,
         options = [];
     
       for (var key in userDisplaySettingsSaved) {
         for (var i = 0; i < obgConfig.length; i++) {
           if (obgConfig[i].name == key) {
     
             if ((obgConfig[i].type == "checkbox")) {
               obgConfig[i].value = userDisplaySettingsSaved[key];
             }
     
             if ((obgConfig[i].type == "select")) {
               options = obgConfig[i].selectOptions;
               for (var t = 0; t < options.length; t++) {
                 options[t].Selected = false;
                 if (options[t].Value == userDisplaySettingsSaved[key]) {
                   options[t].Selected = true;
                 }
               }
             }
           }
         }
       }
     };
     
     $("#show-settings").on("click", function() {
       self.updateUserDisplaySettings();
     
       self.createContentSettingsForm(userDisplaySettings);
     
     
       self.closeAllOpenLi(headerAccountMenu);
     });

     this.uploadFile = function (el) {
         var form = $(el).closest('form');
         var url = $(form).attr('action');
         var entityId = $(form).find('#EntityId').val();
         var entityName = $(form).find('#EntityName').val();
         var docType = $(form).find('#DocumentType').val();
         var description = $(form).find('#Description').val();
         var files = $(form).find('#files').get(0).files;
         var data = new FormData();
     
         //return if there aren't files
         if (!files.length) {
             msg = "<p>Відсутні вкладені файли</p>";
             self.createDialog(true, msg);
             return;
         }
         
         var errMsg = function(fileSize, fileName){
             var num = fileSize / 1024 / 1024,
                 currFileSize = num.toFixed(2),
                 currFileName = fileName,
                 msg = "<p>Розмір вкладеного файлу " + currFileName + ' ' + currFileSize + ' MB' + "</p><p>Максимальний розмір файлу має бути не більше 100 MB.</p>";
             self.createDialog(true, msg); 
             return;           
         }
     
         //include files
         for (var i = 0; i < files.length; i++) {
             data.append(files[i].name, files[i]);
             data.append("EntityId", entityId);
             data.append("EntityName", entityName);
             data.append("DocumentType", docType);
             data.append("Description", description);
     
             //check files size       
             if (files[i].size > 1e+8) {           
                 errMsg(files[i].size, files[i].name);          
             }
         }
     
         var parent = $(form).closest(".upload-edit");
         var fileList = $(parent).find('.fileList');
         self.showLoader(fileList);
     
         //submit files
         $.ajax({
             type: "POST",
             url: url,
             contentType: false,
             processData: false,
             data: data,
             success: function (data) {      
                 if (data.success == false){                
                     self.hideLoader(fileListContainer);   
                     errMsg(data.fileSize, data.fileName);                                          
                   
                 }else{
                     $.ajax({
                         type: "Get",
                         url: '/FileStore/List?EntityId=' + entityId + '&amp;EntityName=' + entityName,
                         success: function (d2) {                  
                             var fileListContainer = fileList;
                             if (fileListContainer) {
                                 $(fileListContainer).html(d2);
                                 self.manageContentReload($(fileListContainer));
                                 self.hideLoader(fileListContainer);
                                 return true;
                             }
                             
                         }
                     });
                 }            
             }
         });
     
         //clean form
         $(form).trigger("reset");
         $('.content-upload-filename').html('');
         $('#Description').removeClass('not-empty').addClass('empty');
     }
     
     $(document).on('submit', '.form-file-upload', function (e) {
         e.preventDefault();
         self.uploadFile($(this));
     });

     // docWindow.on('scroll', function () {
     //     clearTimeout(winScrollTimer);
     //     winScrollTimer = setTimeout(function () {
     //         self.showHideHeader();
     //         self.showHideFooter();
     
     //     }, 66);
     // })
     
     docWindow.on('resize', function () {
         clearTimeout(winResizeTimer);
         winResizeTimer = setTimeout(function () {
             windowHeight = self.calculateWindowHeight();
             windowWidth = self.calculateWindowWidth();
             self.manageAside();
             self.showHideFooter();
             self.manageGridContent();
             self.updateNiceScroll();
             self.manageDetailesContent();
             self.findGrids();
         }, 66);
     })
     
     $(document).ready(function () {
         windowHeight = self.calculateWindowHeight();
         windowWidth = self.calculateWindowWidth();
         self.getBreadCrumbsList(breadCrumbCookie);
         self.checkCollapsedBody();
         self.findValidateForm(body);
         self.showHideFooter();
         //self.createCheckboxLabel(body);
         self.manageContentTabs();
         self.initMask(body);
         self.findFormatDateParagraph(body);
         self.activateFormControls(body);
         //self.onLoadManageCollapseBody()
         self.manageAside();
         self.initAllInputTextareaAfterLoad();
         self.onLoadCheckAllInputs();
         //self.tryLoadContainer($('.data-container'));
         self.updateNiceScroll();
         self.manageDetailesContent();
         self.scrollTop();
     });
     
     document.addEventListener('change',
         function (event) {
             // self.saveToStorage(event.target.name, event.target.value);
         });

     // $(window).on('resize', function(){
     //     $('.screen-marker').each(function(){
     //         if($(this).is(":visible")){
     //             console.log($(this));
     //         }
     //     })
     // })
     
     this.goStepBack = function(){
       window.history.go(-1)
     }

};

var mt = new MainTemplates();