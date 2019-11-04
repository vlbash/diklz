this.createContentSearchForm = function (data, submitBtnFunction) {
  if (!data || typeof data == 'undefined') {
    return;
  }
  //create formObj
  var thisForm = new FormObject($('form[name="' + data.formConfig.formName + '"]'));

  thisForm.controlConfig = data.controlConfig || false;
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