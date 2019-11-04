this.setPresetFilters = function (formObj, str, setSuccessFunction, setErrorFunction) {

     var url,
          controllerName = formObj.presetFiltersUrl.controllerName,
          actionName = formObj.presetFiltersUrl.actionName,
          journalName = journalName = controllerName + '-' + actionName,
          queryData;

     if (str) { //set
          url = controllerName + '/SetPresettings';
          queryData = {
               journalName: journalName,
               presettingsJson: str
          }
     }

     $.ajax({
          type: "POST",
          url: url,
          data: queryData,
          success: function (data) {
               if (data.setSuccess) { //set
                    return setSuccessFunction()
               } else {
                    return setErrorFunction()
               }

          },
          error: function (data) {
               console.error(data);
               return setErrorFunction()
          }
     });
}


this.updatePresetFilters = function (formObj, quiet) {

     if (!quiet) {
          quiet = false;
     }


     var needActivation = false;

     if (formObj.filtersChanging) {
          var msg = "<p>Система зайнята.</p><p>Зачекайте та спробуйте ще раз.</p>"
          self.createDialog(false, msg);
          return;
     }

     if (formObj.presetSettingActive) {
          needActivation = true;
          self.deActivatePresetFiltersSetting(formObj);
          self.addActiveClass(formObj.presetWrapper);

     }

     if (!quiet) {
          self.showLoader(formObj.presetWrapper);
     }

     formObj.filtersChanging = true;

     var data = {};
     data.filters = formObj.filters;

     
     data = JSON.stringify(data);

     self.setPresetFilters(formObj, data, successCallback, errorCallback)
     function successCallback() {

          self.reBuildPresetFilters(formObj);

          if (needActivation) {
               self.activatePresetFiltersSetting(formObj);
          }

          presetFilterJson = data;
     }

     function errorCallback() {
          //
          var unsavedFilters = JSON.parse(presetFilterJson);
          formObj.filters = self.sortPresetFiltersByOrder(unsavedFilters.filters);
          var interval = setInterval(function () { //при удалении открывается диалоговое окно, надо дать время, чтоб оно успело закрыться
               if (!dialogOpen) {
                    var msg = "<p>Виникла помилка при збереженні фільтра</p>";

                    self.reBuildPresetFilters(formObj)
                    self.createDialog(false, msg, false, errorSaveFilter);
                    clearInterval(interval);

               }
          }, 66);

     }

     function errorSaveFilter() {
          return;
     }
}

