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