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