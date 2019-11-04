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