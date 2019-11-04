this.buildPresetFilters = function (formObj) {

     var htmlInner = '';

     self.removeActiveClass(formObj.formPresetSwitcher);
     if (!formObj.filters.length) {
          return;
     }

     self.sortArrObjByMethod(formObj.filters, 'order');

     self.addActiveClass(formObj.formPresetSwitcher);

     for (var i = 0; i < formObj.filters.length; i++) {
          htmlInner += self.createFilter(formObj.filters[i]);
     }
     $.when($(formObj.presetWrapper).find('ul').html(htmlInner)).done(
          self.createCheckboxLabel(formObj.presetWrapper),
          self.setPresetFiltersClickListener(formObj)
     );
}

this.createFilter = function (data) {

     var checked = '';

     if (typeof data.default !== 'undefined' && data.default) {
          checked = ' checked ';
     }

     return (
          '<li class="btn btn-outline preset-btn" ' + 'id="'+ data.filterId + '"' +
          ' <div class="checkbox-group-inner"><input type="checkbox" ' +
          checked +
          '/></div>' +
          '<p>' +
          data.name +
          '</p>' +
          '<span class = "remove-preset-btn icon icon-sm icon-remove" data-title = "Видалити фільтр"></span></>'
     );
};



this.setPresetFiltersClickListener = function (formObj) {

     formObj.presetSettingActive = false;

     $(formObj.presetWrapper).find('.preset-filters-settings').on('click', function () {
          self.activatePresetFiltersSetting(formObj);
     })

     $(formObj.presetWrapper).find('.preset-filters-settings-cancel').on('click', function () {

          self.deActivatePresetFiltersSetting(formObj);
     })



     $(formObj.presetWrapper).find('.preset-btn').each(function () {
          var filterBtn = $(this);
          $(filterBtn).find('span').on('click', function (e) {
               //deleting
               e.stopPropagation();
               self.deletePresetFilter(e, formObj, filterBtn);
          });

          $(filterBtn).find('.checkbox-group-inner').on('click', function (e) {
               //setDefault
               e.stopPropagation();
          });

          $(filterBtn).find('label').on('click', function (e) {
               if (!formObj.presetSettingActive) {
                    e.preventDefault();
                    return;
               }
          })


          $(filterBtn).find('input').on('change', function () {
               self.setDefaultPresetFilter(formObj, filterBtn);
          });

          $(filterBtn).on('click', function () { //submit searchForm
               if (!formObj.presetSettingActive) { //todo нафига эта проверка??? не знаю пока, проверить надо
                    $.when(self.submitPresetFilter(formObj, filterBtn))
                         .done(
                              self.closePresetFiltersForm(formObj)
                         )

                    //self.submitPresetFilter(formObj, filterBtn);
               } 
               

          });
     });
};



this.reBuildPresetFilters = function (formObj) {

     self.clearElEventListeners(formObj.presetWrapper);
     self.hideLoader(formObj.presetWrapper);
     $(formObj.presetWrapper).find('ul').html('');
     formObj.filtersChanging = false;
     self.buildPresetFilters(formObj);
}



this.activatePresetFilterBtn = function (formObj) {

     if (formObj.dirty) {
          self.removeNotDisabledClass(formObj.saveFilterBtn)
     } else {
          self.addNotDisabledClass(formObj.saveFilterBtn)
     }
}



this.activatePresetFiltersSetting = function (formObj) {

     formObj.presetSettingActive = true;
     self.createSortablePresetFilters(formObj);
     self.addActiveClass(formObj.presetWrapper);
}

this.deActivatePresetFiltersSetting = function (formObj) {

     formObj.presetSettingActive = false;
     self.removeActiveClass(formObj.presetWrapper);
     self.destroySortablePresetFilters(formObj);
}