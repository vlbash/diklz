this.showHideFooter = function () {
    if (self.returnScrollHeight() - self.returnWindowHeight() - docWindow.scrollTop() < ($(footerWrapper).outerHeight() /12)) {///??????????????
        self.addActiveClass(footerWrapper)
    }else{
        self.removeActiveClass(footerWrapper)
    }

}