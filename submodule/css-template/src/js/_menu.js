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