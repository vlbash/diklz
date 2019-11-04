this.getPresetFilters = function (formObj, onLoad) {

  var controllerName = formObj.presetFiltersUrl.controllerName,
    actionName = formObj.presetFiltersUrl.actionName,
    url,

    journalName = journalName = controllerName + '-' + actionName,
    queryData;


  url = controllerName + '/GetPresettings';
  queryData = {
    journalName: journalName
  }



  $.ajax({
    type: "POST",
    url: url,
    data: queryData,
    success: function (data) {
      if (typeof data == 'string' && data.length) { //get
        presetFilterJson = data;
        data = JSON.parse(data);
        formObj.filters = self.sortPresetFiltersByOrder(data.filters);
        
        if (onLoad) {
          //activatePresetFiltersWrapper
          self.submitDefaultFilter(formObj);
        }

        formObj.presetWrapper = $(formObj.form).find('.content-search-form-preset')[0];
        self.buildPresetFilters(formObj);
      }
      if (formObj.filters.length <= 0) {
        self.submitSearchForm(formObj);
      }

    },
    error: function (data) {

      var msg = "<p>Виникла помилка при генерації збережених фільтрів</p><p>Обновіть сторінку</p>";
      self.createDialog(false, msg);
      console.error(data);
    }
  });
}