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
