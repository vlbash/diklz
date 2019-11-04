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