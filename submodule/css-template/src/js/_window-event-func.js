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