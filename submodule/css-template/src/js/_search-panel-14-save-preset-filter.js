this.savePresetFilter = function (formObj) {

     var runOk = true;

     if (self.validateForm(formObj.savePresetFilterForm)) {

          var presetFilterName = $.trim($(formObj.savePresetFilterNameInput).val());

          for (var f = 0; f < formObj.filters.length; f++) {
               if ($.trim(formObj.filters[f].name) == presetFilterName) {
                    runOk = false;
                    var msg = "<p>Фільтр з таким ім\'ям вже збережений</p>";
                    self.createDialog(false, msg, confirmDialog);
                    break;
               }
          }

          function confirmDialog() {
               $(formObj.savePresetFilterNameInput).focus();
          }
          if (!runOk)
               return;

          var presetFiltersData = self.findDataForPresetFilters(formObj);

          if (presetFiltersData) {
               var currFilter = {};
               currFilter.name = $(formObj.savePresetFilterNameInput).val();
               currFilter.default = $(formObj.savePresetFilterDefaultCheckbox).prop('checked');
               currFilter.order = formObj.filters.length;
               currFilter.fields = presetFiltersData;

               var filterId = 'filter' + self.generateId();
               currFilter.filterId = filterId;

               if (currFilter.default) {
                    for (var i = 0; i < formObj.filters.length; i++) {
                         formObj.filters[i].default = false;
                    }

                    //удалить дефолтный фильтр, если есть
               }

               formObj.filters.push(currFilter);
               self.updatePresetFilters(formObj);

          } else {
               self.createDialog(false, "Виберіть параметри фільтру")
          }
          self.closeNewPresetFilterForm(formObj);
     }
}