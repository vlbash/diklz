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